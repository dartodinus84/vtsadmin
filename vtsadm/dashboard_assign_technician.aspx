<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_assign_technician.aspx.cs" Inherits="vtsadm.dashboard_assign_technician" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .icon {
            top: 0px !important;
            font-size: 40px !important;
        }

        .inner {
            height: 80px !important;
            padding-top: 2px !important;
        }



        .small-box-footer {
            border-bottom-left-radius: 11px;
            border-bottom-right-radius: 11px;
        }

        .small-box {
            border-radius: 11px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .box {
            border-radius: 11px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .widget-user-2 .widget-user-header {
            border-top-left-radius: 11px;
            border-top-right-radius: 11px;
            height: 60px;
            padding-top: 10px;
        }

        .box-header {
            font-size: 16px;
        }

        li-format {
            height: 45px !important;
        }
    </style>

    <section class="content-header">
        <h1>Dashboard
        <small>Job Order</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="#"><i class="fa fa-dashboard"></i>Job Order</a></li>
        </ol>
    </section>


    <section class="content">

        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Search Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Area</label>
                            <asp:DropDownList ID="CmbFilterType" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Date From</label>
                            <asp:TextBox ID="txtDateFrom" TextMode="Date" runat="server" class="form-control" placeholder="Input date from ..."></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Date To</label>
                            <asp:TextBox ID="txtDateTo" TextMode="Date" runat="server" class="form-control" placeholder="Input date to ..."></asp:TextBox>
                        </div>

                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <input type="button" class="btn btn-primary" value="Search" onclick="changeSearch()">
                    </div>
                </div>
            </div>
        </div>

        
        <div class="row">

            <div class="col-lg-6 col-xs-12">
                <div class="card-body table-responsive p-0">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">Job Order -  New Installation</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->

                        <div class="box-body">
                            <div class="table-responsive">
                                <div id="TableAssignNew"></div>
                            </div>
                            <!-- /.table-responsive -->
                        </div>
                        <div class="box-footer">
                            <button type="button" id="ExportExcelJobNew" class="btn btn-primary">Export Excel</button>
                      
                        </div>
                        <!-- /.box-body -->
                        
                    </div>
                    <!-- /.box -->
                </div>
            </div>

            <div class="col-lg-6 col-xs-12">
                <div class="card-body table-responsive p-0">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">Job Order -  Maintenance</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->

                        <div class="box-body">
                            <div class="table-responsive">
                                <div id="TableAssignMaint"></div>
                            </div>
                            <!-- /.table-responsive -->
                        </div>
                        <div class="box-footer">
                            <button type="button" id="ExportExcelJobMaint" class="btn btn-primary">Export Excel</button>
                      
                        </div>
                        <!-- /.box-body -->
                        
                    </div>
                    <!-- /.box -->
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-lg-6 col-xs-12">
                <div class="card-body table-responsive p-0">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">Job Order Assign PIC Area -  New Installation</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->

                        <div class="box-body">
                            <div class="table-responsive">
                                <div id="TableAssignTechicianNew"></div>
                            </div>
                            <!-- /.table-responsive -->
                        </div>
                        <div class="box-footer">
                            <button type="button" id="ExportExcelAssNew" class="btn btn-primary">Export Excel</button>
                      
                        </div>
                        <!-- /.box-body -->
                        
                    </div>
                    <!-- /.box -->
                </div>
            </div>

            <div class="col-lg-6 col-xs-12">
                <div class="card-body table-responsive p-0">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">Job Order Assign PIC Area - Maintenance</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->

                        <div class="box-body">
                            <div class="table-responsive">
                                <div id="TableAssignTechicianMaint"></div>
                            </div>
                            <!-- /.table-responsive -->
                        </div>
                        <div class="box-footer">
                            <button type="button" id="ExportExcelAssMaint" class="btn btn-primary">Export Excel</button>
                      
                        </div>
                        <!-- /.box-body -->
                        
                    </div>
                    <!-- /.box -->
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-lg-6 col-xs-12">
                <div class="card-body table-responsive p-0">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">Job Order Assign Technician -  New Installation</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->

                        <div class="box-body">
                            <div class="table-responsive">
                                <div id="TableAssignbyTechicianNew"></div>
                            </div>
                            <!-- /.table-responsive -->
                        </div>
                        <div class="box-footer">
                            <button type="button" id="ExportExcelAssTechNew" class="btn btn-primary">Export Excel</button>
                      
                        </div>
                        <!-- /.box-body -->
                        
                    </div>
                    <!-- /.box -->
                </div>
            </div>

            <div class="col-lg-6 col-xs-12">
                <div class="card-body table-responsive p-0">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">Job Order Assign Technician - Maintenance</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->

                        <div class="box-body">
                            <div class="table-responsive">
                                <div id="TableAssignbyTechicianMaint"></div>
                            </div>
                            <!-- /.table-responsive -->
                        </div>
                        <div class="box-footer">
                            <button type="button" id="ExportExcelAssTechMaint" class="btn btn-primary">Export Excel</button>
                      
                        </div>
                        <!-- /.box-body -->
                        
                    </div>
                    <!-- /.box -->
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

    <script src="Scripts/tableExport/js-xlsx/xlsx.core.min.js"></script>
    <script src="Scripts/tableExport/FileSaver/FileSaver.min.js"></script>
    <script src="Scripts/tableExport/tableExport.min.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $('#ExportExcelJobNew').click(function () {

                $('#TableAssignNew').tableExport({
                    type: 'excel',
                    fileName: 'JobOrderNewInstallation',
                    htmlContent: true,
                    mso: {
                        fileFormat: 'xlsx'
                    }
                });
            });

            $('#ExportExcelJobMaint').click(function () {

                $('#TableAssignMaint').tableExport({
                    type: 'excel',
                    fileName: 'JobOrderMaintenance',
                    htmlContent: true,
                    mso: {
                        fileFormat: 'xlsx'
                    }
                });
            });

            $('#ExportExcelAssNew').click(function () {

                $('#TableAssignTechicianNew').tableExport({
                    type: 'excel',
                    fileName: 'JobOrderAssignTechnicianNewInstallation',
                    htmlContent: true,
                    mso: {
                        fileFormat: 'xlsx'
                    }
                });
            });

            $('#ExportExcelAssMaint').click(function () {

                $('#TableAssignTechicianMaint').tableExport({
                    type: 'excel',
                    fileName: 'JobOrderAssignTechnicianMaintenance',
                    htmlContent: true,
                    mso: {
                        fileFormat: 'xlsx'
                    }
                });
            });


            $('#ExportExcelAssTechNew').click(function () {

                $('#TableAssignbyTechicianNew').tableExport({
                    type: 'excel',
                    fileName: 'JobOrderAssignTechnicianNewInstallation',
                    htmlContent: true,
                    mso: {
                        fileFormat: 'xlsx'
                    }
                });
            });

            $('#ExportExcelAssTechMaint').click(function () {

                $('#TableAssignbyTechicianMaint').tableExport({
                    type: 'excel',
                    fileName: 'JobOrderAssignTechnicianMaintenance',
                    htmlContent: true,
                    mso: {
                        fileFormat: 'xlsx'
                    }
                });
            });

            
        });

        

        changeSearch();

        function changeSearch() {
            
            var areaid = $('#ContentPlaceHolder1_CmbFilterType').val();
            var from = $('#ContentPlaceHolder1_txtDateFrom').val();
            var to = $('#ContentPlaceHolder1_txtDateTo').val();

            if (areaid === "[Select]") {
                areaid = "";
            }

            var objvar = {
                "areaid": areaid.toString(),
                "from": from.toString(),
                "to": to.toString()
            };

            $("#overlay").fadeIn(300);
            $.ajax({
                type: "POST",
                url: "dashboard_assign_technician.aspx/dashboardassignnew",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(objvar),
                success: function (r) {
                    var json = JSON.parse(r.d);
                    var result = json.data;

                    var table_body = '<table class="table no-margin"><thead>';
                    table_body += '<tr>';
                    table_body += '<th>Branch</th>';
                    table_body += '<th>Total (JO)</th>';
                    table_body += '<th>Open (JO)</th>';
                    table_body += '<th>Close (JO)</th>';
                    table_body += '<th>Total (Unit)</th>';
                    table_body += '<th>Open (Unit)</th>';
                    table_body += '<th>Close (Unit)</th></tr></thead><tbody>';
                    if (result.length > 0) {
                        var SumCntJoNewTotal = 0;
                        var SumCntJoNewOpen = 0;
                        var SumCntJoNewClose = 0;
                        var SumCntJoNewTotalUnit = 0;
                        var SumCntJoNewOpenUnit = 0;
                        var SumCntJoNewCloseUnit = 0;


                        for (var i = 0; i < result.length; i++) {
                            table_body += '<tr>';
                            table_body += '<td>' + result[i].BranchName + '</td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoNewTotal + '"><a href="dashboard_job_technician_detail?type=jobnew&branchid=' + result[i].BranchID + '&from=' + from + '&to=' + to +'&area=' + areaid +'"><span class="badge bg-aqua">' + result[i].CntJoNewTotal + '</span></a></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoNewOpen + '"><a href="dashboard_job_technician_detail?type=jobnewop&branchid=' + result[i].BranchID + '&from=' + from + '&to=' + to +'&area=' + areaid +'"><span class="badge bg-yellow">' + result[i].CntJoNewOpen + '</span></a></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoNewClose + '"><a href="dashboard_job_technician_detail?type=jobnewcl&branchid=' + result[i].BranchID + '&from=' + from + '&to=' + to +'&area=' + areaid +'"><span class="badge bg-red">' + result[i].CntJoNewClose + '</span></a></td>';

                            table_body += '<td data-tableexport-value="' + result[i].CntJoNewTotalUnit + '"><span class="badge bg-aqua">' + result[i].CntJoNewTotalUnit + '</span></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoNewOpenUnit + '"><span class="badge bg-yellow">' + result[i].CntJoNewOpenUnit + '</span></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoNewCloseUnit + '"><span class="badge bg-red">' + result[i].CntJoNewCloseUnit + '</span></td>';
                            table_body += '</tr>';

                            var CntJoNewTotal = parseInt(result[i].CntJoNewTotal.replace(/.(?=\d{3})/g, ''));
                            var CntJoNewOpen = parseInt(result[i].CntJoNewOpen.replace(/.(?=\d{3})/g, ''));
                            var CntJoNewClose = parseInt(result[i].CntJoNewClose.replace(/.(?=\d{3})/g, ''));

                            var CntJoNewTotalUnit = parseInt(result[i].CntJoNewTotalUnit.replace(/.(?=\d{3})/g, ''));
                            var CntJoNewOpenUnit = parseInt(result[i].CntJoNewOpenUnit.replace(/.(?=\d{3})/g, ''));
                            var CntJoNewCloseUnit = parseInt(result[i].CntJoNewCloseUnit.replace(/.(?=\d{3})/g, ''));


                            SumCntJoNewTotal = parseInt(SumCntJoNewTotal) + CntJoNewTotal;
                            SumCntJoNewOpen = parseInt(SumCntJoNewOpen) + CntJoNewOpen;
                            SumCntJoNewClose = parseInt(SumCntJoNewClose) + CntJoNewClose;

                            SumCntJoNewTotalUnit = parseInt(SumCntJoNewTotalUnit) + CntJoNewTotalUnit;
                            SumCntJoNewOpenUnit = parseInt(SumCntJoNewOpenUnit) + CntJoNewOpenUnit;
                            SumCntJoNewCloseUnit = parseInt(SumCntJoNewCloseUnit) + CntJoNewCloseUnit;
                        }

                        table_body += '<tr>';
                        table_body += '<td colspan="1" align="center"><b>Total</b></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoNewTotal + '"><a href="dashboard_job_technician_detail?type=jobnewall&from=' + from + '&to=' + to +'&area=' + areaid +'"><span class="badge bg-purple">' + SumCntJoNewTotal + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoNewOpen + '"><a href="dashboard_job_technician_detail?type=jobnewopall&from=' + from + '&to=' + to +'&area=' + areaid +'"><span class="badge bg-purple">' + SumCntJoNewOpen + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoNewClose + '"><a href="dashboard_job_technician_detail?type=jobnewclall&from=' + from + '&to=' + to +'&area=' + areaid +'"><span class="badge bg-purple">' + SumCntJoNewClose + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoNewTotalUnit + '"><span class="badge bg-purple">' + SumCntJoNewTotalUnit + '</span></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoNewOpenUnit + '"><span class="badge bg-purple">' + SumCntJoNewOpenUnit + '</span></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoNewCloseUnit + '"><span class="badge bg-purple">' + SumCntJoNewCloseUnit + '</span></td>';
                        table_body += '</tr>';

                    } else {
                        table_body += '<tr>';
                        table_body += '<td colspan="7" align="center">Data is empty</td>';
                        table_body += '</tr>';
                    }

                    table_body += '</tbody></table>';
                    $('#TableAssignNew').html(table_body);
                    result = [];
                    
                },
                error: function (e) {
                    console.log(e);
                },
                complete: function () {
                    setTimeout(function(){
                        $("#overlay").fadeOut(300);
                    },500);
                }
            });

            /** JOB MAINTENANCE */
            $.ajax({
                type: "POST",
                url: "dashboard_assign_technician.aspx/dashboardassignmaint",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(objvar),
                success: function (r) {
                    var json = JSON.parse(r.d);
                    var result = json.data;

                    var table_body = '<table class="table no-margin"><thead>';
                    table_body += '<tr>';
                    table_body += '<th>Branch</th>';
                    table_body += '<th>Total (JO)</th>';
                    table_body += '<th>Open (JO)</th>';
                    table_body += '<th>Close (JO)</th>';
                    table_body += '<th>Total (Unit)</th>';
                    table_body += '<th>Open (Unit)</th>';
                    table_body += '<th>Close (Unit)</th></tr></thead><tbody>';
                    if (result.length > 0) {
                        var SumCntJoMaintTotal = 0;
                        var SumCntJoMaintOpen = 0;
                        var SumCntJoMaintClose = 0;
                        var SumCntJoMaintTotalUnit = 0;
                        var SumCntJoMaintOpenUnit = 0;
                        var SumCntJoMaintCloseUnit = 0;

                        for (var i = 0; i < result.length; i++) {
                            table_body += '<tr>';
                            table_body += '<td>' + result[i].BranchName + '</td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoMaintTotal + '"><a href="dashboard_job_technician_detail?type=jobmaint&branchid=' + result[i].BranchID + '&from=' + from + '&to=' + to +'&area=' + areaid +'"><span class="badge bg-aqua">' + result[i].CntJoMaintTotal + '</span></a></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoMaintOpen + '"><a href="dashboard_job_technician_detail?type=jobmaintop&branchid=' + result[i].BranchID + '&from=' + from + '&to=' + to +'&area=' + areaid +'"><span class="badge bg-yellow">' + result[i].CntJoMaintOpen + '</span></a></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoMaintClose + '"><a href="dashboard_job_technician_detail?type=jobmaintcl&branchid=' + result[i].BranchID + '&from=' + from + '&to=' + to +'&area=' + areaid +'"><span class="badge bg-red">' + result[i].CntJoMaintClose + '</span></a></td>';

                            table_body += '<td data-tableexport-value="' + result[i].CntJoMaintTotalUnit + '"><span class="badge bg-aqua">' + result[i].CntJoMaintTotalUnit + '</span></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoMaintOpenUnit + '"><span class="badge bg-yellow">' + result[i].CntJoMaintOpenUnit + '</span></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoMaintCloseUnit + '"><span class="badge bg-red">' + result[i].CntJoMaintCloseUnit + '</span></td>';
                            table_body += '</tr>';

                            var CntJoMaintTotal = parseInt(result[i].CntJoMaintTotal.replace(/.(?=\d{3})/g, ''));
                            var CntJoMaintOpen = parseInt(result[i].CntJoMaintOpen.replace(/.(?=\d{3})/g, ''));
                            var CntJoMaintClose = parseInt(result[i].CntJoMaintClose.replace(/.(?=\d{3})/g, ''));

                            var CntJoMaintTotalUnit = parseInt(result[i].CntJoMaintTotalUnit.replace(/.(?=\d{3})/g, ''));
                            var CntJoMaintOpenUnit = parseInt(result[i].CntJoMaintOpenUnit.replace(/.(?=\d{3})/g, ''));
                            var CntJoMaintCloseUnit = parseInt(result[i].CntJoMaintCloseUnit.replace(/.(?=\d{3})/g, ''));

                            SumCntJoMaintTotal = parseInt(SumCntJoMaintTotal) + CntJoMaintTotal;
                            SumCntJoMaintOpen = parseInt(SumCntJoMaintOpen) + CntJoMaintOpen;
                            SumCntJoMaintClose = parseInt(SumCntJoMaintClose) + CntJoMaintClose;

                            SumCntJoMaintTotalUnit = parseInt(SumCntJoMaintTotalUnit) + CntJoMaintTotalUnit;
                            SumCntJoMaintOpenUnit = parseInt(SumCntJoMaintOpenUnit) + CntJoMaintOpenUnit;
                            SumCntJoMaintCloseUnit = parseInt(SumCntJoMaintCloseUnit) + CntJoMaintCloseUnit;
                        }

                        table_body += '<tr>';
                        table_body += '<td colspan="1" align="center"><b>Total</b></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoMaintTotal + '"><a href="dashboard_job_technician_detail?type=jobmaintall&from=' + from + '&to=' + to +'&area=' + areaid +'"><span class="badge bg-purple">' + SumCntJoMaintTotal + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoMaintOpen + '"><a href="dashboard_job_technician_detail?type=jobmaintopall&from=' + from + '&to=' + to +'&area=' + areaid +'"><span class="badge bg-purple">' + SumCntJoMaintOpen + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoMaintClose + '"><a href="dashboard_job_technician_detail?type=jobmaintclall&from=' + from + '&to=' + to +'&area=' + areaid +'"><span class="badge bg-purple">' + SumCntJoMaintClose + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoMaintTotalUnit + '"><span class="badge bg-purple">' + SumCntJoMaintTotalUnit + '</span></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoMaintOpenUnit + '"><span class="badge bg-purple">' + SumCntJoMaintOpenUnit + '</span></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoMaintCloseUnit + '"><span class="badge bg-purple">' + SumCntJoMaintCloseUnit + '</span></td>';
                        table_body += '</tr>';

                    } else {
                        table_body += '<tr>';
                        table_body += '<td colspan="7" align="center">Data is empty</td>';
                        table_body += '</tr>';
                    }

                    table_body += '</tbody></table>';
                    $('#TableAssignMaint').html(table_body);
                    result = [];
                },
                complete: function (xhr, status) {
                },
                error: function (e) {
                    console.log(e);
                }
            });


            $.ajax({
                type: "POST",
                url: "dashboard_assign_technician.aspx/dashboardassigntechniciannew",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(objvar),
                success: function (r) {
                    var json = JSON.parse(r.d);
                    var result = json.data;

                    var table_body = '<table class="table no-margin"><thead>';
                    table_body += '<tr>';
                    table_body += '<th>PIC</th>';
                    table_body += '<th>Total</th>';
                    table_body += '<th>Open</th>';
                    table_body += '<th>Close</th></tr></thead><tbody>';
                    if (result.length > 0) {
                        var SumCntJoNewAssignTotal = 0;
                        var SumCntJoNewAssignOpen = 0;
                        var SumCntJoNewAssignClose = 0;

                        for (var i = 0; i < result.length; i++) {
                            table_body += '<tr>';
                            table_body += '<td>' + result[i].JoNewTechnicianName + '</td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoNewAssignTotal + '"><a href="dashboard_assign_technician_detail?type=assignnew&from=' + from + '&to=' + to +'&pic=' + result[i].PicAreaID + '&area=' + result[i].AreaID + '"><span class="badge bg-aqua">' + result[i].CntJoNewAssignTotal + '</span></a></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoNewAssignOpen + '"><a href="dashboard_assign_technician_detail?type=assignnewop&from=' + from + '&to=' + to +'&pic=' + result[i].PicAreaID + '&area=' + result[i].AreaID + '"><span class="badge bg-yellow">' + result[i].CntJoNewAssignOpen + '</span></a></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoNewAssignClose + '"><a href="dashboard_assign_technician_detail?type=assignnewcl&from=' + from + '&to=' + to +'&pic=' + result[i].PicAreaID + '&area=' + result[i].AreaID + '"><span class="badge bg-red">' + result[i].CntJoNewAssignClose + '</span></a></td>';
                            table_body += '</tr>';

                            var CntJoNewAssignTotal = parseInt(result[i].CntJoNewAssignTotal.replace(/.(?=\d{3})/g, ''));
                            var CntJoNewAssignOpen = parseInt(result[i].CntJoNewAssignOpen.replace(/.(?=\d{3})/g, ''));
                            var CntJoNewAssignClose = parseInt(result[i].CntJoNewAssignClose.replace(/.(?=\d{3})/g, ''));

                            SumCntJoNewAssignTotal = parseInt(SumCntJoNewAssignTotal) + CntJoNewAssignTotal;
                            SumCntJoNewAssignOpen = parseInt(SumCntJoNewAssignOpen) + CntJoNewAssignOpen;
                            SumCntJoNewAssignClose = parseInt(SumCntJoNewAssignClose) + CntJoNewAssignClose;
                        }

                        table_body += '<tr>';
                        table_body += '<td colspan="1" align="center"><b>Total</b></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoNewAssignTotal + '"><a href="dashboard_assign_technician_detail?type=assignnewall&from=' + from + '&to=' + to +'"><span class="badge bg-purple">' + SumCntJoNewAssignTotal + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoNewAssignOpen + '"><a href="dashboard_assign_technician_detail?type=assignnewopall&from=' + from + '&to=' + to +'"><span class="badge bg-purple">' + SumCntJoNewAssignOpen + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoNewAssignClose + '"><a href="dashboard_assign_technician_detail?type=assignnewclall&from=' + from + '&to=' + to +'"><span class="badge bg-purple">' + SumCntJoNewAssignClose + '</span></a></td>';
                        table_body += '</tr>';

                    } else {
                        table_body += '<tr>';
                        table_body += '<td colspan="4" align="center">Data is empty</td>';
                        table_body += '</tr>';
                    }

                    table_body += '</tbody></table>';
                    $('#TableAssignTechicianNew').html(table_body);
                    result = [];

                },
                error: function (e) {
                    console.log(e);
                }
            });


            $.ajax({
                type: "POST",
                url: "dashboard_assign_technician.aspx/dashboardassigntechnicianmaint",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(objvar),
                success: function (r) {
                    var json = JSON.parse(r.d);
                    var result = json.data;

                    var table_body = '<table class="table no-margin"><thead>';
                    table_body += '<tr>';
                    table_body += '<th>PIC</th>';
                    table_body += '<th>Total</th>';
                    table_body += '<th>Open</th>';
                    table_body += '<th>Close</th></tr></thead><tbody>';
                    if (result.length > 0) {
                        var SumCntJoMaintAssignTotal = 0;
                        var SumCntJoMaintAssignOpen = 0;
                        var SumCntJoMaintAssignClose = 0;

                        for (var i = 0; i < result.length; i++) {
                            table_body += '<tr>';
                            table_body += '<td>' + result[i].JoMaintTechnicianName + '</td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoMaintAssignTotal + '"><a href="dashboard_assign_technician_detail?type=assignmaint&from=' + from + '&to=' + to +'&pic=' + result[i].PicAreaID + '&area=' + result[i].AreaID + '"><span class="badge bg-aqua">' + result[i].CntJoMaintAssignTotal + '</span></a></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoMaintAssignOpen + '"><a href="dashboard_assign_technician_detail?type=assignmaintop&from=' + from + '&to=' + to +'&pic=' + result[i].PicAreaID + '&area=' + result[i].AreaID + '"><span class="badge bg-yellow">' + result[i].CntJoMaintAssignOpen + '</span></a></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoMaintAssignClose + '"><a href="dashboard_assign_technician_detail?type=assignmaintcl&from=' + from + '&to=' + to +'&pic=' + result[i].PicAreaID + '&area=' + result[i].AreaID + '"><span class="badge bg-red">' + result[i].CntJoMaintAssignClose + '</span></a></td>';
                            table_body += '</tr>';

                            
                            var CntJoMaintAssignTotal = parseInt(result[i].CntJoMaintAssignTotal.replace(/.(?=\d{3})/g, ''));
                            var CntJoMaintAssignOpen = parseInt(result[i].CntJoMaintAssignOpen.replace(/.(?=\d{3})/g, ''));
                            var CntJoMaintAssignClose = parseInt(result[i].CntJoMaintAssignClose.replace(/.(?=\d{3})/g, ''));

                            

                            SumCntJoMaintAssignTotal = parseInt(SumCntJoMaintAssignTotal) + CntJoMaintAssignTotal;
                            SumCntJoMaintAssignOpen = parseInt(SumCntJoMaintAssignOpen) + CntJoMaintAssignOpen;
                            SumCntJoMaintAssignClose = parseInt(SumCntJoMaintAssignClose) + CntJoMaintAssignClose;

                        }

                        table_body += '<tr>';
                        table_body += '<td colspan="1" align="center"><b>Total</b></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoMaintAssignTotal + '"><a href="dashboard_assign_technician_detail?type=assignmaintall&from=' + from + '&to=' + to +'"><span class="badge bg-purple">' + SumCntJoMaintAssignTotal + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoMaintAssignOpen + '"><a href="dashboard_assign_technician_detail?type=assignmaintopall&from=' + from + '&to=' + to +'"><span class="badge bg-purple">' + SumCntJoMaintAssignOpen + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoMaintAssignClose + '"><a href="dashboard_assign_technician_detail?type=assignmaintclall&from=' + from + '&to=' + to +'"><span class="badge bg-purple">' + SumCntJoMaintAssignClose + '</span></a></td>';
                        table_body += '</tr>';

                    } else {
                        table_body += '<tr>';
                        table_body += '<td colspan="4" align="center">Data is empty</td>';
                        table_body += '</tr>';
                    }

                    table_body += '</tbody></table>';
                    $('#TableAssignTechicianMaint').html(table_body);
                    result = [];

                },
                error: function (e) {
                    console.log(e);
                }
            });

            /** GROUP BY TECHNICIAN NAME*/
            $.ajax({
                type: "POST",
                url: "dashboard_assign_technician.aspx/dashboardassignbytechniciannew",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(objvar),
                success: function (r) {
                    var json = JSON.parse(r.d);
                    var result = json.data;

                    var table_body = '<table class="table no-margin"><thead>';
                    table_body += '<tr>';
                    table_body += '<th>Technician</th>';
                    table_body += '<th>Total</th>';
                    table_body += '<th>Open</th>';
                    table_body += '<th>Close</th></tr></thead><tbody>';
                    if (result.length > 0) {
                        var SumCntJoNewAssignTotal = 0;
                        var SumCntJoNewAssignOpen = 0;
                        var SumCntJoNewAssignClose = 0;

                        for (var i = 0; i < result.length; i++) {
                            table_body += '<tr>';
                            table_body += '<td>' + result[i].JoNewTechnicianName + '</td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoNewAssignTotal + '"><a href="dashboard_assign_bytechnician_detail?type=assignnew&from=' + from + '&to=' + to +'&pic=' + result[i].PicAreaID + '&area=' + result[i].AreaID + '"><span class="badge bg-aqua">' + result[i].CntJoNewAssignTotal + '</span></a></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoNewAssignOpen + '"><a href="dashboard_assign_bytechnician_detail?type=assignnewop&from=' + from + '&to=' + to +'&pic=' + result[i].PicAreaID + '&area=' + result[i].AreaID + '"><span class="badge bg-yellow">' + result[i].CntJoNewAssignOpen + '</span></a></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoNewAssignClose + '"><a href="dashboard_assign_bytechnician_detail?type=assignnewcl&from=' + from + '&to=' + to +'&pic=' + result[i].PicAreaID + '&area=' + result[i].AreaID + '"><span class="badge bg-red">' + result[i].CntJoNewAssignClose + '</span></a></td>';
                            table_body += '</tr>';

                            var CntJoNewAssignTotal = parseInt(result[i].CntJoNewAssignTotal.replace(/.(?=\d{3})/g, ''));
                            var CntJoNewAssignOpen = parseInt(result[i].CntJoNewAssignOpen.replace(/.(?=\d{3})/g, ''));
                            var CntJoNewAssignClose = parseInt(result[i].CntJoNewAssignClose.replace(/.(?=\d{3})/g, ''));

                            SumCntJoNewAssignTotal = parseInt(SumCntJoNewAssignTotal) + CntJoNewAssignTotal;
                            SumCntJoNewAssignOpen = parseInt(SumCntJoNewAssignOpen) + CntJoNewAssignOpen;
                            SumCntJoNewAssignClose = parseInt(SumCntJoNewAssignClose) + CntJoNewAssignClose;
                        }

                        table_body += '<tr>';
                        table_body += '<td colspan="1" align="center"><b>Total</b></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoNewAssignTotal + '"><a href="dashboard_assign_bytechnician_detail?type=assignnewall&from=' + from + '&to=' + to +'"><span class="badge bg-purple">' + SumCntJoNewAssignTotal + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoNewAssignOpen + '"><a href="dashboard_assign_bytechnician_detail?type=assignnewopall&from=' + from + '&to=' + to +'"><span class="badge bg-purple">' + SumCntJoNewAssignOpen + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoNewAssignClose + '"><a href="dashboard_assign_bytechnician_detail?type=assignnewclall&from=' + from + '&to=' + to +'"><span class="badge bg-purple">' + SumCntJoNewAssignClose + '</span></a></td>';
                        table_body += '</tr>';

                    } else {
                        table_body += '<tr>';
                        table_body += '<td colspan="4" align="center">Data is empty</td>';
                        table_body += '</tr>';
                    }

                    table_body += '</tbody></table>';
                    $('#TableAssignbyTechicianNew').html(table_body);
                    result = [];

                },
                error: function (e) {
                    console.log(e);
                }
            });


            $.ajax({
                type: "POST",
                url: "dashboard_assign_technician.aspx/dashboardassignbytechnicianmaint",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(objvar),
                success: function (r) {
                    var json = JSON.parse(r.d);
                    var result = json.data;

                    var table_body = '<table class="table no-margin"><thead>';
                    table_body += '<tr>';
                    table_body += '<th>Technician</th>';
                    table_body += '<th>Total</th>';
                    table_body += '<th>Open</th>';
                    table_body += '<th>Close</th></tr></thead><tbody>';
                    if (result.length > 0) {
                        var SumCntJoMaintAssignTotal = 0;
                        var SumCntJoMaintAssignOpen = 0;
                        var SumCntJoMaintAssignClose = 0;

                        for (var i = 0; i < result.length; i++) {
                            table_body += '<tr>';
                            table_body += '<td>' + result[i].JoMaintTechnicianName + '</td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoMaintAssignTotal + '"><a href="dashboard_assign_bytechnician_detail?type=assignmaint&from=' + from + '&to=' + to +'&pic=' + result[i].PicAreaID + '&area=' + result[i].AreaID + '"><span class="badge bg-aqua">' + result[i].CntJoMaintAssignTotal + '</span></a></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoMaintAssignOpen + '"><a href="dashboard_assign_bytechnician_detail?type=assignmaintop&from=' + from + '&to=' + to +'&pic=' + result[i].PicAreaID + '&area=' + result[i].AreaID + '"><span class="badge bg-yellow">' + result[i].CntJoMaintAssignOpen + '</span></a></td>';
                            table_body += '<td data-tableexport-value="' + result[i].CntJoMaintAssignClose + '"><a href="dashboard_assign_bytechnician_detail?type=assignmaintcl&from=' + from + '&to=' + to +'&pic=' + result[i].PicAreaID + '&area=' + result[i].AreaID + '"><span class="badge bg-red">' + result[i].CntJoMaintAssignClose + '</span></a></td>';
                            table_body += '</tr>';

                            
                            var CntJoMaintAssignTotal = parseInt(result[i].CntJoMaintAssignTotal.replace(/.(?=\d{3})/g, ''));
                            var CntJoMaintAssignOpen = parseInt(result[i].CntJoMaintAssignOpen.replace(/.(?=\d{3})/g, ''));
                            var CntJoMaintAssignClose = parseInt(result[i].CntJoMaintAssignClose.replace(/.(?=\d{3})/g, ''));

                            

                            SumCntJoMaintAssignTotal = parseInt(SumCntJoMaintAssignTotal) + CntJoMaintAssignTotal;
                            SumCntJoMaintAssignOpen = parseInt(SumCntJoMaintAssignOpen) + CntJoMaintAssignOpen;
                            SumCntJoMaintAssignClose = parseInt(SumCntJoMaintAssignClose) + CntJoMaintAssignClose;

                        }

                        table_body += '<tr>';
                        table_body += '<td colspan="1" align="center"><b>Total</b></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoMaintAssignTotal + '"><a href="dashboard_assign_bytechnician_detail?type=assignmaintall&from=' + from + '&to=' + to +'"><span class="badge bg-purple">' + SumCntJoMaintAssignTotal + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoMaintAssignOpen + '"><a href="dashboard_assign_bytechnician_detail?type=assignmaintopall&from=' + from + '&to=' + to +'"><span class="badge bg-purple">' + SumCntJoMaintAssignOpen + '</span></a></td>';
                        table_body += '<td data-tableexport-value="' + SumCntJoMaintAssignClose + '"><a href="dashboard_assign_bytechnician_detail?type=assignmaintclall&from=' + from + '&to=' + to +'"><span class="badge bg-purple">' + SumCntJoMaintAssignClose + '</span></a></td>';
                        table_body += '</tr>';

                    } else {
                        table_body += '<tr>';
                        table_body += '<td colspan="4" align="center">Data is empty</td>';
                        table_body += '</tr>';
                    }

                    table_body += '</tbody></table>';
                    $('#TableAssignbyTechicianMaint').html(table_body);
                    result = [];

                },
                error: function (e) {
                    console.log(e);
                }
            });

        }


    </script>

</asp:Content>
