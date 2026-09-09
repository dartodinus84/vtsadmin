<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chart.aspx.cs" Inherits="vtsadm.chart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style>
        #chartdiv {
            width: 100%;
            height: 500px;
        }
    </style>

    <script src="https://www.amcharts.com/lib/4/core.js"></script>
    <script src="https://www.amcharts.com/lib/4/charts.js"></script>
    <script src="https://www.amcharts.com/lib/4/themes/animated.js"></script>

    <script>
        am4core.ready(function () {

            am4core.useTheme(am4themes_animated);

            var chart = am4core.create("chartdiv", am4charts.XYChart);
            chart.hiddenState.properties.opacity = 0;

            chart.padding(0, 0, 0, 0);

            chart.zoomOutButton.disabled = true;

            var data = [];
            var visits = 10;
            var visits2 = 15;
            var i = 0;

            for (i = 0; i <= 30; i++) {
                visits -= Math.round((Math.random() < 0.5 ? 1 : -1) * Math.random() * 10);
                visits2 -= Math.round((Math.random() < 0.5 ? 1 : -1) * Math.random() * 15);
                data.push({ date: new Date().setSeconds(i - 30), value: visits });
            }

            chart.data = data;

            var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
            dateAxis.renderer.grid.template.location = 0;
            dateAxis.renderer.minGridDistance = 30;
            dateAxis.dateFormats.setKey("second", "ss");
            dateAxis.periodChangeDateFormats.setKey("second", "[bold]h:mm a");
            dateAxis.periodChangeDateFormats.setKey("minute", "[bold]h:mm a");
            dateAxis.periodChangeDateFormats.setKey("hour", "[bold]h:mm a");
            dateAxis.renderer.inside = true;
            dateAxis.renderer.axisFills.template.disabled = true;
            dateAxis.renderer.ticks.template.disabled = true;

            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            valueAxis.tooltip.disabled = true;
            valueAxis.interpolationDuration = 500;
            valueAxis.rangeChangeDuration = 500;
            valueAxis.renderer.inside = true;
            valueAxis.renderer.minLabelPosition = 0.05;
            valueAxis.renderer.maxLabelPosition = 0.95;
            valueAxis.renderer.axisFills.template.disabled = true;
            valueAxis.renderer.ticks.template.disabled = true;

            var series = chart.series.push(new am4charts.LineSeries());
            series.dataFields.dateX = "date";
            series.dataFields.valueY = "value";
            series.interpolationDuration = 500;
            series.defaultState.transitionDuration = 0;
            series.tensionX = 0.8;

            var series2 = chart.series.push(new am4charts.LineSeries());
            series2.dataFields.dateX = "date";
            series2.dataFields.valueY = "value";
            series2.interpolationDuration = 200;
            series2.defaultState.transitionDuration = 0;
            series2.tensionX = 0.4;

            chart.events.on("datavalidated", function () {
                dateAxis.zoom({ start: 1 / 15, end: 1.2 }, false, true);
            });

            dateAxis.interpolationDuration = 500;
            dateAxis.rangeChangeDuration = 500;

            document.addEventListener("visibilitychange", function () {
                if (document.hidden) {
                    if (interval) {
                        clearInterval(interval);
                    }
                }
                else {
                    startInterval();
                }
            }, false);

            // add data
            var interval;
            function startInterval() {
                interval = setInterval(function () {
                    visits =
                        visits + Math.round((Math.random() < 0.5 ? 1 : -1) * Math.random() * 5);
                    var lastdataItem = series.dataItems.getIndex(series.dataItems.length - 1);
                    chart.addData(
                        { date: new Date(lastdataItem.dateX.getTime() + 1000), value: visits },
                        1
                    );
                    visits2 =
                        visits2 + Math.round((Math.random() < 0.5 ? 1 : -1) * Math.random() * 5);
                    var lastdataItem2 = series2.dataItems.getIndex(series2.dataItems.length - 1);
                    chart.addData(
                        { date: new Date(lastdataItem2.dateX.getTime() + 1000), value: visits },
                        1
                    );

                }, 1000);
            }

            startInterval();

            // all the below is optional, makes some fancy effects
            // gradient fill of the series
            series.fillOpacity = 1;            
            var gradient = new am4core.LinearGradient();
            gradient.addColor(chart.colors.getIndex(0), 0.2);
            gradient.addColor(chart.colors.getIndex(0), 0);
            series.fill = gradient;

            series2.fillOpacity = 1;
            var gradient2 = new am4core.LinearGradient();
            gradient2.addColor(chart.colors.getIndex(0), 0.2);
            gradient2.addColor(chart.colors.getIndex(0), 0);
            series2.fill = gradient2;

            // this makes date axis labels to fade out
            dateAxis.renderer.labels.template.adapter.add("fillOpacity", function (fillOpacity, target) {
                var dataItem = target.dataItem;
                return dataItem.position;
            })

            // need to set this, otherwise fillOpacity is not changed and not set
            dateAxis.events.on("validated", function () {
                am4core.iter.each(dateAxis.renderer.labels.iterator(), function (label) {
                    label.fillOpacity = label.fillOpacity;
                })
            })

            // this makes date axis labels which are at equal minutes to be rotated
            dateAxis.renderer.labels.template.adapter.add("rotation", function (rotation, target) {
                var dataItem = target.dataItem;
                if (dataItem.date && dataItem.date.getTime() == am4core.time.round(new Date(dataItem.date.getTime()), "minute").getTime()) {
                    target.verticalCenter = "middle";
                    target.horizontalCenter = "left";
                    return -90;
                }
                else {
                    target.verticalCenter = "bottom";
                    target.horizontalCenter = "middle";
                    return 0;
                }
            })

            // bullet at the front of the line
            var bullet = series.createChild(am4charts.CircleBullet);
            bullet.circle.radius = 5;
            bullet.fillOpacity = 1;
            bullet.fill = chart.colors.getIndex(0);
            bullet.isMeasured = false;

            series.events.on("validated", function () {
                bullet.moveTo(series.dataItems.last.point);
                bullet.validatePosition();
            });

            var bullet2 = series2.createChild(am4charts.CircleBullet);
            bullet2.circle.radius = 5;
            bullet2.fillOpacity = 1;
            bullet2.fill = chart.colors.getIndex(0);
            bullet2.isMeasured = false;

            series2.events.on("validated", function () {
                bullet2.moveTo(series2.dataItems.last.point);
                bullet2.validatePosition();
            });
        }); // end am4core.ready()
    </script>

</head>
<body>
    <div id="chartdiv"></div>
</body>
</html>
