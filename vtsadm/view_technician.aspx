<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="view_technician.aspx.cs" Inherits="vtsadm.view_technician" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Technician           
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">View</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Technician</li>
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
                        <h3 class="box-title">List Technician</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TechnicianID" HeaderText="Technician ID" ItemStyle-Wrap="false" SortExpression="TechnicianID"></asp:BoundField>
                                        <asp:BoundField DataField="EmployeeNo" HeaderText="Employee No" ItemStyle-Wrap="false" SortExpression="EmployeeNo"></asp:BoundField>
                                        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Wrap="false" SortExpression="Name"></asp:BoundField>
                                        <asp:BoundField DataField="BranchName" HeaderText="Branch Name" ItemStyle-Wrap="false" SortExpression="BranchName"></asp:BoundField>
                                        <asp:BoundField DataField="StatusDesc" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="StatusDesc"></asp:BoundField>

                                        <asp:BoundField DataField="BranchID" HeaderText="Branch ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BranchAddress" HeaderText="Branch Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDetails" runat="server" Text="<i class='fa fa-list-alt'></i>" ToolTip="Details" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdLog" runat="server" Text="<i class='fa fa-history'></i>" ToolTip="Log" Enabled="true" CssClass="btn btn-primary btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDevice" runat="server" Text="<i class='fa fa-cubes'></i>" ToolTip="Device" Enabled="true" CssClass="btn btn-warning btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdGsm" runat="server" Text="<i class='fa fa-phone'></i>" ToolTip="Gsm" Enabled="true" CssClass="btn btn-primary btn-xs" />
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
        <div class="modal fade bs-example-modal-lg" id="modal-device">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List Device</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframedevice" src="view_technician_device.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportDevice" CssClass="btn btn-primary" runat="server" OnClick="CmdExportDevice_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportDeviceXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportDeviceXls_Click" Text="Export XLS" />
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-gsm">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List Gsm</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframegsm" src="view_technician_gsm.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportGsm" CssClass="btn btn-primary" runat="server" OnClick="CmdExportGsm_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportGsmXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportGsmXls_Click" Text="Export XLS" />
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-log">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Log Technician</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframelog" src="view_technician_log.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
                        <h4 class="modal-title">Technician Details</h4>
                    </div>
                    <div class="modal-body" style="background-color:#ecf0f5">
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Technician Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Technician ID</label>
                                            <p id="LblTechnicianID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Employee No</label>
                                            <p id="LblEmployeeNo" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Name</label>
                                            <p id="LblName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Status</label>
                                            <p id="LblStatus" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Branch Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Branch ID</label>
                                            <p id="LblBranchID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Branch Name</label>
                                            <p id="LblBranchName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Branch Address</label>
                                            <p id="LblBranchAddress" runat="server" class="form-control-static"></p>
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
        function postDevice(sTechnicianID) {
            if (sTechnicianID != '') {
                $('#modal-device').modal('show');
                var objfr = document.getElementById('iframedevice').contentWindow;
                var objTechnicianID = objfr.document.getElementById('txtTechnicianID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');
                objTechnicianID.value = sTechnicianID;
                cmdSearch.click();
            }
        }
        function postGsm(sTechnicianID) {
            if (sTechnicianID != '') {
                $('#modal-gsm').modal('show');
                var objfr = document.getElementById('iframegsm').contentWindow;
                var objTechnicianID = objfr.document.getElementById('txtTechnicianID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');
                objTechnicianID.value = sTechnicianID;
                cmdSearch.click();
            }
        }
        function postLog(sTechnicianID) {
            if (sTechnicianID != '') {
                $('#modal-log').modal('show');
                var objfr = document.getElementById('iframelog').contentWindow;
                var objTechnicianID = objfr.document.getElementById('txtTechnicianID');
                var cmdSearchLog = objfr.document.getElementById('CmdSearchLog');
                objTechnicianID.value = sTechnicianID;
                cmdSearchLog.click();
            }
        }
        function postDetails(sTechnicianID, sEmployeeNo, sName, sBranchName, sStatus, sBranchID, sBranchAddress) {
            if (sTechnicianID != '') {
                document.getElementById('ContentPlaceHolder1_LblTechnicianID').innerText = CheckNbsp(sTechnicianID);
                document.getElementById('ContentPlaceHolder1_LblEmployeeNo').innerText = CheckNbsp(sEmployeeNo);
                document.getElementById('ContentPlaceHolder1_LblName').innerText = CheckNbsp(sName);
                document.getElementById('ContentPlaceHolder1_LblBranchID').innerText = CheckNbsp(sBranchID);
                document.getElementById('ContentPlaceHolder1_LblBranchName').innerText = CheckNbsp(sBranchName);
                document.getElementById('ContentPlaceHolder1_LblBranchAddress').innerText = CheckNbsp(sBranchAddress);
                document.getElementById('ContentPlaceHolder1_LblStatus').innerText = CheckNbsp(sStatus);
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
