<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="job_assign_technician.aspx.cs" Inherits="vtsadm.job_assign_technician" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Job Order <small>Assign Technician</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Job Order</a></li>
            <li class="active">Assign Technician</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Job Order Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Job ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtJobID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button2" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-job_order"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Register Date</label>
                            <input type="text" id="txtRegDate2" runat="server" class="form-control" placeholder="Register Date ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer ID</label>
                            <input type="text" id="txtJobCustID" runat="server" class="form-control" placeholder="Customer ID ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <input type="text" id="txtJobCustName" runat="server" class="form-control" placeholder="Customer Name ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Po ID</label>
                            <input type="text" id="txtPoID" runat="server" class="form-control" placeholder="Po ID ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Schedule Date</label>
                            <input type="text" id="txtScheduleDate2" runat="server" class="form-control" placeholder="Schedule Date ..." readonly="readonly" />
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Detail Job Order Assign</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowDataBound="GridView2_RowDataBound" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="AssignID" HeaderText="Assign ID" ItemStyle-Wrap="false" SortExpression="AssignID"></asp:BoundField>
                                        <asp:BoundField DataField="JobID" HeaderText="Job ID" ItemStyle-Wrap="false" SortExpression="JobID"></asp:BoundField>
                                        <asp:BoundField DataField="Seq" HeaderText="Seq" ItemStyle-Wrap="false" SortExpression="Seq"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianID" HeaderText="Technician ID" ItemStyle-Wrap="false" SortExpression="TechnicianID"></asp:BoundField>
                                        <asp:BoundField DataField="SchDate" HeaderText="Schedule Date" ItemStyle-Wrap="false" SortExpression="TechnicianID"></asp:BoundField>
                                        <asp:BoundField DataField="Name" HeaderText="Name Technician" ItemStyle-Wrap="false" SortExpression="Name"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDeleteDetail" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" Wrap="True" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPagingDetail" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Schedule Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Assign ID</label>
                            <asp:TextBox ID="txtAssignID" runat="server" class="form-control" placeholder="Skip for new job order ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Register Date</label>
                            <input type="date" id="txtRegDate" runat="server" class="form-control" placeholder="Register Date ..." />
                        </div>
                      <%--  <div class="form-group form-group-sm">
                            <label>Schedule Date</label>
                            <input type="date" id="txtScheduleDate" runat="server" class="form-control" placeholder="Schedule Date ..." />
                        </div>--%>

                        <div class="form-group form-group-sm">
                            <label>Area</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtAreaID" runat="server" class="form-control" placeholder="Area ID ..." readonly="readonly" />
                                <span class="input-group-btn">
                                    <button id="Button1" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-area"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Area Name</label>
                            <input type="text" id="txtAreaName" runat="server" class="form-control" placeholder="Area Name ..." readonly="readonly" />
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark ..."></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_Click">Clear</button>
                        <button id="CmdCreate" type="button" class="btn btn-primary" runat="server" onserverclick="CmdCreate_Click">Create</button>
                        <button id="CmdAddDetail" type="button" class="btn btn-primary" runat="server" onclick="return showDetails();">Add Details</button>
                        <button id="CmdAddTraining" type="button" class="btn btn-primary" runat="server" onclick="return showDetails2();">Add Details</button>
                        <asp:Button ID="CmdTelegram" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-telegram').modal('show');return false;" Text="Telegrams" />
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="confirmSubmit(); return false;" Text="Submit" />
                        <button id="CmdLoad" type="button" class="btn btn-primary" style="visibility: hidden;" runat="server" onserverclick="CmdLoad_Click">1</button>
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
                                        <asp:BoundField DataField="AssignID" HeaderText="Assign ID" ItemStyle-Wrap="false" SortExpression="Assign ID"></asp:BoundField>
                                        <asp:BoundField DataField="JobID" HeaderText="Job ID" ItemStyle-Wrap="false" SortExpression="JobID"></asp:BoundField>
                                        <asp:BoundField DataField="sRegDate" HeaderText="Reg Date" ItemStyle-Wrap="false" SortExpression="sRegDate"></asp:BoundField>
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="CustomerName"></asp:BoundField>
                                        <asp:BoundField DataField="AreaName" HeaderText="Area Name" ItemStyle-Wrap="false" SortExpression="AreaName"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CustID" HeaderText="Customer ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="CustTypeDesc" HeaderText="Customer Type" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BranchName" HeaderText="Branch" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PoID" HeaderText="PO ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="AreaID" HeaderText="AreaID" ItemStyle-Wrap="false"></asp:BoundField>

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
        <div class="modal fade" id="modal-delete-detail">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete sequence :&nbsp;</h6>
                        <label id="LblSeq" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtSeqDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYesDetail();" onserverclick="CmdYesDetail_ServerClick" id="CmdYesDetail">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete-detail').modal('hide');">No</button>
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
                        <input type="hidden" id="txtAssignIDDelete" runat="server" />
                        <input type="hidden" id="txtJobIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYes();" onserverclick="CmdYes_ServerClick" id="CmdYes">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete-header').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-job_order" style="max-height:100%" >
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Job Order</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframejoborder" src="job_assign_search.aspx" style="width: 100%; border: none; height: 450px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- /.modal -->
        <div class="modal modal-open fade" id="modal-details" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Add Detail Information</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframedetails" src="job_assign_details.aspx" style="width: 100%; border: none; height: 800px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
         <div class="modal modal-open fade" id="modal-details2" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Add Detail Information</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframedetails2" src="job_assign_details_it.aspx" style="width: 100%; border: none; height: 800px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-area">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Area</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframearea" src="job_assign_area_search.aspx" style="width: 100%; border: none; height: 450px; overflow: hidden;" scrolling="no"></iframe>
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
        function checkNbsp(sBuff) {
            var sOut
            sOut = sBuff;
            if (sBuff == '&nbsp;') {
                sOut = '';
            }
            return sOut;
        }

        function postJobOrderChild(sJobID, sRegDate, sFullName, sPOID, sDeviceTypeDesc, sSchDate, objCmd, sCustID) {
            if (sJobID != '') {
                document.getElementById('ContentPlaceHolder1_txtJobID').value = checkNbsp(sJobID);
                document.getElementById('ContentPlaceHolder1_txtRegDate2').value = checkNbsp(sRegDate);
                document.getElementById('ContentPlaceHolder1_txtJobCustID').value = checkNbsp(sCustID);
                document.getElementById('ContentPlaceHolder1_txtJobCustName').value = checkNbsp(sFullName);
                document.getElementById('ContentPlaceHolder1_txtPoID').value = checkNbsp(sPOID);
                document.getElementById('ContentPlaceHolder1_txtScheduleDate2').value = checkNbsp(sSchDate);

                $('#modal-job_order').modal('hide');

                var objfr = document.getElementById('iframejoborder').contentWindow;
                var objJobID = objfr.document.getElementById('txtJobID');
                var objCustID = objfr.document.getElementById('txtCustID');
                var cmdSearch = objfr.document.getElementById('CmdSearchVehicle');
                objCustID.value = sCustID;
                cmdSearch.click();
            }
        }

        function postAreaChild(sAreaID, sAreaName, objCmd) {
            if (sAreaID != '') {
                document.getElementById('ContentPlaceHolder1_txtAreaID').value = checkNbsp(sAreaID);
                document.getElementById('ContentPlaceHolder1_txtAreaName').value = checkNbsp(sAreaName);
                console.log(sAreaID);
                $('#modal-area').modal('hide');
                
                var objfr1 = document.getElementById('iframearea').contentWindow;
                var objAreaID = objfr1.document.getElementById('txtAreaID');
                var objAreaName = objfr1.document.getElementById('txtAreaName');
              
                var cmdSearchArea = objfr1.document.getElementById('CmdSearchVehicle');
                
                objAreaID.value = sAreaID;
                objAreaName.value = sAreaName;
                cmdSearchArea.click();
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
            var assignid;

            jobid = document.getElementById('ContentPlaceHolder1_txtJobID');
            jobid = document.getElementById('ContentPlaceHolder1_txtJobID');
            assignid = document.getElementById('ContentPlaceHolder1_txtAssignID');
            areaid = document.getElementById('ContentPlaceHolder1_txtAreaID');

            if (jobid.value != '') {
                $('#modal-details').modal('show');
            }
            else {
                document.getElementById('ContentPlaceHolder1_div_comment').innerHTML = '<div class="alert alert-danger" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Failed!</strong> Please create delivery order header first!</div>';
                $('#modal-messagebox').modal('show');
            }
            return false;
        }

        function showDetails2() {
            var jobid;
            var assignid;

            jobid = document.getElementById('ContentPlaceHolder1_txtJobID');
            jobid = document.getElementById('ContentPlaceHolder1_txtJobID');
            assignid = document.getElementById('ContentPlaceHolder1_txtAssignID');

            if (jobid.value != '') {
                $('#modal-details2').modal('show');
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
                document.getElementById('ContentPlaceHolder1_txtAssignIDDelete').value = sText;
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
            $('#modal-details2').on('hidden.bs.modal', function () {
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
