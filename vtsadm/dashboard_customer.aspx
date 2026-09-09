<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_customer.aspx.cs" Inherits="vtsadm.dashboard_customer" EnableEventValidation="false" %>

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

        .nav>li>a.custom {
            padding:0px;
            color: #333;
        }

        .pull-center {
            font-size:15px;
        }
        
    </style>

    <section class="content-header">
        <h1>Dashboard
        <small>Customer</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="#"><i class="fa fa-dashboard"></i>Customer</a></li>
        </ol>
    </section>


    <section class="content">
        <div class="row">
            
            <div class="col-md-8 col-xs-12">
                <div class="col-lg-3 col-xs-6">
                    <div class="small-box bg-blue-gradient">
                        <div class="inner">
                            <h4>
                                <label id="lblTotalCustomer" runat="server">0</label>
                            </h4>
                            <p>Total Customer</p>
                        </div>
                        <div class="icon">
                            <i class="fa fa-registered"></i>
                        </div>
                        <a href="dashboard_customer_detail?type=customer" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-lg-3 col-xs-6">
                    <div class="small-box bg-green-gradient">
                        <div class="inner">
                            <h4>
                                <label id="lblTotalCorporate" runat="server">0</label>
                            </h4>
                            <p>Corporate</p>
                        </div>
                        <div class="icon">
                            <i class="fa fa-building"></i>
                        </div>
                        <a href="dashboard_customer_detail?type=corporate" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-lg-3 col-xs-6">
                    <div class="small-box bg-yellow-gradient">
                        <div class="inner">
                            <h4>
                                <label id="lblTotalPersonal" runat="server">0</label>
                            </h4>
                            <p>Personal</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-person"></i>
                        </div>
                        <a href="dashboard_customer_detail?type=personal" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>

                    </div>
                </div>

                <div class="col-lg-3 col-xs-6">
                    <div class="small-box bg-maroon-gradient">

                        <div class="inner">
                            <table class="table no-margin no-padding">
                                <tbody>
                                <tr>
                                    <!--
                                    <th class="text-center">
                                        <ul class="nav">
                                            <li style="height: 26px;"><a href="dashboard_customer_detail?type=trial" class="custom"><span id="lblTotalTrial" runat="server" class="pull-center badge bg-green">0</span></a></li>
                                            <li style="height: 26px;">Trial</li>
                                        </ul>
                                    </th>
                                    -->
                                    <th class="text-center">
                                        <ul class="nav">
                                            <li style="height: 26px;"><a href="dashboard_customer_detail?type=easttrial" class="custom"><span id="lblTotalEastTrial" runat="server" class="pull-center badge bg-green">0</span></a></li>
                                            <li style="height: 26px;">East</li>
                                        </ul>
                                        
                                    </th>

                                    <th class="text-center">
                                        <ul class="nav">
                                            <li style="height: 26px;"><a href="dashboard_customer_detail?type=westtrial" class="custom"><span id="lblTotalWestTrial" runat="server" class="pull-center badge bg-green">0</span></a></li>
                                            <li style="height: 26px;">West</li>
                                        </ul>
                                        
                                    </th>
                                </tr>
                               
                                </tbody>

                            </table>
                        </div>

                        

                        <span class="small-box-footer"><a href="dashboard_customer_detail?type=trial" class="small-box-footer" style="color:#ffffff">Trial</a></span>
                    </div>
                </div>

                <!-- 3 -->
                <div class="col-lg-6 col-xs-12">
                    <div class="box box-solid">
                        <div class="box-header bg-gray-light">
                            <i class="fa fa-building-o"></i>
                            West Region
                        </div>
                        <div class="box-footer">


                            <div class="col-sm-12 col-md-12">
                                
                                <a href="dashboard_customer_detail?type=west" class="custom"><span class="info-box-number" style="text-align:center; font-weight:bold"><h2 id="lblTotalWest" runat="server">0</h2></span></a>
                                <table class="table table-bordered no-margin no-padding">
                                    <tbody>
                                    <tr>
                                        <th class="text-center">
                                            <ul class="nav">
                                                <li style="height: 26px;">Active</li>
                                                <li style="height: 26px;"><a href="dashboard_customer_detail?type=westunitactive" class="custom"><span id="lblTotalWestUnitActive" runat="server" class="pull-center badge bg-green">0</span></a></li>
                                            </ul>
                                        </th>
                                        <th class="text-center">
                                            <ul class="nav">
                                                <li style="height: 26px;">Suspend</li>
                                                <li style="height: 26px;"><a href="dashboard_customer_detail?type=westsuspend" class="custom"><span id="lblTotalWestSuspend" runat="server" class="pull-center badge bg-green">0</span></a></li>
                                            </ul>
                                        
                                        </th>
                                    </tr>
                                    <tr>
                                        <th class="text-center">
                                            <ul class="nav">
                                                <li style="height: 26px;">Soft Block</li>
                                                <li style="height: 26px;"><a href="dashboard_customer_detail?type=westsoftblock" class="custom"><span id="lblTotalWestSoftBlock" runat="server" class="pull-center badge bg-green">0</span></a></li>
                                            </ul>
                                        </th>
                                        <th class="text-center">
                                            <ul class="nav">
                                                <li style="height: 26px;">Unit Suspend</li>
                                                <li style="height: 26px;"><a href="dashboard_customer_detail?type=westunitsuspend" class="custom"><span id="lblTotalWestUnitSP" runat="server" class="pull-center badge bg-green">0</span></a></li>
                                            </ul>
                                        </th>
                                    </tr>
                                    </tbody>

                                </table>
                            </div>
                            

                            <div class="col-sm-12 col-md-12">
                                <div class="row" style="margin-top:10px">
                                    <div class="col-lg-4 col-xs-6">
                                        <div class="small-box bg-aqua">
                                            <div class="inner">
                                                <h4><a href="dashboard_customer_detail?type=westreguler" class="custom"><span id="lblTotalWestReguler" runat="server" class="pull-center badge bg-green">0</span></a></h4>
                                                <p>< 10 unit</p>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-4 col-xs-6">
                                        <div class="small-box bg-aqua">
                                            <div class="inner">
                                                <h4><a href="dashboard_customer_detail?type=westmedium" class="custom"><span id="lblTotalWestMedium" runat="server" class="pull-center badge bg-green">0</span></a></h4>
                                                <p>10 - 24</p>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-4 col-xs-6">
                                        <div class="small-box bg-aqua">
                                            <div class="inner">
                                                <h4><a href="dashboard_customer_detail?type=westpriority" class="custom"><span id="lblTotalWestPriority" runat="server" class="pull-center badge bg-green">0</span></a></h4>
                                                <p>Priority</p>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <!-- 4 -->
                <div class="col-lg-6 col-xs-12">
                    <div class="box box-solid">
                        <div class="box-header bg-gray-light">
                            <i class="fa fa-building-o"></i>
                            East Region
                        </div>
                        <div class="box-footer">

                            <div class="col-sm-12 col-md-12">
                                <a href="dashboard_customer_detail?type=east" class="custom"><span class="info-box-number" style="text-align:center; font-weight:bold"><h2 id="lblTotalEast" runat="server">0</h2></span></a>
                                <table class="table table-bordered no-margin no-padding">
                                    <tbody>
                                    <tr>
                                        <th class="text-center">
                                            <ul class="nav">
                                                <li style="height: 26px;">Active</li>
                                                <li style="height: 26px;"><a href="dashboard_customer_detail?type=eastunitactive" class="custom"><span id="lblTotalEastUnitActive" runat="server" class="pull-center badge bg-green">0</span></a></li>
                                            </ul>
                                        </th>
                                        <th class="text-center">
                                            <ul class="nav">
                                                <li style="height: 26px;">Suspend</li>
                                                <li style="height: 26px;"><a href="dashboard_customer_detail?type=eastsuspend" class="custom"><span id="lblTotalEastSuspend" runat="server" class="pull-center badge bg-green">0</span></a></li>
                                            </ul>
                                        
                                        </th>
                                    </tr>
                                    <tr>
                                        <th class="text-center">
                                            <ul class="nav">
                                                <li style="height: 26px;">Soft Block</li>
                                                <li style="height: 26px;"><a href="dashboard_customer_detail?type=eastsoftblock" class="custom"><span id="lblTotalEastSoftBlock" runat="server" class="pull-center badge bg-green">0</span></a></li>
                                            </ul>
                                        </th>
                                        <th class="text-center">
                                            <ul class="nav">
                                                <li style="height: 26px;">Unit Suspend</li>
                                                <li style="height: 26px;"><a href="dashboard_customer_detail?type=eastunitsuspend" class="custom"><span id="lblTotalEastUnitSp" runat="server" class="pull-center badge bg-green">0</span></a></li>
                                            </ul>
                                        </th>
                                    </tr>
                                    </tbody>

                                </table>
                            </div>
                            

                            <div class="col-sm-12 col-md-12">
                                <div class="row" style="margin-top:10px">
                                    <div class="col-lg-4 col-xs-6">
                                        <div class="small-box bg-aqua">
                                            <div class="inner">
                                                <h4><a href="dashboard_customer_detail?type=eastreguler" class="custom"><span id="lblTotalEastReguler" runat="server" class="pull-center badge bg-green">0</span></a></h4>
                                                <p>< 10 unit</p>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-4 col-xs-6">
                                        <div class="small-box bg-aqua">
                                            <div class="inner">
                                                <h4><a href="dashboard_customer_detail?type=eastmedium" class="custom"><span id="lblTotalEastMedium" runat="server" class="pull-center badge bg-green">0</span></a></h4>
                                                <p>10 - 24</p>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-4 col-xs-6">
                                        <div class="small-box bg-aqua">
                                            <div class="inner">
                                                <h4><a href="dashboard_customer_detail?type=eastpriority" class="custom"><span id="lblTotalEastPriority" runat="server" class="pull-center badge bg-green">0</span></a></h4>
                                                <p>Priority</p>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>

                    </div>
                </div>

                <!-- 5 -->
                <div class="col-lg-6 col-xs-12">
                    <div class="box box-solid">
                        <div class="box-header bg-gray-light">
                            Type Of Service
                        </div>
                        <div class="box-footer no-padding">
                            <ul class="nav nav-stacked">
                                <li style="height: 46px;"><a href="dashboard_customer_detail?type=gps">GPS <span id="lblTotalGps" runat="server" class="pull-right badge bg-aqua">0</span></a></li>
                                <li style="height: 46px;"><a href="dashboard_customer_detail?type=eseal">ESEAL<span id="lblTotalEseal" runat="server" class="pull-right badge bg-green">0</span></a></li>
                            </ul>
                        </div>

                        <div class="box-header bg-gray-light">
                            Type Of Business
                        </div>
                        <div class="box-footer no-padding">
                            <ul class="nav nav-stacked">
                                <li style="height: 46px;"><a href="#">Cement <span id="Span1" runat="server" class="pull-right badge bg-aqua">0</span></a></li>
                                <li style="height: 46px;"><a href="#">Rent<span id="Span2" runat="server" class="pull-right badge bg-green">0</span></a></li>
                                <li style="height: 46px;"><a href="#">ARPI<span id="Span3" runat="server" class="pull-right badge bg-red">0</span></a></li>
                                <li style="height: 46px;"><a href="#">CIT<span id="Span4" runat="server" class="pull-right badge bg-yellow">0</span></a></li>
                                <li style="height: 46px;"><a href="#">PO Bus<span id="Span5" runat="server" class="pull-right badge bg-blue">0</span></a></li>
                                <li style="height: 46px;"><a href="#">Expedition<span id="Span6" runat="server" class="pull-right badge bg-maroon">0</span></a></li>
                            </ul>
                        </div>

                    </div>
                </div>

                <!-- 6 -->
                <div class="col-lg-6 col-xs-12">
                    <div class="box box-solid">
                        <div class="box-header bg-gray-light">
                            <!-- bg-green-->
                            <i class="fa fa-pie-chart"></i>
                            Outbound Activity East & West
                        </div>
                        <div class="box-body">
                            <div id="chartCustOutboundActivity" style="height: 250px;">
                            </div>
                        </div>
                    </div>
                </div>

            </div>



            <div class="col-md-4 col-xs-12">
                
                <div class="col-md-12 col-xs-12">
                    <div class="box box-solid">
                        <div class="box-header bg-gray-light">
                            <!-- bg-green-->
                            <i class="fa fa-pie-chart"></i>
                            Costumer Population by Type
                        </div>
                        <div class="box-body">
                            <div id="chartCustPopulationType" style="height: 200px;">
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-xs-12">
                    <div class="box box-solid">
                        <div class="box-header bg-gray-light">
                            <!-- bg-maroon-->
                            <i class="fa fa-pie-chart"></i>
                            Costumer Population by Region
                        </div>
                        <div class="box-body">
                            <div id="chartCustPopulationRegion" style="height: 200px;">
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-xs-12">
                    <div class="box box-solid">
                        <div class="box-header bg-gray-light">
                            <!-- bg-maroon-->
                            <i class="fa fa-pie-chart"></i>
                            Costumer Population by User Unit Quantity
                        </div>
                        <div class="box-body">
                            <div id="chartCustPopulationUser" style="height: 200px;">
                            </div>
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
            

            var chart = am4core.create("chartCustPopulationType", am4charts.PieChart);
            var dataJson = JSON.stringify({ "sType": "CHART01" });
            createChart(chart, "dashboard_customer.aspx/DashCustomer", dataJson);
            createSeriesPie(chart, "total", "title");

            var chart2 = am4core.create("chartCustPopulationRegion", am4charts.PieChart);
            var dataJson = JSON.stringify({ "sType": "CHART02" });
            createChart(chart2, "dashboard_customer.aspx/DashCustomer", dataJson);
            createSeriesPie(chart2, "total", "title");

            var chart3 = am4core.create("chartCustPopulationUser", am4charts.PieChart);
            var dataJson = JSON.stringify({ "sType": "CHART03" });
            createChart(chart3, "dashboard_customer.aspx/DashCustomer", dataJson);
            createSeriesPie(chart3, "total", "title");

            var chart4 = am4core.create("chartCustOutboundActivity", am4charts.PieChart);
            var dataJson = JSON.stringify({ "sType": "CHART04" });
            createChart(chart4, "dashboard_customer.aspx/DashCustomer", dataJson);
            createSeriesPie(chart4, "total", "title");
            

            //console.log(window.screen.width);
            
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
