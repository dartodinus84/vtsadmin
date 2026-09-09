<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="setting_fuel_clbr.aspx.cs" Inherits="vtsadm.setting_fuel_clbr" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .fuel-card .box-header {
            border-bottom: 1px solid #f0f0f0;
        }

        .fuel-card .box-title i {
            margin-right: 6px;
        }

        .fuel-required {
            color: #dd4b39;
            font-weight: 600;
        }

        .fuel-help {
            color: #7f8c8d;
            font-size: 12px;
            margin-top: 4px;
            display: block;
        }

        .fuel-toolbar {
            width: 280px;
        }

        .fuel-grid .btn {
            min-width: 30px;
        }
    </style>

    <section class="content-header">
        <h1>Setting Fuel Calibration
            <small>Maintenance</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Setting Fuel Calibration</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-7 col-xs-12">
                <div class="box box-solid fuel-card">
                    <div class="box-header with-border">
                        <h3 class="box-title"><i class="fa fa-sliders"></i>Fuel Calibration Form</h3>
                    </div>
                    <div class="box-body">
                        <div id="div_comment" runat="server"></div>
                        <asp:HiddenField ID="hidID" runat="server" />

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group form-group-sm">
                                    <label>Vendor <span class="fuel-required">*</span></label>
                                    <asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group form-group-sm">
                                    <label>Brand <span class="fuel-required">*</span></label>
                                    <asp:DropDownList ID="ddlBrand" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group form-group-sm">
                                    <label>Type <span class="fuel-required">*</span></label>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group form-group-sm">
                                    <label>Sensor Type <span class="fuel-required">*</span></label>
                                    <asp:DropDownList ID="ddlSensorType" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="[Select]" Value="[Select]"></asp:ListItem>
                                        <asp:ListItem Text="Tegangan" Value="Tegangan"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-group form-group-sm">
                                    <label>ACC <span class="fuel-required">*</span></label>
                                    <asp:DropDownList ID="ddlACC" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="[Select]" Value="[Select]"></asp:ListItem>
                                        <asp:ListItem Text="ON" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="OFF" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group form-group-sm">
                                    <label>Voltage <span class="fuel-required">*</span></label>
                                    <div class="input-group input-group-sm">
                                        <span class="input-group-addon"><i class="fa fa-bolt"></i></span>
                                        <asp:TextBox ID="txtVoltage" runat="server" CssClass="form-control" MaxLength="20" placeholder="0.00" onkeypress="return allowDecimalInput(event);"></asp:TextBox>
                                    </div>
                                    <span class="fuel-help">Decimal number only</span>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group form-group-sm">
                                    <label>Fuel Value <span class="fuel-required">*</span></label>
                                    <div class="input-group input-group-sm">
                                        <span class="input-group-addon"><i class="fa fa-tint"></i></span>
                                        <asp:TextBox ID="txtFuelValue" runat="server" CssClass="form-control" MaxLength="20" placeholder="0.00" onkeypress="return allowDecimalInput(event);"></asp:TextBox>
                                    </div>
                                    <span class="fuel-help">Decimal number only</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-default" Text="Reset" OnClick="btnReset_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>
            <div class="col-md-5 col-xs-12">
                <div class="box box-solid fuel-card">
                    <div class="box-header with-border">
                        <h3 class="box-title"><i class="fa fa-info-circle"></i>Information</h3>
                    </div>
                    <div class="box-body">
                        <p style="margin-bottom: 8px;">Gunakan halaman ini untuk mapping kalibrasi fuel sensor per kombinasi vendor, brand, type, dan sensor.</p>
                        <ul style="padding-left: 18px; margin-bottom: 0;">
                            <li>Data duplicate akan ditolak oleh stored procedure.</li>
                            <li>Klik <b>Update</b> pada grid untuk edit data yang sudah ada.</li>
                            <li>Klik <b>Delete</b> untuk soft delete (status menjadi DE).</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid fuel-card">
                    <div class="box-header with-border">
                        <h3 class="box-title"><i class="fa fa-table"></i>List Fuel Calibration</h3>
                        <div class="box-tools fuel-toolbar">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control pull-right" placeholder="Search by name ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" OnClick="btnSearch_Click" CausesValidation="false"><i class="fa fa-search"></i></asp:LinkButton>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <asp:Panel runat="server" ScrollBars="Auto">
                            <div class="table-responsive fuel-grid">
                                <asp:GridView ID="gvData" runat="server" BackColor="WhiteSmoke" AllowSorting="false" Font-Size="Small" CssClass="table table-bordered table-striped table-hover" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvData_PageIndexChanging" OnRowCommand="gvData_RowCommand" OnRowDataBound="gvData_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="VendorName" HeaderText="VendorName" />
                                        <asp:BoundField DataField="BrandVehicle" HeaderText="BrandVehicle" />
                                        <asp:BoundField DataField="TypeVehicle" HeaderText="TypeVehicle" />
                                        <asp:BoundField DataField="SensorType" HeaderText="SensorType" />
                                        <asp:TemplateField HeaderText="Acc" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%# Convert.ToString(Eval("Acc")) == "1" ? "ON" : "OFF" %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Voltage" HeaderText="Voltage" />
                                        <asp:BoundField DataField="FuelValue" HeaderText="FuelValue" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-warning btn-xs" CommandName="EditData" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" ToolTip="Update" CausesValidation="false"><i class="fa fa-edit"></i></asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-xs" CommandName="DeleteData" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" ToolTip="Delete" OnClientClick="return confirm('Are you sure to delete this data?');" CausesValidation="false"><i class="fa fa-close"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="VendorID" HeaderText="VendorID" />
                                        <asp:BoundField DataField="VehicleBrandID" HeaderText="VehicleBrandID" />
                                        <asp:BoundField DataField="VehicleTypeID" HeaderText="VehicleTypeID" />
                                        <asp:BoundField DataField="Acc" HeaderText="AccValue" />
                                        <asp:BoundField DataField="ID" HeaderText="ID" />
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="5" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                            </div>
                            <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                <asp:Label ID="lblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                            </div>
                        </asp:Panel>
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
                        <div class="form-group form-group-sm" id="div_message" runat="server"></div>
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

        function allowDecimalInput(evt) {
            evt = evt || window.event;
            var charCode = evt.which ? evt.which : evt.keyCode;
            if (charCode === 8 || charCode === 9 || charCode === 13 || charCode === 46) {
                return true;
            }

            var ch = String.fromCharCode(charCode);
            if (!/[0-9.]/.test(ch)) {
                return false;
            }

            var textbox = evt.target || evt.srcElement;
            if (ch === "." && textbox.value.indexOf(".") > -1) {
                return false;
            }
            return true;
        }

        function endRequest() {
            $('#modal-messagebox').on('hidden.bs.modal', function () {
                document.body.style.paddingRight = '0px';
            });

            var message = document.getElementById('ContentPlaceHolder1_div_message');
            if (message && message.innerHTML !== '') {
                $('#modal-messagebox').modal('show');
            }
        }
        endRequest();
    </script>
</asp:Content>
