<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cs_survey_question.aspx.cs" Inherits="vtsadm.cs_survey_question" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>CS - Survey Question <small>Input</small></h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Customer Satisfaction</a></li>
            <li class="active">Survey Question</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-5">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Survey Question Form</h3>
                    </div>
                    <div class="box-body">
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                        <asp:HiddenField ID="hfId" runat="server" />

                        <div class="form-group form-group-sm">
                            <label>Period</label>
                            <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Target <span class="text-danger">*</span></label>
                            <asp:RadioButtonList ID="rblTarget" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblTarget_SelectedIndexChanged">
                                <asp:ListItem Text="Global (all companies)" Value="GLOBAL" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Specific company" Value="COMPANY"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="form-group form-group-sm" id="divCompany" runat="server">
                            <label>Companies <span class="text-danger">*</span></label>
                            <div class="input-group input-group-sm" style="margin-bottom: 8px;">
                                <button type="button" class="btn btn-primary btn-sm" onclick="showSurveyCompanyModal(); return false;">
                                    <i class="fa fa-search"></i> Select Company
                                </button>
                            </div>
                            <asp:HiddenField ID="hfSelectedCompanies" runat="server" />
                            <div id="selectedCompaniesList" class="well well-sm" style="min-height: 42px; margin-bottom: 0;"></div>
                            <small class="text-muted">Click Select Company, search, then pick. You can add more than one.</small>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Category <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Question Type <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlQuestionType" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Rating Scale</label>
                            <asp:DropDownList ID="ddlRatingScale" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Question <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtQuestion" runat="server" CssClass="form-control" MaxLength="500"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Sort <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtSort" runat="server" CssClass="form-control" MaxLength="5"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>
                                <asp:CheckBox ID="chkReason" runat="server" />
                                Is reason required if negative
                            </label>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Negative Option Value</label>
                            <asp:TextBox ID="txtNegativeValue" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>
                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                Active
                            </label>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" Text="Clear" CausesValidation="false" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>

            <div class="col-md-7">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Survey Question</h3>
                    </div>
                    <div class="box-body">
                        <asp:GridView ID="gvData" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="false" EmptyDataText="No survey question data." OnRowCommand="gvData_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="period_name" HeaderText="Period" />
                                <asp:BoundField DataField="target_label" HeaderText="Target" />
                                <asp:BoundField DataField="category_name" HeaderText="Category" />
                                <asp:BoundField DataField="question_type_name" HeaderText="Type" />
                                <asp:BoundField DataField="question_text" HeaderText="Question" />
                                <asp:BoundField DataField="sort_order" HeaderText="Sort" />
                                <asp:BoundField DataField="is_active" HeaderText="Active" />
                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server"
                                            CssClass="btn btn-warning btn-xs"
                                            CommandName="EDIT_DATA"
                                            CommandArgument='<%# Eval("survey_question_id") %>'
                                            CausesValidation="false">
                                            Edit
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server"
                                            CssClass="btn btn-danger btn-xs"
                                            CommandName="DELETE_DATA"
                                            CommandArgument='<%# Eval("survey_question_id") %>'
                                            OnClientClick="return confirmDelete();"
                                            CausesValidation="false">
                                            Delete
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <div class="modal fade bs-example-modal-lg" id="modal-survey-company" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title">Select Company</h4>
                </div>
                <div class="modal-body">
                    <iframe src="cs_survey_question_company_search.aspx" style="width: 100%; border: none; height: 380px;" scrolling="no"></iframe>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function confirmDelete() {
            return confirm("Yakin hapus data ini?");
        }

        function getSelectedCompaniesField() {
            return document.getElementById('<%= hfSelectedCompanies.ClientID %>');
        }

        function parseSelectedCompanies() {
            var raw = (getSelectedCompaniesField().value || '').trim();
            if (!raw) return [];
            return raw.split('|').map(function (part) {
                var idx = part.indexOf(':');
                if (idx < 0) return null;
                return { id: part.substring(0, idx), name: part.substring(idx + 1) };
            }).filter(function (x) { return x && x.id; });
        }

        function serializeSelectedCompanies(items) {
            return items.map(function (x) { return x.id + ':' + x.name; }).join('|');
        }

        function renderSelectedCompanies() {
            var items = parseSelectedCompanies();
            var box = document.getElementById('selectedCompaniesList');
            if (!box) return;
            if (items.length === 0) {
                box.innerHTML = '<span class="text-muted">No company selected.</span>';
                return;
            }
            box.innerHTML = items.map(function (x) {
                var safeId = String(x.id).replace(/"/g, '&quot;');
                var name = String(x.name || '');
                var safeName = name.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                return '<span class="label label-primary" style="display:inline-block;margin:2px 4px 2px 0;padding:6px 8px;font-size:12px;">'
                    + safeId + ' - ' + safeName
                    + ' <a href="#" style="color:#fff;margin-left:6px;" onclick="removeSurveyCompany(\'' + safeId + '\');return false;">&times;</a></span>';
            }).join('');
        }

        function selectSurveyCompany(companyId, companyName) {
            var items = parseSelectedCompanies();
            var exists = items.some(function (x) { return String(x.id) === String(companyId); });
            if (!exists) {
                items.push({ id: String(companyId), name: companyName || String(companyId) });
                getSelectedCompaniesField().value = serializeSelectedCompanies(items);
                renderSelectedCompanies();
            }
        }

        function removeSurveyCompany(companyId) {
            var items = parseSelectedCompanies().filter(function (x) { return String(x.id) !== String(companyId); });
            getSelectedCompaniesField().value = serializeSelectedCompanies(items);
            renderSelectedCompanies();
        }

        function showSurveyCompanyModal() {
            $('#modal-survey-company').modal('show');
        }

        function clearSurveyCompanies() {
            getSelectedCompaniesField().value = '';
            renderSelectedCompanies();
        }

        $(document).ready(function () {
            renderSelectedCompanies();
        });
    </script>
</asp:Content>
