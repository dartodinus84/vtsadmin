<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="so_create_details.aspx.cs" Inherits="vtsadm.so_create_details" %>

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
            <%--            <div class="form-group form-group-sm">
                <label>Sequence</label>
                <asp:TextBox ID="txtSeq" runat="server" class="form-control" placeholder="Skip for new detail ..." required="required" disabled=""></asp:TextBox>
            </div>--%>
            <div class="form-group form-group-sm">
                <input type="hidden" runat="server" id="txtCustID" />
                <input type="hidden" runat="server" id="txtPoID" />
                <div class="row">
                    <div class="col-xs-6">
                        <label>Device Group Desc</label>
                    </div>
                    <div class="col-xs-6">
                        <label>Device Type Desc</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-6">
                        <asp:DropDownList ID="CmbDeviceGroupID" AutoPostBack="true" OnTextChanged="CmbDeviceGroupID_TextChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-xs-6">
                        <asp:DropDownList ID="CmbDeviceTypeID" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group form-group-sm">
                <div class="row">
                    <div class="col-xs-3">
                        <label>Quantity</label>
                    </div>
                    <div class="col-xs-9">
                        <label>Remark</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-3">
                        <input type="text" id="txtQuantity" runat="server" class="form-control" placeholder="Quantity ..." onkeypress="return event.charCode >= 48 && event.charCode <= 57" />
                    </div>
                    <div class="col-xs-9">
                        <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark Detail ..."></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group form-group-sm">
                <div class="row">
                    <div class="col-xs-8">
                        <label>Price</label>
                    </div>
                    <div class="col-xs-4">
                        <label>PPN</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-8">
                        <input type="text" min="1" max="60" id="txtPrice" runat="server" class="form-control" placeholder="Price ..." onkeypress="return event.charCode >= 48 && event.charCode <= 57" />
                    </div>
                    <div class="col-xs-4">
                        <asp:DropDownList ID="CmbPricePPN" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group form-group-sm">
                <div class="row">
                    <div class="col-xs-8">
                        <label>Installation Fee</label>
                    </div>
                    <div class="col-xs-4">
                        <label>PPN</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-8">
                        <input type="text" min="1" max="60" id="txtInstallFee" runat="server" class="form-control" placeholder="Installation Fee ..." onkeypress='return event.charCode >= 48 && event.charCode <= 57' />
                    </div>
                    <div class="col-xs-4">
                        <asp:DropDownList ID="CmbInstallFeePPN" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group form-group-sm">
                <div class="row">
                    <div class="col-xs-8">
                        <label>Monthly Fee</label>
                    </div>
                    <div class="col-xs-4">
                        <label>PPN</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-8">
                        <input type="text" min="1" max="60" id="txtMonthlyFee" runat="server" class="form-control" placeholder="Monthly Fee ..." onkeypress='return event.charCode >= 48 && event.charCode <= 57' />
                    </div>
                    <div class="col-xs-4">
                        <asp:DropDownList ID="CmbMonthlyFeePPN" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div> 

            <div class="form-group form-group-sm">
                <div class="row">  
                    <div class="col-xs-12">
                        <label>ITEM</label>
                    </div>
                    <div class="col-xs-12">
                        <button class="btn btn-info add" type="button">
                            Add
                        </button>
                    </div>
                </div>
            </div>

            <div data-repeater-list="group-a" class="element" id="div_1">
                <div data-repeater-item class="form-group form-group-sm">
                    <div class="row">
                        <div class="col-xs-6">
                            <label>Product</label>
                        </div>
                        <div class="col-xs-3">
                            <label>Quantity</label>
                        </div>

                        <div class="col-xs-3">
                            <label>Price</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-6">
                            <select class="form-control" runat="server" name="txtItemProductID1" id="txtItemProductID1">
                                <option value="">Select</option>
                            </select>
                        </div>

                        <div class="col-xs-3">
                            <input type="text" min="1" max="60" runat="server" name="txtItemQuantity" id="txtItemQuantity1" class="form-control" />
                        </div>
                    
                        <div class="col-xs-3">
                            <input type="text" min="1" max="60" runat="server" name="txtItemPrice" id="txtItemPrice1" class="form-control" />
                        </div>
                    </div>
                </div> 
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
                //$('#modal-messagebox').modal('show');
            }

            $(document).ready(function () {
                // Add new element
                $(".add").click(function(){

                    // Finding total number of elements added
                    var total_element = $(".element").length;
                    
                    // last <div> with element class id
                    var lastid = $(".element:last").attr("id");
                    var split_id = lastid.split("_");
                    var nextindex = Number(split_id[1]) + 1;

                    var max = 5;
                    // Check total number elements
                    if(total_element < max ){
                        // Adding new div container after last occurance of element class
                        $(".element:last").after("<div data-repeater-list='group-a' class='element' id='div_"+ nextindex +"'></div>");
                        
                        // Adding element to <div>
                        $("#div_" + nextindex).append('<div data-repeater-item class="form-group form-group-sm"><div class="row"><div class="col-xs-12"><button class="btn btn-danger waves-effect waves-light remove" type="button" id="remove_'+nextindex+'"><i class="fa fa-close"></i></button></div></div><div class="row"><div class="col-xs-6"><label>Product</label></div><div class="col-xs-3"><label>Quantity</label></div><div class="col-xs-3"><label>Price</label></div></div><div class="row"><div class="col-xs-6"><select class="form-control" runat="server" name="txtItemProductID" id="txtItemProductID"><option value="">Select</option></select></div><div class="col-xs-3"><input type="text" min="1" max="60" runat="server" name="txtItemQuantity" id="txtItemQuantity" class="form-control"></div><div class="col-xs-3"><input type="text" min="1" max="60" runat="server" name="txtItemPrice" id="txtItemPrice" class="form-control"></div></div></div>');
                    
                    }
                    
                });

                $('.box-body').on('click','.remove',function(){
                    var id = this.id;
                    var split_id = id.split("_");
                    var deleteindex = split_id[1];

                    // Remove <div> with id
                    $("#div_" + deleteindex).remove();
                });   
            });
        </script>
    </form>
</body>
</html>