<%@ Page Title="Notification Alert Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_notif_alert.aspx.cs" Inherits="vtsadm.dashboard_notif_alert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <section class="content-header">
    <h1>Dashboard
      <small>Notification Alert</small>
    </h1>
    <ol class="breadcrumb">
      <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i> Dashboard</a></li>
      <li class="active"><i class="fa fa-bell"></i> Notification Alert</li>
    </ol>
  </section>

  <section class="content">
    <div class="row">
      <div class="col-xs-12">
        <div class="box box-solid box-primary">
          <div class="box-header with-border">
            <h3 class="box-title"><i class="fa fa-bell-o"></i> Notification Summary</h3>
          </div>
          <div class="box-body">
            <asp:Literal ID="litStatus" runat="server" />
            <div class="row" id="notifCards">
              <asp:Repeater ID="rptSummary" runat="server">
                <ItemTemplate>
                  <div class="col-md-4 col-sm-6 col-xs-12">
                    <div class="small-box bg-aqua">
                      <div class="inner">
                        <h3><%# Eval("Total", "{0:N0}") %></h3>
                        <p><%# Eval("DisplayName") %></p>
                      </div>
                      <div class="icon"><i class="fa fa-database"></i></div>
                      <asp:HyperLink ID="lnkSummaryDetail"
                                     runat="server"
                                     NavigateUrl='<%# GetDetailUrl(Eval("CollectionName")) %>'
                                     CssClass="small-box-footer">
                        Lihat Top 10 Customer <i class="fa fa-arrow-circle-right"></i>
                      </asp:HyperLink>
                    </div>
                  </div>
                </ItemTemplate>
              </asp:Repeater>
            </div>
            <div class="table-responsive">
              <asp:GridView ID="gvNotificationSummary"
                            runat="server"
                            AutoGenerateColumns="false"
                            CssClass="table table-striped table-bordered table-hover"
                            EmptyDataText="Tidak ada data"
                            GridLines="None">
                <Columns>
                  <asp:BoundField DataField="DisplayName" HeaderText="Tipe Notifikasi" />
                  <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Right" />
                  </asp:BoundField>
                  <asp:TemplateField HeaderText="Detail">
                    <ItemTemplate>
                      <asp:HyperLink ID="lnkGridDetail"
                                     runat="server"
                                     NavigateUrl='<%# GetDetailUrl(Eval("CollectionName")) %>'
                                     CssClass="btn btn-xs btn-primary">
                        Top 10 Customer
                      </asp:HyperLink>
                    </ItemTemplate>
                  </asp:TemplateField>
                </Columns>
              </asp:GridView>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</asp:Content>

