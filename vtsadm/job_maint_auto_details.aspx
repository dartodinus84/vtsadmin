<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="job_maint_auto_details.aspx.cs" Inherits="vtsadm.job_maint_auto_details" %>

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
    <!-- daterange picker -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap-daterangepicker/daterangepicker.css" />
    <!-- bootstrap datepicker -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css" />
    <!-- DataTables -->
    <link rel="stylesheet" href="Content/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css" />
    <!-- iCheck for checkboxes and radio inputs -->
    <link rel="stylesheet" href="Content/plugins/iCheck/all.css" />
    <!-- Bootstrap Color Picker -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap-colorpicker/dist/css/bootstrap-colorpicker.min.css" />
    <!-- Bootstrap time Picker -->
    <link rel="stylesheet" href="Content/plugins/timepicker/bootstrap-timepicker.min.css" />
    <!-- Select2 -->
    <link rel="stylesheet" href="Content/bower_components/select2/dist/css/select2.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="Content/dist/css/AdminLTE.min.css" />
    <!-- AdminLTE Skins. Choose a skin from the css/skins
           folder instead of downloading all of them to reduce the load. -->
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
      <![endif]-->
    <link rel="stylesheet" href="Content/dist/css/skins/_all-skins.min.css" />
    <link rel="stylesheet" href="Content/paginationcs.css" />
    <link rel="stylesheet" href="Content/loader.css" />
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

    <script type="text/javascript" src="Content/bower_components/jquery/dist/jquery.min.js"></script>
    <script type="text/javascript" src="Content/bower_components/jquery-ui/jquery-ui.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script type="text/javascript" src="Content/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="box-body">
            <div class="form-group form-group-sm">
                <div class="input-group input-group-sm">
                    <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-left" placeholder="Search by Police No ..."></asp:TextBox>
                    <span class="input-group-btn">
                        <button id="CmdSearch" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearch_ServerClick"><i class="fa fa-search"></i></button>
                    </span>
                </div>
            </div>
            <div class="form-group form-group-sm">
                <asp:Panel runat="server" ScrollBars="Auto">
                    <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="3" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDataBound="GridView2_RowDataBound">
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <Columns>
                            <asp:BoundField DataField="TvdID" HeaderText="Tvd ID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="VehicleDesc" HeaderText="Vehicle Desc" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="PoliceNo" HeaderText="Police No" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="CmdSelect" runat="server" ToolTip="Select" Text="<i class='fa fa-share'></i>" Enabled="true" CssClass="btn btn-success btn-xs" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle ForeColor="#003481" BackColor="White" />
                        <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                        <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                        <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                        <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
                        <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                    </asp:GridView>
                    <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                        <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                    </div>
                </asp:Panel>
            </div>
            <div class="form-group form-group-sm">
                <input id="txtTvdID" runat="server" type="hidden" />
                <label>Police No</label>
                <asp:TextBox ID="txtPoliceNo" runat="server" class="form-control" placeholder="Police No ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm">
                <label>Maintenance Type</label>
                <asp:DropDownList ID="CmbMaintTypeID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbMaintTypeID_TextChanged"></asp:DropDownList>
            </div>
            <div class="form-group form-group-sm">
                <label>Remark</label>
                <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark Detail ..."></asp:TextBox>
            </div>

            <%--gsm--%>
            <div class="form-group form-group-sm">
                <div class="input-group input-group-sm">
                    <asp:TextBox ID="txtNewGsmID" runat="server" class="form-control pull-left" placeholder="Search by Msisdn Name ..."></asp:TextBox>
                    <span class="input-group-btn">
                        <button id="CmdSearchGsm" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearchGsm_ServerClick"><i class="fa fa-search"></i></button>
                    </span>
                </div>
            </div>
            <div class="form-group form-group-sm" id="LblGridView3" runat="server">
                <asp:Panel runat="server" ScrollBars="Auto">
                    <asp:GridView ID="GridView3" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="3" OnPageIndexChanging="GridView3_PageIndexChanging" OnRowDataBound="GridView3_RowDataBound">
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <Columns>
                            <asp:BoundField DataField="TgtID" HeaderText="Tgt ID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="GsmID" HeaderText="Gsm ID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="MSIDN" HeaderText="MSIDN" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="datearrival" HeaderText="Date Arrival" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="ProviderName" HeaderText="Provider Name" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="SourceName" HeaderText="Source Name" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="WarehouseName" HeaderText="Warehouse Name" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="CmdSelectGsm" runat="server" ToolTip="Select" Text="<i class='fa fa-share'></i>" Enabled="true" CssClass="btn btn-success btn-xs" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle ForeColor="#003481" BackColor="White" />
                        <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                        <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                        <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                        <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
                        <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                    </asp:GridView>
                    <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                        <asp:Label ID="LblPagingGsm" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                    </div>
                </asp:Panel>
            </div>
            <div class="form-group form-group-sm" id="LblNewGSM" runat="server">
                <label>GSM ID</label>
                <asp:TextBox ID="txtNewGSM" runat="server" class="form-control" placeholder="GSMID ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblNewMSIDN" runat="server">
                <label>MSIDN</label>
                <asp:TextBox ID="txtNewMSIDN" runat="server" class="form-control" placeholder="MSIDN ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblNewProviderName" runat="server">
                <label>Provider Name</label>
                <asp:TextBox ID="txtNewProviderName" runat="server" class="form-control" placeholder="Provider Name ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblNewSourceName" runat="server">
                <label>Source Name</label>
                <asp:TextBox ID="txtNewSourceName" runat="server" class="form-control" placeholder="Source Name ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblNewNewTgtID" runat="server">
                <label>Tgt ID</label>
                <asp:TextBox ID="txtNewTgtID" runat="server" class="form-control" placeholder="Tgt ID ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblStatusOldGsm" runat="server">
                <label>Status Old Gsm</label>
                <asp:DropDownList ID="CmbStatusOldGsm" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <%--customer--%>
            <div class="form-group form-group-sm">
                <div class="input-group input-group-sm">
                    <asp:TextBox ID="txtSearchCust" runat="server" class="form-control pull-left" placeholder="Search by Customer Name ..."></asp:TextBox>
                    <span class="input-group-btn">
                        <button id="CmdSearchCust" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearchCust_ServerClick"><i class="fa fa-search"></i></button>
                    </span>
                </div>
            </div>
            <div class="form-group form-group-sm" id="LblGridView1" runat="server">
                <asp:Panel runat="server" ScrollBars="Auto">
                    <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="3" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <Columns>
                            <asp:BoundField DataField="CustID" HeaderText="Customer ID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="Fullname" HeaderText="Customer Name" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="BranchName" HeaderText="Branc hName" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="CmdSelectCust" runat="server" ToolTip="Select" Text="<i class='fa fa-share'></i>" Enabled="true" CssClass="btn btn-success btn-xs" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle ForeColor="#003481" BackColor="White" />
                        <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                        <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                        <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                        <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
                        <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                    </asp:GridView>
                    <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                        <asp:Label ID="LblPagingCust" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                    </div>
                </asp:Panel>
            </div>
            <div class="form-group form-group-sm" id="LblCustID" runat="server">
                <label>Customer ID</label>
                <asp:TextBox ID="txtNewCustID" runat="server" class="form-control" placeholder="Cust ID ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblCustName" runat="server">
                <label>Customer Name</label>
                <asp:TextBox ID="txtCustName" runat="server" class="form-control" placeholder="Customer Name ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblBranch" runat="server">
                <label>Branch Name</label>
                <asp:TextBox ID="txtBranch" runat="server" class="form-control" placeholder="Branch Name ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblServer" runat="server">
                <label>Server Desc</label>
                <asp:DropDownList ID="CmbServer" AutoPostBack="true" runat="server" CssClass="form-control" OnTextChanged="CmbCustServerID_TextChanged"></asp:DropDownList>
            </div>

            <%-- server --%>
            <div class="form-group form-group-sm" id="LblCustServerID" runat="server">
                <label>Server Name</label>
                <asp:DropDownList ID="CmbCustServerID" runat="server" AutoPostBack="true" CssClass="form-control" OnTextChanged="CmbMaintCustServerID_TextChanged"></asp:DropDownList>
            </div>

            <div class="form-group form-group-sm" id="LblGridView5" runat="server">
                <asp:Panel runat="server" ScrollBars="Auto">
                    <asp:GridView ID="GridView5" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="3" OnPageIndexChanging="GridView5_PageIndexChanging">
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <Columns>
                            <asp:BoundField DataField="autoid" HeaderText="Auto ID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="user_id" HeaderText="User ID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="user_nm" HeaderText="User Name" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Check" HeaderStyle-CssClass="gv-header-center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chk1" runat="server" Enabled="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle ForeColor="#003481" BackColor="White" />
                        <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                        <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                        <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                        <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
                        <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                    </asp:GridView>
                    <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                        <asp:Label ID="LblPagingUserAccess" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                    </div>
                </asp:Panel>
            </div>

            <%--vehicle--%>
            <div class="form-group form-group-sm">
                <div class="input-group input-group-sm">
                    <asp:TextBox ID="txtVehicleID" runat="server" class="form-control pull-left" placeholder="Search By Police No  ..."></asp:TextBox>
                    <span class="input-group-btn">
                        <button id="CmdSearchVehicle" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearchVehicle_ServerClic"><i class="fa fa-search"></i></button>
                    </span>
                </div>
            </div>
            <div class="form-group form-group-sm" id="LblGridView4" runat="server">
                <asp:Panel runat="server" ScrollBars="Auto">
                    <asp:GridView ID="GridView4" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="3" OnPageIndexChanging="GridView4_PageIndexChanging" OnRowDataBound="GridView4_RowDataBound">
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <Columns>
                            <asp:BoundField DataField="VehicleID" HeaderText="Vehicle ID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="VehicleDesc" HeaderText="Vehicle Desc" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="PoliceNo" HeaderText="Police No" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="AssetNo" HeaderText="Asset No" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="TvaID" HeaderText="Tva ID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="CmdSelectVehicle" runat="server" ToolTip="Select" Text="<i class='fa fa-share'></i>" Enabled="true" CssClass="btn btn-success btn-xs" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle ForeColor="#003481" BackColor="White" />
                        <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                        <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                        <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                        <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
                        <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                    </asp:GridView>
                    <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                        <asp:Label ID="LblPagingVehicle" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                    </div>
                </asp:Panel>
            </div>
            <div class="form-group form-group-sm" id="LblVehicleID" runat="server">
                <label>Vehicle ID</label>
                <asp:TextBox ID="txtNewVehicleID" runat="server" class="form-control" placeholder="Vehicle ID ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblVehicleDescription" runat="server">
                <label>Vehicle Description</label>
                <asp:TextBox ID="txtNewVehicleDesc" runat="server" class="form-control" placeholder="Vehicle Description ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblPoliceNo" runat="server">
                <label>Police No</label>
                <asp:TextBox ID="txtNewPoliceNo" runat="server" class="form-control" placeholder="Police No ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblAssetNo" runat="server">
                <label>Asset No</label>
                <asp:TextBox ID="txtNewAssetNo" runat="server" class="form-control" placeholder="Asset No ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblTvaID" runat="server">
                <label>Tva ID</label>
                <asp:TextBox ID="txtNewTvaID" runat="server" class="form-control" placeholder="Tva ID ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblStatusVehicle" runat="server">
                <label>Status Old Vehicle</label>
                <asp:DropDownList ID="CmbStatusOldVehicle" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <%-- device --%>
            <div class="form-group form-group-sm" id="LblDeviceServerID" runat="server">
                <label>Server Name</label>
                <asp:DropDownList ID="CmbDeviceServerID" runat="server" AutoPostBack="true" CssClass="form-control" OnTextChanged="CmbDeviceServerID_TextChanged"></asp:DropDownList>
            </div>
            <div class="form-group form-group-sm">
                <div class="input-group input-group-sm">
                    <asp:TextBox ID="txtSearchDevice" runat="server" class="form-control pull-left" placeholder="Search by SN Name ..."></asp:TextBox>
                    <span class="input-group-btn">
                        <button id="CmdSearchDevice" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearchDevice_ServerClick"><i class="fa fa-search"></i></button>
                    </span>
                </div>
            </div>
            <div class="form-group form-group-sm" id="LblGridView6" runat="server">
                <asp:Panel runat="server" ScrollBars="Auto">
                    <asp:GridView ID="GridView6" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="3" OnPageIndexChanging="GridView6_PageIndexChanging" OnRowDataBound="GridView6_RowDataBound">
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <Columns>
                            <asp:BoundField DataField="TdtID" HeaderText="Tdt ID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="DeviceID" HeaderText="Device ID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="vendorname" HeaderText="Vendor Name" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="devicetypedesc" HeaderText="Device Type Desc" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="sourcename" HeaderText="Source Name" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="warehousename" HeaderText="Warehouse Name" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="CmdSelectDevice" runat="server" ToolTip="Select" Text="<i class='fa fa-share'></i>" Enabled="true" CssClass="btn btn-success btn-xs" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle ForeColor="#003481" BackColor="White" />
                        <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                        <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                        <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                        <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
                        <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                    </asp:GridView>
                    <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                        <asp:Label ID="LblPagingDevice" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                    </div>
                </asp:Panel>
            </div>
             <div class="form-group form-group-sm" id="LblNewDeviceID" runat="server">
                <label>Device ID</label>
                <asp:TextBox ID="txtNewDeviceID" runat="server" class="form-control" placeholder="No SN ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblNewNoSN" runat="server">
                <label>No SN</label>
                <asp:TextBox ID="txtNewNoSN" runat="server" class="form-control" placeholder="No SN ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblNewVendorName" runat="server">
                <label>Vendor Name</label>
                <asp:TextBox ID="txtNewVendorName" runat="server" class="form-control" placeholder="Vendor Name ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblNewDeviceTypeDesc" runat="server">
                <label>Device Type Description</label>
                <asp:TextBox ID="txtNewDeviceTypeDesc" runat="server" class="form-control" placeholder="Device Type Desc ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblNewDeviceSourceName" runat="server">
                <label>Source Name</label>
                <asp:TextBox ID="txtNewDeviceSourceName" runat="server" class="form-control" placeholder="Source Name ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblNewWarehouseName" runat="server">
                <label>Warehouse Name</label>
                <asp:TextBox ID="txtNewWarehouseName" runat="server" class="form-control" placeholder="Warehouse Name ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblNewTdtID" runat="server">
                <label>Tdt ID</label>
                <asp:TextBox ID="txtNewTdtID" runat="server" class="form-control" placeholder="Tdt ID ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm" id="LblStatusOldDevice" runat="server">
                <label>Status Old Device</label>
                <asp:DropDownList ID="CmbStatusOldDevice" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
             <div class="form-group form-group-sm" id="LblGridView7" runat="server">
                <asp:Panel runat="server" ScrollBars="Auto">
                    <asp:GridView ID="GridView7" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="3" OnPageIndexChanging="GridView7_PageIndexChanging">
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <Columns>
                            <asp:BoundField DataField="autoid" HeaderText="AutoID " ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="user_id" HeaderText="UserID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="user_nm" HeaderText="UserName" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Check" HeaderStyle-CssClass="gv-header-center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chk2" runat="server" Enabled="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle ForeColor="#003481" BackColor="White" />
                        <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                        <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                        <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                        <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
                        <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                    </asp:GridView>
                    <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                        <asp:Label ID="LblPagingUser" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <div class="box-footer">
            <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" OnClick="CmdClearDetail_Click" Text="Clear" />
            <asp:Button ID="CmdNewLoadData" CssClass="btn btn-primary" runat="server" OnClick="CmdNewLoadData_ServerClick" Text="Load" />
            <asp:Button ID="Button2" CssClass="btn btn-primary" runat="server" OnClick="CmdSaveDetail_Click" Text="Save" />
            <label runat="server" id="lblMsg"></label>
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
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <script type="text/javascript">


            function postDetails(sTvdID, sVehicleDesc, sPoliceNo) {
                if (sTvdID != '') {
                    document.getElementById('txtTvdID').value = sTvdID;
                    document.getElementById('txtPoliceNo').value = sPoliceNo;
                    document.getElementById('CmbMaintTypeID').value = '[Select]';
                    document.getElementById('txtRemark').value = '';
                }
            }
            function postDetailsCust(sCustID, sFullName, sBranchName) {
                if (sCustID != '') {
                    document.getElementById('txtNewCustID').value = sCustID;
                    document.getElementById('txtCustName').value = sFullName;
                    document.getElementById('txtBranch').value = sBranchName;

                    var objfr2 = document.getElementById('CmdNewLoadData');
                    objfr2.click();

                }
            }
            function postNewGsmChild(sTgtID, sGsmID, sMSIDN, sProviderName, sSourceName) {
                if (sTgtID != '') {
                    document.getElementById('txtNewGSM').value = sGsmID;
                    document.getElementById('txtNewMSIDN').value = sMSIDN;
                    document.getElementById('txtNewProviderName').value = sProviderName;
                    document.getElementById('txtNewSourceName').value = sSourceName;
                    document.getElementById('txtNewTgtID').value = sTgtID;
                }
            }

            function postNewVehicleChild(sVehicleID, sVehicleDesc, sPoliceNo, sAssetNo, sTvaID) {
                if (sVehicleID != '') {
                    document.getElementById('txtNewVehicleID').value = sVehicleID;
                    document.getElementById('txtNewVehicleDesc').value = sVehicleDesc;
                    document.getElementById('txtNewPoliceNo').value = sPoliceNo;
                    document.getElementById('txtNewAssetNo').value = sAssetNo;
                    document.getElementById('txtNewTvaID').value = sTvaID;

                }
            }
            function postNewDeviceChild(sDeviceID, sNoSN, sVendorName, sDeviceTypeDesc, sSourceName, sWarehouseName, sTdtID) {
                if (sDeviceID != '') {
                    document.getElementById('txtNewDeviceID').value = sDeviceID;
                    document.getElementById('txtNewNoSN').value = sNoSN;
                    document.getElementById('txtNewVendorName').value = sVendorName;
                    document.getElementById('txtNewDeviceTypeDesc').value = sDeviceTypeDesc;
                    document.getElementById('txtNewDeviceSourceName').value = sSourceName;
                    document.getElementById('txtNewWarehouseName').value = sWarehouseName;
                    document.getElementById('txtNewTdtID').value = sTdtID;

                }
            }
            var lbMsg = document.getElementById('lblMsg');
            var isExists = lbMsg.innerHTML;
            if (isExists != '') {
                if (isExists.includes("Success")) {
                    lbMsg.className = "btn btn-success";
                }
                else {
                    lbMsg.className = "btn btn-danger";
                }
                window.setTimeout(function () { $('#lblMsg').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
            }
        </script>
    </form>
</body>
</html>

