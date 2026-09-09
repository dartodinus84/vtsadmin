<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="expedition_service.aspx.cs" Inherits="vtsadm.expedition_service" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Expedition Service
            <small>Master</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Expedition Service</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Select Expedition</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Expedition</label>
                            <asp:DropDownList ID="CmbExpedisi" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="CmbExpedisi_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:Panel ID="PanelService" runat="server" Visible="false">
            <div class="row">
                <div class="col-md-12 col-xs-12">
                    <div class="box box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title">Service Information</h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group form-group-sm" style="display:none;">
                                <label>Expedition ID</label>
                                <asp:TextBox ID="txtExpedisiID" runat="server" class="form-control" disabled=""></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Service Code</label>
                                <asp:TextBox ID="txtExpedisiCode" runat="server" class="form-control" placeholder=""></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Service Name</label>
                                <asp:TextBox ID="txtExpedisiName" runat="server" class="form-control" placeholder="" required="required"></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Sending Days Min</label>
                                <asp:TextBox ID="txtSendingDaysMin" runat="server" class="form-control" placeholder="0" TextMode="Number"></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Sending Days Max</label>
                                <asp:TextBox ID="txtSendingDaysMax" runat="server" class="form-control" placeholder="0" TextMode="Number"></asp:TextBox>
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
                            <h3 class="box-title">List Expedition Service</h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group form-group-sm">
                                <asp:Panel runat="server" ScrollBars="Auto" style="max-height: 400px; overflow-y: auto;">
                                    <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10" OnRowCommand="GridView1_RowCommand" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting">
                                        <Columns>
                                            <asp:BoundField DataField="ExpedisiID" HeaderText="Expedition ID" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="ExpedisiCode" HeaderText="Service Code" ItemStyle-Wrap="false" SortExpression="ExpedisiCode" />
                                            <asp:BoundField DataField="ExpedisiName" HeaderText="Service Name" ItemStyle-Wrap="false" SortExpression="ExpedisiName" />
                                            <asp:BoundField DataField="SendingDaysMin" HeaderText="Days Min" ItemStyle-Wrap="false" SortExpression="SendingDaysMin" />
                                            <asp:BoundField DataField="SendingDaysMax" HeaderText="Days Max" ItemStyle-Wrap="false" SortExpression="SendingDaysMax" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" />
                                            <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="UsrUpd" HeaderText="UsrUpd" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="DtmUpd" HeaderText="DtmUpd" ItemStyle-Wrap="false" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                        </Columns>
                                        <RowStyle ForeColor="#003481" BackColor="White" />
                                        <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                        <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                        <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                        <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                        <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                    </asp:GridView>
                                    <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;"><asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

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
                        <button type="button" class="btn btn-default" runat="server" id="CmdYesSubmit" onserverclick="CmdYesSubmit_ServerClick">Yes</button>
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
                        <h6 class="modal-title">Are you sure to delete Service :&nbsp;</h6>
                        <label id="LblServiceCode" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtExpedisiIDDelete" runat="server" />
                        <input type="hidden" id="txtExpedisiCodeDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" id="CmdYesDelete" onserverclick="CmdYesDelete_ServerClick">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Info Box</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" id="div_comment" runat="server"></div>
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

        function confirmDelete(sCode, sExpId, sCodeVal, sStatus) {
            if (sCode != '') {
                document.getElementById('ContentPlaceHolder1_LblServiceCode').innerHTML = sCode;
                document.getElementById('ContentPlaceHolder1_txtExpedisiIDDelete').value = sExpId;
                document.getElementById('ContentPlaceHolder1_txtExpedisiCodeDelete').value = sCodeVal;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete").modal('show');
            }
        }
        function endRequest(sender, args) {
            $('#modal-submit').modal('hide');
            $('#modal-delete').modal('hide');
            $('.modal-backdrop').remove();
            $('body').removeClass('modal-open').css('padding-right', '');
            $('#modal-messagebox').on('hidden.bs.modal', function () {
                document.body.style.paddingRight = '0px';
            });
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                $('#modal-messagebox').modal('show');
            }
        }
        endRequest();
    </script>
</asp:Content>
