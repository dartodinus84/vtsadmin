<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="device_upload_procurement.aspx.cs" Inherits="vtsadm.device_upload_procurement" %>

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
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <input type="hidden" runat="server" id="txtBatchNo" />
                            <input type="hidden" runat="server" id="txtFileName" />
                            <input type="hidden" runat="server" id="txtIsUpdate" />
                            <label>File Input</label>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </div>
                        <div class="box-footer">
                            <button id="CmdUpload" runat="server" type="button" class="btn btn-primary" onserverclick="CmdUpload_ServerClick">Upload</button>
                            <button id="CmdCancel" runat="server" type="button" class="btn btn-primary" onserverclick="CmdCancel_ServerClick">Cancel</button>
                            <button id="CmdSubmit" runat="server" type="button" class="btn btn-primary" onserverclick="CmdSubmit_ServerClick">Insert</button>
                            <button id="CmdUpdate" runat="server" type="button" class="btn btn-primary" onserverclick="CmdUpdate_ServerClick">Update</button>
                            <button id="CmdError" runat="server" type="button" class="btn btn-primary" onserverclick="CmdError_ServerClick">File Error</button>
                            <div runat="server" id="lblMsg"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" Width="100%">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="BatchNo" HeaderText="Batch No" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="sDateArrival" HeaderText="Date Arrival" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="SourceName" HeaderText="Source Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type Desc" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseName" HeaderText="Warehouse Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ServerName" HeaderText="Server Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>

                                        <asp:BoundField DataField="DeviceGroupDesc" HeaderText="Device Group Desc" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="VendorID" HeaderText="Vendor ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="VendorAddress" HeaderText="Vendor Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeID" HeaderText="Device Type ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceGroupID" HeaderText="Device Group ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="SourceID" HeaderText="Source ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseID" HeaderText="Warehouse ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ServerID" HeaderText="Server ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Remark" HeaderText="Warehouse ID" ItemStyle-Wrap="false"></asp:BoundField>
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
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
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
