<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="query_editor.aspx.cs" Inherits="vtsadm.query_editor" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="https://unpkg.com/bootstrap-table@1.21.4/dist/bootstrap-table.min.css" rel="stylesheet">
    <style>
        .textarea-query {
            font-size: 20px !important;
            font-weight: bold;
        }
    </style>
    <section class="content-header">
        <h1>Query Editor           
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Query</a></li>
            <li class="active">Post</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Execute/Select</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <div class="row">
                                <div class="col-md-3">
                                    <label>Pilih Server Database</label>
                                </div>
                                <div class="col-md-9">
                                    <select id="pilihDatabase" class="form-control" style="width: 100%">
                                        <option value="" selected="">Database Name</option>
                                        <option value="GPSB">GPSB</option>
                                        <option value="VTS_Admin">VTS_Admin</option>
                                        <option value="VRP">VRP</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <div class="row">
                                <div class="col-md-12">
                                    <input runat="server" id="querySQL" type="hidden" />
                                    <textarea id="txtQueryEditor" class="form-control textarea-query" rows="10"></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="btn btn-sm btn-primary" id="submitQuery">Submit</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <div class="row">
                                <div class="col-md-12">
                                    <label>Result:</label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <div class="row">
                                <div class="col-md-12">
                                    <textarea id="txtQueryEditorResult" class="form-control textarea-query" rows="2"></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <table class="table table-sm table-bordered table-hover" style="width: 100%" id="tblMaster">
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                        </div>
                    </div>
                </div>
            </div>
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
            </div>
        </div>
    </section>
    <script src="https://cdn.jsdelivr.net/npm/tableexport.jquery.plugin@1.10.21/tableExport.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/tableexport.jquery.plugin@1.10.21/libs/jsPDF/jspdf.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/tableexport.jquery.plugin@1.10.21/libs/jsPDF-AutoTable/jspdf.plugin.autotable.js"></script>
    <script src="https://unpkg.com/bootstrap-table@1.21.4/dist/bootstrap-table.min.js"></script>
    <script src="https://unpkg.com/bootstrap-table@1.21.4/dist/bootstrap-table.min.js"></script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);
        window.onselect = selectText;
        var dataSelect = "";
        function endRequest(sender, args) {
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                $('#modal-messagebox').modal('show');
            }
        }

        function refreshSearch() {
            document.getElementById("ContentPlaceHolder1_CmdSearch").click();
            setTimeout(refreshSearch, 5000);
        }

        function selectText(e) {
            var obj = $(e.target);
            if (obj.attr('id') === 'txtQueryEditor') {
                dataSelect = "";
                start = e.target.selectionStart;
                end = e.target.selectionEnd;
                console.log(e.target.value.substring(start, end));
                dataSelect = e.target.value.substring(start, end);
            }

            //alert(e.target.value.substring(start, end));
        }

        function QueryData(query_text, server_name) {
            if (query_text === null || query_text === "") return;
            if (server_name === null || server_name === "") return;
            try {
                $.ajax({
                    type: "POST",
                    url: "query_editor.aspx/ExecuteData",
                    dataType: "json",
                    data: "{ 'query_text': '" + query_text + "', 'server_name': '" + server_name + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (r) {
                        var json = r.d;
                        var objData = JSON.parse(json);
                        var objColumns = [];
                        console.log(objData);
                        if (query_text.toString().toLocaleLowerCase().includes('select')) {
                            $.each(objData[0], function (k, v) {
                                console.log(k);
                                objColumns.push({ "field": "" + k.toString() + "", "title": "" + k.toString() + "", "sortable": true, "width": 50, "visible": true });
                            });
                            loadData(objData, objColumns);
                        } else {
                            $('#txtQueryEditorResult').val(json);
                        }

                        dataSelect = "";
                    },
                    error: function (e) {
                        return "";
                    }
                });
            }
            catch (error) {

            }
        }

        function loadData(data, columns) {
            var table = $('#tblMaster');
            table.bootstrapTable('destroy').bootstrapTable('load').bootstrapTable({
                exportTypes: ['json', 'csv', 'txt', 'doc', 'excel', 'xlsx', 'pdf'],
                data: data,
                buttonsClass: "btn btn-sm btn-info btn-pill btn-icon btn-elevate btn-elevate-air",
                searchOnEnterKey: true,
                searchAlign: "left",
                searchPlaceholder: "Search and Enter",
                toolbar: "#master-table-toolbar",
                toolbarAlign: "right",
                showColumns: true,
                showRefresh: true,
                showExport: true,
                showPrint: true,
                showToggle: true,
                search: true,
                clickToSelect: true,
                singleSelect: true,
                columns: [
                    columns
                ],
            });
            table.bootstrapTable('refresh');
        }

        $('#submitQuery').on('click', function (e) {
            var pilihDatabase = $('#pilihDatabase').val();
            console.log(dataSelect, pilihDatabase);
            QueryData(dataSelect, pilihDatabase);
        });
    </script>
</asp:Content>