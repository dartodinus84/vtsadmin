﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="mirroring_vehicle.aspx.cs" Inherits="vtsadm.mirroring_vehicle" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Mirroring - Vehicle  
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li class="active">Mirroring Vehicle/li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <input type="hidden" runat="server" id="txtVehicleID" />

                        <h3 class="box-title">Search Customer</h3>
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
                            <asp:TextBox ID="txtCustFullName" runat="server" class="form-control" placeholder="Company Name ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Type</label>
                            <asp:TextBox ID="txtCustCustTypeDesc" runat="server" class="form-control" placeholder="Company Address ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <asp:TextBox ID="txtCustBranchName" runat="server" class="form-control" placeholder="Company Address ..."></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_Click">Clear</button>
                        <asp:Button ID="CmdLoadVehicle" CssClass="btn btn-primary" runat="server" OnClick="CmdLoadVehicle_Click" Text="Load" />
                    </div>
                </div>


                <div class="box box-solid">
                    <div class="box-header with-border">
               
                        <h3 class="box-title">Customer Information</h3>
                    </div>
                    <div class="box-body">
                        
                        <div class="form-group form-group-sm">
                            <label>Company Name (Easygo VTS Server)</label>
                            <asp:TextBox ID="txtVtsCompanyName" runat="server" class="form-control" placeholder="Company Name ..."></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <asp:HiddenField ID="txtVtsCompanyID" runat="server"></asp:HiddenField>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Company Name (Indocement Server)</label>
                            <asp:TextBox ID="txtIntpCompanyName" runat="server" class="form-control" placeholder="Company Name ..."></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <asp:HiddenField ID="txtIntpCompanyID" runat="server"></asp:HiddenField>
                        </div>
                        
                    </div>
      
                </div>

            </div>


            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Vehicle</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by Police No ..."></asp:TextBox>
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
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10" OnRowCommand="GridView2_RowCommand" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" >
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="VehicleID" HeaderText="VehicleID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PoliceNo" HeaderText="PoliceNo" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="NoSN" HeaderText="NoSN" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="GsmNo" HeaderText="GsmNo" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ServerInstall" HeaderText="Server Utama" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="is_mirroring" HeaderText="IsMirroring" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ServerID" HeaderText="ServerID" ItemStyle-Wrap="false"></asp:BoundField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdSelected" runat="server" Text="<i class='fa fa-check'></i>" ToolTip="Mirroring" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="UnMirroring" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
                            <iframe src="mirroring_vehicle_customer_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-selected">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to Mirroring NoSN :&nbsp;</h6>
                        <label id="LblSNSelected" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtVehicleIDSelected" runat="server" />
                        <input type="hidden" id="txtNoSNSelected" runat="server" />
                        <input type="hidden" id="txtServerIDSelected" runat="server" />

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-selected').modal('hide');" onserverclick="CmdYesSelected_ServerClick" id="CmdYesSelected">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-selected').modal('hide');">No</button>
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
                        <h6 class="modal-title">Are you sure to UnMirroring NoSN :&nbsp;</h6>
                        <label id="LblSNDelete" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtVehicleIDDelete" runat="server" />
                        <input type="hidden" id="txtNoSNDelete" runat="server" />
                        <input type="hidden" id="txtServerIDDelete" runat="server" />

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

        function confirmSelected(sVehicleID, sNoSN, sServerID) {
            if (sVehicleID != '') {
                document.getElementById('ContentPlaceHolder1_LblSNSelected').innerHTML = sNoSN;
                document.getElementById('ContentPlaceHolder1_txtVehicleIDSelected').value = sVehicleID;
                document.getElementById('ContentPlaceHolder1_txtNoSNSelected').value = sNoSN;
                document.getElementById('ContentPlaceHolder1_txtServerIDSelected').value = sServerID;

                $("#modal-selected").modal('show');
            }
        }

        function confirmDelete(sVehicleID, sNoSN, sServerID) {
            if (sVehicleID != '') {
                document.getElementById('ContentPlaceHolder1_LblSNDelete').innerHTML = sNoSN;
                document.getElementById('ContentPlaceHolder1_txtVehicleIDDelete').value = sVehicleID;
                document.getElementById('ContentPlaceHolder1_txtNoSNDelete').value = sNoSN;
                document.getElementById('ContentPlaceHolder1_txtServerIDDelete').value = sServerID;

                $("#modal-delete").modal('show');
            }
        }

        function postCustChild(sCustID, sFullName, sCustTypeDesc, sBranchName, sVtsCompanyId, sIntpCompanyId, sVtsCompanyNm, sIntpCompanyNm) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtCustID').value = sCustID;
                document.getElementById('ContentPlaceHolder1_txtCustFullName').value = sFullName;
                document.getElementById('ContentPlaceHolder1_txtCustCustTypeDesc').value = sCustTypeDesc;
                document.getElementById('ContentPlaceHolder1_txtCustBranchName').value = sBranchName;
                document.getElementById('ContentPlaceHolder1_txtVtsCompanyID').value = sVtsCompanyId;
                document.getElementById('ContentPlaceHolder1_txtIntpCompanyID').value = sIntpCompanyId;
                document.getElementById('ContentPlaceHolder1_txtVtsCompanyName').value = sVtsCompanyNm;
                document.getElementById('ContentPlaceHolder1_txtIntpCompanyName').value = sIntpCompanyNm;

                $('#modal-customer').modal('hide');
                var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadVehicle');
                objfr2.click();
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