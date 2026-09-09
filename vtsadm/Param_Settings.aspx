<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Param_Settings.aspx.cs" Inherits="vtsadm.Param_Settings" EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Param Setting</h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Setting</a></li>
            <li class="active">Param Setting</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-xl-6 col-lg-6 col-md-12 col-sm-12 col-xs-12">
                <!-- general form elements -->
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Param</h3>
                    </div>
                    <div class="box-body">
                        <div style="display: flex; flex-direction: row; flex-wrap: wrap; align-items: center;">
                            <div class="form-group form-group-sm col-xl-4 col-lg-4 col-md-12 col-sm-12 col-xs-12">
                                <label for="txtSearch">Company Name</label>
                                <asp:TextBox ID="txt_company_name" runat="server" class="form-control pull-right" placeholder="Company Name ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm col-xl-4 col-lg-4 col-md-12 col-sm-12 col-xs-12">
                                <label for="txtSearch2">Param Type</label>
                                <asp:TextBox ID="txt_param_type" runat="server" class="form-control pull-right" placeholder="Param Type ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm col-xl-4 col-lg-4 col-md-12 col-sm-12 col-xs-12">
                                <label for="CmdSearch" style="display: block;">&nbsp;</label>
                                <button id="CmdSearch" runat="server" type="button" class="btn btn-primary btn-sm" onclick="showOverlay();" onserverclick="CmdSearch_Click"><i class="fa fa-search"></i></button>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <div class="form-group form-group-sm" id="div_comment" runat="server"></div>
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="autoid" HeaderText="Auto ID" ItemStyle-Wrap="false" SortExpression="AutoID"></asp:BoundField>
                                        <asp:BoundField DataField="company_id" HeaderText="Company ID" ItemStyle-Wrap="false" SortExpression="CompanyID"></asp:BoundField>
                                        <asp:BoundField DataField="company_nm" HeaderText="Company Name" ItemStyle-Wrap="false" SortExpression="CompanyName"></asp:BoundField>
                                        <asp:BoundField DataField="par_type" HeaderText="Param Type" ItemStyle-Wrap="false" SortExpression="ParamType"></asp:BoundField>
                                        <asp:BoundField DataField="par_code" HeaderText="Param Code" ItemStyle-Wrap="false" SortExpression="ParamCode"></asp:BoundField>
                                        <asp:BoundField DataField="par_value" HeaderText="Param Value" ItemStyle-Wrap="false" SortExpression="ParamValue"></asp:BoundField>
                                        <asp:BoundField DataField="notes" HeaderText="Note" ItemStyle-Wrap="false" SortExpression="Note"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdEdit" runat="server" Text="Edit" ToolTip="Edit" Enabled="true" CssClass="btn btn-warning btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-remove'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPagingParam" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xl-6 col-lg-6 col-md-12 col-sm-12 col-xs-12">
                <!-- general form elements -->
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <input type="hidden" runat="server" id="txtCompanyID" />
                        <h3 class="box-title">Form New Param</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Company ID</label>
                            <div class="input-group input-group-sm">
                                <input type="hidden" id="txtAutoId" runat="server" readonly="readonly"/>
                                <input type="text" id="txtCompID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" />
                                <span class="input-group-btn">
                                    <button id="BtnSearchCustomer" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-customer"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Company Name</label>
                            <asp:TextBox ID="txtCompanyName" runat="server" class="form-control" placeholder="Company Name ..." disabled="disabled"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Param Type</label>
                            <asp:TextBox ID="txtParamType" runat="server" class="form-control" placeholder="Param Type ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Param Code</label>
                            <asp:TextBox ID="txtParamCode" runat="server" class="form-control" placeholder="Param Code ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Param Value</label>
                            <asp:TextBox ID="txtParamValue" runat="server" class="form-control" placeholder="Param Value ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Param Note</label>
                            <asp:TextBox ID="txtParamNote" runat="server" class="form-control" placeholder="Param Note ..."></asp:TextBox>
                        </div>
                        <div id="comment_save" runat="server" class="form-group form-group-sm"></div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdEdit" type="button" class="btn btn-warning" runat="server" onclick="CmdEdit_Click()">Update</button>
                        <button id="CmdSave" type="button" class="btn btn-primary" runat="server" onserverclick="CmdSave_Click">Save</button>
                        <asp:Button ID="CmdClear" CssClass="btn btn-secondary" runat="server" Text="Clear" OnClick="CmdClear_Click"/>
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
                        <h6 class="modal-title">Are you sure to delete param ID :&nbsp;</h6>
                        <label id="LblParamID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtParamIDDelete" runat="server" />
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
                        <h6 class="modal-title">Are you sure to update param ID :&nbsp;</h6>
                        <label id="LblParamIDUpdate" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="Hidden1" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-update').modal('hide');" onserverclick="CmdYesUpdate_Click" id="Button1">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-update').modal('hide');">No</button>
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
                            <iframe src="param_settings_customer_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
        function postCustChild(sCustID, sFullName) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtCompID').value = sCustID;
                document.getElementById('ContentPlaceHolder1_txtCompanyName').value = sFullName;
                $('#ContentPlaceHolder1_txtParamType').removeAttr('disabled')
                $('#ContentPlaceHolder1_txtParamCode').removeAttr('disabled')
                $('#ContentPlaceHolder1_txtParamValue').removeAttr('disabled')
                $('#ContentPlaceHolder1_CmdSave').removeAttr('disabled')

                $('#modal-customer').modal('hide');
            }
        }
        function postDelete(sAutoid) {
            if (sAutoid != '') {
                document.getElementById('ContentPlaceHolder1_LblParamID').innerHTML = sAutoid;
                document.getElementById('ContentPlaceHolder1_txtParamIDDelete').value = sAutoid;
                $('#modal-delete').modal('show');
            }
        }

        function postEdit(autoid, company_id, company_name, param_type, param_code, param_value, param_note) {
            if (autoid !== 0) {
                document.getElementById('ContentPlaceHolder1_txtAutoId').value = autoid;
                document.getElementById('ContentPlaceHolder1_txtCompID').value = company_id;
                document.getElementById('ContentPlaceHolder1_txtCompanyName').value = company_name;
                document.getElementById('ContentPlaceHolder1_txtParamType').value = param_type;
                document.getElementById('ContentPlaceHolder1_txtParamCode').value = param_code;
                document.getElementById('ContentPlaceHolder1_txtParamValue').value = param_value;
                document.getElementById('ContentPlaceHolder1_txtParamNote').value = param_note;
                //$('#ContentPlaceHolder1_txtParamType').removeAttr('disabled')
                //$('#ContentPlaceHolder1_txtParamCode').removeAttr('disabled')
                //$('#ContentPlaceHolder1_txtParamValue').removeAttr('disabled')
                $('#ContentPlaceHolder1_CmdSave').removeAttr('disabled')
                document.getElementById('ContentPlaceHolder1_CmdEdit').style.display = 'inline-block';
                document.getElementById('ContentPlaceHolder1_CmdSave').style.display = 'none';

            }
        }
        function endRequest(sender, args) {
            $('#modal-messagebox').on('hidden.bs.modal', function () {
                document.body.style.paddingRight = '0px';
            });
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
            }
        }

        function selectCustomer(company_id, company_nm) {
            $('#ContentPlaceHolder1_InpCustomerName').val(company_nm)
            $('#ContentPlaceHolder1_InpCompanyId').val(company_id)
        }

        function showModalCustomer() {
            $('#modal-customer').modal({ show: true });
        }

        function cmdsave_click() {
            $('#ContentPlaceHolder1_txtParamType').removeAttr('disabled')
            $('#ContentPlaceHolder1_txtParamCode').removeAttr('disabled')
            $('#ContentPlaceHolder1_txtParamValue').removeAttr('disabled')
            $('#ContentPlaceHolder1_CmdSave').removeAttr('disabled')
        }

        function CmdEdit_Click() {
            $('#modal-update').modal('show')
            document.getElementById('ContentPlaceHolder1_LblParamIDUpdate').innerHTML = document.getElementById('ContentPlaceHolder1_txtAutoId').value
        }

        $(document).ready(() => {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(endRequest);
            endRequest();
            document.getElementById('ContentPlaceHolder1_CmdEdit').style.display = 'none';
        })
    </script>
</asp:Content>
