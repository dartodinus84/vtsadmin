<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="gsm_maint_suspend_gsm_search.aspx.cs" Inherits="vtsadm.gsm_maint_suspend_gsm_search" %>

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
    <!-- AdminLTE Skins. Choose a skin from the css/skins
       folder instead of downloading all of them to reduce the load. -->
    <link rel="stylesheet" href="Content/dist/css/skins/_all-skins.min.css" />
    <link rel="stylesheet" href="Content/paginationcs.css" />

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->
    <!-- Google Font -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic" />
    <style type="text/css">
        .example-modal .modal {
            position: relative;
            top: auto;
            bottom: auto;
            right: auto;
            left: auto;
            display: block;
            z-index: 1;
        }

        .example-modal .modal {
            background: transparent !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-group form-group-sm">
            <div class="input-group input-group-sm">
                <input type="hidden" id="txtJobID" runat="server" />
                <input type="hidden" id="txtCustID" runat="server" />
                <input type="text" id="txtSearchGsm" runat="server" class="form-control" placeholder="Search By MSIDN ..." />
                <span class="input-group-btn">
                    <button id="CmdSearchGsm" runat="server" type="button" class="btn btn-block btn-primary btn-xs" onserverclick="CmdSearchGsm_Click"><i class="fa fa-search"></i></button>
                </span>
            </div>
        </div>
        <div class="form-group form-group-sm">
            <asp:Panel runat="server" CssClass="" ScrollBars="Auto" Width="100%">
                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging">
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="GsmID" HeaderText="Gsm ID" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="MSIDN" HeaderText="MSIDN" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="providername" HeaderText="Provider Name" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="GsmSourceName" HeaderText="Source Name" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="PoliceNo" HeaderText="Police No" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="CmdSelect" runat="server" ToolTip="Select" Text="<i class='fa fa-share'></i>" Enabled="true" CssClass="btn btn-success btn-xs" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TgtID" HeaderText="Tgt ID" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="VehicleID" HeaderText="Vehicle ID" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="VehicleDesc" HeaderText="Vehicle Desc" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="AssetNo" HeaderText="Asset No" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="TvaID" HeaderText="Tva ID" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="BranchName" HeaderText="Branch Name" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="FullName" HeaderText="Customer Name" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="technicianid" HeaderText="Technician ID" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="employeeno" HeaderText="Employee No" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="employeename" HeaderText="Technician Name" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="branchname_technician" HeaderText="Branch Name Technician" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="DeviceID" HeaderText="Device ID" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="vendor_name" HeaderText="Vendor Name" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="devicetypedesc" HeaderText="Device Type Desc" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="DeviceSourceName" HeaderText="Source Name" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="warehousename_device" HeaderText="Warehouse Name" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="TdtID" HeaderText="Tdt ID" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="InstalDate" HeaderText="Install Date" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:BoundField DataField="TvdID" HeaderText="Tvd ID" ItemStyle-Wrap="false"></asp:BoundField>
                    </Columns>
                    <RowStyle ForeColor="#003481" BackColor="White" />
                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                    <PagerStyle ForeColor="#003481" CssClass="pagination-ys" HorizontalAlign="Left" BorderColor="White" />
                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                </asp:GridView>
                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;"><asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>

