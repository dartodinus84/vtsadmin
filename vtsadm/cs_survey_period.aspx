<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cs_survey_period.aspx.cs" Inherits="vtsadm.cs_survey_period" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>CS - Survey Period <small>Input</small></h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Customer Satisfaction</a></li>
            <li class="active">Survey Period</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-5">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Survey Period Form</h3>
                    </div>
                    <div class="box-body">
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                        <asp:HiddenField ID="hfId" runat="server" />

                        <div class="form-group form-group-sm">
                            <label>Period Code <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtPeriodCode" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Period Name <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtPeriodName" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Start Date</label>
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>End Date</label>
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Send survey to <span class="text-danger">*</span></label>
                            <div class="box box-solid" style="border: 1px solid #d2d6de; margin-bottom: 8px; box-shadow: none;">
                                <div class="box-body" style="padding: 10px 12px;">
                                    <asp:RadioButtonList ID="rblAudience" runat="server" CssClass="survey-audience-list" RepeatDirection="Vertical" RepeatLayout="Flow">
                                        <asp:ListItem Text="All companies (Global) — every TMS company receives this survey" Value="GLOBAL" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Specific companies only — pick who receives this survey" Value="COMPANY"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <p id="audienceHelp" class="help-block" style="margin: 8px 0 0; font-size: 12px; color: #777;">
                                        Every company that uses TMS will receive this survey.
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="form-group form-group-sm" id="divCompany" runat="server" style="display: none;">
                            <label>
                                Selected companies
                                <span id="selectedCompanyCount" class="label label-primary" style="margin-left: 6px;">0</span>
                            </label>
                            <div style="margin-bottom: 8px;">
                                <button type="button" class="btn btn-primary btn-sm" onclick="showSurveyCompanyModal(); return false;">
                                    <i class="fa fa-plus"></i> Add company
                                </button>
                                <button type="button" class="btn btn-default btn-sm" onclick="clearSurveyCompanies(); return false;">
                                    <i class="fa fa-trash-o"></i> Clear
                                </button>
                            </div>
                            <asp:HiddenField ID="hfSelectedCompanies" runat="server" />
                            <div id="selectedCompaniesList" class="well well-sm" style="min-height: 48px; max-height: 160px; overflow-y: auto; margin-bottom: 4px; background: #fafafa;"></div>
                            <small class="text-muted">Search and pick companies. Click × on a tag to remove one.</small>
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
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" Text="Clear" OnClick="btnCancel_Click" CausesValidation="false" />
                        <span class="text-muted" style="margin-left:8px;font-size:12px;">Save audience first. Use Deploy on the list to show this period in TMS.</span>
                    </div>
                </div>
            </div>

            <div class="col-md-7">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Survey Period</h3>
                    </div>
                    <div class="box-body">
                        <asp:GridView
                            ID="gvData"
                            runat="server"
                            CssClass="table table-bordered table-striped"
                            AutoGenerateColumns="False"
                            EmptyDataText="No survey period data."
                            OnRowCommand="gvData_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="period_code" HeaderText="Code" />
                                <asp:BoundField DataField="period_name" HeaderText="Name" />
                                <asp:BoundField DataField="target_label" HeaderText="Send To" />
                                <asp:BoundField DataField="start_date" HeaderText="Start Date" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="end_date" HeaderText="End Date" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="is_active" HeaderText="Active" />
                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "is_deployed_flag") ?? false) ? "Deployed" : "Draft" %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="220px" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:LinkButton
                                            ID="btnEdit"
                                            runat="server"
                                            CssClass="btn btn-warning btn-xs"
                                            CommandName="EDIT_DATA"
                                            CommandArgument='<%# Eval("survey_period_id") %>'
                                            CausesValidation="false">
                                            Edit
                                        </asp:LinkButton>
                                        <asp:LinkButton
                                            runat="server"
                                            CssClass="btn btn-success btn-xs"
                                            CommandName="DEPLOY_DATA"
                                            CommandArgument='<%# Eval("survey_period_id") %>'
                                            OnClientClick="return confirmDeploy();"
                                            CausesValidation="false"
                                            Visible='<%# !Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "is_deployed_flag") ?? false) %>'
                                            Text="Deploy" />
                                        <asp:LinkButton
                                            runat="server"
                                            CssClass="btn btn-default btn-xs"
                                            CommandName="UNDEPLOY_DATA"
                                            CommandArgument='<%# Eval("survey_period_id") %>'
                                            OnClientClick="return confirmUndeploy();"
                                            CausesValidation="false"
                                            Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "is_deployed_flag") ?? false) %>'
                                            Text="Undeploy" />
                                        <asp:LinkButton
                                            ID="btnDelete"
                                            runat="server"
                                            CssClass="btn btn-danger btn-xs"
                                            CommandName="DELETE_DATA"
                                            CommandArgument='<%# Eval("survey_period_id") %>'
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

    <style type="text/css">
        .survey-audience-list label {
            display: block;
            font-weight: normal;
            margin: 0 0 8px 0;
            padding: 8px 10px;
            border: 1px solid #e6e6e6;
            border-radius: 4px;
            background: #fff;
            cursor: pointer;
        }
        .survey-audience-list label:hover {
            background: #f7f7f7;
            border-color: #c8c8c8;
        }
        .survey-audience-list input[type="radio"] {
            margin-right: 8px;
            vertical-align: middle;
        }
        .survey-company-chip {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            margin: 3px 4px 3px 0;
            padding: 6px 10px;
            border-radius: 16px;
            background: #3c8dbc;
            color: #fff;
            font-size: 12px;
            line-height: 1.2;
        }
        .survey-company-chip a {
            color: #fff;
            font-weight: bold;
            text-decoration: none;
            opacity: 0.9;
        }
        .survey-company-chip a:hover {
            opacity: 1;
        }
    </style>

    <script type="text/javascript">
        function confirmDelete() {
            return confirm("Yakin ingin hapus data ini?");
        }

        function confirmDeploy() {
            return confirm("Deploy periode ini ke TMS? Survei dikirim ke audience yang dipilih.");
        }

        function confirmUndeploy() {
            return confirm("Undeploy periode ini dari TMS? Survei periode ini tidak akan ditampilkan.");
        }

        function getAudienceValue() {
            var checked = document.querySelector('input[name$="rblAudience"]:checked');
            return checked ? checked.value : 'GLOBAL';
        }

        function updateAudienceUi() {
            var mode = getAudienceValue();
            var companyBox = document.getElementById('<%= divCompany.ClientID %>');
            var help = document.getElementById('audienceHelp');
            var showCompanies = mode === 'COMPANY';

            if (companyBox) {
                companyBox.style.display = showCompanies ? 'block' : 'none';
            }

            if (help) {
                if (mode === 'COMPANY') {
                    help.textContent = 'Only the companies you pick below will receive this survey after Deploy.';
                } else {
                    help.textContent = 'Every company that uses TMS will receive this survey after Deploy.';
                }
            }

            renderSelectedCompanies();
        }

        function getSelectedCompaniesField() {
            return document.getElementById('<%= hfSelectedCompanies.ClientID %>');
        }

        function parseSelectedCompanies() {
            var field = getSelectedCompaniesField();
            if (!field) return [];
            var raw = (field.value || '').trim();
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

        function updateSelectedCount(count) {
            var badge = document.getElementById('selectedCompanyCount');
            if (badge) badge.textContent = String(count || 0);
        }

        function renderSelectedCompanies() {
            var items = parseSelectedCompanies();
            var box = document.getElementById('selectedCompaniesList');
            updateSelectedCount(items.length);
            if (!box) return;

            if (items.length === 0) {
                box.innerHTML = '<span class="text-muted">No company selected yet. Click Add company.</span>';
                return;
            }

            box.innerHTML = items.map(function (x) {
                var safeId = String(x.id).replace(/'/g, "\\'");
                var name = String(x.name || '');
                var safeName = name.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                return '<span class="survey-company-chip">'
                    + '<strong>' + String(x.id).replace(/</g, '&lt;') + '</strong> ' + safeName
                    + ' <a href="#" title="Remove" onclick="removeSurveyCompany(\'' + safeId + '\');return false;">&times;</a></span>';
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
            $('#modal-survey-company').modal('hide');
        }

        function removeSurveyCompany(companyId) {
            var items = parseSelectedCompanies().filter(function (x) { return String(x.id) !== String(companyId); });
            getSelectedCompaniesField().value = serializeSelectedCompanies(items);
            renderSelectedCompanies();
        }

        function clearSurveyCompanies() {
            getSelectedCompaniesField().value = '';
            renderSelectedCompanies();
        }

        function showSurveyCompanyModal() {
            $('#modal-survey-company').modal('show');
        }

        $(document).ready(function () {
            $('input[name$="rblAudience"]').on('change', updateAudienceUi);
            updateAudienceUi();
        });
    </script>
</asp:Content>
