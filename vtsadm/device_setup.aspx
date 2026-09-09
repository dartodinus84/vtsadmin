<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="device_setup.aspx.cs" Inherits="vtsadm.device_setup" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Device Setup         
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Setup</a></li>
            <li class="active">Device Setup</li>
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
                            <label>Warehouse Name</label>
                            <asp:TextBox ID="txtWareName" runat="server" class="form-control" placeholder="Warehouse Name ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">GSM Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>GSM ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtGsmID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required"/>
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
                                <input type="text" id="txtTechnicianID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required"/>
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
                            <asp:TextBox ID="txtBranchName" runat="server" class="form-control" placeholder="Technician Name ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_ServerClick" Text="Clear" />
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClick="CmdSubmit_ServerClick" Text="Submit" />
                    </div>
                </div>                

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Device Setup</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowCommand="GridView2_RowCommand" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TdsID" HeaderText="Tds ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceID" HeaderText="Device ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type Desc" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseName" HeaderText="Warehouse Name" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="GsmID" HeaderText="Gsm ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="MSIDN" HeaderText="MSIDN" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider Name" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianID" HeaderText="Technician ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianName" HeaderText="Technician Name" ItemStyle-Wrap="true"></asp:BoundField>
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
                            <iframe src="device_setup_device_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
                            <iframe src="device_setup_gsm_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
                            <iframe src="device_setup_technician_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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

        function postDeviceChild(sDeviceID, sNoSN, sVendorName, sDeviceTypeDesc, sWarehouseName) {
            if (sDeviceID != '') {
                document.getElementById('ContentPlaceHolder1_txtDeviceID').value = sDeviceID;
                document.getElementById('ContentPlaceHolder1_txtNoSN').value = sNoSN;
                document.getElementById('ContentPlaceHolder1_txtVendorName').value = sVendorName;
                document.getElementById('ContentPlaceHolder1_txtDeviceTypeDesc').value = sDeviceTypeDesc;
                document.getElementById('ContentPlaceHolder1_txtWareName').value = sWarehouseName;

                $('#modal-device').modal('hide');

                //var objfr1 = document.getElementById('iframe1').contentWindow;
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }

        function postGsmChild(sGsmID, sMSIDN, sProviderName) {
            if (sGsmID != '') {
                document.getElementById('ContentPlaceHolder1_txtGsmID').value = sGsmID;
                document.getElementById('ContentPlaceHolder1_txtMSIDN').value = sMSIDN;
                document.getElementById('ContentPlaceHolder1_txtProviderName').value = sProviderName;

                $('#modal-gsm').modal('hide');

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
                document.getElementById('ContentPlaceHolder1_txtBranchName').value = sBranchName;

                $('#modal-technician').modal('hide');

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
