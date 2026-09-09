<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="gsm.aspx.cs" Inherits="vtsadm.gsm" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>GSM           
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">GSM</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">GSM Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>GSM ID</label>
                            <asp:TextBox ID="txtGSMID" runat="server" class="form-control" placeholder="Skip for new gsm ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>MSIDN</label>
                            <asp:TextBox ID="txtMSIDN" runat="server" class="form-control" placeholder="MSIDN ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Date Arrival</label>
                            <asp:TextBox ID="txtDate" TextMode="Date" runat="server" class="form-control" placeholder="Date Arrival ..." required="required"></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Last Topup</label>
                            <asp:TextBox ID="txtActivation" TextMode="Date" runat="server" class="form-control" placeholder="Activation Date ..." ></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Provider Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Provider Name</label>
                            <asp:DropDownList ID="CmbProviderID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Source</label>
                            <asp:DropDownList ID="CmbSource" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Type</label>
                            <asp:DropDownList ID="CmbType" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>ICC ID</label>
                            <asp:TextBox ID="txtICID" runat="server" class="form-control" placeholder="ICID ..." required="required"></asp:TextBox>
                        </div>

                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                        <button id="CmdUpload" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload">Upload</button>
                        <button id="CmdUploadProvider" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload-provider">Bill Provider</button>
                         <button id="CmdUploadActivation" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload-activation">Topup</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List GSM</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by MSIDN ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="CmdSearch" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearch_ServerClick"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowCommand="GridView2_RowCommand" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="GSMID" HeaderText="GSM ID" ItemStyle-Wrap="false" SortExpression="GSMID"></asp:BoundField>
                                        <asp:BoundField DataField="MSIDN" HeaderText="MSIDN" ItemStyle-Wrap="false" SortExpression="MSIDN"></asp:BoundField>
                                        <asp:BoundField DataField="ProviderID" HeaderText="Provider ID" ItemStyle-Wrap="false" SortExpression="ProviderID"></asp:BoundField>
                                        <asp:BoundField DataField="sDateArrival" HeaderText="Date Arrival" ItemStyle-Wrap="false" SortExpression="sDateArrival"></asp:BoundField>
                                        <asp:BoundField DataField="LastTopupDate" HeaderText="Last Topup" ItemStyle-Wrap="false" SortExpression="sActivationDate"></asp:BoundField>
                                        <asp:BoundField DataField="sExpDate" HeaderText="Expired Date" ItemStyle-Wrap="false" SortExpression="sExpDate"></asp:BoundField>
                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider Name" ItemStyle-Wrap="false" SortExpression="ProviderName"></asp:BoundField>
                                        <asp:BoundField DataField="SourceName" HeaderText="Source Name" ItemStyle-Wrap="false" SortExpression="SourceName"></asp:BoundField>
                                        <asp:BoundField DataField="TypeName" HeaderText="Type Name" ItemStyle-Wrap="false" SortExpression="TypeName"></asp:BoundField>
                                        <asp:BoundField DataField="BatchNo" HeaderText="BatchNo" ItemStyle-Wrap="false" SortExpression="BatchNo"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <%--<asp:ButtonField ControlStyle-CssClass="btn btn-block btn-primary btn-xs" Text="Delete" ButtonType="Image" CommandName="Delete"></asp:ButtonField>--%>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdUpdate" runat="server" Text="<i class='fa fa-history'></i>" ToolTip="Update Status" Enabled="true" CssClass="btn btn-primary btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="SourceID" HeaderText="Source ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ICCID" HeaderText="ICCID" ItemStyle-Wrap="false" SortExpression="ICCID"></asp:BoundField>
                                        <asp:BoundField DataField="TypeID" HeaderText="Type ID" ItemStyle-Wrap="false"></asp:BoundField>
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
                        <h6 class="modal-title">Are you sure to delete Gsm ID :&nbsp;</h6>
                        <label id="LblGsmID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtGsmIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-update">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to update status Gsm ID :&nbsp;</h6>
                        <label id="LblGsmIDUpdate" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtGsmIDUpdate" runat="server" />
                        <input type="hidden" id="txtStatusUpdate" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-update').modal('hide');" onserverclick="CmdYesUpdate_ServerClick" id="CmdYesUpdate">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-update').modal('hide');">No</button>
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
                        <h4 class="modal-title">Upload [<a href="Export/gsm_upload_final.csv">Download Template</a>]</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="gsm_upload.aspx" style="width: 100%; border: none; height: 430px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-upload-provider">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Upload [<a href="Export/compaire_gsm_provider.csv">Download Template</a>]</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="gsm_validation_provider.aspx" style="width: 100%; border: none; height: 530px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        
        <div class="modal fade bs-example-modal-lg" id="modal-upload-activation">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Upload [<a href="Export/gsm_activation_date.csv">Download Template</a>]</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="gsm_activation_date.aspx" style="width: 100%; border: none; height: 530px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
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

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function confirmDelete(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblGsmID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtGsmIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete").modal('show');
            }
        }

        function confirmUpdate(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblGsmIDUpdate').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtGsmIDUpdate').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusUpdate').value = sStatus;
                $("#modal-update").modal('show');
            }
        }

        function endRequest(sender, args) {
            $('#modal-upload').on('hidden.bs.modal', function () {
                var objClear = document.getElementById('ContentPlaceHolder1_CmdClear');
                objClear.click();
            });
            $('#modal-messagebox').on('hidden.bs.modal', function () {
                document.body.style.paddingRight = '0px';
            });
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
                $('#modal-messagebox').modal('show');
                //$('#modal-messagebox').show();
            }
        }
        endRequest();
    </script>
</asp:Content>
