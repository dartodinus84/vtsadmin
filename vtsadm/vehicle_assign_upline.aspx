<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="vehicle_assign_upline.aspx.cs" Inherits="vtsadm.vehicle_assign_upline" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Vehicle Assignment - Upline  
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Setup</a></li>
            <li><a href="#">Vehicle Assignment</a></li>
            <li class="active">Upline</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Customer ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtCustID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button2" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-customer"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Full Name</label>
                            <asp:TextBox ID="txtCustFullName" runat="server" class="form-control" placeholder="Full Name ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Type</label>
                            <asp:TextBox ID="txtCustCustTypeDesc" runat="server" class="form-control" placeholder="Customer Type ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <asp:TextBox ID="txtCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
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
                                    <button id="Button1" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-vehicle"><i class="fa fa-search"></i></button>
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
                            <label>Asset Number</label>
                            <asp:TextBox ID="txtAssetNo" runat="server" class="form-control" placeholder="Asset Number ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tva ID</label>
                            <asp:TextBox ID="txtTvaID" runat="server" class="form-control" placeholder="Tva ID ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_Click">Clear</button>
                        <asp:Button ID="CmdLoadVehicle" CssClass="btn btn-primary" runat="server" OnClick="CmdLoadVehicle_Click" Text="Load" />
                    </div>
                </div>

                <div id="box-server" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Server Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Server Name</label>
                            <asp:DropDownList ID="CmbCustServer" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Upline Available</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearchAvai" runat="server" class="form-control pull-right" placeholder="Search by Name ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="CmdSearchAvai" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearchAvai_ServerClick">
                                        <i class="fa fa-search"></i>
                                    </button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowCommand="GridView2_RowCommand" OnPageIndexChanging="GridView2_PageIndexChanging">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TvaID" HeaderText="Tva ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UplineID" HeaderText="Upline ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UplineName" HeaderText="Upline Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <%--<asp:BoundField DataField="UplineAddress" HeaderText="Upline Address" ItemStyle-Wrap="false"></asp:BoundField>--%>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-success btn-xs" Text="<i class='fa fa-check'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Select"></asp:ButtonField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;"><asp:Label ID="LblPagingA" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Upline Selected</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearchSel" runat="server" class="form-control pull-right" placeholder="Search by Name ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="CmdSearchSel" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearchSel_ServerClick">
                                        <i class="fa fa-search"></i>
                                    </button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowCommand="GridView1_RowCommand" OnPageIndexChanging="GridView1_PageIndexChanging">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TvaID" HeaderText="Tva ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UplineID" HeaderText="Upline ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UplineName" HeaderText="Upline Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <%--<asp:BoundField DataField="UplineAddress" HeaderText="Upline Address" ItemStyle-Wrap="false"></asp:BoundField>--%>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-danger btn-xs" Text="<i class='fa fa-close'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Remove"></asp:ButtonField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;"><asp:Label ID="LblPagingS" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-customer">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Customer</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="vehicle_assign_upline_customer_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
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
                            <iframe id="iframevehicle" src="vehicle_assign_upline_vehicle_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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

        function postCustChild(sCustID, sFullName, sCustTypeDesc, sBranchName) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtCustID').value = sCustID;
                document.getElementById('ContentPlaceHolder1_txtCustFullName').value = sFullName;
                document.getElementById('ContentPlaceHolder1_txtCustCustTypeDesc').value = sCustTypeDesc;
                document.getElementById('ContentPlaceHolder1_txtCustBranchName').value = sBranchName;

                $('#modal-customer').modal('hide');

                var objfr2 = document.getElementById('iframevehicle').contentWindow;
                var objCustID = objfr2.document.getElementById('txtCustID');
                var cmdSearchVehicle = objfr2.document.getElementById('CmdSearchVehicle');
                objCustID.value = sCustID;
                cmdSearchVehicle.click();
            }
        }

        function postVehChild(sVehicleID, sVehicleDesc, sPoliceNo, sAssetNo, sTvaID) {
            if (sVehicleID != '') {
                document.getElementById('ContentPlaceHolder1_txtVehicleID').value = sVehicleID;
                document.getElementById('ContentPlaceHolder1_txtVehicleDesc').value = sVehicleDesc;
                document.getElementById('ContentPlaceHolder1_txtPoliceNo').value = sPoliceNo;
                document.getElementById('ContentPlaceHolder1_txtAssetNo').value = sAssetNo;
                document.getElementById('ContentPlaceHolder1_txtTvaID').value = sTvaID;

                $('#modal-vehicle').modal('hide');
                var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadVehicle');
                objfr2.click();
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
