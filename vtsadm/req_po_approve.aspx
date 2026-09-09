<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="req_po_approve.aspx.cs" Inherits="vtsadm.req_po_approve" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Purchase Order of Goods 
            <small>Aprrove</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Purchase Order of Goods</a></li>
            <li class="active">Approve</li>
        </ol>
    </section>

    <section class="content">
        <div class="modal modal-open fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
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
            </div>
        </div>
    </section>

    <script type="text/javascript">
        window.onload = function () {
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                $('#modal-messagebox').modal('show');
            }
        }
    </script>
</asp:Content>
