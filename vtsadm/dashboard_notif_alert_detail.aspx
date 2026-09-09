<%@ Page Title="Notification Alert Detail" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_notif_alert_detail.aspx.cs" Inherits="vtsadm.dashboard_notif_alert_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <section class="content-header">
    <h1>Dashboard
      <small>Notification Alert Detail</small>
    </h1>
    <ol class="breadcrumb">
      <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i> Dashboard</a></li>
      <li><a href="dashboard_notif_alert.aspx"><i class="fa fa-bell"></i> Notification Alert</a></li>
      <li class="active"><i class="fa fa-list"></i> Detail</li>
    </ol>
  </section>

  <section class="content">
    <div class="row">
      <div class="col-xs-12">
        <div class="box box-solid box-primary">
          <div class="box-header with-border">
            <h3 class="box-title">
              <asp:Label ID="lblTitle" runat="server" Text="Top 10 Customer" />
            </h3>
            <div class="box-tools pull-right">
              <asp:HyperLink ID="lnkBack"
                             runat="server"
                             CssClass="btn btn-default btn-sm"
                             NavigateUrl="dashboard_notif_alert.aspx">
                <i class="fa fa-arrow-left"></i> Kembali
              </asp:HyperLink>
            </div>
          </div>
          <div class="box-body">
            <asp:Literal ID="litStatus" runat="server" />
            <asp:GridView ID="gvDetail"
                          runat="server"
                          AutoGenerateColumns="false"
                          CssClass="table table-striped table-bordered table-hover"
                          EmptyDataText="Tidak ada data"
                          GridLines="None">
              <Columns>
                <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:N0}">
                  <ItemStyle HorizontalAlign="Right" />
                  <HeaderStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Detail">
                  <ItemTemplate>
                    <asp:HyperLink ID="lnkEntryDetail"
                                   runat="server"
                                   CssClass="btn btn-xs btn-info"
                                   NavigateUrl='<%# GetEntryUrl(Eval("CustomerName")) %>'>
                      Lihat Notifikasi
                    </asp:HyperLink>
                  </ItemTemplate>
                </asp:TemplateField>
              </Columns>
            </asp:GridView>
          </div>
        </div>
      </div>
    </div>
  </section>
</asp:Content>

