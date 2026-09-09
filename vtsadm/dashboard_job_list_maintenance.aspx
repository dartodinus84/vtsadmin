<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_job_list_maintenance.aspx.cs" Inherits="vtsadm.dashboard_job_list_maintenance" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>
            <asp:Label ID="lblHeaderTitle" runat="server" Text="Maintenance - All"></asp:Label>
            <small>List</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="dashboard_job.aspx">Dashboard - Job Order</a></li>
            <li class="active">Maintenance List</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Filter Context</h3>
                    </div>
                    <div class="box-body">
                        <p style="margin-bottom: 6px;"><strong><asp:Label ID="lblContextStatus" runat="server" Text="Maintenance - All"></asp:Label></strong></p>
                        <p style="margin-bottom: 6px;">Region: <asp:Label ID="lblContextRegion" runat="server" Text="All"></asp:Label></p>
                        <p style="margin-bottom: 0;">Date: <asp:Label ID="lblContextDate" runat="server" Text="-"></asp:Label></p>
                    </div>
                </div>

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Job Order - Maintenance</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Button ID="btnExport" runat="server" Text="Export Excel" OnClick="btnExport_Click" CssClass="btn btn-success" />
                        </div>
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="gvMaintenance" runat="server" BackColor="WhiteSmoke" AllowSorting="false" Font-Size="Small"
                                    CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False"
                                    CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px"
                                    AllowPaging="true" PageSize="10" OnPageIndexChanging="gvMaintenance_PageIndexChanging">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="JobID" HeaderText="Job ID" />
                                        <asp:BoundField DataField="sRegDate" HeaderText="Register Date" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                        <asp:BoundField DataField="MarketingName" HeaderText="Marketing" />
                                        <asp:BoundField DataField="sSchDate" HeaderText="Schedule Date" />
                                        <asp:BoundField DataField="CloseDate" HeaderText="Close Date" />
                                        <asp:BoundField DataField="sla" HeaderText="SLA" />
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                        <asp:BoundField DataField="BillAbleDesc" HeaderText="Billable" />
                                        <asp:BoundField DataField="IsMigrationDesc" HeaderText="Migration" />
                                        <asp:BoundField DataField="RegionalName" HeaderText="Regional Name" />
                                        <asp:BoundField DataField="AreaName" HeaderText="Area Name" />
                                        <asp:BoundField DataField="AreaGroupName" HeaderText="Area Group Name" />
                                        <asp:BoundField DataField="ValueStatus" HeaderText="Status" />
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="lblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
