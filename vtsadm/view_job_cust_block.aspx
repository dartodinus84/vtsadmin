<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="view_job_cust_block.aspx.cs" Inherits="vtsadm.view_job_cust_block" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Job Order - Customer - Blocked
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">View</a></li>
            <li><a href="#">Job Order</a></li>
            <li><a href="#">Customer</a></li>
            <li class="active">Blocked</li>
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
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Job Order - Customer - Blocked</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="BlockID" HeaderText="Block ID" ItemStyle-Wrap="false" SortExpression="BlockID"></asp:BoundField>
                                        <asp:BoundField DataField="sReqDate" HeaderText="Request Date" ItemStyle-Wrap="false" SortExpression="sReqDate"></asp:BoundField>
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="CustomerName"></asp:BoundField>
                                        <asp:BoundField DataField="sSchDate" HeaderText="Schedule Date" ItemStyle-Wrap="false" SortExpression="sSchDate"></asp:BoundField>
                                        <asp:BoundField DataField="ValueStatus" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="ValueStatus"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDetails" runat="server" Text="<i class='fa fa-list-alt'></i>" ToolTip="Details" Enabled="true" CssClass="btn btn-primary btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdPic" runat="server" Text="<i class='fa fa-file-image-o'></i>" ToolTip="Picture" Enabled="true" CssClass="btn btn-warning btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CustID" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Remark" HeaderText="Remark Order" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UsrUpd" HeaderText="User Update" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DtmUpd" HeaderText="Date Update" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BlockedID" HeaderText="BlockedID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="sBlockedDate" HeaderText="Blocked Date" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="RemarkBlocked" HeaderText="Remark Blocked" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UsrUpdBlocked" HeaderText="User Update" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DtmUpdBlocked" HeaderText="Date Update" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PictureFileName" HeaderText="Picture FileName" ItemStyle-Wrap="false"></asp:BoundField>
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
        <div class="modal fade bs-example-modal-lg" id="modal-details">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Job Order Customer Blocked Details</h4>
                    </div>
                    <div class="modal-body" style="background-color: #ecf0f5">
                        <div class="row">
                            <div class="col-sm-4">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Job Info</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Block ID</label>
                                            <p id="LblBlockID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Customer Name</label>
                                            <p id="LblCustName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Request Date</label>
                                            <p id="LblReqDate" runat="server" class="form-control-static"></p>
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
                                            <label>Remark</label>
                                            <textarea id="txtRemark" runat="server" class="form-control" style="background-color:white;" rows="1" disabled></textarea>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Status</label>
                                            <p id="LblStatus" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>User Update Order</label>
                                            <p id="LblUsrUpdOrder" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Date Update Order</label>
                                            <p id="LblDtmUpdOrder" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Blocked Info</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Blocked Date</label>
                                            <p id="LblBlockedDate" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Blocked Remark</label>
                                            <textarea id="txtBlockedRemark" runat="server" class="form-control" style="background-color:white;" rows="1" disabled></textarea>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>User Update</label>
                                            <p id="LblUsrUpd" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Date Update</label>
                                            <p id="LblDtmUpd" runat="server" class="form-control-static"></p>
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
            </div>
        </div>
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

        function postPic(picFileName) {
            if (picFileName != '') {
                $('#modal-pic').modal('show');
                var img1 = document.getElementById('ImgInstall');
                img1.src = "Picture/" + picFileName;
            }
        }
        function postDetails(sBlockID, sReqDate, sCustomerName, sSchDate, sValueStatus, objDetails, objPic, sCustID, sRemark, 
                             sUsrUpd, sDtmUpd, sBlockedID, sBlockedDate, sRemarkBlocked, sUsrUpdBlocked, sDtmUpdBlocked, sPictureFile) {
            if (sBlockID != '') {
                document.getElementById('ContentPlaceHolder1_LblBlockID').innerText = CheckNbsp(sBlockID);
                document.getElementById('ContentPlaceHolder1_LblCustName').innerText = CheckNbsp(sCustomerName);
                document.getElementById('ContentPlaceHolder1_LblReqDate').innerText = CheckNbsp(sReqDate);
                document.getElementById('ContentPlaceHolder1_LblSchDate').innerText = CheckNbsp(sSchDate);
                document.getElementById('ContentPlaceHolder1_txtRemark').innerText = CheckNbsp(sRemark);
                document.getElementById('ContentPlaceHolder1_LblStatus').innerText = CheckNbsp(sValueStatus);
                document.getElementById('ContentPlaceHolder1_LblUsrUpdOrder').innerText = CheckNbsp(sUsrUpd);
                document.getElementById('ContentPlaceHolder1_LblDtmUpdOrder').innerText = CheckNbsp(sDtmUpd);
                document.getElementById('ContentPlaceHolder1_LblBlockedDate').textContent = CheckNbsp(sBlockedDate);
                document.getElementById('ContentPlaceHolder1_txtBlockedRemark').textContent = CheckNbsp(sRemarkBlocked);
                document.getElementById('ContentPlaceHolder1_LblUsrUpd').textContent = CheckNbsp(sUsrUpdBlocked);
                document.getElementById('ContentPlaceHolder1_LblDtmUpd').textContent = CheckNbsp(sDtmUpdBlocked);

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
