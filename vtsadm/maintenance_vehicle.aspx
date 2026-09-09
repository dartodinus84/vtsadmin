<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="maintenance_vehicle.aspx.cs" Inherits="vtsadm.maintenance_vehicle" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Vehicle           
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Vehicle</li>
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
                            <asp:TextBox ID="txtVehID" runat="server" class="form-control" placeholder="Skip for new vehicle ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Brand Vehicle</label>
                            <asp:DropDownList ID="CmbBrandID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbBrandID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Type</label>
                            <asp:DropDownList ID="CmbTypeID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbTypeID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Model</label>
                            <asp:DropDownList ID="CmbModelID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Vehicle Identity Number</label>
                            <asp:TextBox ID="txtVin" runat="server" class="form-control" placeholder="Vehicle Identity Number ..." required="required"></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Engine Number</label>
                            <asp:TextBox ID="txtEngineNumber" runat="server" class="form-control" placeholder="Engine Number ..." required="required"></asp:TextBox>
                        </div>

                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Other Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Vehicle Description</label>
                            <asp:TextBox ID="txtVehDesc" runat="server" class="form-control" placeholder="Vehicle Description ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm" style="display: none">
                            <label>Police No</label>
                            <asp:TextBox ID="txtPoliceNoOld" runat="server" class="form-control" placeholder="Police No ..." required="required"></asp:TextBox>
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
                            <label>Vehicle Type</label>
                            <asp:DropDownList ID="CmbVehicleType" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Container Size</label>
                            <asp:DropDownList ID="CmbContainerSize" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Icon Vehicle</label>
                            <asp:DropDownList ID="CmbIconVehicle" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                        <button id="CmdUploadExcel" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload">Upload Excel</button>
                    </div>
                </div>
            </div>

            <%--<div class="row">--%>
            <div class="col-md-12">
                <!-- general form elements -->
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Vehicle</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by police no ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="CmdSearch" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearch_ServerClick"><i class="fa fa-search"></i></button>
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
                                        <asp:BoundField DataField="VehicleID" HeaderText="Vehicle ID" ItemStyle-Wrap="false" SortExpression="VehicleID"></asp:BoundField>
                                        <asp:BoundField DataField="VehicleDesc" HeaderText="Vehicle Description" ItemStyle-Wrap="false" SortExpression="VehicleDesc"></asp:BoundField>
                                        <asp:BoundField DataField="Brand" HeaderText="Brand" ItemStyle-Wrap="false" SortExpression="Brand"></asp:BoundField>
                                        <asp:BoundField DataField="Model" HeaderText="Model" ItemStyle-Wrap="false" SortExpression="Model"></asp:BoundField>
                                        <asp:BoundField DataField="Type" HeaderText="Type" ItemStyle-Wrap="false" SortExpression="Type"></asp:BoundField>
                                        <asp:BoundField DataField="PoliceNo" HeaderText="Police No" ItemStyle-Wrap="false" SortExpression="PoliceNo"></asp:BoundField>
                                        <asp:BoundField DataField="AssetNo" HeaderText="Asset No" ItemStyle-Wrap="false" SortExpression="AssetNo"></asp:BoundField>

                                        <asp:BoundField DataField="Vin" HeaderText="Vin" ItemStyle-Wrap="false" SortExpression="Vin"></asp:BoundField>
                                        <asp:BoundField DataField="EngineNumber" HeaderText="Engine Number" ItemStyle-Wrap="false" SortExpression="EngineNumber"></asp:BoundField>

                                        <asp:BoundField DataField="VehicleTypeDesc" HeaderText="Vehicle Type" ItemStyle-Wrap="false" SortExpression="VehicleTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="ContainerSizeDesc" HeaderText="Container Size" ItemStyle-Wrap="false" SortExpression="ContainerSizeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="BatchNo" HeaderText="Batch No" ItemStyle-Wrap="false" SortExpression="BatchNo"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <%--<asp:ButtonField ControlStyle-CssClass="btn btn-block btn-primary btn-xs" Text="Delete" ButtonType="Image" CommandName="Delete"></asp:ButtonField>--%>
                                        <asp:BoundField DataField="VehicleTypeID" HeaderText="Vehicle Type ID" ItemStyle-Wrap="false" SortExpression="VehicleTypeID"></asp:BoundField>
                                        <asp:BoundField DataField="ContainerSizeID" HeaderText="Container Size ID" ItemStyle-Wrap="false" SortExpression="ContainerSizeID"></asp:BoundField>
                                        <asp:BoundField DataField="BrandID" HeaderText="Brand ID" ItemStyle-Wrap="false" SortExpression="BrandID"></asp:BoundField>
                                        <asp:BoundField DataField="ModelID" HeaderText="Model ID" ItemStyle-Wrap="false" SortExpression="ModelID"></asp:BoundField>
                                        <asp:BoundField DataField="TypeID" HeaderText="Type ID" ItemStyle-Wrap="false" SortExpression="TypeID"></asp:BoundField>
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
        <div class="modal fade bs-example-modal-lg" id="modal-upload">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Bulk Upload Car Master</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="maintenance_vehicle_upload.aspx" style="width: 100%; border: none; height: 680px;" scrolling="auto"></iframe>
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

        function endRequest(sender, args) {
            $('#modal-upload').on('hidden.bs.modal', function () {
                var objClear = document.getElementById('ContentPlaceHolder1_CmdClear');
                objClear.click();
            });
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
