<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="user.aspx.cs" Inherits="vtsadm.user" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>User
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">User Access</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">User</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">User Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>User ID</label>
                            <asp:TextBox ID="txtRegUserID" runat="server" class="form-control" placeholder="User ID ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Password</label>
                            <asp:TextBox ID="txtRegPassword" TextMode="Password" runat="server" class="form-control" placeholder="Password ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Confirm Password</label>
                            <asp:TextBox ID="txtRegConfPassword" TextMode="Password" runat="server" class="form-control" placeholder="Confirm Password ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Full Name</label>
                            <asp:TextBox ID="txtRegUserName" runat="server" class="form-control" placeholder="User Name ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Email</label>
                            <asp:TextBox ID="txtRegEmail" runat="server" class="form-control" placeholder="Email ..." required="required"></asp:TextBox>
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
                            <label>User Type</label>
                            <asp:DropDownList ID="CmbUserTypeID" runat="server" AutoPostBack="true" OnTextChanged="CmbUserTypeID_TextChanged" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Technician Name</label>
                            <asp:DropDownList ID="CmbTechnicianID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Marketing Name</label>
                            <asp:DropDownList ID="CmbMarketingID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List User</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by Name ..."></asp:TextBox>
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
                                        <asp:BoundField DataField="UserID" HeaderText="User ID" ItemStyle-Wrap="false" SortExpression="UserID"></asp:BoundField>
                                        <asp:BoundField DataField="Password" HeaderText="Password" ItemStyle-Wrap="false" SortExpression="Password"></asp:BoundField>
                                        <asp:BoundField DataField="FullName" HeaderText="Full Name" ItemStyle-Wrap="false" SortExpression="FullName"></asp:BoundField>
                                        <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Wrap="false" SortExpression="Email"></asp:BoundField>
                                        <asp:BoundField DataField="UserTypeDesc" HeaderText="User Type" ItemStyle-Wrap="false" SortExpression="UserTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianName" HeaderText="Technician" ItemStyle-Wrap="false" SortExpression="TechnicianName"></asp:BoundField>
                                        <asp:BoundField DataField="MarketingName" HeaderText="Marketing" ItemStyle-Wrap="false" SortExpression="MarketingName"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <%--<asp:ButtonField ControlStyle-CssClass="btn btn-success btn-xs" Text="<i class='fa fa-unlock'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Unlocked"></asp:ButtonField>--%>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdUnlock" runat="server" Text="<i class='fa fa-unlock'></i>" ToolTip="Unlock" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <%--<asp:ButtonField ControlStyle-CssClass="btn btn-danger btn-xs" Text="<i class='fa fa-remove'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Delete"></asp:ButtonField>--%>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="UserTypeID" HeaderText="User Type ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianID" HeaderText="Technician ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MarketingID" HeaderText="Marketing ID" ItemStyle-Wrap="false"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
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
                        <h6 class="modal-title">Are you sure to delete User ID :&nbsp;</h6><label id="LblUserIDDelete" runat="server"></label>&nbsp;?
                        <input type="hidden" id="txtUserIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal-unlock">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to unlock User ID :&nbsp;</h6><label id="LblUserIDUnlock" runat="server"></label>&nbsp;?
                        <input type="hidden" id="txtUserIDUnlock" runat="server" />
                        <input type="hidden" id="txtStatusUnlock" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-unlock').modal('hide');" onserverclick="CmbYesUnlock_ServerClick" id="CmbYesUnlock">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-unlock').modal('hide');">No</button>
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
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function confirmDelete(sUserID, sStatus) {
            if (sUserID != '') {
                document.getElementById('ContentPlaceHolder1_LblUserIDDelete').innerHTML = sUserID;
                document.getElementById('ContentPlaceHolder1_txtUserIDDelete').value = sUserID;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete").modal('show');
            }
        }
        function confirmUnlock(sUserID, sStatus) {
            if (sUserID != '') {
                document.getElementById('ContentPlaceHolder1_LblUserIDUnlock').innerHTML = sUserID;
                document.getElementById('ContentPlaceHolder1_txtUserIDUnlock').value = sUserID;
                document.getElementById('ContentPlaceHolder1_txtStatusUnlock').value = sStatus;
                $("#modal-unlock").modal('show');
            }
        }
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
                //$('#modal-messagebox').show();
            }
        }
        endRequest();
    </script>
</asp:Content>
