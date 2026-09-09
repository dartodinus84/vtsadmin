<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="mdvr_company_safety_ai_policy.aspx.cs" Inherits="vtsadm.mdvr_company_safety_ai_policy" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>MDVR Company Safety AI Policy
            <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">MDVR</a></li>
            <li class="active">Safety AI Policy</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-5">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Safety AI Policy Form</h3>
                    </div>
                    <div class="box-body">
                        <asp:HiddenField ID="hfIsEdit" runat="server" Value="0" />
                        <div id="div_comment" class="form-group form-group-sm" runat="server"></div>

                        <div style="position: absolute; left: -9999px; height: 0; overflow: hidden;" aria-hidden="true">
                            <input type="text" tabindex="-1" autocomplete="username" />
                            <input type="password" tabindex="-1" autocomplete="current-password" />
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Company ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtCompID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" autocomplete="off" />
                                <span class="input-group-btn">
                                    <button id="BtnSearchCustomer" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-customer"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Company Name</label>
                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" placeholder="Company Name ..." ReadOnly="true" autocomplete="off"></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Report Interval (seconds)</label>
                            <input type="text" id="txtIntervalSeconds" runat="server" class="form-control"
                                placeholder="Default 30 seconds if not set" maxlength="10"
                                autocomplete="one-time-code" inputmode="numeric"
                                data-lpignore="true" data-1p-ignore="true" data-form-type="other"
                                data-bwignore="true" />
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Active</label><br />
                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="btnCancel" CssClass="btn btn-default" runat="server" Text="Clear" OnClick="btnCancel_Click" CausesValidation="false" />
                        <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" UseSubmitBehavior="false" />
                    </div>
                </div>
            </div>

            <div class="col-md-7">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Safety AI Policy</h3>
                    </div>
                    <div class="box-body">
                        <asp:Panel runat="server" ScrollBars="Auto">
                            <asp:GridView ID="gvData" runat="server" BackColor="WhiteSmoke" Font-Size="Small"
                                CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False"
                                EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px"
                                DataKeyNames="company_id" OnRowCommand="gvData_RowCommand">
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <Columns>
                                    <asp:BoundField DataField="company_id" HeaderText="Company ID" Visible="false" />
                                    <asp:BoundField DataField="company_nm" HeaderText="Company Name" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="safety_ai_report_interval_seconds" HeaderText="Interval (sec)" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="is_active" HeaderText="Active" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="created_at" HeaderText="Created At" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="updated_at" HeaderText="Updated At" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="updated_by" HeaderText="Updated By" ItemStyle-Wrap="false" />
                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server"
                                                CssClass="btn btn-warning btn-xs"
                                                CommandName="EDIT_DATA"
                                                CausesValidation="false"
                                                CommandArgument='<%# Eval("company_id") %>'>
                                                Edit
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server"
                                                CssClass="btn btn-danger btn-xs"
                                                CommandName="DELETE_DATA"
                                                CausesValidation="false"
                                                CommandArgument='<%# Eval("company_id") %>'
                                                OnClientClick="return confirmDelete();">
                                                <i class="fa fa-remove"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle ForeColor="#003481" BackColor="White" />
                                <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-customer">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title">Customer</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="mdvr_company_safety_ai_policy_company_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
        function confirmDelete() {
            return confirm("Yakin hapus data ini?");
        }

        function postCustChild(sCustID, sFullName) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtCompID').value = sCustID;
                document.getElementById('ContentPlaceHolder1_txtCompanyName').value = sFullName;
                $('#modal-customer').modal('hide');
            }
        }

        function clearIntervalAutofill() {
            var hf = document.getElementById('ContentPlaceHolder1_hfIsEdit');
            var el = document.getElementById('ContentPlaceHolder1_txtIntervalSeconds');
            if (!el || !hf || hf.value === '1') {
                return;
            }
            el.value = '';
        }

        $(function () {
            clearIntervalAutofill();
            setTimeout(clearIntervalAutofill, 100);
            setTimeout(clearIntervalAutofill, 500);
            setTimeout(clearIntervalAutofill, 1000);
        });
    </script>
</asp:Content>
