<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="unblocked_cust.aspx.cs" Inherits="vtsadm.unblocked_cust" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .form-control {
            background-color: white !important;
        }

    </style>
    <section class="content-header">
        <h1>Customer
                <small>Unblocked</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Unblocked</a></li>
            <li class="active">Customer</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6 col-xs-12">
                <div id="box-jo" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Job Unblocked Information</h3>
                        <div class="box-tools pull-right">
                            <button id="btnid" type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Unblock ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtUnblockID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button5" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-job_unblocked"><i class="fa fa-search"></i></button>
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
                        <h3 class="box-title">Unblocked Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Unblocked Date</label>
                            <asp:TextBox ID="txtUnblockedDate" TextMode="Date" runat="server" class="form-control" placeholder="Unblocked Date ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <textarea id="txtRemark" runat="server" class="form-control" placeholder="Remark ..." rows="2"/>
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
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
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
                        <button type="button" class="btn btn-default btn-confirmasi" runat="server" onclick="$('.btn-confirmasi').attr('disabled','disabled');$('button.close').hide();$('#iload').show();" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
                        <button type="button" class="btn btn-primary btn-confirmasi" onclick="$('#modal-submit').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="unblocked_cust_upload.aspx" style="width: 100%; border: none; height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-job_unblocked">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Job Unblocked</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="unblocked_cust_job_unblock_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal modal-open fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="$('.modal-backdrop').remove();">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Info Box</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" id="div_comment" runat="server">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" onclick="$('.modal-backdrop').remove();" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function checkNbsp(sBuff) {
            var sOut
            sOut = sBuff;
            if (sBuff == '&nbsp;') {
                sOut = '';
            }
            return sOut;
        }

        function postJobUnblockChild(sUnblockID, sReqDate, sFullName, sSchDate, objCmd, sCustID, sCustBranchName) {
            if (sUnblockID != '') {
                document.getElementById('ContentPlaceHolder1_txtUnblockID').value = checkNbsp(sUnblockID);
                document.getElementById('ContentPlaceHolder1_txtReqDate').value = checkNbsp(sReqDate);
                document.getElementById('ContentPlaceHolder1_txtCustName').value = checkNbsp(sFullName);
                document.getElementById('ContentPlaceHolder1_txtSchDate').value = checkNbsp(sSchDate);
                document.getElementById('ContentPlaceHolder1_txtCustBranchName').value = checkNbsp(sCustBranchName);
                document.getElementById('ContentPlaceHolder1_txtCustomerName').value = checkNbsp(sFullName);

                $('#modal-job_unblocked').modal('hide');
            }
        }

        function endRequest(sender, args) {
            $('#modal-messagebox').on('hidden.bs.modal', function () {
                document.body.style.paddingRight = '0px';
            });
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
                $('#modal-messagebox').modal('show');
            }
        }
        endRequest();
    </script>
</asp:Content>
