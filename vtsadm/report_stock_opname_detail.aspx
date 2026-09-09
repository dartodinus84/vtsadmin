<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report_stock_opname_detail.aspx.cs" Inherits="vtsadm.report_stock_opname_detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Stock Opname Detail</title> 
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport"/>
    <!-- Bootstrap 3.3.7 -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap/dist/css/bootstrap.min.css"/>
    <!-- Font Awesome -->
    <link rel="stylesheet" href="Content/bower_components/font-awesome/css/font-awesome.min.css"/>
    <!-- Ionicons -->
    <link rel="stylesheet" href="Content/bower_components/Ionicons/css/ionicons.min.css"/>
    <!-- Theme style -->
    <link rel="stylesheet" href="Content/dist/css/AdminLTE.min.css"/>
    <!-- AdminLTE Skins -->
    <link rel="stylesheet" href="Content/dist/css/skins/_all-skins.min.css"/>
    <link rel="stylesheet" href="Content/paginationcs.css">
    <!-- Google Font -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic"/>
    
    <style type="text/css">
        body {
            padding: 10px;
            background-color: #f9f9f9;
        }
        .box-header {
            background-color: #3c8dbc;
            color: white;
            border-radius: 3px 3px 0 0;
            padding: 10px 15px;
            margin-bottom: 15px;
        }
        .box-title {
            font-size: 18px;
            font-weight: 600;
        }
        .table-container {
            background-color: #fff;
            border-radius: 3px;
            box-shadow: 0 1px 2px rgba(0,0,0,0.1);
            padding: 15px;
            margin-bottom: 15px;
        }
        .table > tbody > tr.success > td {
            background-color: #dff0d8;
        }
        .table > tbody > tr.warning > td {
            background-color: #fcf8e3;
        }
        .table > tbody > tr.danger > td {
            background-color: #f2dede;
        }
        .table > thead > tr > th {
            background-color: #f4f4f4;
            border-bottom: 2px solid #ddd;
        }
        .export-btn {
            margin-bottom: 15px;
        }
        .status-badge {
            display: inline-block;
            padding: 3px 7px;
            font-size: 12px;
            font-weight: bold;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            border-radius: 10px;
        }
        .badge-success {
            background-color: #00a65a;
        }
        .badge-warning {
            background-color: #f39c12;
        }
        .badge-danger {
            background-color: #dd4b39;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Header -->
        <div class="box-header">
            <h3 class="box-title">
                <i class="fa fa-clipboard"></i> Stock Opname Detail - Opname Code: <span id="lblOpnameCode" runat="server"></span>
            </h3>
            <input type="hidden" id="hidOpnameId" runat="server" />
        </div>
        
        <!-- Error Message Container -->
        <div id="errorContainer" runat="server" visible="false" class="alert alert-danger" role="alert">
            <i class="fa fa-exclamation-circle"></i> <strong>Error:</strong> <span id="errorMessage" runat="server"></span>
        </div>
        
        <!-- Export Button -->
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="CmdExport" CssClass="btn btn-primary export-btn" runat="server" OnClick="CmdExport_Click" Text="Export CSV" />
            </div>
        </div>
        
        <!-- Data Table -->
        <div class="row">
            <div class="col-md-12">
                <div class="table-container">
                    <asp:Panel runat="server" CssClass="" ScrollBars="Auto" Width="100%">
                        <asp:GridView ID="GridView1" runat="server" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered table-hover" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#333333" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="15" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting">
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <Columns>
                                <asp:BoundField DataField="detail_id" HeaderText="Detail ID" ItemStyle-Wrap="false" SortExpression="detail_id"></asp:BoundField>
                                <asp:BoundField DataField="device_id" HeaderText="Device ID" ItemStyle-Wrap="false" SortExpression="device_id"></asp:BoundField>
                                <asp:BoundField DataField="nosn" HeaderText="Serial Number" ItemStyle-Wrap="false" SortExpression="nosn"></asp:BoundField>
                                <asp:BoundField DataField="current_status" HeaderText="System Status" ItemStyle-Wrap="false" SortExpression="current_status"></asp:BoundField>
                                <asp:BoundField DataField="physical_found" HeaderText="Found" ItemStyle-Wrap="false" SortExpression="physical_found"></asp:BoundField>
                                <asp:BoundField DataField="physical_status" HeaderText="Physical Status" ItemStyle-Wrap="false" SortExpression="physical_status"></asp:BoundField>
                                <asp:BoundField DataField="remarks" HeaderText="Remarks" ItemStyle-Wrap="false" SortExpression="remarks"></asp:BoundField>
                                <asp:BoundField DataField="created_at" HeaderText="Created At" ItemStyle-Wrap="false" SortExpression="created_at" DataFormatString="{0:dd/MM/yyyy HH:mm}"></asp:BoundField>
                                <asp:BoundField DataField="opname_code" HeaderText="Opname Code" Visible="false"></asp:BoundField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" CssClass="status-badge"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle BackColor="White" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle CssClass="pagination-ys" HorizontalAlign="Left" BorderColor="White" />
                            <PagerSettings PageButtonCount="5" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                            <HeaderStyle Height="30px" CssClass="bg-primary" ForeColor="White" Font-Bold="True" />
                            <AlternatingRowStyle BackColor="#F9F9F9" />
                        </asp:GridView>
                        <div class="pagination-info">
                            <asp:Label ID="LblPaging" runat="server"></asp:Label>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        
        <!-- Legend -->
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <strong>Status Legend:</strong>
                        <div style="margin-top: 5px;">
                            <span class="label label-success" style="margin-right: 10px;">Match</span>
                            <span class="label label-warning" style="margin-right: 10px;">Not Found</span>
                            <span class="label label-danger">Mismatch</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
     </form>
     
     <script type="text/javascript" src="Content/bower_components/jquery/dist/jquery.min.js"></script>
     <script type="text/javascript" src="Content/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
</body>
</html>