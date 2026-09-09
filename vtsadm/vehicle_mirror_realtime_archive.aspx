<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="vehicle_mirror_realtime_archive.aspx.cs" Inherits="vtsadm.vehicle_mirror_realtime_archive" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Vehicle Mirror Realtime
            <small>Archive / Restore</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Installation</a></li>
            <li><a href="#">Maintenance</a></li>
            <li class="active">Mirror Archive / Restore</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Filter</h3>
                    </div>
                    <div class="box-body">
                        <div id="div_comment" class="form-group form-group-sm" runat="server"></div>

                        <div class="row">
                            <div class="col-md-3 col-sm-6">
                                <div class="form-group form-group-sm">
                                    <label>Company ID</label>
                                    <div class="input-group input-group-sm">
                                        <input type="text" id="txtCompID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" autocomplete="off" />
                                        <span class="input-group-btn">
                                            <button id="BtnSearchCustomer" runat="server" type="button" class="btn btn-primary btn-xs" data-toggle="modal" data-target="#modal-customer"><i class="fa fa-search"></i></button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-6">
                                <div class="form-group form-group-sm">
                                    <label>Company Name</label>
                                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" placeholder="Company Name ..." ReadOnly="true" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-6">
                                <div class="form-group form-group-sm">
                                    <label>View</label>
                                    <asp:RadioButtonList ID="rblViewMode" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Text="Active" Value="active" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Archived" Value="archived"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="col-md-2 col-sm-6">
                                <div class="form-group form-group-sm">
                                    <label>&nbsp;</label>
                                    <div>
                                        <asp:Button ID="btnLoad" CssClass="btn btn-primary btn-sm" runat="server" Text="Load" OnClick="btnLoad_Click" CausesValidation="false" />
                                        <asp:Button ID="btnClear" CssClass="btn btn-default btn-sm" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="false" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group form-group-sm" style="margin-bottom:0;">
                            <asp:Button ID="btnArchive" CssClass="btn btn-warning btn-sm" runat="server" Text="Archive Whole Company"
                                OnClick="btnArchive_Click" OnClientClick="return confirmArchiveCompany();" CausesValidation="false" />
                            <asp:Button ID="btnRestore" CssClass="btn btn-success btn-sm" runat="server" Text="Restore Whole Company"
                                OnClick="btnRestore_Click" OnClientClick="return confirmRestoreCompany();" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title"><asp:Literal ID="litGridTitle" runat="server" Text="Active Mirror Rows"></asp:Literal></h3>
                        <asp:Label ID="lblRowCount" runat="server" CssClass="pull-right text-muted" style="margin-top:6px;"></asp:Label>
                    </div>
                    <div class="box-body table-responsive">
                        <asp:GridView ID="gvData" runat="server" BackColor="WhiteSmoke" Font-Size="Small"
                            CssClass="table table-bordered table-striped" CellPadding="2" Width="100%" AutoGenerateColumns="False"
                            EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px"
                            AllowPaging="true" PageSize="50"
                            OnPageIndexChanging="gvData_PageIndexChanging"
                            OnRowCommand="gvData_RowCommand"
                            OnRowDataBound="gvData_RowDataBound"
                            DataKeyNames="vehicle_id">
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <Columns>
                                <asp:BoundField DataField="autoid" HeaderText="AutoID" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="vehicle_id" HeaderText="Vehicle ID" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="mirror_server" HeaderText="Mirror Server" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="id_vendor" HeaderText="Vendor ID" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="imei_vendor" HeaderText="IMEI Vendor" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="is_enabled" HeaderText="Enabled" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="dtmupd" HeaderText="DtmUpd" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="notes" HeaderText="Notes" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="company_id" HeaderText="Company" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="archived_at" HeaderText="Archived At" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="archived_by" HeaderText="Archived By" ItemStyle-Wrap="false" />
                                <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnRowArchive" runat="server"
                                            CssClass="btn btn-warning btn-xs"
                                            CommandName="ARCHIVE_VEHICLE"
                                            CausesValidation="false"
                                            CommandArgument='<%# Eval("vehicle_id") %>'
                                            OnClientClick="return confirm('Archive this vehicle mirror row(s)?');"
                                            Visible="false">
                                            Archive
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnRowRestore" runat="server"
                                            CssClass="btn btn-success btn-xs"
                                            CommandName="RESTORE_VEHICLE"
                                            CausesValidation="false"
                                            CommandArgument='<%# Eval("vehicle_id") %>'
                                            OnClientClick="return confirm('Restore this vehicle mirror row(s)?');"
                                            Visible="false">
                                            Restore
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle ForeColor="#003481" BackColor="White" />
                            <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                            <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                            <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                            <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-customer">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title">Company</h4>
                    </div>
                    <div class="modal-body">
                        <iframe src="mdvr_company_safety_ai_policy_company_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <style type="text/css">
        #<%= rblViewMode.ClientID %> label {
            font-weight: normal;
            margin-right: 12px;
            margin-left: 4px;
        }
        .table-responsive {
            overflow-x: auto;
        }
    </style>

    <script type="text/javascript">
        function confirmArchiveCompany() {
            return confirm("Archive ALL active mirror rows for this company?\nThey will be removed from vehicle_mirror_realtime.");
        }

        function confirmRestoreCompany() {
            return confirm("Restore ALL archived mirror rows for this company?\nThey will be removed from the archive table.");
        }

        function postCustChild(sCustID, sFullName) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtCompID').value = sCustID;
                document.getElementById('ContentPlaceHolder1_txtCompanyName').value = sFullName;
                $('#modal-customer').modal('hide');
            }
        }
    </script>
</asp:Content>
