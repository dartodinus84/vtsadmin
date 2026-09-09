<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="vehicle_assign_customer.aspx.cs" Inherits="vtsadm.vehicle_assign_customer" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Vehicle Assignment - Customer  
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Setup</a></li>
            <li><a href="#">Vehicle Assignment</a></li>
            <li class="active">Customer</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <input type="hidden" runat="server" id="txtTvaID" />
                        <input type="hidden" runat="server" id="txtNewCustID" />
                        <input type="hidden" runat="server" id="txtVehicleID" />

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
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_Click">Clear</button>
                        <asp:Button ID="CmdLoadVehicle" CssClass="btn btn-primary" runat="server" OnClick="CmdLoadVehicle_Click" Text="Load" />
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Vehicle Available</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearchAvai" runat="server" class="form-control pull-right" placeholder="Search by Police No ..."></asp:TextBox>
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
                                        <asp:BoundField DataField="VehicleID" HeaderText="Vehicle ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="VehicleDesc" HeaderText="Vehicle Desc" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PoliceNo" HeaderText="Police No" ItemStyle-Wrap="false"></asp:BoundField>
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
                        <h3 class="box-title">List Vehicle Selected</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearchSel" runat="server" class="form-control pull-right" placeholder="Search by Police No ..."></asp:TextBox>
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
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowCommand="GridView1_RowCommand" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TvaID" HeaderText="Tva ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="VehicleID" HeaderText="Vehicle ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="VehicleDesc" HeaderText="Vehicle Desc" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PoliceNo" HeaderText="Police No" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdMutation" runat="server" Text="<i class='ion ion-person'></i>" ToolTip="Customer Mutation" Enabled="true" CssClass="btn btn-warning btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>    
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

        <div class="modal fade" id="modal-mutation">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to mutated Vehicle ID :&nbsp;</h6><label id="LblVehicleID" runat="server"></label>&nbsp;?
                    </div>
                    <div class="modal-footer">
                        <img src="Content/ajax-loader2.gif" style="display:none;" id="iload"/>
                        <button type="button" class="btn btn-default btn-confirmasi" runat="server" onclick="$('.btn-confirmasi').attr('disabled','disabled');$('button.close').hide();$('#iload').show();" onserverclick="CmdYesMutated_ServerClick" id="CmdYesMutated">Yes</button>
                        <button type="button" class="btn btn-primary btn-confirmasi" onclick="$('#modal-mutation').modal('hide');">No</button>
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
                            <iframe src="vehicle_assign_customer_customer_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-customer-mutation">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Customer Mutation</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframemutation" src="vehicle_assign_customer_mutation.aspx" style="width: 100%; border: none; height: 350px;" overflow:hidden;" scrolling="no"></iframe>
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
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="$('.modal-backdrop').remove();">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Info Box</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" id="div_comment" runat="server">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" onclick="$('.modal-backdrop').remove();" data-dismiss="modal">Close</button>
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
                var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadVehicle');
                objfr2.click();
            }
        }

        function postCustomerMutation(sNewCustID) {
            if (sNewCustID != '') {
                //document.getElementById('ContentPlaceHolder1_txtTvaID').value = sTvaID;
                document.getElementById('ContentPlaceHolder1_txtNewCustID').value = sNewCustID;
                //document.getElementById('ContentPlaceHolder1_txtVehicleID').value = sVehicleID;
                //document.getElementById('ContentPlaceHolder1_LblVehicleID').innerHTML = sVehicleID;
                $('#modal-customer-mutation').modal('hide');
                $("#modal-mutation").modal('show');
            }
        }

        function buttonYesMutated() {
            $("#modal-mutation").modal('hide');
        }

        function customerMutation(sCustID, sTvaID, sVehicleID) {
            if (sTvaID.value != '') {
                var objfr = document.getElementById("iframemutation").contentWindow;
                var objCustID = objfr.document.getElementById("txtCustID");
                document.getElementById('ContentPlaceHolder1_txtTvaID').value = sTvaID;
                document.getElementById('ContentPlaceHolder1_txtVehicleID').value = sVehicleID;
                document.getElementById('ContentPlaceHolder1_LblVehicleID').innerHTML = sVehicleID;
                //var objTvaID = objfr.document.getElementById("txtTvaID");
                //var objVehicleID = objfr.document.getElementById("txtVehicleID");
                var cmdSearch = objfr.document.getElementById("CmdSearch");
                objCustID.value = sCustID;
                //objTvaID.value = sTvaID;
                //objVehicleID.value = sVehicleID;
                cmdSearch.click();
                $('#modal-customer-mutation').modal('show');
            }
        }

        function endRequest(sender, args) {
            $('.modal').on('hidden.bs.modal', function () {
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
