var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_endRequest(endRequest);


//function createChart(chart, urlapi, par) {
//    chart.data = null;
//    getData(chart, urlapi, par);
//}

//function getData(chart, urlapi, par) {
//    $.ajax({
//        type: "POST",
//        url: urlapi,
//        dataType: "json",
//        data: par,
//        contentType: "application/json; charset=utf-8",
//        success: function (r) {
//            var json = JSON.parse(r.d);
//            chart.data = json.data;
//        },
//        error: function (e) {
//            return "";
//        }
//    });
//}

//function setBarChart(chart, fieldAxis, titleAxis) {
//    if (window.screen.width > 720) {
//        var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
//    }
//    else {
//        var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
//        categoryAxis.renderer.inversed = true;
//    }
//    categoryAxis.dataFields.category = fieldAxis; //"warehouse"
//    //categoryAxis.title.text = "Performances";

//    categoryAxis.renderer.grid.template.location = 0;
//    categoryAxis.renderer.minGridDistance = 20;
//    categoryAxis.renderer.labels.template.fontSize = 11;

//    categoryAxis.renderer.cellStartLocation = 0.1;
//    categoryAxis.renderer.cellEndLocation = 0.9;

//    if (window.screen.width > 720) {
//        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
//    }
//    else {
//        var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
//    }
//    valueAxis.min = 0;
//    valueAxis.title.text = titleAxis //"Units"
//    valueAxis.renderer.labels.template.fontSize = 9;
//}


//function createSeriesPie(chart, field, category) {
//    var pieSeries = chart.series.push(new am4charts.PieSeries());
//    pieSeries.dataFields.value = field; //"litres";
//    pieSeries.dataFields.category = category; //"country";
//    pieSeries.slices.template.stroke = am4core.color("#fff");
//    pieSeries.slices.template.strokeWidth = 2;
//    pieSeries.slices.template.strokeOpacity = 1;
//    pieSeries.labels.template.fontSize = 10;
//    pieSeries.labels.template.text = "{category}: {value.value}"

//    pieSeries.slices.template.width = am4core.percent(50);
//    // This creates initial animation
//    pieSeries.hiddenState.properties.opacity = 1;
//    pieSeries.hiddenState.properties.endAngle = -90;
//    pieSeries.hiddenState.properties.startAngle = -90;
//}

//function createSeries(chart, field, name, category, stacked) {
//    var series = chart.series.push(new am4charts.ColumnSeries());
//    if (window.screen.width > 720) {
//        series.dataFields.valueY = field;
//        series.dataFields.categoryX = category;
//        series.name = name;
//        series.columns.template.tooltipText = "{name}: [bold]{valueY}[/]";
//    }
//    else {
//        series.dataFields.valueX = field;
//        series.dataFields.categoryY = category;
//        series.name = name;
//        series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
//    }
//    series.columns.template.width = am4core.percent(85);
//    series.columns.template.column.cornerRadiusTopLeft = 3;
//    series.columns.template.column.cornerRadiusTopRight = 3;
//    series.columns.template.column.fillOpacity = 0.8;

//    var seriesLabel = series.bullets.push(new am4charts.LabelBullet());
//    if (window.screen.width > 720) {
//        seriesLabel.label.text = "{valueY}";
//        seriesLabel.verticalCenter = "center";
//        seriesLabel.dy = -8;
//    }
//    else {
//        seriesLabel.label.text = "{valueX}";
//        seriesLabel.label.horizontalCenter = "center";
//        seriesLabel.label.dx = 8;
//    }
//    seriesLabel.label.hideOversized = false;
//    seriesLabel.label.truncate = false;
//    seriesLabel.label.fontSize = 10;
//}

//function createLine(chart, field, name, category, stacked) {
//    var line = chart.series.push(new am4charts.LineSeries())
//    if (window.screen.width > 720) {
//        line.dataFields.valueY = field;
//        line.dataFields.categoryX = category;
//    }
//    else {
//        line.dataFields.valueX = field;
//        line.dataFields.categoryY = category;
//    }
//    line.name = name;
//    line.bullets.push(new am4charts.CircleBullet());
//    line.strokeWidth = 2;
//    line.stroke = new am4core.InterfaceColorSet().getFor("alternativeBackground");
//    line.strokeOpacity = 0.5;

//    var lineLabel = line.bullets.push(new am4charts.LabelBullet());
//    if (window.screen.width > 720) {
//        lineLabel.label.text = "{valueY}";
//        lineLabel.verticalCenter = "center";
//        lineLabel.dy = -12;
//    }
//    else {
//        lineLabel.label.text = "{valueX}";
//        lineLabel.label.horizontalCenter = "center";
//        lineLabel.label.dx = 10;
//    }
//    lineLabel.label.hideOversized = false;
//    lineLabel.label.truncate = false;
//    lineLabel.label.fontSize = 10;
//}

function generateChart1() {
    $.ajax({
        type: "POST",
        url: "dashboard_customer.aspx/CustomerEasygo",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (r) {
            //console.log(r.d)
            var json = JSON.parse(r.d);
            data = json.data[0];

            var totalCustomer = data.totalCustomer,
                jmlTMS = data.jmlTms,
                tmsActive = data.tmsActive,
                tmsBlock = data.tmsBlock,
                jmlIGO = data.jmlIgo
            google.charts.load("current", { packages: ["corechart"] });
            google.charts.setOnLoadCallback(drawChart);
            google.charts.setOnLoadCallback(drawChart2);

            function drawChart() {
                var data = google.visualization.arrayToDataTable([
                    ['Task', 'Hours per Day'],
                    ['TMS', jmlTMS],
                    ['IGO', jmlIGO]
                ]);

                var options = {
                    //title: 'Server TMS dan IGO',
                    pieHole: 0.4,
                    chartArea: { 'width': '100%', 'height': '90%' },
                };

                var chart = new google.visualization.PieChart(document.getElementById('chartCustomerEasygo'));
                chart.draw(data, options);
            }

            function drawChart2() {
                var data = google.visualization.arrayToDataTable([
                    ['Task', 'Hours per Day'],
                    ['ACTIVE', tmsActive],
                    ['BLOK', tmsBlock]
                ]);

                var options = {
                    //title: 'Status Customer TMS',
                    pieHole: 0.4,
                    chartArea: { 'width': '100%', 'height': '90%' },
                };

                var chart = new google.visualization.PieChart(document.getElementById('chartCustomerTMSstatus'));
                chart.draw(data, options);
            }
        },
        error: function (e) {
            console.log(e);
        }
    });
}

function generateChart3() {
    $.ajax({
        type: "POST",
        url: "dashboard_customer.aspx/CustomerBranch",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (r) {
            var json = JSON.parse(r.d);
            console.log(json)
            data = json.data[0];

            var jkt = data.jktActive + data.jktBlock,
                sby = data.sbyActive + data.sbyBlock,
                mdn = data.mdnActive + data.mdnBlock,
                pdg = data.pdgActive + data.pdgBlock,
                plmb = data.plmbActive + data.plmbBlock,
                smrg = data.smrgActive + data.smrgBlock,
                yogy = data.yogActive + data.yogBlock
            google.charts.load("current", { packages: ["corechart"] });
            google.charts.setOnLoadCallback(drawChart);
            function drawChart() {
                var data = google.visualization.arrayToDataTable([
                    ['Task', 'Hours per Day'],
                    ['Jakarta Branch', jkt],
                    ['Surabaya Branch', sby],
                    ['Medan Branch', mdn],
                    ['Padang Branch', pdg],
                    ['Semarang Branch', smrg],
                    ['Palembang Branch', plmb],
                    ['Yogyakarta Branch', yogy]
                ]);

                var options = {
                    //title: 'Easygo Branch',
                    is3D: true,
                    chartArea: { 'width': '100%', 'height': '90%' },
                };

                var chart = new google.visualization.PieChart(document.getElementById('chartCustomerBranch'));
                chart.draw(data, options);
            }
        },
        error: function (e) {
            console.log(e);
        }
    });    
}

function generateChart4() {
    $.ajax({
        type: "POST",
        url: "dashboard_customer.aspx/CustomerRangeUnit",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (r) {
            var json = JSON.parse(r.d);
            console.log(json)
            data = json.data[0];

            var person = data.a,
                range1 = data.b,
                range2 =  data.c,
                range3 =  data.d,
                range4 =  data.e,
                range5 =  data.f
            google.charts.load('current', { 'packages': ['corechart'] });
            google.charts.setOnLoadCallback(drawChart);

            function drawChart() {
                var data = google.visualization.arrayToDataTable([
                    ['Task', 'Hours per Day'],
                    ['Personal', person],
                    ['3-5 Unit', range1],
                    ['6-10 Unit', range2],
                    ['11-20 Unit', range3],
                    ['21-30 Unit', range4],
                    ['>30 Unit', range5]
                ]);

                var options = {
                    //title: 'Unit Range'
                    chartArea: { 'width': '100%', 'height': '90%' },
                };

                var chart = new google.visualization.PieChart(document.getElementById('chartCustomerTMSrange'));
                chart.draw(data, options);
            }
        },
        error: function (e) {
            console.log(e);
        }
    });    
}

var $table = $('#table_')
var $remove = $('#remove')
var selections = []

function getIdSelections() {
    return $.map($table.bootstrapTable('getSelections'), function (row) {
        return row.id
    })
}

function responseHandler(res) {
    $.each(res.rows, function (i, row) {
        row.state = $.inArray(row.id, selections) !== -1
    })
    return res
}

function detailFormatter(index, row) {
    var html = []
    $.each(row, function (key, value) {
        html.push('<p><b>' + key + ':</b> ' + value + '</p>')
    })
    return html.join('')
}

function operateFormatter(value, row, index) {
    return [
        '<a class="like" href="javascript:void(0)" title="Like">',
        '<i class="fa fa-heart"></i>',
        '</a>  ',
        '<a class="remove" href="javascript:void(0)" title="Remove">',
        '<i class="fa fa-trash"></i>',
        '</a>'
    ].join('')
}

function totalTextFormatter(data) {
    return 'Total'
}

function totalNameFormatter(data) {
    return data.length
}

function totalPriceFormatter(data) {
    var field = this.field
    return '$' + data.map(function (row) {
        return +row[field].substring(1)
    }).reduce(function (sum, i) {
        return sum + i
    }, 0)
}

function initTable() {
    $table.bootstrapTable('destroy').bootstrapTable({
        height: 550,
        locale: $('#locale').val(),
        columns: [
            [{
                field: 'state',
                checkbox: true,
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            }, {
                title: 'Item ID',
                field: 'id',
                rowspan: 2,
                align: 'center',
                valign: 'middle',
                sortable: true,
                footerFormatter: totalTextFormatter
            }, {
                title: 'Item Detail',
                colspan: 3,
                align: 'center'
            }],
            [{
                field: 'name',
                title: 'Item Name',
                sortable: true,
                footerFormatter: totalNameFormatter,
                align: 'center'
            }, {
                field: 'price',
                title: 'Item Price',
                sortable: true,
                align: 'center',
                footerFormatter: totalPriceFormatter
            }, {
                field: 'operate',
                title: 'Item Operate',
                align: 'center',
                clickToSelect: false,
                events: window.operateEvents,
                formatter: operateFormatter
            }]
        ]
    })
    $table.on('check.bs.table uncheck.bs.table ' +
        'check-all.bs.table uncheck-all.bs.table',
        function () {
            $remove.prop('disabled', !$table.bootstrapTable('getSelections').length)

            // save your data, here just save the current page
            selections = getIdSelections()
            // push or splice the selections if you want to save all data selections
        })
    $table.on('all.bs.table', function (e, name, args) {
        console.log(name, args)
    })
    $remove.click(function () {
        var ids = getIdSelections()
        $table.bootstrapTable('remove', {
            field: 'id',
            values: ids
        })
        $remove.prop('disabled', true)
    })
}



function endRequest(sender, args) {
    generateChart1();
    generateChart3();
    generateChart4();


    window.operateEvents = {
        'click .like': function (e, value, row, index) {
            alert('You click like action, row: ' + JSON.stringify(row))
        },
        'click .remove': function (e, value, row, index) {
            $table.bootstrapTable('remove', {
                field: 'id',
                values: [row.id]
            })
        }
    }

    //$(function () {
        initTable()

        $table.bootstrapTable('refreshOptions', {
            locale: 'en-US',
            //ajax: 'ajaxRequest_get_list_mahasiswa'
        })

        $('#locale').change(initTable)
    //})

    //am4core.useTheme(am4themes_animated);
    //var chart = am4core.create("chartDeviceStatusWarehouse", am4charts.XYChart); //am4charts.XYChart
    //createChart(chart, "dashboard_customer.aspx/CustomerWarehouse", "");
    //setBarChart(chart, "warehouse", "Units");
    //createSeries(chart, "mw", "Warehouse Stock", "warehouse", false);
    //createSeries(chart, "mt", "Technician Stock", "warehouse", false);
    //createSeries(chart, "inst", "Installed", "warehouse", false);
    //createSeries(chart, "br", "Brooken", "warehouse", false);
    //chart.legend = new am4charts.Legend();
    //chart.legend.fontSize = 12;

    //var chart2 = am4core.create("chartDeviceTechnicianJkt", am4charts.PieChart);
    //var dataJson = JSON.stringify({"sBranchID": "BRC0000001" });
    //createChart(chart2, "dashboard_device.aspx/DeviceTechnician", dataJson);
    //createSeriesPie(chart2, "device", "technician");

    //var chart3 = am4core.create("chartDeviceTechnicianSby", am4charts.PieChart);
    //var dataJson = JSON.stringify({"sBranchID": "BRC0000002" });
    //createChart(chart3, "dashboard_device.aspx/DeviceTechnician", dataJson);
    //createSeriesPie(chart3, "device", "technician");

    //var chart4 = am4core.create("chartDeviceTechnicianPdg", am4charts.PieChart);
    //var dataJson = JSON.stringify({"sBranchID": "BRC0000004" });
    //createChart(chart4, "dashboard_device.aspx/DeviceTechnician", dataJson);
    //createSeriesPie(chart4, "device", "technician");

    //var chart5 = am4core.create("chartDeviceTechnicianSmg", am4charts.PieChart);
    //var dataJson = JSON.stringify({"sBranchID": "BRC0000005" });
    //createChart(chart5, "dashboard_device.aspx/DeviceTechnician", dataJson);
    //createSeriesPie(chart5, "device", "technician");

    //var chart6 = am4core.create("chartDeviceTechnicianPlb", am4charts.PieChart);
    //var dataJson = JSON.stringify({"sBranchID": "BRC0000006" });
    //createChart(chart6, "dashboard_device.aspx/DeviceTechnician", dataJson);
    //createSeriesPie(chart6, "device", "technician");

    //console.log(window.screen.width);
    //if (window.screen.width > 720) {
    //    document.getElementById("chartDeviceStatusWarehouse").style.height = '257px';
    //} else {
    //    document.getElementById("chartDeviceStatusWarehouse").style.height = '550px';
    //}
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

//window.onload = function () {
//    //var totalVisitors = 883000;
//    var data = {
//        "TMS vs IGO TRACKER": [{
//            //click: visitorsChartDrilldownHandler,
//            //cursor: "pointer",
//            //explodeOnClick: false,
//            //innerRadius: "75%",
//            //legendMarkerType: "square",
//            //name: "New vs Returning Visitors",
//            radius: "100%",
//            showInLegend: true,
//            startAngle: 90,
//            type: "doughnut",
//            dataPoints: [
//                { y: 219960, name: "IGO TRACKER", color: "#546BC1" },
//                { y: 663040, name: "TMS", color: "#E7823A" }
//            ]
//        }],
//        //"New Visitors": [{
//        // color: "#E7823A",
//        // name: "New Visitors",
//        // type: "column",
//        // xValueFormatString: "MMM YYYY",
//        // dataPoints: [
//        //  { x: new Date("1 Jan 2015"), y: 33000 },
//        //  { x: new Date("1 Feb 2015"), y: 35960 },
//        //  { x: new Date("1 Mar 2015"), y: 42160 },
//        //  { x: new Date("1 Apr 2015"), y: 42240 },
//        //  { x: new Date("1 May 2015"), y: 43200 },
//        //  { x: new Date("1 Jun 2015"), y: 40600 },
//        //  { x: new Date("1 Jul 2015"), y: 42560 },
//        //  { x: new Date("1 Aug 2015"), y: 44280 },
//        //  { x: new Date("1 Sep 2015"), y: 44800 },
//        //  { x: new Date("1 Oct 2015"), y: 48720 },
//        //  { x: new Date("1 Nov 2015"), y: 50840 },
//        //  { x: new Date("1 Dec 2015"), y: 51600 }
//        // ]
//        //}],
//        //"Returning Visitors": [{
//        // color: "#546BC1",
//        // name: "Returning Visitors",
//        // type: "column",
//        // xValueFormatString: "MMM YYYY",
//        // dataPoints: [
//        //  { x: new Date("1 Jan 2015"), y: 22000 },
//        //  { x: new Date("1 Feb 2015"), y: 26040 },
//        //  { x: new Date("1 Mar 2015"), y: 25840 },
//        //  { x: new Date("1 Apr 2015"), y: 23760 },
//        //  { x: new Date("1 May 2015"), y: 28800 },
//        //  { x: new Date("1 Jun 2015"), y: 29400 },
//        //  { x: new Date("1 Jul 2015"), y: 33440 },
//        //  { x: new Date("1 Aug 2015"), y: 37720 },
//        //  { x: new Date("1 Sep 2015"), y: 35200 },
//        //  { x: new Date("1 Oct 2015"), y: 35280 },
//        //  { x: new Date("1 Nov 2015"), y: 31160 },
//        //  { x: new Date("1 Dec 2015"), y: 34400 }
//        // ]
//        //}]
//    };


//    var statusCust = {
//        "STATUS CUSTOMER": [{
//            //click: visitorsChartDrilldownHandler,
//            //cursor: "pointer",
//            //explodeOnClick: false,
//            //innerRadius: "75%",
//            //legendMarkerType: "square",
//            //name: "New vs Returning Visitors",
//            radius: "100%",
//            showInLegend: true,
//            startAngle: 90,
//            type: "doughnut",
//            dataPoints: [
//                { y: 719960, name: "ACTIVE", color: "#00d20a" },
//                { y: 163040, name: "BLOCK", color: "#d22500" }
//            ]
//        }],
//        //"New Visitors": [{
//        // color: "#E7823A",
//        // name: "New Visitors",
//        // type: "column",
//        // xValueFormatString: "MMM YYYY",
//        // dataPoints: [
//        //  { x: new Date("1 Jan 2015"), y: 33000 },
//        //  { x: new Date("1 Feb 2015"), y: 35960 },
//        //  { x: new Date("1 Mar 2015"), y: 42160 },
//        //  { x: new Date("1 Apr 2015"), y: 42240 },
//        //  { x: new Date("1 May 2015"), y: 43200 },
//        //  { x: new Date("1 Jun 2015"), y: 40600 },
//        //  { x: new Date("1 Jul 2015"), y: 42560 },
//        //  { x: new Date("1 Aug 2015"), y: 44280 },
//        //  { x: new Date("1 Sep 2015"), y: 44800 },
//        //  { x: new Date("1 Oct 2015"), y: 48720 },
//        //  { x: new Date("1 Nov 2015"), y: 50840 },
//        //  { x: new Date("1 Dec 2015"), y: 51600 }
//        // ]
//        //}],
//        //"Returning Visitors": [{
//        // color: "#546BC1",
//        // name: "Returning Visitors",
//        // type: "column",
//        // xValueFormatString: "MMM YYYY",
//        // dataPoints: [
//        //  { x: new Date("1 Jan 2015"), y: 22000 },
//        //  { x: new Date("1 Feb 2015"), y: 26040 },
//        //  { x: new Date("1 Mar 2015"), y: 25840 },
//        //  { x: new Date("1 Apr 2015"), y: 23760 },
//        //  { x: new Date("1 May 2015"), y: 28800 },
//        //  { x: new Date("1 Jun 2015"), y: 29400 },
//        //  { x: new Date("1 Jul 2015"), y: 33440 },
//        //  { x: new Date("1 Aug 2015"), y: 37720 },
//        //  { x: new Date("1 Sep 2015"), y: 35200 },
//        //  { x: new Date("1 Oct 2015"), y: 35280 },
//        //  { x: new Date("1 Nov 2015"), y: 31160 },
//        //  { x: new Date("1 Dec 2015"), y: 34400 }
//        // ]
//        //}]
//    };

//    var newVSReturningVisitorsOptions = {
//        animationEnabled: true,
//        theme: "light2",
//        title: {
//            text: "Customer Easygo"
//        },
//        //subtitles: [{
//        // text: "Click on Any Segment to Drilldown",
//        // backgroundColor: "#2eacd1",
//        // fontSize: 16,
//        // fontColor: "white",
//        // padding: 5
//        //}],
//        //legend: {
//        // fontFamily: "calibri",
//        // fontSize: 14,
//        // itemTextFormatter: function (e) {
//        //  return e.dataPoint.name + ": " + Math.round(e.dataPoint.y / totalVisitors * 100) + "%";
//        // }
//        //},
//        //data: []
//    };


//    var statusCustomer = {
//        animationEnabled: true,
//        theme: "light2",
//        title: {
//            text: "Customer TMS Status"
//        },
//        //subtitles: [{
//        // text: "Click on Any Segment to Drilldown",
//        // backgroundColor: "#2eacd1",
//        // fontSize: 16,
//        // fontColor: "white",
//        // padding: 5
//        //}],
//        //legend: {
//        // fontFamily: "calibri",
//        // fontSize: 14,
//        // itemTextFormatter: function (e) {
//        //  return e.dataPoint.name + ": " + Math.round(e.dataPoint.y / totalVisitors * 100) + "%";
//        // }
//        //},
//        //data: []
//    };

//    //var visitorsDrilldownedChartOptions = {
//    // //animationEnabled: true,
//    // //theme: "light2",
//    // //axisX: {
//    // // labelFontColor: "#717171",
//    // // lineColor: "#a2a2a2",
//    // // tickColor: "#a2a2a2"
//    // //},
//    // //axisY: {
//    // // gridThickness: 0,
//    // // includeZero: false,
//    // // labelFontColor: "#717171",
//    // // lineColor: "#a2a2a2",
//    // // tickColor: "#a2a2a2",
//    // // lineThickness: 1
//    // //},
//    // //data: []
//    //};

//    newVSReturningVisitorsOptions.data = data["TMS vs IGO TRACKER"];
//    $("#chartCustomerEasygo").CanvasJSChart(newVSReturningVisitorsOptions);


//    statusCustomer.data = statusCust["STATUS CUSTOMER"];
//    $("#chartCustomerTMSstatus").CanvasJSChart(statusCustomer);

//    //function visitorsChartDrilldownHandler(e) {
//    // e.chart.options = visitorsDrilldownedChartOptions;
//    // e.chart.options.data = visitorsData[e.dataPoint.name];
//    // e.chart.options.title = { text: e.dataPoint.name }
//    // e.chart.render();
//    // $("#backButton").toggleClass("invisible");
//    //}

//    //$("#backButton").click(function() {
//    // $(this).toggleClass("invisible");
//    // newVSReturningVisitorsOptions.data = visitorsData["New vs Returning Visitors"];
//    // $("#chartCustomerEasygo").CanvasJSChart(newVSReturningVisitorsOptions);
//    //});
//}



//$.ajax({
//    url: "dashboard_customer.aspx/jmlCustomerEasygo",
//    dataType: "json",
//    success: function (result) {
//        console.log(result)
//    }
//});

//drawChartJumlahCusotmerEasygo()

//function drawChartJumlahCusotmerEasygo() {

//    var xhr = new XMLHttpRequest();
//    xhr.withCredentials = true;

//    xhr.addEventListener("readystatechange", function () {
//        if (this.readyState === 4) {
//            console.log(this.responseText);
//        }
//    });

//    xhr.open("GET", "dashboard_customer.aspx/jmlCustomerEasygo");
//    xhr.setRequestHeader("Content-Type", "application/json");

//    xhr.send();

    //$.ajax({
    //    url: "dashboard_customer.aspx/jmlCustomerEasygo",
    //    dataType: "json",
    //    success: function (result) {
    //        console.log(result)

    //        //var jmlTMS = 53610,
    //        //    jmlIGO = 730
    //        ////var jmlTMS = result.tms,
    //        ////    jmlIGO = result.igo
    //        //google.charts.load("current", { packages: ["corechart"] });
    //        //google.charts.setOnLoadCallback(drawChart);
    //        //function drawChart() {
    //        //    var data = google.visualization.arrayToDataTable([
    //        //        ['Task', 'Hours per Day'],
    //        //        ['TMS', jmlTMS],
    //        //        ['IGO', jmlIGO]
    //        //    ]);

    //        //    var options = {
    //        //        //title: 'Server TMS dan IGO',
    //        //        pieHole: 0.4,
    //        //        chartArea: { 'width': '100%', 'height': '90%' },
    //        //    };

    //        //    var chart = new google.visualization.PieChart(document.getElementById('chartCustomerEasygo'));
    //        //    chart.draw(data, options);
    //        //}
    //    }
    //});
//}


//   #backButton {
//       border - radius: 4px;
//       padding: 8px;
//       border: none;
//       font - size: 16px;
//       background - color: #2eacd1;
//       color: white;
//       position: absolute;
//       top: 10px;
//       right: 10px;
//       cursor: pointer;
//   }
//.invisible {
//       display: none;
//   }

$('th').click(function () {
    var table = $(this).parents('table').eq(0)
    var rows = table.find('tr:gt(0)').toArray().sort(comparer($(this).index()))
    this.asc = !this.asc
    if (!this.asc) { rows = rows.reverse() }
    for (var i = 0; i < rows.length; i++) { table.append(rows[i]) }
})
function comparer(index) {
    return function (a, b) {
        var valA = getCellValue(a, index), valB = getCellValue(b, index)
        return $.isNumeric(valA) && $.isNumeric(valB) ? valA - valB : valA.toString().localeCompare(valB)
    }
}
function getCellValue(row, index) { return $(row).children('td').eq(index).text() }



function ajaxRequest_get_anggota_rombel(params) {
    var settings = {
        "url": "https://api.mysiakad.my.id/api/rombel/mahasiswa",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json",
            "Authorization": "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiMyIsImNvbXBhbnlfaWQiOiIyIiwicm9sZV9pZCI6IjEiLCJyb2xlIjoiYWRtaW4iLCJlbWFpbCI6ImFkbWluQHN0aWtvbWVscmFobWEuY29tIiwiaWF0IjoxMzU2OTk5NTI0LCJuYmYiOjEzNTcwMDAwMDAsImRldiI6Imh0dHBzOi8vYm9jYWhnYW50ZW5nLmNvbS8iLCJhcHAiOiJteXNpYWthZCJ9.L6m-xn7gPEDn5o61S_P_2SPY_ptJ7F2YcHUoku-gSfo"
        },
        "data": JSON.stringify({
            "rombel_id": ali_rombel_id
        }),
    };

    $.ajax(settings).done(function (response) {
        // console.log(response);
        params.success(response.rows.mahasiswa)
    });

    // var url = 'https://examples.wenzhixin.net.cn/examples/bootstrap_table/data'
    // $.get(url + '?' + $.param(params.data)).then(function (res) {
    //   params.success(res)
    // })
}

function generateChart4() {
    $.ajax({
        type: "POST",
        url: "dashboard_customer.aspx/CustomerRangeUnit",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (r) {
            var json = JSON.parse(r.d);
            console.log(json)
            data = json.data[0];

            var person = data.a,
                range1 = data.b,
                range2 = data.c,
                range3 = data.d,
                range4 = data.e,
                range5 = data.f
            google.charts.load('current', { 'packages': ['corechart'] });
            google.charts.setOnLoadCallback(drawChart);

            function drawChart() {
                var data = google.visualization.arrayToDataTable([
                    ['Task', 'Hours per Day'],
                    ['Personal', person],
                    ['3-5 Unit', range1],
                    ['6-10 Unit', range2],
                    ['11-20 Unit', range3],
                    ['21-30 Unit', range4],
                    ['>30 Unit', range5]
                ]);

                var options = {
                    //title: 'Unit Range'
                    chartArea: { 'width': '100%', 'height': '90%' },
                };

                var chart = new google.visualization.PieChart(document.getElementById('chartCustomerTMSrange'));
                chart.draw(data, options);
            }
        },
        error: function (e) {
            console.log(e);
        }
    });
}
