<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_gsm.aspx.cs" Inherits="vtsadm.dashboard_gsm" EnableEventValidation="false" %>
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
        <small>Gsm</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="#"><i class="fa fa-dashboard"></i>Gsm</a></li>
        </ol>
    </section>


    <section class="content">
        <div class="row">
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-blue-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblGsmMW" runat="server">1,000</label>
                        </h3>
                        <p>Warehouse</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-building-o"></i>
                    </div>
                    <a href="rpt_sum_gsm.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-green-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblGsmMT" runat="server">1,000</label>
                        </h3>
                        <p>Technician</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-person"></i>
                    </div>
                    <a href="rpt_sum_gsm.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-yellow-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblGsmIS" runat="server">1,000</label>
                        </h3>
                        <p>Installed</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-check"></i>
                    </div>
                    <a href="rpt_sum_gsm.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>

                </div>
            </div>
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-maroon-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblGsmRS" runat="server">1,000</label>
                        </h3>
                        <p>Ready to Suspend</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-warning"></i>
                    </div>
                    <a href="rpt_sum_gsm.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblGsmSP" runat="server">1,000</label>
                        </h3>
                        <p>Suspended</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-remove"></i>
                    </div>
                    <a href="rpt_sum_gsm.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-purple-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblGsmSB" runat="server">1,000</label>
                        </h3>
                        <p>Soft Blocked</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-hand-paper-o"></i>
                    </div>
                    <a href="rpt_sum_gsm.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-3 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-building-o"></i>
                        Warehouse
                    </div>
                    <div class="box-footer no-padding">
                        <ul class="nav nav-stacked">
                            <li style="height: 46px;"><a href="#">Jakarta <span id="lblWarehouseJKT" runat="server" class="pull-right badge bg-aqua">5</span></a></li>
                            <li style="height: 46px;"><a href="#">Surabaya<span id="lblWarehouseSBY" runat="server" class="pull-right badge bg-green">12</span></a></li>
                            <li style="height: 46px;"><a href="#">Surabaya QC<span id="lblWarehouseSBYQC" runat="server" class="pull-right badge bg-red">842</span></a></li>
                            <li style="height: 46px;"><a href="#">Padang<span id="lblWarehousePDG" runat="server" class="pull-right badge bg-yellow">842</span></a></li>
                            <li style="height: 46px;"><a href="#">Semarang<span id="lblWarehouseSMG" runat="server" class="pull-right badge bg-blue">842</span></a></li>
                            <li style="height: 46px;"><a href="#">Palembang<span id="lblWarehousePLB" runat="server" class="pull-right badge bg-maroon">842</span></a></li>
                        </ul>
                    </div>
                </div>

            </div>
            <div class="col-lg-9 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <!--bg-aqua-->
                        <i class="fa fa-phone"></i>
                        Gsm Status by Warehouse
                    </div>
                    <div class="box-body">
                        <div id="chartGsmStatusWarehouse">
                            <!--style="height: 200px;"-->
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <!-- bg-green-->
                        <i class="fa fa-pie-chart"></i>
                        Top 10 Jakarta Branch
                    </div>
                    <div class="box-body">
                        <div id="chartGsmTechnicianJkt" style="height: 170px;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <!-- bg-maroon-->
                        <i class="fa fa-pie-chart"></i>
                        Top 10 Surabaya Branch
                    </div>
                    <div class="box-body">
                        <div id="chartGsmTechnicianSby" style="height: 170px;">
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-md-4 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <!-- bg-orange-->
                        <i class="fa fa-pie-chart"></i>
                        Top 10 Padang Branch
                    </div>
                    <div class="box-body">
                        <div id="chartGsmTechnicianPdg" style="height: 170px;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <!-- bg-green-->
                        <i class="fa fa-pie-chart"></i>
                        Top 10 Semarang Branch
                    </div>
                    <div class="box-body">
                        <div id="chartGsmTechnicianSmg" style="height: 170px;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <!-- bg-maroon-->
                        <i class="fa fa-pie-chart"></i>
                        Top 10 Palembang Branch
                    </div>
                    <div class="box-body">
                        <div id="chartGsmTechnicianPlb" style="height: 170px;">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script src="https://www.amcharts.com/lib/4/core.js"></script>
    <script src="https://www.amcharts.com/lib/4/charts.js"></script>
    <script src="https://www.amcharts.com/lib/4/themes/animated.js"></script>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);


        function createChart(chart, urlapi, par) {
            chart.data = null;
            getData(chart, urlapi, par);
        }

        function getData(chart, urlapi, par) {
            $.ajax({
                type: "POST",
                url: urlapi,
                dataType: "json",
                data: par,
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    var json = JSON.parse(r.d);
                    chart.data = json.data;
                },
                error: function (e) {
                    return "";
                }
            });
        }

        function setBarChart(chart, fieldAxis, titleAxis) {
            if (window.screen.width > 720) {
                var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            }
            else {
                var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
                categoryAxis.renderer.inversed = true;
            }
            categoryAxis.dataFields.category = fieldAxis; //"warehouse"
            //categoryAxis.title.text = "Performances";

            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 20;
            categoryAxis.renderer.labels.template.fontSize = 11;

            categoryAxis.renderer.cellStartLocation = 0.1;
            categoryAxis.renderer.cellEndLocation = 0.9;

            if (window.screen.width > 720) {
                var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            }
            else {
                var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
            }
            valueAxis.min = 0;
            valueAxis.title.text = titleAxis //"Units" 
            valueAxis.renderer.labels.template.fontSize = 9;
        }


        function createSeriesPie(chart, field, category) {
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = field; //"litres";
            pieSeries.dataFields.category = category; //"country";
            pieSeries.slices.template.stroke = am4core.color("#fff");
            pieSeries.slices.template.strokeWidth = 2;
            pieSeries.slices.template.strokeOpacity = 1;
            pieSeries.labels.template.fontSize = 10;
            pieSeries.labels.template.text = "{category}: {value.value}"

            pieSeries.slices.template.width = am4core.percent(50);
            // This creates initial animation
            pieSeries.hiddenState.properties.opacity = 1;
            pieSeries.hiddenState.properties.endAngle = -90;
            pieSeries.hiddenState.properties.startAngle = -90;
        }

        function createSeries(chart, field, name, category, stacked) {
            var series = chart.series.push(new am4charts.ColumnSeries());
            if (window.screen.width > 720) {
                series.dataFields.valueY = field;
                series.dataFields.categoryX = category;
                series.name = name;
                series.columns.template.tooltipText = "{name}: [bold]{valueY}[/]";
            }
            else {
                series.dataFields.valueX = field;
                series.dataFields.categoryY = category;
                series.name = name;
                series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
            }
            series.columns.template.width = am4core.percent(85);
            series.columns.template.column.cornerRadiusTopLeft = 3;
            series.columns.template.column.cornerRadiusTopRight = 3;
            series.columns.template.column.fillOpacity = 0.8;

            var seriesLabel = series.bullets.push(new am4charts.LabelBullet());
            if (window.screen.width > 720) {
                seriesLabel.label.text = "{valueY}";
                seriesLabel.verticalCenter = "center";
                seriesLabel.dy = -8;
            }
            else {
                seriesLabel.label.text = "{valueX}";
                seriesLabel.label.horizontalCenter = "center";
                seriesLabel.label.dx = 8;
            }
            seriesLabel.label.hideOversized = false;
            seriesLabel.label.truncate = false;
            seriesLabel.label.fontSize = 10;
        }

        function createLine(chart, field, name, category, stacked) {
            var line = chart.series.push(new am4charts.LineSeries())
            if (window.screen.width > 720) {
                line.dataFields.valueY = field;
                line.dataFields.categoryX = category;
            }
            else {
                line.dataFields.valueX = field;
                line.dataFields.categoryY = category;
            }
            line.name = name;
            line.bullets.push(new am4charts.CircleBullet());
            line.strokeWidth = 2;
            line.stroke = new am4core.InterfaceColorSet().getFor("alternativeBackground");
            line.strokeOpacity = 0.5;

            var lineLabel = line.bullets.push(new am4charts.LabelBullet());
            if (window.screen.width > 720) {
                lineLabel.label.text = "{valueY}";
                lineLabel.verticalCenter = "center";
                lineLabel.dy = -12;
            }
            else {
                lineLabel.label.text = "{valueX}";
                lineLabel.label.horizontalCenter = "center";
                lineLabel.label.dx = 10;
            }
            lineLabel.label.hideOversized = false;
            lineLabel.label.truncate = false;
            lineLabel.label.fontSize = 10;
        }


        function endRequest(sender, args) {

            am4core.useTheme(am4themes_animated);
            var chart = am4core.create("chartGsmStatusWarehouse", am4charts.XYChart); //am4charts.XYChart
            createChart(chart, "dashboard_gsm.aspx/GsmWarehouse", "");
            setBarChart(chart, "warehouse", "Units");
            createSeries(chart, "mw", "Warehouse Stock", "warehouse", false);
            createSeries(chart, "mt", "Technician Stock", "warehouse", false);
            createSeries(chart, "inst", "Installed", "warehouse", false);
            createSeries(chart, "rs", "Ready to Suspend", "warehouse", false);
            createSeries(chart, "sp", "Suspend", "warehouse", false);
            createSeries(chart, "sb", "Soft Blocked", "warehouse", false);
            chart.legend = new am4charts.Legend();
            chart.legend.fontSize = 12;

            var chart2 = am4core.create("chartGsmTechnicianJkt", am4charts.PieChart);
            var dataJson = JSON.stringify({ "sBranchID": "BRC0000001" });
            createChart(chart2, "dashboard_gsm.aspx/GsmTechnician", dataJson);
            createSeriesPie(chart2, "gsm", "technician");

            var chart3 = am4core.create("chartGsmTechnicianSby", am4charts.PieChart);
            var dataJson = JSON.stringify({ "sBranchID": "BRC0000002" });
            createChart(chart3, "dashboard_gsm.aspx/GsmTechnician", dataJson);
            createSeriesPie(chart3, "gsm", "technician");

            var chart4 = am4core.create("chartGsmTechnicianPdg", am4charts.PieChart);
            var dataJson = JSON.stringify({ "sBranchID": "BRC0000004" });
            createChart(chart4, "dashboard_gsm.aspx/GsmTechnician", dataJson);
            createSeriesPie(chart4, "gsm", "technician");

            var chart5 = am4core.create("chartGsmTechnicianSmg", am4charts.PieChart);
            var dataJson = JSON.stringify({ "sBranchID": "BRC0000005" });
            createChart(chart5, "dashboard_gsm.aspx/GsmTechnician", dataJson);
            createSeriesPie(chart5, "gsm", "technician");

            var chart6 = am4core.create("chartGsmTechnicianPlb", am4charts.PieChart);
            var dataJson = JSON.stringify({ "sBranchID": "BRC0000006" });
            createChart(chart6, "dashboard_gsm.aspx/GsmTechnician", dataJson);
            createSeriesPie(chart6, "gsm", "technician");

            //console.log(window.screen.width);
            if (window.screen.width > 720) {
                document.getElementById("chartGsmStatusWarehouse").style.height = '257px';
            } else {
                document.getElementById("chartGsmStatusWarehouse").style.height = '550px';
            }
            //console.log(document.getElementById("chartDeviceStatusWarehouse"));
            //$('#modal-messagebox').on('hidden.bs.modal', function () {
            //    document.body.style.paddingRight = '0px';
            //});

            //var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            //if (isExists != '') {
            //    //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
            //    $('#modal-messagebox').modal('show');
            //}
        }
        endRequest();
    </script>
</asp:Content>
