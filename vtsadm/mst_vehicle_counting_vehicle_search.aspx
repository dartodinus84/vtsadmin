<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mst_vehicle_counting_vehicle_search.aspx.cs" Inherits="vtsadm.mst_vehicle_counting_vehicle_search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <!-- Bootstrap 3.3.7 -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="Content/bower_components/font-awesome/css/font-awesome.min.css" />
    <!-- Ionicons -->
    <link rel="stylesheet" href="Content/bower_components/Ionicons/css/ionicons.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="Content/dist/css/AdminLTE.min.css" />
    <!-- AdminLTE Skins -->
    <link rel="stylesheet" href="Content/dist/css/skins/_all-skins.min.css" />
    <link rel="stylesheet" href="Content/paginationcs.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-group form-group-sm">
            <div class="input-group input-group-sm">
                <input type="text" id="txtSearchVehicle" runat="server" class="form-control" placeholder="Search by Vehicle ID, Nopol or GPS SN ..." />
                <span class="input-group-btn">
                    <button id="CmdSearchVehicle" runat="server" type="button" class="btn btn-block btn-primary btn-xs" onserverclick="CmdSearchVehicle_Click"><i class="fa fa-search"></i></button>
                </span>
            </div>
        </div>
        <div class="form-group form-group-sm">
            <asp:Panel runat="server" CssClass="" ScrollBars="Auto" Width="100%">
                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging">
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="vehicle_id" HeaderText="Vehicle ID" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="car_plate" HeaderText="Nopol" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="gps_sn" HeaderText="GPS SN" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="CmdSelect" runat="server" ToolTip="Select" Text="<i class='fa fa-share'></i>" Enabled="true" CssClass="btn btn-success btn-xs" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle ForeColor="#003481" BackColor="White" />
                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                    <PagerStyle ForeColor="#003481" CssClass="pagination-ys" HorizontalAlign="Left" BorderColor="White" />
                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                </asp:GridView>
                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                    <asp:Label ID="LblPagingVehicle" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>

