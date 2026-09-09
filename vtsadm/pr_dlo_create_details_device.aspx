<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pr_dlo_create_details_device.aspx.cs" Inherits="vtsadm.pr_dlo_create_details_device" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <link rel="stylesheet" href="Content/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="Content/bower_components/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="Content/bower_components/Ionicons/css/ionicons.min.css" />
    <link rel="stylesheet" href="Content/bower_components/bootstrap-daterangepicker/daterangepicker.css" />
    <link rel="stylesheet" href="Content/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css" />
    <link rel="stylesheet" href="Content/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css" />
    <link rel="stylesheet" href="Content/plugins/iCheck/all.css" />
    <link rel="stylesheet" href="Content/bower_components/bootstrap-colorpicker/dist/css/bootstrap-colorpicker.min.css" />
    <link rel="stylesheet" href="Content/plugins/timepicker/bootstrap-timepicker.min.css" />
    <link rel="stylesheet" href="Content/bower_components/select2/dist/css/select2.min.css" />
    <link rel="stylesheet" href="Content/dist/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="Content/dist/css/skins/_all-skins.min.css" />
    <link rel="stylesheet" href="Content/paginationcs.css" />
    <link rel="stylesheet" href="Content/loader.css" />
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
    <script type="text/javascript" src="Content/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>

</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div class="box-body">
            <div class="form-group form-group-sm">
                <input type="hidden" runat="server" id="txtDloID" />
                <input type="hidden" runat="server" id="txtSeq" />
                <div class="row">
                    <div class="col-xs-8">
                        <label>DLO ID</label>
                        <asp:TextBox ID="txtDloIDView" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col-xs-4">
                        <label>Seq</label>
                        <asp:TextBox ID="txtSeqView" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group form-group-sm">
                <div class="row">
                    <div class="col-xs-6">
                        <label>No SN</label>
                    </div>
                    <div class="col-xs-6">
                        <label>Packing List</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-6">
                        <asp:TextBox ID="txtNoSN" runat="server" CssClass="form-control" placeholder="No SN ..."></asp:TextBox>
                    </div>
                    <div class="col-xs-6">
                        <asp:TextBox ID="txtPackingList" runat="server" CssClass="form-control" placeholder="Packing List ..."></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group form-group-sm">
                <div class="row">
                    <div class="col-xs-4">
                        <label>Batch No</label>
                        <asp:TextBox ID="txtBatchNo" runat="server" CssClass="form-control" placeholder="Batch No ..."></asp:TextBox>
                    </div>
                    <div class="col-xs-8">
                        <label>Remark</label>
                        <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" placeholder="Remark ..."></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group form-group-sm">
                <div class="row">
                    <div class="col-xs-12">
                        <label>Upload SN File (.xlsx / .csv)</label>
                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                        <small>Format kolom: A=DeviceGroupID, B=DeviceTypeID, C=NoSN, D=PackingList, E=BatchNo, F=Remark. DLO ID diambil dari row detail yang dipilih.</small>
                    </div>
                </div>
            </div>
            <div class="form-group form-group-sm">
                <asp:Panel runat="server" ScrollBars="Auto">
                    <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" OnRowCommand="GridView1_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="NoSN" HeaderText="No SN"></asp:BoundField>
                            <asp:BoundField DataField="PackingList" HeaderText="Packing List"></asp:BoundField>
                            <asp:BoundField DataField="BatchNo" HeaderText="Batch No"></asp:BoundField>
                            <asp:BoundField DataField="Remark" HeaderText="Remark"></asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="CmdDeleteSN" runat="server" CommandName="DELETESN" CommandArgument='<%# Eval("NoSN") %>' Text="<i class='fa fa-close'></i>" ToolTip="Delete SN" CssClass="btn btn-danger btn-xs" OnClientClick="return confirm('Delete SN ini?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>
        <div class="box-footer">
            <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" OnClick="CmdClearDetail_Click" Text="Clear" />
            <asp:Button ID="Button2" CssClass="btn btn-primary" runat="server" OnClick="CmdSaveDetail_Click" Text="Add SN" />
            <asp:Button ID="Button3" CssClass="btn btn-primary" runat="server" OnClick="CmdUploadSN_Click" Text="Upload SN" />
            <asp:Label runat="server" ID="lblMsg" />
        </div>

        <script type="text/javascript">
            var lbMsg = document.getElementById('lblMsg');
            var isExists = lbMsg.innerHTML;
            if (isExists != '') {
                if (isExists.includes("Success")) {
                    document.getElementById("Button1").click();
                }
            }
        </script>
    </form>
</body>
</html>
