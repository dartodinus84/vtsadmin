<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="req_procurement_detail.aspx.cs" Inherits="vtsadm.req_procurement_detail" %>

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
                <label>Group Tools ID</label>
                <asp:DropDownList ID="CmbGroupID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbGroupID_TextChanged"></asp:DropDownList>
            </div>
            <div class="form-group form-group-sm">
                <label>Type Tools</label>
                <asp:DropDownList ID="CmbTypeID" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="form-group form-group-sm">
                <label>Quantity</label>
                <input type="number" min="1" id="txtQuantity" runat="server" class="form-control" placeholder="Quantity ..." onkeypress='return event.charCode >= 48 && event.charCode <= 57' />
            </div>
            <div class="form-group form-group-sm">
                <label>Amount (Item)</label>
                <input type="number" min="1" id="txtAmount" runat="server" class="form-control" placeholder="Amount (Items) ..." onkeypress='return event.charCode >= 48 && event.charCode <= 57' />
            </div>
            <div class="form-group form-group-sm">
                <label>Remarks</label>
                <asp:TextBox ID="txtRemarks" runat="server" class="form-control" placeholder="Remarks Detail ..."></asp:TextBox>
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

            function postDetails(s1, s2) {
                if (s1 != '') {
                    document.getElementById('txtTypeToolsID').value = s1;
                    document.getElementById('txtProcurementType').value = s2;
                    document.getElementById('txtQuantity').value = '';
                }
            }
            function postDetailsTools(ss1) {
                if (ss1 != '') {
                    document.getElementById('txtToolsID2').value = ss1;
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

