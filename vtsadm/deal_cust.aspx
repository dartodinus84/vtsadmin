<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="deal_cust.aspx.cs" Inherits="vtsadm.deal_cust" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .form-control {
            background-color: white !important;
        }
    </style>

    <section class="content-header">
        <h1>Customer
                <small>Deal</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Deal</a></li>
            <li class="active">Customer</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6 col-xs-12">
                <div id="box-jo" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Job Deal Information</h3>
                        <div class="box-tools pull-right">
                            <button id="btnid" type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Deal Code</label>
                            <div class="input-group input-group-sm">
                                <input type="hidden" id="txtJobActivityID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <input type="text" id="txtActivityCode" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button5" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-jobdeal"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Request Date</label>
                            <input type="text" id="txtReqDate" runat="server" class="form-control" placeholder="Request Date ..." readonly="readonly" />
                        </div>

                    </div>
                </div>
                <div id="box-customer" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <input type="text" id="txtCustName" runat="server" class="form-control" placeholder="Customer Name ..." readonly="readonly" />
                            <input type="hidden" id="txtCustID" runat="server" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <input type="text" id="txtCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Marketing Name</label>
                            <input type="text" id="txtMarketingName" runat="server" class="form-control" placeholder="Marketing Name ..." readonly="readonly" />
                        </div>

                        <div class="form-group form-group-sm">
                            <label>PIC Name</label>
                            <input type="text" id="txtPICName" runat="server" class="form-control" placeholder="PIC Name ..." readonly="readonly" />
                        </div>

                        <div class="form-group form-group-sm">
                            <label>PIC Phone</label>
                            <input type="text" id="txtPICPhone" runat="server" class="form-control" placeholder="PIC Phone ..." readonly="readonly" />
                        </div>

                    </div>
                </div>

            </div>
            <div class="col-md-6 col-xs-12">

                <div id="box-installation" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Deal Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Product Name</label>
                            <input type="text" id="txtProductName" runat="server" class="form-control" placeholder="Product Name ..." readonly="readonly" />
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Price</label>
                            <input type="text" id="txtPrice" runat="server" class="form-control" placeholder="Price ..." readonly="readonly" />
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Activity Date</label>
                            <asp:TextBox ID="txtActivityDate" TextMode="Date" runat="server" class="form-control" placeholder="Activity Date ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Next Follow-Up Date</label>
                            <asp:TextBox ID="txtFollowUpDate" TextMode="Date" runat="server" class="form-control" placeholder="Follwu Up Date ..."></asp:TextBox>
                        </div>

                        <!-- Location Tagging Feature -->
                        <div class="form-group">
                            <label>Current Location</label>
                            <div class="input-group">
                                <button type="button" id="btnGetLocation" class="btn btn-primary" onclick="getCurrentLocation();">
                                    <i class="fa fa-map-marker"></i> Tag Current Location
                                </button>
                            </div>
                            <div id="locationStatus" class="help-block"></div>
                            <!-- Hidden fields to store latitude and longitude -->
                            <input type="hidden" id="hfLatitude" runat="server" />
                            <input type="hidden" id="hfLongitude" runat="server" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Address Location</label>
                            <textarea id="txtAddressLocation" runat="server" class="form-control" placeholder="Address Location ..." rows="2"></textarea>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Stage</label>
                            <asp:DropDownList ID="CmbDealStatusID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <textarea id="txtRemark" runat="server" class="form-control" placeholder="Remark ..." rows="2"/>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Picture Attachment</label>
                            <p>
                                <button id="CmdUpload" type="button" class="btn btn-primary" onclick="$('#modal-picture').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image</button>
                            </p>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>File Attachment</label>
                            <p>
                                <button id="CmdUploadFile" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload">Upload File</button>
                            </p>
                        </div>

                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="reset" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdLoadDeal" CssClass="btn btn-primary" runat="server" OnClick="CmdLoadDeal_Click" Text="Load" />
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                        
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Detail</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDeleting="GridView2_RowDeleting" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="ActivityCode" HeaderText="Activity Code" ItemStyle-Wrap="false" SortExpression="ActivityCode"></asp:BoundField>
                                        <asp:BoundField DataField="ActivityDate" HeaderText="Activity Date" ItemStyle-Wrap="false" SortExpression="ActivityDate"></asp:BoundField>
                                        <asp:BoundField DataField="FollowUpDate" HeaderText="FollowUpDate" ItemStyle-Wrap="false" SortExpression="FollowUpDate"></asp:BoundField>
                                        <asp:BoundField DataField="HeaderRemark" HeaderText="Remark" ItemStyle-Wrap="true" SortExpression="HeaderRemark"></asp:BoundField>
                                        <asp:BoundField DataField="DealStatusDesc" HeaderText="Deal Status" ItemStyle-Wrap="true" SortExpression="DealStatusDesc"></asp:BoundField>
                                        <asp:BoundField DataField="HeaderStatus" HeaderText="Status" ItemStyle-Wrap="true" SortExpression="HeaderStatus"></asp:BoundField>
                                        <asp:BoundField DataField="HeaderUsrUpd" HeaderText="UsrUpd" ItemStyle-Wrap="false" SortExpression="HeaderUsrUpd"></asp:BoundField>
                                        <asp:BoundField DataField="HeaderDtmUpd" HeaderText="DtmUpd" ItemStyle-Wrap="false" SortExpression="HeaderDtmUpd"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdPic" runat="server" Text="<i class='fa fa-file-image-o'></i>" ToolTip="Picture" Enabled="true" CssClass="btn btn-warning btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="PictureFileName" HeaderText="Picture FileName" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ActivityID" HeaderText="ActivityID" ItemStyle-Wrap="false" SortExpression="ActivityID"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
     
        <div class="modal fade" id="modal-submit" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure ?</h6>
                    </div>
                    <div class="modal-footer">
                        <img src="Content/ajax-loader2.gif" style="display:none;" id="iload"/>
                        <button type="button" class="btn btn-default btn-confirmasi" runat="server" onclick="$('.btn-confirmasi').attr('disabled','disabled');$('button.close').hide();$('#iload').show();" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
                        <button type="button" class="btn btn-primary btn-confirmasi" onclick="$('#modal-submit').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-upload">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Upload</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="deal_cust_upload_doc.aspx" style="width: 100%; border: none; height: 460px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="deal_picture_upload.aspx" style="width: 100%; border: none; height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-pic">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture Attachment</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <div id="divpic" class="form-group form-group-sm" style="height: 300px; text-align: center;">
                                <img id="ImgInstall" src="_blank" style="height: 100%; width: 100%;">
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-jobdeal">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Job Deal</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="deal_cust_job_deal_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-delete">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete :&nbsp;</h6>
                        <label id="LblActivityID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtActivityIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal modal-open fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="$('.modal-backdrop').remove();">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Info Box</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" id="div_comment" runat="server">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" onclick="$('.modal-backdrop').remove();" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript" src="Scripts/geolocation.js"></script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function checkNbsp(sBuff) {
            var sOut
            sOut = sBuff;
            if (sBuff == '&nbsp;') {
                sOut = '';
            }
            return sOut;
        }

        function postPic(picFileName) {
            console.log(picFileName);
            if (picFileName != '') {
                $('#modal-pic').modal('show');
                var img1 = document.getElementById('ImgInstall');
                img1.src = "Picture/" + picFileName;
                //img1.he
            }
        }

        function confirmDelete(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblActivityID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtActivityIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete").modal('show');
            }
        }

        function getCurrentLocation() {
            if (navigator.geolocation) {
                // Show loading indicator or message
                document.getElementById('locationStatus').innerHTML = '<i class="fa fa-spinner fa-spin"></i> Getting location...';

                navigator.geolocation.getCurrentPosition(
                    function (position) {
                        // Success callback
                        var latitude = position.coords.latitude;
                        var longitude = position.coords.longitude;

                        // Update hidden fields with location data
                        document.getElementById('<%=hfLatitude.ClientID%>').value = latitude;
                        document.getElementById('<%=hfLongitude.ClientID%>').value = longitude;

                        // Update status message
                        document.getElementById('locationStatus').innerHTML =
                            '<i class="fa fa-map-marker"></i> Location captured: ' +
                            latitude.toFixed(6) + ', ' + longitude.toFixed(6);
                    },
                    function (error) {
                        // Error callback
                        var errorMessage;
                        switch (error.code) {
                            case error.PERMISSION_DENIED:
                                errorMessage = "User denied the request for geolocation.";
                                break;
                            case error.POSITION_UNAVAILABLE:
                                errorMessage = "Location information is unavailable.";
                                break;
                            case error.TIMEOUT:
                                errorMessage = "The request to get user location timed out.";
                                break;
                            case error.UNKNOWN_ERROR:
                                errorMessage = "An unknown error occurred.";
                                break;
                        }
                        document.getElementById('locationStatus').innerHTML =
                            '<i class="fa fa-exclamation-triangle"></i> Error: ' + errorMessage;
                    },
                    {
                        enableHighAccuracy: true,
                        timeout: 10000,
                        maximumAge: 0
                    }
                );
            } else {
                document.getElementById('locationStatus').innerHTML =
                    '<i class="fa fa-exclamation-triangle"></i> Geolocation is not supported by this browser.';
            }
        }

        function postJobDealChild(sJobActivityID, sJobActivityCode, sReqDate, sFullName, sProductName, sCustID, sCustBranchName, sBusinessFieldID, sPicName, sPicPhone, sMarketingName, sPrice) {
           
            if (sJobActivityID != '') {
                document.getElementById('ContentPlaceHolder1_txtJobActivityID').value = checkNbsp(sJobActivityID);
                document.getElementById('ContentPlaceHolder1_txtActivityCode').value = checkNbsp(sJobActivityCode);
                document.getElementById('ContentPlaceHolder1_txtReqDate').value = checkNbsp(sReqDate);
                document.getElementById('ContentPlaceHolder1_txtProductName').value = checkNbsp(sProductName); 
                document.getElementById('ContentPlaceHolder1_txtCustName').value = checkNbsp(sFullName);
       
                document.getElementById('ContentPlaceHolder1_txtCustBranchName').value = checkNbsp(sCustBranchName);
                document.getElementById('ContentPlaceHolder1_txtCustID').value = checkNbsp(sCustID);

                document.getElementById('ContentPlaceHolder1_txtPICName').value = checkNbsp(sPicName);
                document.getElementById('ContentPlaceHolder1_txtPICPhone').value = checkNbsp(sPicPhone);

                document.getElementById('ContentPlaceHolder1_txtMarketingName').value = checkNbsp(sMarketingName);
                document.getElementById('ContentPlaceHolder1_txtPrice').value = checkNbsp(sPrice);

                //console.log(checkNbsp(sJobActivityID));
                $('#modal-jobdeal').modal('hide');

                var objfr1 = document.getElementById('ContentPlaceHolder1_CmdLoadDeal');
                objfr1.click();
            }
        }

        function endRequest(sender, args) {
            $('#modal-messagebox').on('hidden.bs.modal', function () {
                document.body.style.paddingRight = '0px';
            });
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
                $('#modal-messagebox').modal('show');
            }
        }
        endRequest();
    </script>
</asp:Content>
