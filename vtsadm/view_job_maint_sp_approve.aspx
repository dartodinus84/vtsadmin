<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="view_job_maint_sp_approve.aspx.cs" Inherits="vtsadm.view_job_maint_sp_approve" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Job Order Maintenance           
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">View</a></li>
            <li><a href="#">Job Order Maintenance -  Suspend</a></li>
            <li class="active">Approve</li>
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
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Job Order Maintenance -  Suspend</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="JobID" HeaderText="Job ID" ItemStyle-Wrap="false" SortExpression="JobID"></asp:BoundField>
                                        <asp:BoundField DataField="SchDate" HeaderText="Schedule Date" ItemStyle-Wrap="false" SortExpression="SchDate"></asp:BoundField>
                                        <asp:BoundField DataField="Fullname" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="Fullname"></asp:BoundField>
                                        <asp:BoundField DataField="MaintTypeDesc" HeaderText="Desc Maintenance" ItemStyle-Wrap="false" SortExpression="MaintTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="policeno" HeaderText="Car Plate" ItemStyle-Wrap="false" SortExpression="policeno"></asp:BoundField>
                                        <asp:BoundField DataField="nosn" HeaderText="GPS SN" ItemStyle-Wrap="false" SortExpression="nosn"></asp:BoundField>
                                        <asp:BoundField DataField="msidn" HeaderText="GSM SN" ItemStyle-Wrap="false" SortExpression="msidn"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdApprove" runat="server" Text="<i class='fa fa-check'></i>" ToolTip="Cancel" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Cancel" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TvdID" HeaderText="Customer ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Seq" HeaderText="Customer ID" ItemStyle-Wrap="false"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
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
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <div class="modal fade" id="modal-delete">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to reject Job Order Maintenance ID :&nbsp;</h6>
                        <label id="LblJoIDDelete" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtJoIDDelete" runat="server" />
                        <input type="hidden" id="txtTvdIDDelete" runat="server" />
                        <input type="hidden" id="txtSeqIDDelete" runat="server" />
                        <div class="form-group form-group-sm">
                            <label>Decline Reason</label>
                            <textarea id="txtDecline" runat="server" class="form-control" placeholder="Decline Reason ..." rows="2"></textarea>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
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
                        <h6 class="modal-title">Are you sure to Job Order Maintenance ID :&nbsp;</h6>
                        <label id="LblJoIDApprove" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtJoIDApprove" runat="server" />
                        <input type="hidden" id="txtTvdIDApprove" runat="server" />
                        <input type="hidden" id="txtSeqIDApprove" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-approve').modal('hide');" onserverclick="CmdYesApprove_ServerClick" id="CmdYesApprove">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-approve').modal('hide');">No</button>
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
        function confirmDelete(sJoID, sTvdID,sSeq) {
            if (sJoID != '') {
                document.getElementById('ContentPlaceHolder1_LblJoIDDelete').innerHTML = sJoID;
                document.getElementById('ContentPlaceHolder1_txtJoIDDelete').value = sJoID;
                document.getElementById('ContentPlaceHolder1_txtTvdIDDelete').value = sTvdID;
                document.getElementById('ContentPlaceHolder1_txtSeqIDDelete').value = sSeq;

                $("#modal-delete").modal('show');
            }
        }
        function confirmApprove(sJoID, sTvdID,sSeq) {
            if (sJoID != '') {
                document.getElementById('ContentPlaceHolder1_LblJoIDApprove').innerHTML = sJoID;
                document.getElementById('ContentPlaceHolder1_txtJoIDApprove').value = sJoID;
                document.getElementById('ContentPlaceHolder1_txtTvdIDApprove').value = sTvdID;
                document.getElementById('ContentPlaceHolder1_txtSeqIDApprove').value = sSeq;
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
