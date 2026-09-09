<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="job_maint_details.aspx.cs" Inherits="vtsadm.job_maint_details" %>

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
                            <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="PoTypeDesc" HeaderText="PO Type" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="Expired" HeaderText="Exp Warranty" ItemStyle-Wrap="false"></asp:BoundField>
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
                        <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                </asp:Panel>
            </div>

            <div class="form-group form-group-sm">
                <input id="txtTvdID" runat="server" type="hidden" />
                <label>Police No</label>
                <asp:TextBox ID="txtPoliceNo" runat="server" class="form-control" placeholder="Police No ..."></asp:TextBox>
            </div>

            <div class="form-group form-group-sm">
                <label>Maintenance Type</label>
                <asp:DropDownList ID="CmbMaintTypeID" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="form-group form-group-sm">
                <label>Reason</label>
                <asp:TextBox ID="txtReason" runat="server" class="form-control" placeholder="Reason ..."></asp:TextBox>
                <!-- Dropdown untuk Suspended -->
                <asp:DropDownList ID="ddlRemarkSuspended" runat="server" CssClass="form-control" style="display: none;">
                    <asp:ListItem Text="[Select]" Value="[Select]"></asp:ListItem>
                    <asp:ListItem Text="Perbaikan" Value="Perbaikan"></asp:ListItem>
                    <asp:ListItem Text="Tidak ada driver/tidak beroperasional" Value="Tidak ada driver/tidak beroperasional"></asp:ListItem>
                    <asp:ListItem Text="Peremajaan unit" Value="Peremajaan unit"></asp:ListItem>
                    <asp:ListItem Text="Cabut Sementara (akan pindah pasang)" Value="Cabut Sementara (akan pindah pasang)"></asp:ListItem>
                    <asp:ListItem Text="Cabut sementara (akan di uninstall)" Value="Cabut sementara (akan di uninstall)"></asp:ListItem>
                    <asp:ListItem Text="Suspend karena tunggakan" Value="Suspend karena tunggakan"></asp:ListItem>
                    <asp:ListItem Text="Perusahaan bangkrut" Value="Perusahaan bangkrut"></asp:ListItem>
                    <asp:ListItem Text="unit dijual (beli putus)" Value="unit dijual (beli putus)"></asp:ListItem>
                    <asp:ListItem Text="unit laka" Value="unit laka"></asp:ListItem>
                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                </asp:DropDownList>
    
                <!-- Dropdown untuk Uninstalling -->
                <asp:DropDownList ID="ddlRemarkUninstalling" runat="server" CssClass="form-control" style="display: none;">
                    <asp:ListItem Text="[Select]" Value="[Select]"></asp:ListItem>
                    <asp:ListItem Text="Putus Kontrak/Berhenti berlangganan" Value="Putus Kontrak/Berhenti berlangganan"></asp:ListItem>
                    <asp:ListItem Text="Unit dijual" Value="Unit dijual"></asp:ListItem>
                    <asp:ListItem Text="Perusahaan bangkrut" Value="Perusahaan bangkrut"></asp:ListItem>
                    <asp:ListItem Text="Tunggakan" Value="Tunggakan"></asp:ListItem>
                    <asp:ListItem Text="Tidak garansi" Value="Tidak garansi"></asp:ListItem>
                    <asp:ListItem Text="Upgrade alat" Value="Upgrade alat"></asp:ListItem>
                    <asp:ListItem Text="Kecewa dengan aftersales" Value="Kecewa dengan aftersales"></asp:ListItem>
                    <asp:ListItem Text="Migrasi dari Lite ke IGO Tracker" Value="Migrasi dari Lite ke IGO Tracker"></asp:ListItem>
                    <asp:ListItem Text="Unit laka" Value="Unit laka"></asp:ListItem>
                    <asp:ListItem Text="alat trial (tidak menjadi PO)" Value="alat trial (tidak menjadi PO)"></asp:ListItem>
                    <asp:ListItem Text="cabut alat karena peremmajaan unit" Value="cabut alat karena peremmajaan unit"></asp:ListItem>
                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-group form-group-sm">
                <label>Remark</label>
                <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark Detail ..."></asp:TextBox>
            </div>

        </div>
        <div class="box-footer">
            <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" OnClick="CmdClearDetail_Click" Text="Clear" />
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

            var lbMsg = document.getElementById('lblMsg');
            var isExists = lbMsg.innerHTML;
            //console.log(isExists);
            //console.log("test " + isExists + " test");
            if (isExists != '') {
                if (isExists.includes("Success")) {
                    lbMsg.className = "btn btn-success";
                }
                else {
                    lbMsg.className = "btn btn-danger";
                }
                window.setTimeout(function () { $('#lblMsg').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
                //$('#modal-messagebox').modal('show');
            }

            document.addEventListener('DOMContentLoaded', function() {
                // Definisikan fungsi sebagai referensi global untuk diakses oleh inline event
                window.handleMaintTypeChange = function() {
                    console.log('handleMaintTypeChange dipanggil');
        
                    // Ambil referensi semua elemen yang diperlukan
                    var cmbMaintTypeID = document.getElementById('<%= CmbMaintTypeID.ClientID %>');
                    var txtReason = document.getElementById('<%= txtReason.ClientID %>');
                    var ddlSuspended = document.getElementById('<%= ddlRemarkSuspended.ClientID %>');
                    var ddlUninstalling = document.getElementById('<%= ddlRemarkUninstalling.ClientID %>');
        
                    // Debugging - cek apakah elemen ditemukan
                    console.log('Elements found:', {
                        cmbMaintTypeID: !!cmbMaintTypeID,
                        ddlSuspended: !!ddlSuspended,
                        ddlUninstalling: !!ddlUninstalling
                    });
        
                    // Lanjutkan hanya jika semua elemen ditemukan
                    if (cmbMaintTypeID && txtReason && ddlSuspended && ddlUninstalling) {
                        var selectedValue = cmbMaintTypeID.value;
                        console.log('Selected value:', selectedValue);
            
                        // Atur visibilitas
                        txtReason.style.display = 'none';
                        ddlSuspended.style.display = 'none';
                        ddlUninstalling.style.display = 'none';
            
                        if (selectedValue === 'MTY0000011') {
                            ddlSuspended.style.display = 'block';
                            // Ambil nilai dari dropdown untuk texbox
                            ddlSuspended.onchange = function() {
                                txtReason.value = this.value;
                            };
                        } else if (selectedValue === 'MTY0000006') {
                            ddlUninstalling.style.display = 'block';
                            // Ambil nilai dari dropdown untuk texbox
                            ddlUninstalling.onchange = function() {
                                txtReason.value = this.value;
                            };
                        } else {
                            txtReason.style.display = 'block';
                        }
                    } else {
                        console.error('Satu atau lebih elemen form tidak ditemukan');
                    }
                };
    
                // Tambahkan event listener
                var cmbMaintTypeID = document.getElementById('<%= CmbMaintTypeID.ClientID %>');
                if (cmbMaintTypeID) {
                    cmbMaintTypeID.addEventListener('change', window.handleMaintTypeChange);
                    // Jalankan fungsi saat awal untuk mengatur visibilitas awal
                    window.handleMaintTypeChange();
                } else {
                    console.error('CmbMaintTypeID tidak ditemukan');
                }
            });
        </script>
    </form>
</body>
</html>

