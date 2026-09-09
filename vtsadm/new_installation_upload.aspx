<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="new_installation_upload.aspx.cs" Inherits="vtsadm.new_installation_upload" %>

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
    <script type="text/javascript">
        //$(() => {
        //    'use strict';
        //    (register => register.register(register))({
        //        data: {
        //            dom: {
        //                container: $('body'),
        //                upload: $('body input#FileUpload1'),
        //                imageText: $('body input#ImageText'),
        //                imgOriginal: $('body img#ImgInstall')
        //            },
        //            imageQuality: 0.2,
        //            fileReader: null,
        //            filterType: /^(?:image\/bmp|image\/cis\-cod|image\/gif|image\/ief|image\/jpeg|image\/jpeg|image\/jpeg|image\/pipeg|image\/png|image\/svg\+xml|image\/tiff|image\/x\-cmu\-raster|image\/x\-cmx|image\/x\-icon|image\/x\-portable\-anymap|image\/x\-portable\-bitmap|image\/x\-portable\-graymap|image\/x\-portable\-pixmap|image\/x\-rgb|image\/x\-xbitmap|image\/x\-xpixmap|image\/x\-xwindowdump)$/i
        //        },
        //        register: register => register.events.register(register),
        //        ui: {},
        //        events: {
        //            register: register => ((a, b) => {
        //                a.fileReaderActivator(register);
        //                a.imageLoader(register);
        //                b.upload(register);
        //            })(register.utils, register.events),
        //            fileReader: {
        //                load: (register, e) => {
        //                    let image = new Image();
        //                    image.onload = function () {
        //                        register.data.dom.imgOriginal.attr('src', image.src);

        //                        let canvas = document.createElement("canvas");
        //                        let context = canvas.getContext("2d");
        //                        canvas.width = image.width;
        //                        canvas.height = image.height;
        //                        context.drawImage(image, 0x0, 0x0, image.width, image.height, 0x0, 0x0, canvas.width, canvas.height);

        //                        let blob = canvas.toDataURL('image/jpeg', register.data.imageQuality);

        //                        let imageText = blob;
        //                        imageText = imageText.substr(imageText.indexOf("base64,") + 0x7);
        //                        register.data.dom.imageText.val(imageText);

        //                        register.data.dom.imgOriginal.attr('src', blob);
        //                    }

        //                    image.src = e.target.result;
        //                }
        //            },
        //            upload: register => register.data.dom.upload.on('change', () => register.utils.imageLoader(register))
        //        },
        //        utils: {
        //            fileReaderActivator: register => (self => self.register(self))({
        //                register: () => {
        //                    register.data.fileReader = new FileReader();
        //                    register.data.fileReader.onload = e => register.events.fileReader.load(register, e);
        //                }
        //            }),
        //            imageLoader: (register) => {
        //                const ui = register.data.dom.upload[0x0];

        //                if (0x0 === ui.files.length) return;

        //                let uploadFile = ui.files[0x0];
        //                if (!register.data.filterType.test(uploadFile.type)) {
        //                    alert('Please select a valid image.');
        //                    return;
        //                }

        //                register.data.fileReader.readAsDataURL(uploadFile);
        //            }
        //        }
        //    });
        //});
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="form-group col-lg-12">
                                                <label>File 1</label>
                                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                                <asp:TextBox ID="ImageText" runat="server" CssClass="hidden" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="form-group col-lg-12">
                                                <label>File 2</label>
                                                <asp:FileUpload ID="FileUpload2" runat="server" />
                                                <asp:TextBox ID="ImageText2" runat="server" CssClass="hidden" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="form-group col-lg-12">
                                                <label>File 3</label>
                                                <asp:FileUpload ID="FileUpload3" runat="server" />
                                                <asp:TextBox ID="ImageText3" runat="server" CssClass="hidden" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="form-group col-lg-12">
                                                <label>File 4</label>
                                                <asp:FileUpload ID="FileUpload4" runat="server" />
                                                <asp:TextBox ID="ImageText4" runat="server" CssClass="hidden" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="form-group col-lg-12">
                                                <label>File 5</label>
                                                <asp:FileUpload ID="FileUpload5" runat="server" />
                                                <asp:TextBox ID="ImageText5" runat="server" CssClass="hidden" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="form-group col-lg-12">
                                                <label>File 6</label>
                                                <asp:FileUpload ID="FileUpload6" runat="server" />
                                                <asp:TextBox ID="ImageText6" runat="server" CssClass="hidden" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="form-group col-lg-12">
                                                <label>File 7</label>
                                                <asp:FileUpload ID="FileUpload7" runat="server" />
                                                <asp:TextBox ID="ImageText7" runat="server" CssClass="hidden" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="form-group col-lg-12">
                                                <label>File 8</label>
                                                <asp:FileUpload ID="FileUpload8" runat="server" />
                                                <asp:TextBox ID="ImageText8" runat="server" CssClass="hidden" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="form-group col-lg-12">
                                                <label>File 9</label>
                                                <asp:FileUpload ID="FileUpload9" runat="server" />
                                                <asp:TextBox ID="ImageText9" runat="server" CssClass="hidden" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="form-group col-lg-12">
                                                <label>File 10</label>
                                                <asp:FileUpload ID="FileUpload10" runat="server" />
                                                <asp:TextBox ID="ImageText10" runat="server" CssClass="hidden" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            
                        </div>
                        <div class="box-footer">
                            <button id="CmdUpload" runat="server" type="button" class="btn btn-primary" onserverclick="CmdUpload_ServerClick">Upload</button>
                            <button id="CmdRemove" runat="server" type="button" class="btn btn-primary" onserverclick="CmdRemove_ServerClick">Remove</button>
                            <div runat="server" id="lblMsg"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12" id="boxpic1">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div id="divpic" class="form-group form-group-sm" style="height:200px;text-align:center;">                            
                            <asp:Image ID="ImgInstall" runat="server" BorderWidth="0px" height="100%"/>
                        </div>


                    </div>
                </div>
            </div>

            <div class="col-md-12" id="boxpic2">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div id="divpic2" class="form-group form-group-sm" style="height:200px;text-align:center;">                            
                            <asp:Image ID="ImgInstall2" runat="server" BorderWidth="0px" height="100%"/>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-md-12" id="boxpic3">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div id="divpic3" class="form-group form-group-sm" style="height:200px;text-align:center;">                            
                            <asp:Image ID="ImgInstall3" runat="server" BorderWidth="0px" height="100%"/>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-md-12" id="boxpic4">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div id="divpic4" class="form-group form-group-sm" style="height:200px;text-align:center;">                            
                            <asp:Image ID="ImgInstall4" runat="server" BorderWidth="0px" height="100%"/>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-md-12" id="boxpic4">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div id="divpic5" class="form-group form-group-sm" style="height:200px;text-align:center;">                            
                            <asp:Image ID="ImgInstall5" runat="server" BorderWidth="0px" height="100%"/>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-md-12" id="boxpic4">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div id="divpic6" class="form-group form-group-sm" style="height:200px;text-align:center;">                            
                            <asp:Image ID="ImgInstall6" runat="server" BorderWidth="0px" height="100%"/>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-md-12" id="boxpic4">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div id="divpic7" class="form-group form-group-sm" style="height:200px;text-align:center;">                            
                            <asp:Image ID="ImgInstall7" runat="server" BorderWidth="0px" height="100%"/>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-md-12" id="boxpic4">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div id="divpic8" class="form-group form-group-sm" style="height:200px;text-align:center;">                            
                            <asp:Image ID="ImgInstall8" runat="server" BorderWidth="0px" height="100%"/>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-md-12" id="boxpic4">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div id="divpic9" class="form-group form-group-sm" style="height:200px;text-align:center;">                            
                            <asp:Image ID="ImgInstall9" runat="server" BorderWidth="0px" height="100%"/>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-md-12" id="boxpic4">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div id="divpic10" class="form-group form-group-sm" style="height:200px;text-align:center;">                            
                            <asp:Image ID="ImgInstall10" runat="server" BorderWidth="0px" height="100%"/>
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