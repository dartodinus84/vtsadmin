<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="view_job_customer_maint_open.aspx.cs" Inherits="vtsadm.view_job_customer_maint_open" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Job Order - Maintenance (Open)
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="dashboard_jo.aspx">Dashboard - Job Order</a></li>
            <li class="active">Maintenance - Open</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    
                </div>

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Job Order - Maintenance (Open)</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                            <asp:BoundField DataField="JobID" HeaderText="Job ID" ItemStyle-Wrap="false" SortExpression="JobID"></asp:BoundField>
                                            <asp:BoundField DataField="sRegDate" HeaderText="Register Date" ItemStyle-Wrap="false" SortExpression="sRegDate"></asp:BoundField>
                                            <asp:BoundField DataField="MaintTypeDesc" HeaderText="Job Type" ItemStyle-Wrap="false" SortExpression="MaintTypeDesc"></asp:BoundField>
                                            <asp:BoundField DataField="CustID" HeaderText="Customer ID" ItemStyle-Wrap="false" SortExpression="CustomerName"></asp:BoundField>
                                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="CustomerName"></asp:BoundField>
                                            <asp:BoundField DataField="MarketingName" HeaderText="Marketing" ItemStyle-Wrap="false" SortExpression="MarketingName"></asp:BoundField>
                                            <asp:BoundField DataField="sSchDate" HeaderText="Schedule Date" ItemStyle-Wrap="false" SortExpression="sSchDate"></asp:BoundField>
                                            <asp:BoundField DataField="CloseDate" HeaderText="Close Date" ItemStyle-Wrap="false" SortExpression="CloseDate"></asp:BoundField>
                                            <asp:BoundField DataField="sla" HeaderText="SLA" ItemStyle-Wrap="false" SortExpression="sla"></asp:BoundField>
                                            <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Wrap="false" SortExpression="Remark"></asp:BoundField>
                                            <asp:BoundField DataField="BillAbleDesc" HeaderText="Billable" ItemStyle-Wrap="false" SortExpression="BillAbleDesc"></asp:BoundField>
                                            <asp:BoundField DataField="IsMigrationDesc" HeaderText="Migration" ItemStyle-Wrap="false" SortExpression="IsMigrationDesc"></asp:BoundField>
                                            <asp:BoundField DataField="Name" HeaderText="Support Area Name" ItemStyle-Wrap="false" SortExpression="Name"></asp:BoundField>
                                            <asp:BoundField DataField="ValueStatus" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="ValueStatus"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;"><asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdExport" CssClass="btn btn-primary" runat="server" OnClick="CmdExport_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportXls_Click" Text="Export XLS" />
                    
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-jobdetails">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List Job Order Maintenance Details</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframejobdetails" src="view_job_order_maint_details.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportDetails" CssClass="btn btn-primary" runat="server" OnClick="CmdExportDetails_Click" Text="Export CSV" />                        
                        <asp:Button ID="CmdExportDetailsXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportDetailsXls_Click" Text="Export XLS" />                        
                       
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
        <div class="modal fade bs-example-modal-lg" id="modal-details">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Job Order Maintenance Details</h4>
                    </div>
                    <div class="modal-body" style="background-color: #ecf0f5">
                        <div class="row">
                            <div class="col-sm-4">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Job Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Job ID</label>
                                            <p id="LblJobID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Register Date</label>
                                            <p id="LblRegDate" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Schedule Date</label>
                                            <p id="LblSchDate" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Others Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Billable</label>
                                            <p id="LblBillable" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Migration</label>
                                            <p id="LblIsMigration" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Remark</label>
                                            <textarea id="txtRemark" runat="server" class="form-control" style="background-color:white;" rows="2" disabled></textarea>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Status</label>
                                            <p id="LblStatus" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Customer Info</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Customer ID</label>
                                            <p id="LblCustID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Customer Name</label>
                                            <p id="LblCustomerName" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">User Info</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>User ID</label>
                                            <p id="LblUserID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Update Date</label>
                                            <p id="LblDateUpdate" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>

                            </div>                            
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

        function CheckNbsp(sbuff) {
            var sOut;
            if (sbuff == "&nbsp;") {
                sOut = "";
            }
            else {
                sOut = sbuff;
            }
            return sOut;
        }

        function postJobOrderDetails(sJobID) {
            if (sJobID != '') {
                $('#modal-jobdetails').modal('show');
                var objfr = document.getElementById('iframejobdetails').contentWindow;
                var objJobID = objfr.document.getElementById('txtJobID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');
                objJobID.value = sJobID;
                cmdSearch.click();
            }
        }

        function postDetails(sJobID, sRegDate, sCustomerName, sSchDate, sBillable, sIsMigrationDesc, sStatus, objDetails, objJobDetails,
            sCustID, sRemark, sBillableID, sIsMigration, sUsrUpd, sDtmUpd) {
            if (sJobID != '') {
                document.getElementById('ContentPlaceHolder1_LblJobID').innerText = CheckNbsp(sJobID);
                document.getElementById('ContentPlaceHolder1_LblRegDate').innerText = CheckNbsp(sRegDate);
                document.getElementById('ContentPlaceHolder1_LblSchDate').innerText = CheckNbsp(sSchDate);
                document.getElementById('ContentPlaceHolder1_LblBillable').innerText = CheckNbsp(sBillable);
                document.getElementById('ContentPlaceHolder1_LblIsMigration').innerText = CheckNbsp(sIsMigrationDesc);
                document.getElementById('ContentPlaceHolder1_txtRemark').innerText = CheckNbsp(sRemark);
                document.getElementById('ContentPlaceHolder1_LblStatus').innerText = CheckNbsp(sStatus);
                document.getElementById('ContentPlaceHolder1_LblCustID').textContent = CheckNbsp(sCustID);
                document.getElementById('ContentPlaceHolder1_LblCustomerName').textContent = CheckNbsp(sCustomerName);
                document.getElementById('ContentPlaceHolder1_LblUserID').textContent = CheckNbsp(sUsrUpd);
                document.getElementById('ContentPlaceHolder1_LblDateUpdate').textContent = CheckNbsp(sDtmUpd);
                $('#modal-details').modal('show');
            }
        }

        function endRequest(sender, args) {
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
                $('#modal-messagebox').modal('show');
            }
        }
        endRequest();
    </script>
</asp:Content>
