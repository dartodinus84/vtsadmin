<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="view_job_training_close.aspx.cs" Inherits="vtsadm.view_job_training_close" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Job Order - Customer - Training 
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="dashboard_jo.aspx">Dashboard - Job Order</a></li>
            <li class="active">Training - Close</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    
                </div>

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Job Order - Customer - Training</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TrainingID" HeaderText="Training ID" ItemStyle-Wrap="false" SortExpression="TrainingID"></asp:BoundField>
                                        <asp:BoundField DataField="sReqDate" HeaderText="Request Date" ItemStyle-Wrap="false" SortExpression="sReqDate"></asp:BoundField>
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="CustomerName"></asp:BoundField>
                                        <asp:BoundField DataField="sSchDate" HeaderText="Schedule Date" ItemStyle-Wrap="false" SortExpression="sSchDate"></asp:BoundField>
                                        <asp:BoundField DataField="CloseDate" HeaderText="Close Date" ItemStyle-Wrap="false" SortExpression="CloseDate"></asp:BoundField>
                                        <asp:BoundField DataField="sla" HeaderText="SLA" ItemStyle-Wrap="false" SortExpression="sla"></asp:BoundField>
                                        <asp:BoundField DataField="RemarkTraining" HeaderText="Remark" ItemStyle-Wrap="false" SortExpression="RemarkTraining"></asp:BoundField>
                                        <asp:BoundField DataField="BillAbleDesc" HeaderText="Billable" ItemStyle-Wrap="false" SortExpression="BillAbleDesc"></asp:BoundField>
                                        <asp:BoundField DataField="ValueStatus" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="ValueStatus"></asp:BoundField>
                                        <asp:BoundField DataField="Trainers" HeaderText="Trainers" ItemStyle-Wrap="false" SortExpression="ValueStatus"></asp:BoundField>
                                        <asp:BoundField DataField="MarketingName" HeaderText="Marketing" ItemStyle-Wrap="false" SortExpression="ValueStatus"></asp:BoundField>
                                        
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
                        <asp:Button ID="CmdExport" CssClass="btn btn-primary" runat="server" OnClick="CmdExport_Click" Text="Export" />
                        <asp:Button ID="CmdExportXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportXls_Click" Text="Export" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-function">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Function Menu</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframefunc" src="view_job_training_function.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
        <div class="modal fade bs-example-modal-lg" id="modal-details">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Job Order Customer Training Details</h4>
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
                                            <label>Training ID</label>
                                            <p id="LblTrainingID" runat="server" class="form-control-static"></p>
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
                                            <label>Billable</label>
                                            <p id="LblBillable" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Remark</label>
                                            <textarea id="txtRemark" runat="server" class="form-control" style="background-color: white;" rows="1" disabled></textarea>
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
                                        <h3 class="box-title">Training Info</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Training Date</label>
                                            <p id="LblTrainingDate" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Trainers</label>
                                            <textarea id="txtTrainers" runat="server" class="form-control" style="background-color: white;" rows="1" disabled></textarea>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Attendances</label>
                                            <textarea id="txtAttendances" runat="server" class="form-control" style="background-color: white;" rows="1" disabled></textarea>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Training Remark</label>
                                            <textarea id="txtTrainingRemark" runat="server" class="form-control" style="background-color: white;" rows="1" disabled></textarea>
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

        function postFunc(sTrainID) {
            $('#modal-function').modal('show');
            var objfr = document.getElementById('iframefunc').contentWindow;
            var objTrainID = objfr.document.getElementById('txtTrainID');
            var cmdSearch = objfr.document.getElementById('CmdSearch');
            objTrainID.value = sTrainID;
            cmdSearch.click();
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
        function postDetails(sTrainingID, sReqDate, sCustomerName, sSchDate, sBillAble, sValueStatus, objDetails, objPic, objFunc, sCustID, sRemark, sBillAbleID,
                             sUsrUpd, sDtmUpd, sTrainID, sTrainingDate, sTrainers, sAttendances, sRemarkTraining, sUsrUpdTraining, sDtmUpdTraining, sPictureFile) {
            if (sTrainingID != '') {
                console.log(sRemark);
                console.log(sTrainers);
                console.log(sAttendances);
                console.log(sRemarkTraining);


                document.getElementById('ContentPlaceHolder1_LblTrainingID').innerText = CheckNbsp(sTrainingID);
                document.getElementById('ContentPlaceHolder1_LblCustName').innerText = CheckNbsp(sCustomerName);
                document.getElementById('ContentPlaceHolder1_LblReqDate').innerText = CheckNbsp(sReqDate);
                document.getElementById('ContentPlaceHolder1_LblSchDate').innerText = CheckNbsp(sSchDate);
                document.getElementById('ContentPlaceHolder1_LblBillable').innerText = CheckNbsp(sBillAble);
                document.getElementById('ContentPlaceHolder1_txtRemark').innerText = CheckNbsp(sRemark);
                document.getElementById('ContentPlaceHolder1_LblStatus').innerText = CheckNbsp(sValueStatus);
                document.getElementById('ContentPlaceHolder1_LblUsrUpdOrder').innerText = CheckNbsp(sUsrUpd);
                document.getElementById('ContentPlaceHolder1_LblDtmUpdOrder').innerText = CheckNbsp(sDtmUpd);
                document.getElementById('ContentPlaceHolder1_LblTrainingDate').textContent = CheckNbsp(sTrainingDate);
                document.getElementById('ContentPlaceHolder1_txtTrainers').textContent = CheckNbsp(sTrainers);
                document.getElementById('ContentPlaceHolder1_txtAttendances').textContent = CheckNbsp(sAttendances);
                document.getElementById('ContentPlaceHolder1_txtTrainingRemark').textContent = CheckNbsp(sRemarkTraining);
                document.getElementById('ContentPlaceHolder1_LblUsrUpd').textContent = CheckNbsp(sUsrUpdTraining);
                document.getElementById('ContentPlaceHolder1_LblDtmUpd').textContent = CheckNbsp(sDtmUpdTraining);

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
