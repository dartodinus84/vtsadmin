<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="device_mutation_in.aspx.cs" Inherits="vtsadm.device_mutation_in" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Procurement Of Goods</h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Mutation</a></li>
            <li class="active">Procurement Of Goods</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Procurement Of Goods Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Type Procurement</label>
                            <asp:DropDownList ID="CmbProcurement" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Warehouse</label>
                            <asp:DropDownList ID="CmbWarehouse" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>Device Type</label>
                                <asp:DropDownList ID="CmbTypeDevice" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="txtQtyDevice">Quantity Device</label>
                                <asp:TextBox ID="txtQtyDevice" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>GSM Type</label>
                                <asp:DropDownList ID="CmbTypeGSM" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="txtQtyGSM">Quantity GSM</label>
                                <asp:TextBox ID="txtQtyGSM" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label for="txtQty12">Relay 12 V</label>
                                <asp:TextBox ID="txtQty12" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="txtQty24">Relay 24 V</label>
                                <asp:TextBox ID="txtQty24" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Header Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Req ID</label>
                            <asp:TextBox ID="txtReqID" runat="server" class="form-control" placeholder="Skip for new job order ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Migration</label>
                            <asp:DropDownList ID="CmbMigration" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Schedule Date</label>
                            <input type="date" id="txtScheduleDate" runat="server" class="form-control" placeholder="Schedule Date ..." />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark ..."></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_Click">Clear</button>
                        <asp:Button ID="CmdTelegram" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-telegram').modal('show');return false;" Text="Telegrams" />
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="confirmSubmit(); return false;" Text="Submit" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Job Order</h3>
                        <div class="box-tools">
                            <div class="input-group input-group-sm" style="width: 200px;">
                                <input type="text" id="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by any fields ..." />
                                <span class="input-group-btn">
                                    <button id="CmdSearch" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearch_ServerClick"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDeleting="GridView1_RowDeleting" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowEditing="GridView1_RowEditing" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" OnSorting="GridView1_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="ProcurementID" HeaderText="Job ID" ItemStyle-Wrap="false" SortExpression="ProcurementID"></asp:BoundField>
                                        <asp:BoundField DataField="ProcurementType" HeaderText="Reg Date" ItemStyle-Wrap="false" SortExpression="ProcurementType"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseID" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="WarehouseID"></asp:BoundField>
                                        <asp:BoundField DataField="QtyDevice" HeaderText="Sch Date" ItemStyle-Wrap="false" SortExpression="QtyDevice"></asp:BoundField>
                                        <asp:BoundField DataField="QtyGSM" HeaderText="Billable" ItemStyle-Wrap="false" SortExpression="QtyGSM"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPagingHeader" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
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
                        <h6 class="modal-title">Are you sure to submit Job ID :&nbsp;</h6>
                        <label id="LblJobIDSubmit" runat="server"></label>
                        &nbsp;?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYesSubmit();" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-submit').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal-telegram">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Telegram Account</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto" Height="350px">
                                <asp:GridView ID="GridView11" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="100" OnRowDataBound="GridView11_RowDataBound">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TelegramName" HeaderText="Telegram Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="AccountID" HeaderText="AccountID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PhoneNumber" HeaderText="PhoneNumber" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Check" HeaderStyle-CssClass="gv-header-center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk1" runat="server" Enabled="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ChatID" HeaderText="ChatID" ItemStyle-Wrap="false"></asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="Black" />
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPagingTelegram" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-delete-header">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete Job ID :&nbsp;</h6>
                        <label id="LblJobID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtReqIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYes();" onserverclick="CmdYes_ServerClick" id="CmdYes">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete-header').modal('hide');">No</button>
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
                            <iframe src="job_maint_customer_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
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
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
    </section>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function postCustChild(sCustID, sFullName, sCustTypeDesc, sBranchName) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtCustID').value = sCustID;
                document.getElementById('ContentPlaceHolder1_txtCustFullName').value = sFullName;
                document.getElementById('ContentPlaceHolder1_txtCustTypeDesc').value = sCustTypeDesc;
                document.getElementById('ContentPlaceHolder1_txtCustBranchName').value = sBranchName;
                $('#modal-customer').modal('hide');
            }
        }
        function updateDevice(objDD) {
            var objfr = document.getElementById("iframedetails").contentWindow;
            var cmdClear = objfr.document.getElementById("Button1");
            var objCustID = objfr.document.getElementById("txtCustID");
            if (objDD.value == "JTP0000002") {
                objCustID.value = document.getElementById("ContentPlaceHolder1_txtCustID").value;
            }
            else {
                objCustID.value = "";
            }
            cmdClear.click();
            return false;
        }

        function showDetails() {
            var jobid;
            jobid = document.getElementById('ContentPlaceHolder1_txtJobID');

            if (jobid.value != '') {
                $('#modal-details').modal('show');
            }
            else {
                document.getElementById('ContentPlaceHolder1_div_comment').innerHTML = '<div class="alert alert-danger" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Failed!</strong> Please create delivery order header first!</div>';
                $('#modal-messagebox').modal('show');
            }
            return false;
        }

        function buttonYesSubmit() {
            $("#modal-submit").modal('hide');
        }

        function confirmSubmit() {
            var objJobID = document.getElementById('ContentPlaceHolder1_txtJobID');
            if (objJobID.value != '') {
                document.getElementById('ContentPlaceHolder1_LblJobIDSubmit').innerHTML = objJobID.value;
                $("#modal-submit").modal('show');
            }
        }

        function buttonYesDetail() {
            $("#modal-delete-detail").modal('hide');
        }

        function confirmDeleteDetail(sText) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblSeq').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtSeqDelete').value = sText;
                $("#modal-delete-detail").modal('show');
            }
        }

        function buttonYes() {
            $("#modal-delete-header").modal('hide');
        }

        function confirmDelete(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblJobID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtJobIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete-header").modal('show');
            }
        }


        function endRequest(sender, args) {
            $("ContentPlaceHolder1_txtRegDate").readOnly = true;
            $('#modal-details').on('hidden.bs.modal', function () {
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
