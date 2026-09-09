<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="mst_igo_commision.aspx.cs" Inherits="vtsadm.mst_igo_commision" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>iGO Track
                <small>Commision</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Commision Withdraw</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Search Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Search By</label>
                            <asp:TextBox ID="txtSearch" runat="server" class="form-control" placeholder="Search by any fields ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Date From</label>
                            <asp:TextBox ID="txtDateFrom" TextMode="Date" runat="server" class="form-control" placeholder="Input date from ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Date To</label>
                            <asp:TextBox ID="txtDateTo" TextMode="Date" runat="server" class="form-control" placeholder="Input date to ..."></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Purchase Order</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="Commision ID" ItemStyle-Wrap="false" SortExpression="PoID"></asp:BoundField>
                                        <asp:BoundField DataField="cust_id" HeaderText="Custommer ID" ItemStyle-Wrap="false" SortExpression="PoID"></asp:BoundField>
                                        <asp:BoundField DataField="customer_name" HeaderText="Custommer Name" ItemStyle-Wrap="false" SortExpression="sPoDate"></asp:BoundField>
                                        <asp:BoundField DataField="date" HeaderText="Withdraw Date" ItemStyle-Wrap="false" SortExpression="CustomerName"></asp:BoundField>
                                        <asp:BoundField DataField="bank" HeaderText="Account Bank" ItemStyle-Wrap="false" SortExpression="PoNumber"></asp:BoundField>
                                        <asp:BoundField DataField="account_no" HeaderText="No Account" ItemStyle-Wrap="false" SortExpression="PoNumber"></asp:BoundField>
                                        <asp:BoundField DataField="account_name" HeaderText="Account Name" ItemStyle-Wrap="false" SortExpression="PoNumber"></asp:BoundField>
                                        <asp:BoundField DataField="withdraw" HeaderText="Withdraw" ItemStyle-Wrap="false" SortExpression="ValueStatus"></asp:BoundField>
                                        <asp:BoundField DataField="saldo" HeaderText="Saldo" ItemStyle-Wrap="false" SortExpression="ValueStatus"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdApprove" runat="server" Text="<i class='fa fa-check'></i>" ToolTip="Approve" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdReject" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Cancel" Enabled="true" CssClass="btn btn-danger btn-xs" />
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
                    <div class="box-footer">
                        <asp:Button ID="CmdExport" CssClass="btn btn-primary" runat="server" OnClick="CmdExport_Click" Text="Export" />
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
         <div class="modal fade" id="modal-approve">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to Approve Commision :&nbsp;</h6>
                        <label id="LblCommisionIDApprove" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtCommisionIDApprove" runat="server" />
                        </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-approve').modal('hide');" onserverclick="CmdYesApprove_ServerClick" id="CmdYesApprove">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-approve').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>
         <div class="modal fade" id="modal-reject">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to Commision ID :&nbsp;</h6>
                        <label id="LblCommisionIDReject" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtCommisionIDReject" runat="server" />
                        <br><br>
                        <label>Remark</label>
                        <input type="text" id="txtCommisionRejectRemark" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-reject').modal('hide');" onserverclick="CmdYesReject_ServerClick" id="CmdYesReject">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-reject').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function CheckNbsp(sbuff) {
            var sOut;
            if (sbuff == "&nbsp;") {
                sOut = "";
            }
            else {
                sOut = sbuff;
            }
            return sOut;
        }
        function confirmReject(sCommisionID) {
            if (sCommisionID != '') {
                document.getElementById('ContentPlaceHolder1_LblCommisionIDReject').innerHTML = sCommisionID;
                document.getElementById('ContentPlaceHolder1_txtCommisionIDReject').value = sCommisionID;
                $("#modal-reject").modal('show');
            }
        }
        function confirmApprove(sCommisionID,sCommisionAmount,sCommisionRefOs) {
            if (sCommisionID != '') {
                document.getElementById('ContentPlaceHolder1_LblCommisionIDApprove').innerHTML = sCommisionID;
                document.getElementById('ContentPlaceHolder1_txtCommisionIDApprove').value = sCommisionID;
                $("#modal-approve").modal('show');
            }
        }
        function endRequest(sender, args) {
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
                $('#modal-messagebox').modal('show');
            }
        }
        endRequest();
    </script>
</asp:Content>
