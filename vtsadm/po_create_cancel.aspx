<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="po_create_cancel.aspx.cs" Inherits="vtsadm.po_create_cancel" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Sales Order 
            <small>Cancel</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Sales Order</a></li>
            <li class="active">Cancel</li>
        </ol>
    </section>

    <section class="content">
        <div class="modal modal-open fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>--%>
                        <h4 class="modal-title">Info Box</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" id="div_comment" runat="server">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <input type="button" class="btn btn-default pull-left" onclick="location.href='dashboard.aspx';" value="Close" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

    </section>

    <script type="text/javascript">
        //window.onload = function () {
        //    document.getElementById('ContentPlaceHolder1_div_comment').innerHTML = '<div class="alert alert-success" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Hi There !</strong> We just wanted to let you know your sales order  on has been approved</div>';
        //    $('#modal-messagebox').modal('show');
        //}
        window.onload = function () {
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                $('#modal-messagebox').modal('show');
            }
        }
    </script>
</asp:Content>
