<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="view_mutation_gsm_approve.aspx.cs" Inherits="vtsadm.view_mutation_gsm_approve" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Mutation Gsm           
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">View</a></li>
            <li><a href="#">Mutation</a></li>
            <li class="active">Gsm</li>
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
                        <h3 class="box-title">List Mutation Gsm</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>

                                        <asp:BoundField DataField="GsmID" HeaderText="Gsm ID" ItemStyle-Wrap="false" SortExpression="GsmID"></asp:BoundField>
                                        <asp:BoundField DataField="MSIDN" HeaderText="MSIDN" ItemStyle-Wrap="false" SortExpression="MSIDN"></asp:BoundField>
                                        <asp:BoundField DataField="ProviderID" HeaderText="Provider ID" ItemStyle-Wrap="false" SortExpression="ProviderID"></asp:BoundField>
                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider Name" ItemStyle-Wrap="false" SortExpression="ProviderName"></asp:BoundField>
                                        <asp:BoundField DataField="warehousename" HeaderText="Warehouse Name" ItemStyle-Wrap="false" SortExpression="WarehouseName"></asp:BoundField>
                                        <asp:BoundField DataField="technicianname" HeaderText="Technician Name" ItemStyle-Wrap="false" SortExpression="TechnicianName"></asp:BoundField>
                                        <asp:BoundField DataField="StatusDesc" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="StatusDesc"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDetails" runat="server" Text="<i class='fa fa-list-alt'></i>" ToolTip="Details" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdApprove" runat="server" Text="<i class='fa fa-check'></i>" ToolTip="Cancel" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Cancel" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="WarehouseID" HeaderText="Warehouse ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseRemark" HeaderText="Warehouse Remark" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseUsrUpd" HeaderText="Warehouse UsrUpd" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseDtmUpd" HeaderText="Warehouse DtmUpd" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianID" HeaderText="Technician ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianRemark" HeaderText="Technician Remark" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianUsrUpd" HeaderText="Technician UsrUpd" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianDtmUpd" HeaderText="Technician DtmUpd" ItemStyle-Wrap="false"></asp:BoundField>
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
                    <div class="box-footer">
                        <asp:Button ID="CmdExport" CssClass="btn btn-primary" runat="server" OnClick="CmdExport_Click" Text="Export" />
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
                        <h4 class="modal-title">Log Gsm Warehouse</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframelogwarehouse" src="view_mutation_gsm_warehouse.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportWarehouse" CssClass="btn btn-primary" runat="server" OnClick="CmdExportWarehouse_Click" Text="Export" />  
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
                        <h4 class="modal-title">Log Gsm Technician</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframelogtechnician" src="view_mutation_gsm_technician.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportTechnician" CssClass="btn btn-primary" runat="server" OnClick="CmdExportTechnician_Click" Text="Export" />  
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
                        <h4 class="modal-title">Mutation Gsm Details</h4>
                    </div>
                    <div class="modal-body" style="background-color: #ecf0f5">
                        <div class="row">
                            <div class="col-sm-4">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Gsm Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Gsm ID</label>
                                            <p id="LblGsmID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>MSIDN</label>
                                            <p id="LblMSIDN" runat="server" class="form-control-static"></p>
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
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Provider Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Provider ID</label>
                                            <p id="LblProviderID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Provider Name</label>
                                            <p id="LblProviderName" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4">
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
                            <div class="col-sm-4">
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
          <div class="modal fade" id="modal-delete">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete request Gsm ID :&nbsp;</h6>
                        <label id="LblGsmID2" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtGsmeIDDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal-approve">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to Approve Gsm ID :&nbsp;</h6>
                        <label id="LblGsmID3" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtGsmIDApprove" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-approve').modal('hide');" onserverclick="CmdYesApprove_ServerClick" id="CmdYesApprove">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-approve').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>
    
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
        function postLogWarehouse(sGsmID) {
            if (sGsmID != '') {
                $('#modal-logwarehouse').modal('show');
                var objfr = document.getElementById('iframelogwarehouse').contentWindow;
                var objGsmID = objfr.document.getElementById('txtGsmID');
                var cmdSearchLog = objfr.document.getElementById('CmdSearchLog');
                objGsmID.value = sGsmID;
                cmdSearchLog.click();
            }
        }

        function postLogTechnician(sGsmID) {
            if (sGsmID != '') {
                $('#modal-logtechnician').modal('show');
                var objfr = document.getElementById('iframelogtechnician').contentWindow;
                var objGsmID = objfr.document.getElementById('txtGsmID');
                var cmdSearchLog = objfr.document.getElementById('CmdSearchLog');
                objGsmID.value = sGsmID;
                cmdSearchLog.click();
            }
        }
        function postDetails(sGsmID, sMSIDN, sProviderID, sProviderName, sWarehouseName, sTechnicianName, sStatus, objCmd, sWarehouseID, sWarehouseRemark,
            sWarehouseUsrUpd, sWarehouseDtmUpd, sTechnicianID, sTechnicianRemark, sTechnicianUsrUpd, sTechnicianDtmUpd, sSourceName) {
            if (sGsmID != '') {
                document.getElementById('ContentPlaceHolder1_LblGsmID').innerText = CheckNbsp(sGsmID);
                document.getElementById('ContentPlaceHolder1_LblMSIDN').innerText = CheckNbsp(sMSIDN);
                document.getElementById('ContentPlaceHolder1_LblStatus').innerText = CheckNbsp(sStatus);

                document.getElementById('ContentPlaceHolder1_LblWarehouseID').innerText = CheckNbsp(sWarehouseID);
                document.getElementById('ContentPlaceHolder1_LblWarehouseName').innerText = CheckNbsp(sWarehouseName);
                document.getElementById('ContentPlaceHolder1_LblWarehouseRemark').innerText = CheckNbsp(sWarehouseRemark);
                document.getElementById('ContentPlaceHolder1_LblWarehouseUsrUpd').innerText = CheckNbsp(sWarehouseUsrUpd);
                document.getElementById('ContentPlaceHolder1_LblWarehouseDtmUpd').innerText = CheckNbsp(sWarehouseDtmUpd);

                document.getElementById('ContentPlaceHolder1_LblProviderID').innerText = CheckNbsp(sProviderID);
                document.getElementById('ContentPlaceHolder1_LblProviderName').innerText = CheckNbsp(sProviderName);

                document.getElementById('ContentPlaceHolder1_LblTechnicianID').innerText = CheckNbsp(sTechnicianID);
                document.getElementById('ContentPlaceHolder1_LblTechnicianName').innerText = CheckNbsp(sTechnicianName);
                document.getElementById('ContentPlaceHolder1_LblTechnicianRemark').innerText = CheckNbsp(sTechnicianRemark);
                document.getElementById('ContentPlaceHolder1_LblTechnicianUsrUpd').innerText = CheckNbsp(sTechnicianUsrUpd);
                document.getElementById('ContentPlaceHolder1_LblTechnicianDtmUpd').innerText = CheckNbsp(sTechnicianDtmUpd);

                document.getElementById('ContentPlaceHolder1_LblSourceName').innerText = CheckNbsp(sSourceName);
                $('#modal-details').modal('show');
            }
        }
        function confirmDelete(sGsmID) {
            if (sGsmID != '') {
                document.getElementById('ContentPlaceHolder1_LblGsmID2').innerHTML = sGsmID;
                document.getElementById('ContentPlaceHolder1_txtGsmeIDDelete').value = sGsmID;
                $("#modal-delete").modal('show');
            }
        }
        function confirmApprove(sGsmID) {
            if (sGsmID != '') {
                document.getElementById('ContentPlaceHolder1_LblGsmID3').innerHTML = sGsmID;
                document.getElementById('ContentPlaceHolder1_txtGsmIDApprove').value = sGsmID;
                $("#modal-approve").modal('show');
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
