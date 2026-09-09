<%@ Page Title="Notification Alert Entry Detail" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_notif_alert_detail_item.aspx.cs" Inherits="vtsadm.dashboard_notif_alert_detail_item" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <section class="content-header">
    <h1>Dashboard
      <small>Notification Alert Entry Detail</small>
    </h1>
    <ol class="breadcrumb">
      <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i> Dashboard</a></li>
      <li><a href="dashboard_notif_alert.aspx"><i class="fa fa-bell"></i> Notification Alert</a></li>
      <li><a id="lnkBreadcrumbParent" runat="server" href="dashboard_notif_alert_detail.aspx"><i class="fa fa-list"></i> Top 10 Customer</a></li>
      <li class="active"><i class="fa fa-table"></i> Detail</li>
    </ol>
  </section>

  <section class="content">
    <div class="row">
      <div class="col-xs-12">
        <div class="box box-solid box-primary">
          <div class="box-header with-border">
            <h3 class="box-title">
              <asp:Label ID="lblTitle" runat="server" Text="Detail Notifikasi" />
            </h3>
            <div class="box-tools pull-right">
              <asp:HyperLink ID="lnkBack"
                             runat="server"
                             CssClass="btn btn-default btn-sm">
                <i class="fa fa-arrow-left"></i> Kembali
              </asp:HyperLink>
            </div>
          </div>
          <div class="box-body">
            <p>
              <strong>Customer:</strong>
              <asp:Label ID="lblCustomer" runat="server" Text="-" />
            </p>
            <asp:Literal ID="litStatus" runat="server" />
            <asp:GridView ID="gvEntries"
                          runat="server"
                          AutoGenerateColumns="false"
                          CssClass="table table-striped table-bordered table-hover"
                          EmptyDataText="Tidak ada data"
                          GridLines="None">
              <Columns>
                <asp:BoundField DataField="GpsSn" HeaderText="GPS SN" />
                <asp:BoundField DataField="Nopol" HeaderText="No. Polisi" />
                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:N0}">
                  <ItemStyle HorizontalAlign="Right" />
                  <HeaderStyle HorizontalAlign="Right" />
                </asp:BoundField>
              </Columns>
            </asp:GridView>
          </div>
        </div>
      </div>
    </div>
  </section>
</asp:Content>


