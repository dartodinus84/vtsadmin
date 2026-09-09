<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="job_assign_details_it.aspx.cs" Inherits="vtsadm.job_assign_details_it" %>

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
                <label>Schedule Date</label>
                <input type="date" id="txtScheduleDate" runat="server" class="form-control" placeholder="Schedule Date ..." />
            </div>
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
                            <asp:BoundField DataField="ITID" HeaderText="IT Support ID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="Name" HeaderText="IT Support  Name" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="BranchName" HeaderText="Branch Name" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="Total" HeaderText="Total Assign / day" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="ScheduleTime" HeaderText="Last Assign / day" ItemStyle-Wrap="false"></asp:BoundField>
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
                <input id="txtTechnicianID" runat="server" type="hidden" />
                <label>IT Support ID</label>
                <asp:TextBox ID="txtTechnicianID2" runat="server" class="form-control" placeholder="Technician ID ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm">
                <label>IT Support Name</label>
                <asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Technician Name ..."></asp:TextBox>
            </div>
            <div class="form-group form-group-sm">
                <label>Schedule Time</label>
                <input type="time" id="txtTimeSchedule" runat="server" class="form-control" placeholder="Schedule Time ..." />
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

            function postDetails(sTechnicianID, sTechnicianID2, sName) {
                if (sTechnicianID != '') {
                    document.getElementById('txtTechnicianID').value = sTechnicianID;
                    document.getElementById('txtTechnicianID2').value = sTechnicianID2;
                    document.getElementById('txtName').value = sName;
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
        </script>
    </form>
</body>
</html>

