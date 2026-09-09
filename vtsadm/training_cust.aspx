<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="training_cust.aspx.cs" Inherits="vtsadm.training_cust" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .form-control {
            background-color: white !important;
        }

        /* Keep form boxes visible in mobile WebView / narrow viewports */
        #box-jo,
        #box-customer,
        #box-installation,
        #box-jo .box-body,
        #box-customer .box-body,
        #box-installation .box-body {
            display: block !important;
            visibility: visible !important;
            height: auto !important;
        }

        #modal-picture .training-upload-preview {
            height: 195px;
            text-align: center;
            margin-top: 10px;
            overflow: hidden;
            background: #f7f7f7;
        }

        #modal-picture .training-upload-preview img {
            max-height: 100%;
            max-width: 100%;
        }
    </style>
    <section class="content-header">
        <h1>Customer
                <small>Training</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Training</a></li>
            <li class="active">Customer</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6 col-xs-12">
                <div id="box-jo" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Job Training Information</h3>
                        <div class="box-tools pull-right">
                            <button id="btnid" type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Training ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtTrainingID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button5" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-job_training"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Request Date</label>
                            <input type="text" id="txtReqDate" runat="server" class="form-control" placeholder="Request Date ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <input type="text" id="txtCustName" runat="server" class="form-control" placeholder="Customer Name ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Schedule Date</label>
                            <input type="text" id="txtSchDate" runat="server" class="form-control" placeholder="Schedule Date ..." readonly="readonly" />
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Business Fields</label>
                            <asp:DropDownList ID="CmbBusinessField" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div id="box-customer" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <input type="text" id="txtCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <input type="text" id="txtCustomerName" runat="server" class="form-control" placeholder="Customer Name ..." readonly="readonly" />
                            <input type="hidden" id="txtCustID" runat="server" />
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-md-6 col-xs-12">

                <div id="box-installation" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Training Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Training Date</label>
                            <asp:TextBox ID="txtTrainingDate" TextMode="Date" runat="server" class="form-control" placeholder="Training Date ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Trainers</label>
                            <textarea id="txtTrainers" runat="server" class="form-control" placeholder="Trainers ..." rows="2"></textarea>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Attendances</label>
                            <textarea id="txtAttendances" runat="server" class="form-control" placeholder="Attedances ..." rows="2"></textarea>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Category <span style="color:red">*</span></label>
                            <asp:DropDownList ID="CmbTrainCategoryID" runat="server" CssClass="form-control" required="required"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <textarea id="txtRemark" runat="server" class="form-control" placeholder="Remark ..." rows="2"></textarea>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Picture Attachment</label>
                            <p>
                                <button id="CmdUpload" type="button" class="btn btn-primary" onclick="$('#modal-picture').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image</button>
                            </p>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="reset" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdLoadTraining" CssClass="btn btn-primary" runat="server" OnClick="CmdLoadTraining_Click" Text="Load" CausesValidation="false" />
                        <asp:Button ID="CmdFunc" CssClass="btn btn-primary" runat="server" OnClientClick="return showFunctionModal();" Text="Functions" />
                        <asp:Button ID="CmdRemark" CssClass="btn btn-warning" runat="server" OnClientClick="return confirmAddNote();" Text="Add Note" />
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="return confirmSubmit();" Text="Submit" />
                        
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Remark</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TrainingID" HeaderText="TrainingID" ItemStyle-Wrap="false" SortExpression="TrainingID"></asp:BoundField>
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="CustomerName"></asp:BoundField>
                                        <asp:BoundField DataField="TrainingCategoryName" HeaderText="Category" ItemStyle-Wrap="false" SortExpression="TrainingCategoryName"></asp:BoundField>
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Wrap="true" SortExpression="Remark"></asp:BoundField>
                                        <asp:BoundField DataField="UsrUpd" HeaderText="UsrUpd" ItemStyle-Wrap="false" SortExpression="UsrUpd"></asp:BoundField>
                                        <asp:BoundField DataField="DtmUpd" HeaderText="DtmUpd" ItemStyle-Wrap="false" SortExpression="DtmUpd"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdPic" runat="server" Text="<i class='fa fa-file-image-o'></i>" ToolTip="Picture" Enabled="true" CssClass="btn btn-warning btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PictureFileName" HeaderText="Picture FileName" ItemStyle-Wrap="false"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>



        <div class="modal fade bs-example-modal-lg" id="modal-function">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Function Menu</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto" Height="350px">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="100" OnRowDataBound="GridView1_RowDataBound">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="FunctionID" HeaderText="Function ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="FunctionName" HeaderText="Function Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Yes" HeaderStyle-CssClass="gv-header-center">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="RdoYes" runat="server" Checked="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="No" HeaderStyle-CssClass="gv-header-center">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="RdoNo" runat="server" Checked="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Remark" HeaderStyle-CssClass="gv-header-center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFunctionRemark" runat="server" class="form-control" placeholder="Remark ..."></asp:TextBox>                                               
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="Black" />
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPagingFunc" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal-submit" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure ?</h6>
                    </div>
                    <div class="modal-footer">
                        <img src="Content/ajax-loader2.gif" style="display:none;" id="iload"/>
                        <button type="button" class="btn btn-default btn-confirmasi" runat="server" onclick="prepareConfirmPostback('iload');" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
                        <button type="button" class="btn btn-primary btn-confirmasi" onclick="$('#modal-submit').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-remark" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure ?</h6>
                    </div>
                    <div class="modal-footer">
                        <img src="Content/ajax-loader2.gif" style="display:none;" id="iload2"/>
                        <button type="button" class="btn btn-default btn-confirmasi" runat="server" onclick="prepareConfirmPostback('iload2');" onserverclick="CmdYesRemark_ServerClick" id="CmdYesRemark">Yes</button>
                        <button type="button" class="btn btn-primary btn-confirmasi" onclick="$('#modal-remark').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <label>File Input</label>
                            <asp:FileUpload ID="FileUploadTraining" runat="server" accept="image/*,.jpg,.jpeg,.png,image/jpeg,image/png" />
                        </div>
                        <div class="form-group form-group-sm">
                            <button id="CmdUploadPicture" runat="server" type="button" class="btn btn-primary" onserverclick="CmdUploadPicture_ServerClick">Upload</button>
                            <button id="CmdRemovePicture" runat="server" type="button" class="btn btn-primary" onserverclick="CmdRemovePicture_ServerClick">Remove</button>
                            <div runat="server" id="lblUploadMsg" style="display:inline-block;margin-left:8px;"></div>
                        </div>
                        <div class="training-upload-preview">
                            <asp:Image ID="ImgTrainingUpload" runat="server" BorderWidth="0px" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-pic">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture Attachment</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <div id="divpic" class="form-group form-group-sm" style="height: 300px; text-align: center;">
                                <img id="ImgInstall" src="_blank" style="height: 100%; width: 100%;">
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-job_training">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Job Training</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="training_cust_job_training_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="clearModalBackdrop();">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Info Box</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" id="div_comment" runat="server">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" onclick="clearModalBackdrop();" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        function checkNbsp(sBuff) {
            var sOut = sBuff;
            if (sBuff == '&nbsp;') {
                sOut = '';
            }
            return sOut;
        }

        function clearModalBackdrop() {
            var $ = window.jQuery;
            if (!$) return;
            $('.modal-backdrop').remove();
            $('body').removeClass('modal-open').css('padding-right', '');
        }

        function hasCommentMessage(el) {
            if (!el) {
                return false;
            }
            var html = el.innerHTML.replace(/&nbsp;/gi, ' ').replace(/<br\s*\/?>/gi, ' ').trim();
            if (html === '') {
                return false;
            }
            return el.querySelector('.alert') !== null || el.textContent.replace(/\s+/g, '').length > 0;
        }

        function getCategoryValue() {
            var category = document.getElementById('ContentPlaceHolder1_CmbTrainCategoryID');
            return category ? (category.value || '').trim() : '';
        }

        function isCategorySelected() {
            var value = getCategoryValue();
            return value !== '' && value !== '[Select]';
        }

        function showFunctionModal() {
            var $ = window.jQuery;
            if ($ && typeof $.fn.modal === 'function') {
                $('#modal-function').modal('show');
            }
            return false;
        }

        function confirmSubmit() {
            if (!isCategorySelected()) {
                alert('Please select Category');
                return false;
            }
            var $ = window.jQuery;
            if ($ && typeof $.fn.modal === 'function') {
                $('#modal-submit').modal('show');
            }
            return false;
        }

        function confirmAddNote() {
            if (!isCategorySelected()) {
                alert('Please select Category');
                return false;
            }
            var $ = window.jQuery;
            if ($ && typeof $.fn.modal === 'function') {
                $('#modal-remark').modal('show');
            }
            return false;
        }

        // Do not disable the Yes button before __doPostBack (breaks UpdatePanel / WebView).
        function prepareConfirmPostback(loaderId) {
            var $ = window.jQuery;
            if ($) {
                if (loaderId) {
                    $('#' + loaderId).show();
                }
                $('button.close').hide();
            }
        }

        function postPic(picFileName) {
            if (picFileName != '') {
                var $ = window.jQuery;
                if ($ && typeof $.fn.modal === 'function') {
                    $('#modal-pic').modal('show');
                }
                var img1 = document.getElementById('ImgInstall');
                if (img1) {
                    img1.src = "Picture/" + picFileName;
                }
            }
        }

        function postJobTrainingChild(sTrainingID, sReqDate, sFullName, sSchDate, objCmd, sCustID, sCustBranchName, sBusinessFieldID) {
            if (sTrainingID != '') {
                document.getElementById('ContentPlaceHolder1_txtTrainingID').value = checkNbsp(sTrainingID);
                document.getElementById('ContentPlaceHolder1_txtReqDate').value = checkNbsp(sReqDate);
                document.getElementById('ContentPlaceHolder1_txtCustName').value = checkNbsp(sFullName);
                document.getElementById('ContentPlaceHolder1_txtSchDate').value = checkNbsp(sSchDate);
                document.getElementById('ContentPlaceHolder1_txtCustBranchName').value = checkNbsp(sCustBranchName);
                document.getElementById('ContentPlaceHolder1_txtCustomerName').value = checkNbsp(sFullName);
                document.getElementById('ContentPlaceHolder1_txtCustID').value = checkNbsp(sCustID);
                var cmbBiz = document.getElementById('ContentPlaceHolder1_CmbBusinessField');
                if (cmbBiz) {
                    cmbBiz.value = checkNbsp(sBusinessFieldID);
                }

                var $ = window.jQuery;
                if ($ && typeof $.fn.modal === 'function') {
                    $('#modal-job_training').modal('hide');
                }

                var objfr1 = document.getElementById('ContentPlaceHolder1_CmdLoadTraining');
                if (objfr1) {
                    objfr1.click();
                }
            }
        }

        function ensureFormVisible() {
            var $ = window.jQuery;
            if (!$) return;
            $('#box-jo, #box-customer, #box-installation').removeClass('collapsed-box');
            $('#box-jo .box-body, #box-customer .box-body, #box-installation .box-body').show();
        }

        function refreshUploadMsgStyle() {
            var lbMsg = document.getElementById('ContentPlaceHolder1_lblUploadMsg');
            if (!lbMsg) return;
            var isExists = lbMsg.innerHTML || '';
            if (isExists !== '') {
                if (isExists.indexOf('Success') >= 0) {
                    lbMsg.className = 'btn btn-success btn-xs';
                } else {
                    lbMsg.className = 'btn btn-danger btn-xs';
                }
            }
        }

        function endRequest(sender, args) {
            var $ = window.jQuery;
            if (!$ || typeof $.fn.modal !== 'function') {
                return;
            }

            ensureFormVisible();
            refreshUploadMsgStyle();

            $('#modal-submit').modal('hide');
            $('#modal-remark').modal('hide');
            clearModalBackdrop();
            $('.btn-confirmasi').prop('disabled', false);
            $('#iload, #iload2').hide();
            $('button.close').show();

            var commentEl = document.getElementById('ContentPlaceHolder1_div_comment');
            if (!hasCommentMessage(commentEl)) {
                return;
            }

            $('#modal-messagebox').off('hidden.bs.modal.vtsMessageBox').on('hidden.bs.modal.vtsMessageBox', function () {
                document.body.style.paddingRight = '0px';
                clearModalBackdrop();
            });
            $('#modal-messagebox').modal('show');
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        // Bootstrap is loaded after ContentPlaceHolder in Site.Master; defer initial run.
        if (window.jQuery) {
            jQuery(function () {
                ensureFormVisible();
            });
            jQuery(window).on('load', function () {
                endRequest();
            });
        }
    </script>
</asp:Content>
