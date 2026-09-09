<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_job_new.aspx.cs" Inherits="vtsadm.rpt_job_new" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Job Order - New 
                <small>Report</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Report</a></li>
            <li><a href="#">Job Order</a></li>
            <li class="active">New</li>
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
                            <asp:DropDownList ID="CmbSearchBy" runat="server" CssClass="form-control">
                                <asp:ListItem Value="ALL" Selected="True">All Fields</asp:ListItem>
                                <asp:ListItem Value="SN">SN (Serial Number)</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Search</label>
                            <asp:TextBox ID="txtSearch" runat="server" class="form-control" placeholder="Search by any fields ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                             <label>Group Area Name</label>
                             <asp:DropDownList ID="CmbGroupAreaID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbGroupAreaID_TextChanged"></asp:DropDownList>
                         </div>
                        <div class="form-group form-group-sm">
                            <label>Area</label>
                            <asp:DropDownList ID="CmbAreaID" runat="server" CssClass="form-control"></asp:DropDownList>
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
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClientClick="showOverlay();" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Job Order - New</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>

                                        <%--<asp:BoundField DataField="JobID" HeaderText="Job ID" ItemStyle-Wrap="false" SortExpression="JobID"></asp:BoundField>
                                        <asp:BoundField DataField="sRegDate" HeaderText="Register Date" ItemStyle-Wrap="false" SortExpression="sRegDate"></asp:BoundField>
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="CustomerName"></asp:BoundField>
                                        <asp:BoundField DataField="MarketingName" HeaderText="Marketing" ItemStyle-Wrap="false" SortExpression="MarketingName"></asp:BoundField>
                                        <asp:BoundField DataField="PONumber" HeaderText="PO Number" ItemStyle-Wrap="false" SortExpression="PoNumber"></asp:BoundField>
                                        <asp:BoundField DataField="sSchDate" HeaderText="Schedule Date" ItemStyle-Wrap="false" SortExpression="sSchDate"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>

                                        <asp:BoundField DataField="CustID" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Remark_Job_Header" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UsrUpd" HeaderText="User Update" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DtmUpd" HeaderText="Date Update" ItemStyle-Wrap="false"></asp:BoundField>--%>

                                        <asp:BoundField DataField="jobid" HeaderText="Job ID" ItemStyle-Wrap="false" SortExpression="jobid"></asp:BoundField>
                                        <asp:BoundField DataField="regdate" HeaderText="Register Date" ItemStyle-Wrap="false" SortExpression="regdate" DataFormatString="{0:dd/MM/yyyy HH:mm}"></asp:BoundField>
                                        <asp:BoundField DataField="customername" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="customername"></asp:BoundField>
                                        <asp:BoundField DataField="marketingname" HeaderText="Marketing" ItemStyle-Wrap="false" SortExpression="marketingname"></asp:BoundField>
                                        <asp:BoundField DataField="ponumber" HeaderText="PO Number" ItemStyle-Wrap="false" SortExpression="ponumber"></asp:BoundField>
                                        <asp:BoundField DataField="scheduledate" HeaderText="Schedule Date" ItemStyle-Wrap="false" SortExpression="scheduledate" DataFormatString="{0:dd/MM/yyyy HH:mm}"></asp:BoundField>
                                        <asp:BoundField DataField="status_mis" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="status_mis"></asp:BoundField>

                                        <asp:BoundField DataField="custid" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="remark_job_header" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="user_mis" HeaderText="User Update" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="tgl_mis" HeaderText="Date Update" ItemStyle-Wrap="false" DataFormatString="{0:dd/MM/yyyy HH:mm}"></asp:BoundField>

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
