<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_job_monitoring_assign.aspx.cs" Inherits="vtsadm.dashboard_job_monitoring_assign" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .icon {
            top: 0px !important;
            font-size: 40px !important;
        }

        .inner {
            height: 80px !important;
            padding-top: 2px !important;
        }



        .small-box-footer {
            border-bottom-left-radius: 11px;
            border-bottom-right-radius: 11px;
        }

        .small-box {
            border-radius: 11px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .box {
            border-radius: 11px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .widget-user-2 .widget-user-header {
            border-top-left-radius: 11px;
            border-top-right-radius: 11px;
            height: 60px;
            padding-top: 10px;
        }

        .box-header {
            font-size: 16px;
        }

        li-format {
            height: 45px !important;
        }
    </style>

    <section class="content-header">
        <h1>Dashboard
        <small>Job Order</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="#"><i class="fa fa-dashboard"></i>Job Order</a></li>
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
                            <label>Filter Type</label>
                            <asp:DropDownList ID="CmbFilterType" runat="server" CssClass="form-control"></asp:DropDownList>
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
            </div>
        </div>

        <div class="row">
            <div class="col-lg-4 col-xs-6">
                <div class="small-box bg-maroon-gradient">
                    <div class="inner">
                        <h3>
                            <label id="LblCntJoOpen" runat="server">0</label>
                        </h3>
                        <p>JO - OPEN</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-wrench"></i>
                    </div>
                    <!--<a href="view_job_assign.aspx" class="small-box-footer"> &nbsp;</a>-->
                </div>
            </div>

             <div class="col-lg-4 col-xs-6">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <h3>
                            <label id="LblCntAllocation" runat="server">0</label>
                        </h3>
                        <p>JO - ALLOCATION</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-user"></i>
                    </div>
                    <!--<a href="view_job_assign_allocation.aspx" class="small-box-footer"> &nbsp;</a>-->
                </div>
            </div>

            <div class="col-lg-4 col-xs-6">
                <div class="small-box bg-yellow-gradient">
                    <div class="inner">
                        <h3>
                            <label id="LblCntNotAllocation" runat="server">0</label>
                        </h3>
                        <p>JO - NOT ALLOCATION</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-user"></i>
                    </div>
                    <!--<a href="view_job_assign_notallocation.aspx" class="small-box-footer"> &nbsp;</a>-->

                </div>
            </div>
            
        </div>
        <div class="row">


            <div class="col-lg-6 col-xs-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Technisi Available</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Name" ItemStyle-Wrap="false" SortExpression="DeviceTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="Total" HeaderText="Area" ItemStyle-Wrap="false" SortExpression="Total"></asp:BoundField>
                                        <asp:BoundField DataField="Registered" HeaderText="Area" ItemStyle-Wrap="false" SortExpression="Registered"></asp:BoundField>

                                        <asp:BoundField DataField="WareHouse" HeaderText="Warehouse" ItemStyle-Wrap="false" SortExpression="WareHouse"></asp:BoundField>
                                        <asp:BoundField DataField="Technician" HeaderText="Technician" ItemStyle-Wrap="false" SortExpression="Technician"></asp:BoundField>
                                        <asp:BoundField DataField="Installed" HeaderText="Installed" ItemStyle-Wrap="false" SortExpression="Installed"></asp:BoundField>
                                        <asp:BoundField DataField="Broken" HeaderText="Broken" ItemStyle-Wrap="false" SortExpression="Broken"></asp:BoundField>
                                        <asp:BoundField DataField="Deactivaded" HeaderText="Deactivated" ItemStyle-Wrap="false" SortExpression="Deactivaded"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="Label1" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="box-footer">
                        
                    </div>
                </div>
            </div>

            <div class="col-lg-6 col-xs-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Schedule</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Name" ItemStyle-Wrap="false" SortExpression="DeviceTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="Total" HeaderText="Customer" ItemStyle-Wrap="false" SortExpression="Total"></asp:BoundField>
                                        <asp:BoundField DataField="Registered" HeaderText="JO" ItemStyle-Wrap="false" SortExpression="Registered"></asp:BoundField>
                                        <asp:BoundField DataField="WareHouse" HeaderText="Schedule" ItemStyle-Wrap="false" SortExpression="WareHouse"></asp:BoundField>
                                        <asp:BoundField DataField="Technician" HeaderText="Area" ItemStyle-Wrap="false" SortExpression="Technician"></asp:BoundField>
                                        <asp:BoundField DataField="Installed" HeaderText="Installed" ItemStyle-Wrap="false" SortExpression="Installed"></asp:BoundField>
                                        <asp:BoundField DataField="Broken" HeaderText="Broken" ItemStyle-Wrap="false" SortExpression="Broken"></asp:BoundField>
                                        <asp:BoundField DataField="Deactivaded" HeaderText="Deactivated" ItemStyle-Wrap="false" SortExpression="Deactivaded"></asp:BoundField>
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

    <script src="https://www.amcharts.com/lib/4/core.js"></script>
    <script src="https://www.amcharts.com/lib/4/charts.js"></script>
    <script src="https://www.amcharts.com/lib/4/themes/animated.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
           
        });

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

