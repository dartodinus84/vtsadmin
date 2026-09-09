<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="vehicle_position.aspx.cs" Inherits="vtsadm.vehicle_position" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content-header">
        <h1>Vehicle Position <small>dashboard</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="#">Position</a></li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-9">
                <div class="box box-solid">
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <div style="width: 100%; height: 770px" id="maps"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Customer ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtCustID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button2" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-customer"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Full Name</label>
                            <asp:TextBox ID="txtCustFullName" runat="server" class="form-control" placeholder="Full Name ..."></asp:TextBox>
                            <input type="hidden" id="txtPosition" runat="server" />
                        </div>

                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdLoad" CssClass="btn btn-primary" runat="server" OnClick="CmdLoad_Click" Text="Load" />
                        <button id="CmdReal" class="btn btn-primary" onclick="CmdReal_Click();return false;">Realtime</button>
                    </div>
                </div>

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Vehicle</h3>
                        <div class="box-tools">
                            <div class="input-group input-group-sm" style="width: 150px;">
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by Police No ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="CmdSearch" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearch_ServerClick">
                                        <i class="fa fa-search"></i>
                                    </button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="VehicleDesc" HeaderText="Vehicle Desc" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="PoliceNo" HeaderText="Police No" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="Gps_Time" HeaderText="GPS Time" ItemStyle-Wrap="true"></asp:BoundField>

                                        <asp:BoundField DataField="VehicleID" HeaderText="Vehicle ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="Long" HeaderText="Longitude" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="Lat" HeaderText="Latitude" ItemStyle-Wrap="true"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-customer">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Customer</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="vehicle_position_customer_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
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
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </section>

    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBy3GtrgKW_JMEm8atzQaSX6dEehvgJYOQ"></script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        var xmap, loopmarker, prev_long, prev_lat, div_long;
        var markers = new Array();

        function CmdReal_Click() {
            try {
                var d = new Date();
                console.log('timer jalan 1' + ' - ' + d.getFullYear() + '-' + d.getMonth() + '-' + d.getDate() + ' ' + d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds());
                setTimeout(function () {
                    CmdReal_Click();
                }, 180000) // 60000 = 1 menit

                var sCustID = document.getElementById("ContentPlaceHolder1_txtCustID").value;
                var sSearch = document.getElementById("ContentPlaceHolder1_txtSearch").value;
                var dataJson = JSON.stringify({ "sCustID": sCustID, "sPoliceNo": sSearch });
                $.ajax({
                    type: "POST",
                    url: "vehicle_position.aspx/realtime",
                    dataType: "json",
                    data: dataJson,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var r = JSON.parse(data.d);
                        var RowNo = r.data.RowNo;
                        var CustID = r.data.CustID;
                        var VehicleID = r.data.VehicleID;
                        var PoliceNo = r.data.PoliceNo;
                        var Long = r.data.Long;
                        var Lat = r.data.Lat;
                        var GPS_Time = r.data.GPS_Time;
                        console.log(RowNo + ' - ' + PoliceNo + ' - ' + Long + ' - ' + Lat + ' - ' + GPS_Time);
                        div_long = null;
                        loopmarker = 0;
                        console.log(markers);
                        prev_long = markers[parseInt(RowNo)].position.lng();
                        prev_lat = markers[parseInt(RowNo)].position.lat();
                        console.log(PoliceNo + ' - ' + Long + ' - ' + Lat + ' - ' + prev_long + ' - ' + prev_lat);
                        //if (parseFloat(prev_long).toFixed(10) != parseFloat(Long).toFixed(10) && parseFloat(prev_lat).toFixed(10) != parseFloat(Lat).toFixed(10)) {
                            updateMarker(parseInt(RowNo), Long, Lat);
                        //}


                    },
                    error: function (e) {
                        console.log(e);
                    }
                });
            }
            catch (error) {
                console.log(error);
            }
        }

        function updateMarker(m_index, long, lat) {
            prev_long = markers[m_index].position.lng();
            prev_lat = markers[m_index].position.lat();
            //console.log('previous : ' + prev_long + ';' + prev_lat);
            //console.log('current : ' + long + ';' + lat);
            if (div_long == null) {
                div_long = (long - prev_long) / 900;  //60 ==> 60 detik
                div_lat = (lat - prev_lat) / 900;   //60 ==> 60 detik
            }

            var latv = prev_lat + div_lat;
            var lonv = prev_long + div_long;
            //console.log('update : ' + lonv + ';' + latv);
            markers[m_index].setPosition({
                lat: latv,
                lng: lonv
            });

            xmap.setCenter({ lat: latv, lng: lonv });

            loopmarker = loopmarker + 1;
            if (loopmarker == 890) {
                console.log(loopmarker);
            }

            if (parseInt(loopmarker) < 900) {  //60 ==> 60 detik
                //console.log('test_loop : ' + loopmarker);
                setTimeout(function () {
                    updateMarker(m_index, long, lat);
                }, 200) // 1000 setiap 1 detik
            }
        }

        function postCustChild(sCustID, sFullName, sCustTypeDesc, sBranchName) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtCustID').value = sCustID;
                document.getElementById('ContentPlaceHolder1_txtCustFullName').value = sFullName;
                $('#modal-customer').modal('hide');

                var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoad');
                objfr2.click();
            }
        }

        function initMap() {
            var sPos = document.getElementById('ContentPlaceHolder1_txtPosition');
            var sPosition = sPos.value;
            var ilat = '';
            var ilng = '';
            var marker;

            if (sPos.value == '') {
                ilat = -6.17144;
                ilng = 106.82629999999995;
                ilat = Number(ilat);
                ilng = Number(ilng);
                xmap = new google.maps.Map(document.getElementById('maps'), {
                    center: { lat: ilat, lng: ilng },
                    zoom: 15
                });
            }
            else {
                xmap = new google.maps.Map(document.getElementById('maps'));
            }
            markers = [];
            if (sPosition != '') {
                var vehs = sPosition.split("|");
                var i, x, veh, vehid, ilong, ilati, slong, slat;

                for (i = 0; i < vehs.length; i++) {
                    if (vehs[i] != '') {
                        veh = vehs[i].split(";");
                        vehid = veh[0];
                        slong = veh[1];
                        slat = veh[2];
                        if (slong != '') {
                            ilong = Number(slong);
                            ilati = Number(slat);
                            var marker = new google.maps.Marker({
                                position: { lat: ilati, lng: ilong },
                                map: xmap,
                                anchorPoint: new google.maps.Point(0, -29)
                            });
                            markers.push(marker);
                        }
                    }
                }
                xmap.setCenter(new google.maps.LatLng(ilati, ilong));
                xmap.setZoom(15);
                console.log(markers);
                //console.log(marker.position.lng());
                //console.log(markers[9].position.lng());
            }

        }

        function refreshMap() {

        }

        function endRequest(sender, args) {
            initMap();
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerText;
            if (isExists != '') {
                //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
                $('#modal-messagebox').modal('show');
            }
        }
        endRequest();
    </script>
</asp:Content>
