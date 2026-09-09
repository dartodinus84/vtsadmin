<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_customer_detail.aspx.cs" Inherits="vtsadm.dashboard_customer_detail" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Customer
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="dashboard_customer">Dashboard - Customer</a></li>
            <li class="active">New - Open</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Customer</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="CustID" HeaderText="CustID" ItemStyle-Wrap="false" SortExpression="CustID"></asp:BoundField>
                                        <asp:BoundField DataField="FullName" HeaderText="FullName" ItemStyle-Wrap="false" SortExpression="FullName"></asp:BoundField>
                                        <asp:BoundField DataField="CustTypeID" HeaderText="CustTypeID" ItemStyle-Wrap="false" SortExpression="CustTypeID"></asp:BoundField>
                                        <asp:BoundField DataField="SoftBlock" HeaderText="SoftBlock" ItemStyle-Wrap="false" SortExpression="SoftBlock"></asp:BoundField>
                                        <asp:BoundField DataField="CustStatus" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="CustStatus"></asp:BoundField>
                                        <asp:BoundField DataField="BranchName" HeaderText="BranchName" ItemStyle-Wrap="false" SortExpression="BranchName"></asp:BoundField>
                                        <asp:BoundField DataField="MarketingName" HeaderText="MarketingName" ItemStyle-Wrap="false" SortExpression="MarketingName"></asp:BoundField>
                                        <asp:BoundField DataField="total_unit" HeaderText="total_unit" ItemStyle-Wrap="false" SortExpression="total_unit"></asp:BoundField>
                                        <asp:BoundField DataField="unit_active" HeaderText="unit_active" ItemStyle-Wrap="false" SortExpression="unit_active"></asp:BoundField>
                                        <asp:BoundField DataField="unit_suspend" HeaderText="unit_suspend" ItemStyle-Wrap="false" SortExpression="unit_suspend"></asp:BoundField>
                                        <asp:BoundField DataField="visit_it" HeaderText="visit_it" ItemStyle-Wrap="false" SortExpression="visit_it"></asp:BoundField>
                                        <asp:BoundField DataField="visit_technician" HeaderText="visit_technician" ItemStyle-Wrap="false" SortExpression="visit_technician"></asp:BoundField>
                                        <asp:BoundField DataField="PICName1" HeaderText="PICName1" ItemStyle-Wrap="false" SortExpression="PICName1"></asp:BoundField>
                                        <asp:BoundField DataField="PICPosition1" HeaderText="PICPosition1" ItemStyle-Wrap="false" SortExpression="PICPosition1"></asp:BoundField>
                                        <asp:BoundField DataField="OfficePhone1" HeaderText="OfficePhone1" ItemStyle-Wrap="false" SortExpression="OfficePhone1"></asp:BoundField>
                                        <asp:BoundField DataField="MobilePhone1" HeaderText="MobilePhone1" ItemStyle-Wrap="false" SortExpression="MobilePhone1"></asp:BoundField>
                                        <asp:BoundField DataField="Email1" HeaderText="Email1" ItemStyle-Wrap="false" SortExpression="Email1"></asp:BoundField>
                                        <asp:BoundField DataField="BusinessFields" HeaderText="BusinessFields" ItemStyle-Wrap="false" SortExpression="BusinessFields"></asp:BoundField>
                                        <asp:BoundField DataField="OperationalArea" HeaderText="OperationalArea" ItemStyle-Wrap="false" SortExpression="OperationalArea"></asp:BoundField>
                                        <asp:BoundField DataField="ITS" HeaderText="IT Support" ItemStyle-Wrap="false" SortExpression="ITS"></asp:BoundField>
                                        <asp:BoundField DataField="ITOutbound" HeaderText="IT Outbound" ItemStyle-Wrap="false" SortExpression="ITOutbound"></asp:BoundField>
                                        <asp:BoundField DataField="NPWP" HeaderText="NPWP" ItemStyle-Wrap="false" SortExpression="NPWP"></asp:BoundField>
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

