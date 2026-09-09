<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="mst_telegram_bot.aspx.cs" Inherits="vtsadm.mst_telegram_bot" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Telegram           
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Telegram</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Telegram Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>CompanyID</label>
                            <asp:TextBox ID="txtCompanyID" runat="server" class="form-control" placeholder="Skip for CompanyID ..." required="required" disabled=""></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Company Name</label>
                            <asp:TextBox ID="txtCompanyName" runat="server" class="form-control" placeholder="Skip for Company Name ..." required="required" disabled=""></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>TelegramID</label>
                            <asp:TextBox ID="txtTelegramID" runat="server" class="form-control" placeholder="Skip for TelegramID ..." required="required" disabled=""></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Name</label>
                            <asp:TextBox ID="txtTelegramName" runat="server" class="form-control" placeholder="Telegram Name ..." required="required" disabled></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Username</label>
                            <asp:TextBox ID="txtUserName" runat="server" class="form-control" placeholder="Skip for Username ..." required="required" disabled=""></asp:TextBox>
                        </div>

                         <div class="form-group form-group-sm">
                            <label>GroupChatID</label>
                             <asp:DropDownList ID="CmbGroupBotID" runat="server" CssClass="form-control" required="required"></asp:DropDownList>
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
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Telegram</h3>
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
                                        <asp:BoundField DataField="company_id" HeaderText="CompanyID" ItemStyle-Wrap="false" SortExpression="company_id"></asp:BoundField>
                                        <asp:BoundField DataField="company_nm" HeaderText="Company Name" ItemStyle-Wrap="false" SortExpression="company_nm"></asp:BoundField>
                                        <asp:BoundField DataField="TelegramID" HeaderText="Telegram ID" ItemStyle-Wrap="false" SortExpression="TelegramID"></asp:BoundField>
                                        <asp:BoundField DataField="TelegramName" HeaderText="Telegram Name" ItemStyle-Wrap="false" SortExpression="TelegramName"></asp:BoundField>
                                        <asp:BoundField DataField="UserName" HeaderText="User Name" ItemStyle-Wrap="false" SortExpression="UserName"></asp:BoundField>
                                        <asp:BoundField DataField="groupBotID" HeaderText="groupBotID" ItemStyle-Wrap="false" SortExpression="groupBotID"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
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
                        <h6 class="modal-title">Are you sure to delete Telegram ID :&nbsp;</h6>
                        <label id="LblTelegramID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtTelegramIDDelete" runat="server" />
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
                document.getElementById('ContentPlaceHolder1_LblTelegramID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtTelegramIDDelete').value = sText;
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

