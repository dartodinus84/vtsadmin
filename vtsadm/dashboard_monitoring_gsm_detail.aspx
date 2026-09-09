<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_monitoring_gsm_detail.aspx.cs" Inherits="vtsadm.dashboard_monitoring_gsm_detail" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>GSM
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="dashboard_monitoring_gsm">Dashboard - Monitoring GSM</a></li>
            <li class="active">List</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List GSM</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="JobID" HeaderText="JobID" ItemStyle-Wrap="false" SortExpression="JobID"></asp:BoundField>
                                        <asp:BoundField DataField="fullname" HeaderText="Customer" ItemStyle-Wrap="false" SortExpression="fullname"></asp:BoundField>
                                        <asp:BoundField DataField="branchname" HeaderText="Branch" ItemStyle-Wrap="false" SortExpression="branchname"></asp:BoundField>
                                        <asp:BoundField DataField="AreaGroupName" HeaderText="Group Area" ItemStyle-Wrap="false" SortExpression="AreaGroupName"></asp:BoundField>
                                        <asp:BoundField DataField="AreaName" HeaderText="Area" ItemStyle-Wrap="false" SortExpression="AreaName"></asp:BoundField>
                                        <asp:BoundField DataField="policeno" HeaderText="Police No" ItemStyle-Wrap="false" SortExpression="policeno"></asp:BoundField>
                                        <asp:BoundField DataField="nosn" HeaderText="SN No" ItemStyle-Wrap="false" SortExpression="nosn"></asp:BoundField>
                                        <asp:BoundField DataField="msidn" HeaderText="Gsm No" ItemStyle-Wrap="false" SortExpression="msidn"></asp:BoundField>
                                        <asp:BoundField DataField="Type" HeaderText="Gsm Type" ItemStyle-Wrap="false" SortExpression="Type"></asp:BoundField>
                                        <asp:BoundField DataField="status_gsm" HeaderText="Gsm Status" ItemStyle-Wrap="false" SortExpression="status_gsm"></asp:BoundField>
                                        <asp:BoundField DataField="InstallDate" HeaderText="Install Date" ItemStyle-Wrap="false" SortExpression="InstallDate"></asp:BoundField>
                                        <asp:BoundField DataField="DurasiHari" HeaderText="Day" ItemStyle-Wrap="false" SortExpression="DurasiHari"></asp:BoundField>
                                        <asp:BoundField DataField="UsrUpd" HeaderText="UsrUpd" ItemStyle-Wrap="false" SortExpression="UsrUpd"></asp:BoundField>
                                        <asp:BoundField DataField="DtmUpd" HeaderText="DtmUpd" ItemStyle-Wrap="false" SortExpression="DtmUpd"></asp:BoundField>

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
                        <asp:Button ID="CmdExport" CssClass="btn btn-primary" runat="server" OnClick="CmdExport_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportXls_Click" Text="Export XLS" />
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


