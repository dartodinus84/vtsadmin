<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="report_stock_opname.aspx.cs" Inherits="vtsadm.report_stock_opname" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Stock Opname           
                <small>Report</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Report</a></li>
            <li class="active">Stock Opname</li>
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
                            <label>Location Type</label>
                            <asp:DropDownList ID="CmbLocationType" runat="server" CssClass="form-control">
                                <asp:ListItem Text="[All]" Value=""></asp:ListItem>
                                <asp:ListItem Text="Warehouse" Value="WAREHOUSE"></asp:ListItem>
                                <asp:ListItem Text="Technician" Value="TECHNICIAN"></asp:ListItem>
                            </asp:DropDownList>
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
                        <asp:Button ID="CmdClear" CssClass="btn btn-default" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Stock Opname Report</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="opname_id" HeaderText="Opname ID" ItemStyle-Wrap="false" SortExpression="opname_id"></asp:BoundField>
                                        <asp:BoundField DataField="opname_code" HeaderText="Opname Code" ItemStyle-Wrap="false" SortExpression="opname_code"></asp:BoundField>
                                        <asp:BoundField DataField="opname_date" HeaderText="Opname Date" ItemStyle-Wrap="false" SortExpression="opname_date" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="location_type" HeaderText="Location Type" ItemStyle-Wrap="false" SortExpression="location_type"></asp:BoundField>
                                        <asp:BoundField DataField="location_name" HeaderText="Location Name" ItemStyle-Wrap="false" SortExpression="location_name"></asp:BoundField>
                                        <asp:BoundField DataField="total_device" HeaderText="Total Device" ItemStyle-Wrap="false" SortExpression="total_device"></asp:BoundField>
                                        <asp:BoundField DataField="not_found" HeaderText="Not Found" ItemStyle-Wrap="false" SortExpression="not_found"></asp:BoundField>
                                        <asp:BoundField DataField="mismatch_status" HeaderText="Mismatch" ItemStyle-Wrap="false" SortExpression="mismatch_status"></asp:BoundField>
                                        <asp:BoundField DataField="accuracy_percentage" HeaderText="Accuracy %" ItemStyle-Wrap="false" SortExpression="accuracy_percentage" DataFormatString="{0:0.0}%"></asp:BoundField>
                                        <asp:BoundField DataField="created_by" HeaderText="Created By" ItemStyle-Wrap="false" SortExpression="created_by"></asp:BoundField>
                                        <asp:BoundField DataField="created_at" HeaderText="Created At" ItemStyle-Wrap="false" SortExpression="created_at" DataFormatString="{0:dd/MM/yyyy HH:mm}"></asp:BoundField>
                                        <asp:BoundField DataField="remark" HeaderText="Remark" Visible="false"></asp:BoundField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDetails" runat="server" Text="<i class='fa fa-list-alt'></i>" ToolTip="View Details" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="5" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
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
        
        <div class="modal fade bs-example-modal-lg" id="modal-details">
            <div class="modal-dialog modal-lg" style="width: 90%; max-width: 1200px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Stock Opname Details</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframedetails" src="about:blank" style="width: 100%; border: none; height: 500px;" scrolling="auto"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
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

        function postDetails(sOpnameId) {
            if (sOpnameId != '') {
                try {
                    // Reset iframe dan tampilkan loading
                    var iframe = document.getElementById('iframedetails');
                    iframe.src = 'about:blank';
            
                    // Tampilkan modal
                    $('#modal-details').modal('show');
            
                    // Tambahkan timestamp untuk menghindari cache
                    var timestamp = new Date().getTime();
                    var url = 'report_stock_opname_detail.aspx?opname_id=' + sOpnameId + '&t=' + timestamp;
            
                    // Set timeout untuk memastikan modal sudah tampil
                    setTimeout(function() {
                        iframe.src = url;
                
                        // Tangani error loading
                        iframe.onerror = function() {
                            console.error('Error loading iframe');
                            alert('Error loading detail page. Please try again.');
                        };
                    }, 300);
                } catch (e) {
                    console.error('Error showing details:', e);
                    alert('Error showing details. Please try again.');
                }
            }
        }

        function endRequest(sender, args) {
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                $('#modal-messagebox').modal('show');
            }
        }
        endRequest();
    </script>
</asp:Content>