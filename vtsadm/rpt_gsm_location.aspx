<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_gsm_location.aspx.cs" Inherits="vtsadm.rpt_gsm_location" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    

    <style>
    .uniqueClassName {
        text-align: center;
    }

    .dataTable > thead > tr > th[class*="sort"]:before,
    .dataTable > thead > tr > th[class*="sort"]:after {
        content: "" !important;
    }

    table.dataTable thead .sorting, table.dataTable thead .sorting_asc, table.dataTable thead .sorting_desc, table.dataTable thead .sorting_asc_disabled, table.dataTable thead .sorting_desc_disabled {
        cursor: default;
        position: relative;
    }

    table.table-fit {
        /*! width: auto !important; */
        /*! table-layout: auto !important; */
    }

    table.table-fit thead th, table.table-fit tfoot th {
        width: auto !important;
    }

    table.table-fit tbody td, table.table-fit tfoot td {
        white-space: nowrap !important;
    }

    .table td.fit, 
    .table th.fit {
        white-space: nowrap;
        width: 1%;
    }
    </style>

    <section class="content-header">
        <h1>Gsm - Postpaid
                <small>Report</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Report</a></li>
            <li><a href="#">Job Order</a></li>
            <li class="active">New</li>
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
                            <label>Search By Kota / Address</label>
                            <input type="text" name="kota" id="kota" class="form-control" placeholder="Search by kota ..."/>
                        </div>
                        
                        <div class="form-group form-group-sm">
                            <label>Radius dari titik user (Meter) optional </label>
                            <input type="number" name="radius" id="radius" class="form-control" placeholder="Radius ..."/>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Geofence Type</label>
                            <asp:DropDownList ID="CmbGeofenceField" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>


                    </div>
                    <div class="box-footer">
                       
                        <input type="button" class="btn btn-primary" value="Search" onclick="changeSearch()">
                    </div>
                </div>

                
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Gsm - Postpaid</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <table id="tableData" class="table table-scroll table-striped table-bordered table-hover table-checkable table-sm table-fit"
                               data-toolbar="#toolbar"
                               data-toggle="table"
                               data-search="false"
                               data-show-toggle="false"
                               data-show-export="true"
                               data-single-select="false"
                               data-show-columns="false"
                               data-search-align="left"
                               data-show-jump-to="false"
                               data-show-columns-toggle-all="true"
                               data-pagination="true"
                               data-page-size="10"
                               data-page-list="[10, 25, 50, 100, all]"
                               data-buttons-class="btn btn-info btn-pill btn-icon btn-elevate btn-elevate-air" style="width:100% !important">
                                <thead>
                                    <tr>
                                        <th data-valign="middle" data-sortable="true" data-formatter="btnAction">#</th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="gps_sn"><strong>SN No</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="car_plate"><strong>Nopol</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="customer_name"><strong>Customer Name</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="marketing_name"><strong>Marketing Name</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="gsm_no"><strong>GSM No</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="gsm_source"><strong>GSM Source</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="status_gsm"><strong>Status GSM</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="lon"><strong>Lon</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="lat"><strong>Lat</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="addr"><strong>Address</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="kota"><strong>Kota</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="gps_time"><strong>GPS Time</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="acc"><strong>ACC</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="speed"><strong>Speed</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="driver_nm"><strong>Driver</strong></th>
                                        <th data-valign="middle" data-align="left" data-sortable="true" data-field="phone"><strong>Phone</strong></th>


                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                    <div class="box-footer">
                        
                    </div>
                </div>
                
            </div>
        </div>

        <div class="modal" tabindex="-1" role="dialog" id="frmMaps">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">×</span>
                </button>
                <h4 class="modal-title">Maps</h4>
              </div>
              <div class="modal-body">
                  
              </div>
              <div class="modal-footer">
                
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
              </div>
            </div>
          </div>
        </div>

     

        <div id="overlay">
            <div class="cv-spinner">
                <span class="spinner"></span>
            </div>
        </div>

    </section>

    
    <script type="text/javascript">
        
        $("#tableData").delegate('.detail-maps', 'click', function () {
            //$(this).attr('data-index')

            var that = $(this);
            var index = that.attr('data-index');

            var data = $("#tableData").bootstrapTable('getData')[index];
            var lat = data.lat.replace(',', '.');
            var lon = data.lon.replace(',', '.');
            
            console.log(lat);
            console.log(lon);
            const html = `<iframe src="https://maps.google.com/maps?q=${lat}, ${lon}&z=15&output=embed" height="300" frameborder="0" style="border:0; width:100%"></iframe>`;
            $('#frmMaps .modal-body').empty().html(html)

            //var ifr = document.getElementById("frameMaps");
            //ifr.src = "https://maps.google.com/maps?q=" + data.lat + "," + data.lon + "&hl=es&z=14&amp;output=embed";
            $('#frmMaps').modal('show');
        });


        function btnAction (value, row, index) {
            var html = "<div class='columns columns-right btn-group float-right'>";
            html += "<button type='button' class='btn btn-btn btn-sm btn-info btn-pill btn-icon btn-elevate btn-elevate-air detail-maps' data-tag='maps' data-index='"+index+"'><i class='fa fa-map-marker'></i></button>";
            html += "</div>";
            return html;
        }

        function changeSearch() {
            var geo_type = $('#ContentPlaceHolder1_CmbGeofenceField').val();
            var kota = $('#kota').val();
            var radius = $('#radius').val();
            
            if ("geolocation" in navigator){
		      
                navigator.geolocation.getCurrentPosition(function (position) { 
                    var lat = position.coords.latitude;
                    var lon = position.coords.longitude;

                    if (radius === "") {
                        radius = "0";
                    }

                    if (geo_type === "[Select]"){
                        geo_type = 99;
                    }
                    
                    var objvar = {
                            "kota": kota.toString(),
                            "radius": radius.toString(),
                            "geo_type": geo_type.toString(),
                            "lon": lon.toString(),
                            "lat": lat.toString()
                        };

                    
                    $.ajax({
                        type: "POST",
                        url: "rpt_gsm_location.aspx/getdatagsm",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(objvar),
                        beforeSend: function () {
                            $("#overlay").fadeIn(300);
                        },
                        success: function (r) {
                            var json = JSON.parse(r.d);
                            var data = json.data;
                            $('#tableData').bootstrapTable('destroy').bootstrapTable('load').bootstrapTable({
                                exportTypes: ['excel'],
                                exportDataType: 'all',
                                data: data
                            });
                            data = [];

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

                });

	        }else{
		        console.log("Browser doesn't support geolocation!");
	        }
        }
    </script>
</asp:Content>
