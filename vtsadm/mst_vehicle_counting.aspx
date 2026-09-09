<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="mst_vehicle_counting.aspx.cs" Inherits="vtsadm.mst_vehicle_counting" EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Vehicle Counting</h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Vehicle Counting</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-xl-6 col-lg-6 col-md-12 col-sm-12 col-xs-12">
                <!-- general form elements -->
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Vehicle Counting</h3>
                    </div>
                    <div class="box-body">
                        <div style="display: flex; flex-direction: row; flex-wrap: wrap; align-items: center;">
                            <div class="form-group form-group-sm col-xl-4 col-lg-4 col-md-12 col-sm-12 col-xs-12">
                                <label for="txtSearch">Company Name</label>
                                <asp:TextBox ID="txt_company_name" runat="server" class="form-control pull-right" placeholder="Company Name ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm col-xl-4 col-lg-4 col-md-12 col-sm-12 col-xs-12">
                                <label for="txtSearch2">No SN</label>
                                <asp:TextBox ID="txt_param_type" runat="server" class="form-control pull-right" placeholder="No SN ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm col-xl-4 col-lg-4 col-md-12 col-sm-12 col-xs-12">
                                <label for="CmdSearch" style="display: block;">&nbsp;</label>
                                <button id="CmdSearch" runat="server" type="button" class="btn btn-primary btn-sm" onclick="showOverlay();" onserverclick="CmdSearch_Click"><i class="fa fa-search"></i></button>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <div class="form-group form-group-sm" id="div_comment" runat="server"></div>
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="ID" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="company_id" HeaderText="Company ID" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="company_nm" HeaderText="Company Name" ItemStyle-Wrap="false" SortExpression="CompanyName"></asp:BoundField>
                                        <asp:BoundField DataField="vehicle_id" HeaderText="Vehicle ID" ItemStyle-Wrap="false" SortExpression="VehicleID"></asp:BoundField>
                                        <asp:BoundField DataField="car_plate" HeaderText="Nopol" ItemStyle-Wrap="false" SortExpression="CarPlate"></asp:BoundField>
                                        <asp:BoundField DataField="gps_sn" HeaderText="GPS SN" ItemStyle-Wrap="false" SortExpression="GPSSN"></asp:BoundField>
                                        <asp:BoundField DataField="next_channel" HeaderText="Next Channel" ItemStyle-Wrap="false" SortExpression="NextChannel"></asp:BoundField>
                                        <asp:BoundField DataField="channel" HeaderText="Channel" Visible="false"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdEdit" runat="server" Text="Edit" ToolTip="Edit" Enabled="true" CssClass="btn btn-warning btn-xs" />
                                                <button type="button" class="btn btn-info btn-xs btn-zoning" data-id='<%# Eval("id") %>'>Zoning</button>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-remove'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
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
                                    <asp:Label ID="LblPagingParam" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xl-6 col-lg-6 col-md-12 col-sm-12 col-xs-12">
                <!-- general form elements -->
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <input type="hidden" runat="server" id="txtCompanyID" />
                        <h3 class="box-title">Form New Vehicle Counting</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Company ID</label>
                            <div class="input-group input-group-sm">
                                <input type="hidden" id="txtAutoId" runat="server" readonly="readonly"/>
                                <input type="text" id="txtCompID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" />
                                <span class="input-group-btn">
                                    <button id="BtnSearchCustomer" runat="server" type="button" class="btn btn-block btn-primary btn-xs" onclick="showModalCustomer(); return false;"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Company Name</label>
                            <asp:TextBox ID="txtCompanyName" runat="server" class="form-control" placeholder="Company Name ..." disabled="disabled"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Vehicle ID</label>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtParamType" runat="server" class="form-control" placeholder="Please select ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="BtnSearchVehicle" runat="server" type="button" class="btn btn-block btn-primary btn-xs" onclick="showModalVehicle();"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Nopol</label>
                            <asp:TextBox ID="txtVehiclePlate" runat="server" class="form-control" placeholder="Car Plate ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>GPS SN</label>
                            <asp:TextBox ID="txtParamCode" runat="server" class="form-control" placeholder="GPS SN ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Channel <span class="text-red">*</span></label>
                            <asp:CheckBoxList ID="txtParamValue" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" CssClass="checkbox-list">
                                <asp:ListItem Value="0">CH 1</asp:ListItem>
                                <asp:ListItem Value="1">CH 2</asp:ListItem>
                                <asp:ListItem Value="2">CH 3</asp:ListItem>
                                <asp:ListItem Value="3">CH 4</asp:ListItem>
                                <asp:ListItem Value="4">CH 5</asp:ListItem>
                                <asp:ListItem Value="5">CH 6</asp:ListItem>
                                <asp:ListItem Value="6">CH 7</asp:ListItem>
                                <asp:ListItem Value="7">CH 8</asp:ListItem>
                                <asp:ListItem Value="8">CH 9</asp:ListItem>
                                <asp:ListItem Value="9">CH 10</asp:ListItem>
                                <asp:ListItem Value="10">CH 11</asp:ListItem>
                                <asp:ListItem Value="11">CH 12</asp:ListItem>
                                <asp:ListItem Value="12">CH 13</asp:ListItem>
                                <asp:ListItem Value="13">CH 14</asp:ListItem>
                                <asp:ListItem Value="14">CH 15</asp:ListItem>
                                <asp:ListItem Value="15">CH 16</asp:ListItem>
                            </asp:CheckBoxList>
                            <small class="text-muted">Select one or more channels</small>
                        </div>
                        <div id="comment_save" runat="server" class="form-group form-group-sm"></div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdEdit" type="button" class="btn btn-warning" runat="server" onclick="CmdEdit_Click()">Update</button>
                        <button id="CmdSave" type="button" class="btn btn-primary" runat="server" onserverclick="CmdSave_Click">Save</button>
                        <asp:Button ID="CmdClear" CssClass="btn btn-secondary" runat="server" Text="Clear" OnClick="CmdClear_Click"/>
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
                        <h6 class="modal-title">Are you sure to delete vehicle counting ID :&nbsp;</h6>
                        <label id="LblParamID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtParamIDDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-update">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to update vehicle counting ID :&nbsp;</h6>
                        <label id="LblParamIDUpdate" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="Hidden1" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-update').modal('hide');" onserverclick="CmdYesUpdate_Click" id="Button1">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-update').modal('hide');">No</button>
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
                            <iframe src="mst_vehicle_counting_customer_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
                            <iframe id="iframe-vehicle-search" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-zoning" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-lg" style="width: 95%; max-width: 1200px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Setting Zoning Vehicle Counting</h4>
                    </div>
                    <div class="modal-body">
                        <div class="zoning-header-info well well-sm" style="margin-bottom: 15px;">
                            <div class="row">
                                <div class="col-sm-6 col-md-4"><strong>Company</strong> : <span id="zoningInfoCompany"></span></div>
                                <div class="col-sm-6 col-md-4"><strong>Vehicle ID</strong> : <span id="zoningInfoVehicleId"></span></div>
                                <div class="col-sm-6 col-md-4"><strong>Nopol</strong> : <span id="zoningInfoNopol"></span></div>
                                <div class="col-sm-6 col-md-4"><strong>GPS SN</strong> : <span id="zoningInfoGpsSn"></span></div>
                                <div class="col-sm-6 col-md-8"><strong>Channel</strong> : <span id="zoningInfoChannel"></span></div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <label>Active Channels</label>
                                <div id="zoningChannelList" class="list-group zoning-channel-list"></div>
                            </div>
                            <div class="col-md-9">
                                <div class="form-group form-group-sm">
                                    <label>Selected Channel</label>
                                    <p class="form-control-static" id="zoningSelectedChannelLabel" style="font-weight: bold;">-</p>
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Upload Image</label>
                                    <input type="file" id="zoningImageUpload" accept=".jpg,.jpeg,.png,image/jpeg,image/png" class="form-control input-sm" />
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Preview Image</label>
                                    <div id="zoningPreviewWrap" class="zoning-preview-wrap">
                                        <img id="zoningPreviewImg" alt="Channel preview" />
                                        <canvas id="zoningPreviewCanvas"></canvas>
                                    </div>
                                    <div id="zoningPreviewError" class="text-danger" style="display:none; margin-top:6px; font-size:12px;"></div>
                                    <small class="text-muted">Klik pada gambar untuk menambah titik polygon.</small>
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Image Path</label>
                                    <input type="text" id="zoningImagePath" class="form-control input-sm" readonly="readonly" />
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Zone Points</label>
                                    <textarea id="zoningZonePoints" class="form-control" rows="6" style="font-family: Consolas, monospace; font-size: 12px;"></textarea>
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Polygon Info</label>
                                    <p class="form-control-static" id="zoningPolygonInfo">0 points</p>
                                </div>
                                <div class="btn-group btn-group-sm" style="margin-bottom: 10px;">
                                    <button type="button" class="btn btn-default" id="btnZoningUndo">Undo Last Point</button>
                                    <button type="button" class="btn btn-default" id="btnZoningClearPoints">Clear Points</button>
                                    <button type="button" class="btn btn-warning" id="btnZoningValidate">Validate</button>
                                    <button type="button" class="btn btn-danger" id="btnZoningClearChannel">Clear Channel</button>
                                    <button type="button" class="btn btn-info" id="btnZoningImportJson">Import JSON</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" id="btnZoningSave">Save Zoning</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <style>
        /* CheckBoxList Styling */
        .checkbox-list label {
            font-weight: normal !important;
            margin-right: 15px;
            cursor: pointer;
            display: inline-flex;
            align-items: center;
        }
        
        .checkbox-list input[type="checkbox"] {
            margin-right: 5px;
            transform: scale(1.2);
            cursor: pointer;
        }
        
        .checkbox-list td {
            padding: 5px 10px !important;
            border: none !important;
        }
        
        .checkbox-list table {
            margin-bottom: 5px;
        }

        .zoning-channel-list .list-group-item {
            cursor: pointer;
            font-weight: 600;
            font-size: 12px;
        }
        .zoning-channel-list .list-group-item.active {
            background-color: #003481;
            border-color: #003481;
        }
        .zoning-channel-list .status-ready { color: #00a65a; }
        .zoning-channel-list .status-empty { color: #999; }
        .zoning-channel-list .status-image-only { color: #3c8dbc; }
        .zoning-channel-list .status-zone-only { color: #f39c12; }
        .zoning-channel-list .status-invalid { color: #dd4b39; }
        .zoning-preview-wrap canvas.zoning-canvas-disabled {
            pointer-events: none;
            cursor: default;
        }
        .zoning-preview-wrap canvas.zoning-canvas-enabled {
            pointer-events: auto;
            cursor: crosshair;
        }
        .zoning-preview-wrap {
            position: relative;
            display: inline-block;
            max-width: 100%;
            border: 1px solid #ddd;
            background: #f4f4f4;
            min-height: 120px;
            min-width: 200px;
        }
        .zoning-preview-wrap img {
            display: block;
            max-width: 100%;
            height: auto;
            user-select: none;
            position: relative;
            z-index: 1;
        }
        .zoning-preview-wrap canvas {
            position: absolute;
            top: 0;
            left: 0;
            cursor: crosshair;
            z-index: 2;
            background: transparent;
        }
    </style>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script type="text/javascript">
        var vcAppRoot = '<%= ResolveUrl("~/") %>';
        function postCustChild(sCustID, sFullName) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtCompID').value = sCustID;
                document.getElementById('ContentPlaceHolder1_txtCompanyName').value = sFullName;
                document.getElementById('ContentPlaceHolder1_txtParamType').value = '';
                document.getElementById('ContentPlaceHolder1_txtParamCode').value = '';
                document.getElementById('ContentPlaceHolder1_txtVehiclePlate').value = '';
                $('#ContentPlaceHolder1_txtParamCode').removeAttr('disabled')
                $('#ContentPlaceHolder1_txtParamValue').removeAttr('disabled')
                $('#ContentPlaceHolder1_CmdSave').removeAttr('disabled')

                $('#modal-customer').modal('hide');
            }
        }
        function postDelete(sAutoid) {
            if (sAutoid != '') {
                document.getElementById('ContentPlaceHolder1_LblParamID').innerHTML = sAutoid;
                document.getElementById('ContentPlaceHolder1_txtParamIDDelete').value = sAutoid;
                $('#modal-delete').modal('show');
            }
        }

        function postEdit(autoid, company_id, company_name, vehicle_id, car_plate, gps_sn, channel) {
            if (autoid && autoid !== '0') {
                document.getElementById('ContentPlaceHolder1_txtAutoId').value = autoid;
                document.getElementById('ContentPlaceHolder1_txtCompID').value = company_id;
                document.getElementById('ContentPlaceHolder1_txtCompanyName').value = company_name;
                document.getElementById('ContentPlaceHolder1_txtParamType').value = vehicle_id;
                document.getElementById('ContentPlaceHolder1_txtVehiclePlate').value = (car_plate && car_plate !== '&nbsp;') ? car_plate : '';
                document.getElementById('ContentPlaceHolder1_txtParamCode').value = (gps_sn && gps_sn !== '&nbsp;') ? gps_sn : '';
                
                // Set CheckBoxList values from comma-separated string
                var channelCheckboxList = document.getElementById('ContentPlaceHolder1_txtParamValue');
                if (channelCheckboxList) {
                    // Clear all checkboxes first
                    var checkboxes = channelCheckboxList.getElementsByTagName('input');
                    for (var i = 0; i < checkboxes.length; i++) {
                        checkboxes[i].checked = false;
                    }
                    
                    // Check selected channels
                    if (channel) {
                        var channels = channel.split(',');
                        for (var i = 0; i < checkboxes.length; i++) {
                            for (var j = 0; j < channels.length; j++) {
                                if (checkboxes[i].value === channels[j].trim()) {
                                    checkboxes[i].checked = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                
                $('#ContentPlaceHolder1_txtParamCode').removeAttr('disabled')
                $('#ContentPlaceHolder1_txtParamValue').removeAttr('disabled')
                $('#ContentPlaceHolder1_CmdSave').removeAttr('disabled')
                document.getElementById('ContentPlaceHolder1_CmdEdit').style.display = 'inline-block';
                document.getElementById('ContentPlaceHolder1_CmdSave').style.display = 'none';
                fetchVehicleDetail(company_id, vehicle_id);

                $('#ContentPlaceHolder1_BtnSearchCustomer').prop('disabled', true);
                $('#ContentPlaceHolder1_BtnSearchVehicle').prop('disabled', true);

            }
        }
        function endRequest(sender, args) {
            $('#modal-messagebox').on('hidden.bs.modal', function () {
                document.body.style.paddingRight = '0px';
            });
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
            }
            setReadOnlyFields();
        }

        function selectCustomer(company_id, company_nm) {
            $('#ContentPlaceHolder1_InpCustomerName').val(company_nm)
            $('#ContentPlaceHolder1_InpCompanyId').val(company_id)
        }

        function showModalCustomer() {
            $('#modal-customer').modal({ show: true });
        }

        function cmdsave_click() {
            $('#ContentPlaceHolder1_txtParamCode').removeAttr('disabled')
            $('#ContentPlaceHolder1_txtParamValue').removeAttr('disabled')
            $('#ContentPlaceHolder1_CmdSave').removeAttr('disabled')
        }

        function CmdEdit_Click() {
            $('#modal-update').modal('show')
            document.getElementById('ContentPlaceHolder1_LblParamIDUpdate').innerHTML = document.getElementById('ContentPlaceHolder1_txtAutoId').value
        }

        $(document).ready(() => {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(endRequest);
            endRequest();
            document.getElementById('ContentPlaceHolder1_CmdEdit').style.display = 'none';
            setReadOnlyFields();
        })

        function setReadOnlyFields() {
            $('#ContentPlaceHolder1_txtParamType').prop('readonly', true);
            $('#ContentPlaceHolder1_txtVehiclePlate').prop('readonly', true);
            $('#ContentPlaceHolder1_txtParamCode').prop('readonly', true);
            if (!document.getElementById('ContentPlaceHolder1_txtAutoId').value) {
                $('#ContentPlaceHolder1_BtnSearchCustomer').prop('disabled', false);
                $('#ContentPlaceHolder1_BtnSearchVehicle').prop('disabled', false);
            }
        }
        function showModalVehicle() {
            var companyId = document.getElementById('ContentPlaceHolder1_txtCompID').value;
            if (companyId === '' || companyId === null || companyId === undefined) {
                alert('Please select company first!');
                return;
            }

            var iframe = document.getElementById('iframe-vehicle-search');
            iframe.src = 'mst_vehicle_counting_vehicle_search.aspx?company_id=' + encodeURIComponent(companyId);
            $('#modal-vehicle').modal({ show: true });
        }

        function postVehicleChild(vehicle_id, car_plate, gps_sn) {
            if (vehicle_id !== '') {
                document.getElementById('ContentPlaceHolder1_txtParamType').value = vehicle_id;
                document.getElementById('ContentPlaceHolder1_txtVehiclePlate').value = (car_plate && car_plate !== '&nbsp;') ? car_plate : '';
                document.getElementById('ContentPlaceHolder1_txtParamCode').value = (gps_sn && gps_sn !== '&nbsp;') ? gps_sn : '';
                $('#modal-vehicle').modal('hide');
            }
        }

        function fetchVehicleDetail(companyId, vehicleId) {
            if (!companyId || !vehicleId) {
                return;
            }

            if (typeof PageMethods === 'undefined' || !PageMethods.GetVehicleInfo) {
                return;
            }

            PageMethods.GetVehicleInfo(companyId, vehicleId, function (data) {
                if (data) {
                    document.getElementById('ContentPlaceHolder1_txtVehiclePlate').value = data.CarPlate || '';
                    if (data.GpsSn) {
                        document.getElementById('ContentPlaceHolder1_txtParamCode').value = data.GpsSn;
                    }
                }
            }, function (err) {
                console.error('Failed to fetch vehicle detail', err);
            });
        }

    </script>
    <script type="text/javascript">
        window.vcZoningApiUrl = '<%= ResolveUrl("~/vehicle_counting_zoning_api.ashx") %>';
        window.vcZoningUploadUrl = window.vcZoningApiUrl;
        window.vcZoningDebug = false;
    </script>
    <script src="<%= ResolveUrl("~/Scripts/vc_vehicle_counting_zoning.js") %>?v=6"></script>
</asp:Content>
