<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="view_auth_customer.aspx.cs" Inherits="vtsadm.view_auth_customer" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Customer Authentication           
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">View</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Customer</li>
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
                            <label>Search By</label>
                            <asp:TextBox ID="txtSearch" runat="server" class="form-control" placeholder="Search by any fields ..."></asp:TextBox>
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
                        
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Customer Authentication</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowCommand="GridView2_RowCommand" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="CustID" HeaderText="Cust ID" ItemStyle-Wrap="false" SortExpression="CustID"></asp:BoundField>
                                        <asp:BoundField DataField="FullName" HeaderText="Full Name" ItemStyle-Wrap="false" SortExpression="FullName"></asp:BoundField>
                                        <asp:BoundField DataField="ServerID" HeaderText="Server ID" ItemStyle-Wrap="false" SortExpression="ServerID"></asp:BoundField>
                                        <asp:BoundField DataField="ServerName" HeaderText="Server Name" ItemStyle-Wrap="false" SortExpression="ServerName"></asp:BoundField>
                                        <asp:BoundField DataField="PackageID" HeaderText="Package ID" ItemStyle-Wrap="false" SortExpression="PackageID"></asp:BoundField>
                                        <asp:BoundField DataField="PackageName" HeaderText="Package Name" ItemStyle-Wrap="false" SortExpression="PackageName"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:BoundField DataField="UsrUpd" HeaderText="User Update" ItemStyle-Wrap="false" SortExpression="UsrUpd"></asp:BoundField>
                                        <asp:BoundField DataField="DtmUpd" HeaderText="Date Update" ItemStyle-Wrap="false" SortExpression="DtmUpd"></asp:BoundField>
                                        
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdPic" runat="server" Text="<i class='fa fa-file-image-o'></i>" ToolTip="Logo" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdFavicon" runat="server" Text="<i class='fa fa-image'></i>" ToolTip="Favicon" Enabled="true" CssClass="btn btn-primary btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdBackground" runat="server" Text="<i class='fa fa-image'></i>" ToolTip="Background" Enabled="true" CssClass="btn btn-primary btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="LogoFileName" HeaderText="Logo" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="FaviconFileName" HeaderText="Favicon" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BackgroundFileName" HeaderText="Background" ItemStyle-Wrap="false"></asp:BoundField>
                                        </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="box-footer">
 
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-logo">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Logo Attachment</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <div id="divpic" class="form-group form-group-sm" style="height:300px;text-align:center;">                            
                                <img id="ImgInstall" src="_blank" style="height:100%;width:100%;">
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

         <div class="modal fade bs-example-modal-lg" id="modal-icon">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Favicon Attachment</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <div id="divfav" class="form-group form-group-sm" style="height:300px;text-align:center;">                            
                                <img id="ImgInstallFav" src="_blank" style="height:100%;width:100%;">
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
         <div class="modal fade bs-example-modal-lg" id="modal-background-image">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Favicon Attachment</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <div id="divbac" class="form-group form-group-sm" style="height:300px;text-align:center;">                            
                                <img id="ImgInstallBac" src="_blank" style="height:100%;width:100%;">
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
         <div class="modal fade bs-example-modal-lg" id="modal-favicon">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Favicon</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="fms_auth_customer_upload_fav.aspx" style="width: 100%; border: none; height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>


        <div class="modal fade bs-example-modal-lg" id="modal-background">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Background</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="fms_auth_customer_upload_background.aspx" style="width: 100%; border: none; height: 370px; overflow: hidden;" scrolling="no"></iframe>
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
                        <h4 class="modal-title">Logo</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="fms_auth_customer_upload.aspx" style="width: 100%; border: none; height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
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
                            <iframe src="fms_auth_customer_customer_search.aspx" style="width: 100%; border: none; height: 350px;" overflow:hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-submit">
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
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-submit').modal('hide');" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-submit').modal('hide');">No</button>
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
                        <h6 class="modal-title">Are you sure to delete authentication Customer ID :&nbsp;</h6><label id="LblCustIDDelete" runat="server"></label>&nbsp;- Server ID :&nbsp;
                        <label id="LblServerIDDelete" runat="server"></label>&nbsp;?       
                        <input type="hidden" id="txtCustIDDelete" runat="server" />
                        <input type="hidden" id="txtServerIDDelete" runat="server" />
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

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function postCustChild(sCustID, sFullName) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtCustID').value = sCustID;
                document.getElementById('ContentPlaceHolder1_txtCustFullName').value = sFullName;
                $('#modal-customer').modal('hide');
            }
        }

        function confirmDelete(sCustID, sServerID, sStatus) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_LblCustIDDelete').innerHTML = sCustID;
                document.getElementById('ContentPlaceHolder1_txtCustIDDelete').value = sCustID;
                document.getElementById('ContentPlaceHolder1_LblServerIDDelete').innerHTML = sServerID;
                document.getElementById('ContentPlaceHolder1_txtServerIDDelete').value = sServerID;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete").modal('show');
            }
        }

        function postPic(picFileName) {
            //console.log(picFileName);
            if (picFileName != '') {
                $('#modal-logo').modal('show');
                var img1 = document.getElementById('ImgInstall');
                img1.src = "Picture/" + picFileName;
            }
        }

        function postFavicon(faviconFileName) {
            //console.log(picFileName);
            if (faviconFileName != '') {
                $('#modal-icon').modal('show');
                var img1 = document.getElementById('ImgInstallFav');
                img1.src = "Picture/" + faviconFileName;
            }
        }

        function postBackground(backgroundFileName) {
            //console.log(picFileName);
            if (backgroundFileName != '') {
                $('#modal-background-image').modal('show');
                var img1 = document.getElementById('ImgInstallBac');
                img1.src = "Picture/" + faviconFileName;
            }
        }

        function endRequest(sender, args) {
            $('#modal-customer').on('hidden.bs.modal', function () {
                var objLoad = document.getElementById('ContentPlaceHolder1_CmdLoad');
                objLoad.click();
            });
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
