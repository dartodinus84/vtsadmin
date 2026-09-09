<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_job_maint_sbs.aspx.cs" Inherits="vtsadm.rpt_job_maint_sbs" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Job Order - Maintenance - Soft Blocked
                <small>Report</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Report</a></li>
            <li><a href="#">Job Order</a></li>
            <li><a href="#">Maintenance</a></li>
            <li class="active">Soft Blocked</li>
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
                          <div class="form-group form-group-sm">
                            <label>Device Type</label>
                            <asp:DropDownList ID="CmbDeviceType" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Branch</label>
                            <asp:DropDownList ID="CmbBranch" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Job Order - Maintenance</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <%--<asp:BoundField DataField="JobID" HeaderText="Job ID" ItemStyle-Wrap="false" SortExpression="JobID"></asp:BoundField>
                                        <asp:BoundField DataField="sRegDate" HeaderText="Register Date" ItemStyle-Wrap="false" SortExpression="sRegDate"></asp:BoundField>
                                        <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="CustomerName"></asp:BoundField>
                                        <asp:BoundField DataField="sSchDate" HeaderText="Schedule Date" ItemStyle-Wrap="false" SortExpression="sSchDate"></asp:BoundField>
                                        <asp:BoundField DataField="Status_jo_header" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>

                                        <asp:BoundField DataField="CustID" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Remark_Jo_Header" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UsrUpd_jo_header" HeaderText="User Update" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DtmUpd_jo_header" HeaderText="Date Update" ItemStyle-Wrap="false"></asp:BoundField>--%>


                                        <asp:BoundField DataField="customer_name" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="customer_name"></asp:BoundField>
                                        <asp:BoundField DataField="dtumpd_jo_header" HeaderText="Date" ItemStyle-Wrap="false" SortExpression="dtumpd_jo_header"></asp:BoundField>
                                        <asp:BoundField DataField="ScheduleDate" HeaderText="Schedule Date" ItemStyle-Wrap="false" SortExpression="ScheduleDate"></asp:BoundField>
                                        <asp:BoundField DataField="policeno" HeaderText="Car Plate" ItemStyle-Wrap="false" SortExpression="PoliceNo"></asp:BoundField>
                                        <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="false" SortExpression="NoSN"></asp:BoundField>
                                        <asp:BoundField DataField="MSIDN" HeaderText="No GSM" ItemStyle-Wrap="false" SortExpression="MSIDN"></asp:BoundField>
                                        <asp:BoundField DataField="technician_name" HeaderText="Technician Name" ItemStyle-Wrap="false" SortExpression="technician_name"></asp:BoundField>
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Wrap="false" SortExpression="Remark"></asp:BoundField>
                                        <asp:BoundField DataField="ServerName" HeaderText="Server" ItemStyle-Wrap="false" SortExpression="ServerName"></asp:BoundField>
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
