<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ref_vehiclemodel.aspx.cs" Inherits="vtsadm.ref_vehiclemodel" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Model Vehicle           
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Model Vehicle</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Model Vehicle Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Vehicle ID</label>
                            <asp:TextBox ID="txtVehicleModelID" runat="server" class="form-control" placeholder="Skip for new Model Vehicle ..." required="required" disabled=""></asp:TextBox>
                        </div>
                       <div class="form-group form-group-sm">
                            <label>Brand Vehicle</label>
                            <asp:DropDownList ID="CmbVehicleBrandID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbBrandID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Vehicle Type</label>
                            <asp:DropDownList ID="CmbVehicleTypeID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Vehicle Model</label>
                            <asp:TextBox ID="txtModelVehicle" runat="server" class="form-control" placeholder="Model Vehicle Name ..." required="required"></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Is Ev</label>
                            <asp:DropDownList ID="CmbIsEv" runat="server" CssClass="form-control">
                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Ev Power Consumption</label>
                            <asp:TextBox ID="txtEvPowerConsumption" TextMode="Number" runat="server" class="form-control" placeholder="Ev Power Consumption ..."></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Ev Range</label>
                            <asp:TextBox ID="txtEvRange" TextMode="Number" runat="server" class="form-control" placeholder="Ev Range ..."></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Ev Capacity</label>
                            <asp:TextBox ID="txtEvCapacity" TextMode="Number" runat="server" class="form-control" placeholder="Ev Capacity ..."></asp:TextBox>
                        </div>


                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Model Vehicle</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by name ..."></asp:TextBox>
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
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowCommand="GridView2_RowCommand" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="VehicleModelID" HeaderText="Model Vehicle ID" ItemStyle-Wrap="false" SortExpression="VehicleModelID"></asp:BoundField>
                                        <asp:BoundField DataField="VehicleBrandID" HeaderText="Vehicle Brand ID" ItemStyle-Wrap="false" SortExpression="VehicleBrandID"></asp:BoundField>
                                        <asp:BoundField DataField="BrandVehicle" HeaderText="Brand Vehicle" ItemStyle-Wrap="false" SortExpression="BrandVehicle"></asp:BoundField>
                                        <asp:BoundField DataField="VehicleTypeID" HeaderText="Type Vehicle ID" ItemStyle-Wrap="false" SortExpression="VehicleTypeID"></asp:BoundField>
                                        <asp:BoundField DataField="TypeVehicle" HeaderText="Type Vehicle" ItemStyle-Wrap="false" SortExpression="TypeVehicle"></asp:BoundField>
                                        <asp:BoundField DataField="ModelVehicle" HeaderText="Model Vehicle" ItemStyle-Wrap="false" SortExpression="ModelVehicle"></asp:BoundField>

                                        <asp:BoundField DataField="is_ev" HeaderText="Is Ev" ItemStyle-Wrap="false" SortExpression="is_ev"></asp:BoundField>
                                        <asp:BoundField DataField="ev_power_consumption" HeaderText="Ev Power Consumption" ItemStyle-Wrap="false" SortExpression="ev_power_consumption"></asp:BoundField>
                                        <asp:BoundField DataField="ev_range" HeaderText="Ev Range" ItemStyle-Wrap="false" SortExpression="ev_range"></asp:BoundField>
                                        <asp:BoundField DataField="ev_capacity" HeaderText="Ev Capacity" ItemStyle-Wrap="false" SortExpression="ev_capacity"></asp:BoundField>

                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <%--<asp:ButtonField ControlStyle-CssClass="btn btn-block btn-primary btn-xs" Text="Delete" ButtonType="Image" CommandName="Delete"></asp:ButtonField>--%>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
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
                        <h6 class="modal-title">Are you sure to delete Model Vehicle ID :&nbsp;</h6>
                        <label id="LblVehicleModelID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtVehicleModelIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
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

        function confirmDelete(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblVehicleModelID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtVehicleModelIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete").modal('show');
            }
        }
        function endRequest(sender, args) {
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
