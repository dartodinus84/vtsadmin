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
                            <label>Period <span class="text-danger">*</span></label>
                            <div class="input-group">
                                <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="form-control"></asp:DropDownList>
                                <span class="input-group-btn">
                                    <asp:Button ID="btnLoad" runat="server" CssClass="btn btn-default" Text="Load"
                                        CausesValidation="false" OnClick="btnLoad_Click" />
                                </span>
                            </div>
                            <small class="text-muted">Select a period, then Load to show that period’s questions.</small>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Category <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Question Type <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlQuestionType" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm" id="wrapRatingScale">
                            <label>Rating Scale</label>
                            <asp:DropDownList ID="ddlRatingScale" runat="server" CssClass="form-control"></asp:DropDownList>
                            <small class="text-muted">Only for Rating type.</small>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Question <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtQuestion" runat="server" CssClass="form-control" MaxLength="500"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Sort <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtSort" runat="server" CssClass="form-control" MaxLength="5"></asp:TextBox>
                        </div>
                        <div id="wrapNegativeRules" class="form-group form-group-sm">
                            <label>Negative answers (need reason)</label>
                            <asp:CheckBox ID="chkReason" runat="server" style="display:none;" />
                            <asp:TextBox ID="txtNegativeValue" runat="server" CssClass="form-control" style="display:none;" MaxLength="50"></asp:TextBox>
                            <div id="negativeValuePicker" class="well well-sm" style="margin-bottom: 4px; background: #fafafa;"></div>
                            <small id="negativeValueHelp" class="text-muted">Selected answers will ask for a written reason. Leave none selected if no reason is needed.</small>
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
                        <span class="text-muted" style="margin-left:8px;font-size:12px;">Send to Global/company is set on Survey Period, then Deploy.</span>
                    </div>
                </div>
            </div>

            <div class="col-md-7">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Survey Question</h3>
                    </div>
                    <div class="box-body">
                        <asp:Panel ID="pnlList" runat="server" Visible="false">
                            <p class="text-muted" style="margin-top:0;">
                                Period:
                                <asp:Label ID="lblLoadedPeriod" runat="server" CssClass="text-bold"></asp:Label>
                            </p>
                            <asp:GridView ID="gvData" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="false" EmptyDataText="No questions for this period yet." OnRowCommand="gvData_RowCommand">
                                <Columns>
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
                        </asp:Panel>
                        <asp:Panel ID="pnlListEmpty" runat="server" Visible="true">
                            <p class="text-muted" style="margin:12px 0;">Select <strong>Period</strong> on the form, then click <strong>Load</strong> to show questions for that period.</p>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        function confirmDelete() {
            return confirm("Yakin hapus data ini?");
        }

        function getQuestionTypeCode() {
            var ddl = document.getElementById('<%= ddlQuestionType.ClientID %>');
            return ddl ? String(ddl.value || '').toUpperCase() : '';
        }

        function getNegativeField() {
            return document.getElementById('<%= txtNegativeValue.ClientID %>');
        }

        function parseNegativeSet(raw) {
            return String(raw || '')
                .split(',')
                .map(function (x) { return String(x || '').trim(); })
                .filter(function (x) { return x.length > 0; });
        }

        function setNegativeValue(values) {
            var field = getNegativeField();
            var unique = [];
            (values || []).forEach(function (v) {
                var s = String(v).trim();
                if (s && unique.indexOf(s) < 0) unique.push(s);
            });
            if (field) field.value = unique.join(',');
        }

        function getRatingScaleRange() {
            var ddl = document.getElementById('<%= ddlRatingScale.ClientID %>');
            if (!ddl || !ddl.value) {
                return null;
            }

            var meta = window.__csRatingScaleMeta || {};
            var info = meta[String(ddl.value)];
            if (info && !isNaN(parseInt(info.min, 10)) && !isNaN(parseInt(info.max, 10))) {
                var metaMin = parseInt(info.min, 10);
                var metaMax = parseInt(info.max, 10);
                if (metaMin > metaMax) {
                    var swapMeta = metaMin; metaMin = metaMax; metaMax = swapMeta;
                }
                return { min: metaMin, max: metaMax };
            }

            if (ddl.selectedIndex < 0 || !ddl.options.length) {
                return null;
            }
            var opt = ddl.options[ddl.selectedIndex];
            var minAttr = opt.getAttribute('data-min');
            var maxAttr = opt.getAttribute('data-max');
            if (minAttr == null || maxAttr == null || minAttr === '' || maxAttr === '') {
                return null;
            }
            var min = parseInt(minAttr, 10);
            var max = parseInt(maxAttr, 10);
            if (isNaN(min) || isNaN(max)) {
                return null;
            }
            if (min > max) {
                var t = min; min = max; max = t;
            }
            return { min: min, max: max };
        }

        function defaultNegativeForRating(min, max) {
            // Small scales (e.g. 1-3): default only the lowest score — user can tick more.
            // Larger scales (e.g. 1-5): low half including midpoint → 1,2,3.
            if ((max - min) <= 2) {
                return [String(min)];
            }
            var mid = Math.floor((min + max) / 2);
            var list = [];
            for (var v = min; v <= mid; v++) list.push(String(v));
            return list;
        }

        function filterNegativesToRange(values, min, max) {
            return (values || []).filter(function (x) {
                var n = parseInt(x, 10);
                return !isNaN(n) && n >= min && n <= max;
            });
        }

        function renderNegativePicker(preserveExisting) {
            var type = getQuestionTypeCode();
            var picker = document.getElementById('negativeValuePicker');
            var help = document.getElementById('negativeValueHelp');
            var field = getNegativeField();
            if (!picker || !field) return;

            var existing = parseNegativeSet(field.value);
            picker.innerHTML = '';

            if (type === 'TEXT' || type === 'TEXT2' || !type) {
                setNegativeValue([]);
                return;
            }

            if (type === 'YES_NO') {
                var yesNoChecked = preserveExisting
                    ? existing.some(function (x) { return String(x).toUpperCase() === 'NO'; })
                    : true;

                setNegativeValue(yesNoChecked ? ['NO'] : []);
                picker.innerHTML = '<label style="font-weight:normal;margin:0;">'
                    + '<input type="checkbox" id="jsNegYesNo" value="NO"' + (yesNoChecked ? ' checked' : '') + ' /> '
                    + 'No (Tidak) — ask for a written reason'
                    + '</label>';
                if (help) help.textContent = 'For Yes/No, only No is treated as negative.';

                $('#jsNegYesNo').off('change').on('change', function () {
                    setNegativeValue(this.checked ? ['NO'] : []);
                });
                return;
            }

            if (type === 'RATING') {
                var range = getRatingScaleRange();
                if (!range) {
                    setNegativeValue([]);
                    picker.innerHTML = '<span class="text-muted">Select a rating scale first. Negative options follow that scale min–max.</span>';
                    if (help) help.textContent = 'Negative answers follow the selected rating scale only.';
                    return;
                }

                var selected;
                if (preserveExisting) {
                    selected = filterNegativesToRange(existing, range.min, range.max);
                } else {
                    selected = defaultNegativeForRating(range.min, range.max);
                }
                setNegativeValue(selected);

                var html = '';
                for (var v = range.min; v <= range.max; v++) {
                    var checked = selected.indexOf(String(v)) >= 0 ? ' checked' : '';
                    html += '<label style="font-weight:normal;margin-right:12px;display:inline-block;">'
                        + '<input type="checkbox" class="js-neg-rating" value="' + v + '"' + checked + ' /> '
                        + v
                        + '</label>';
                }
                picker.innerHTML = html;
                if (help) {
                    help.textContent = 'Scale ' + range.min + '–' + range.max
                        + ' only. Tick any that need a reason (one or more). Default: '
                        + defaultNegativeForRating(range.min, range.max).join(', ')
                        + '.';
                }

                $(picker).find('.js-neg-rating').off('change').on('change', function () {
                    var vals = [];
                    $(picker).find('.js-neg-rating:checked').each(function () {
                        vals.push(String(this.value));
                    });
                    setNegativeValue(vals);
                });
                return;
            }

            // CHOICE / other — free text still available
            field.style.display = 'block';
            picker.innerHTML = '<span class="text-muted">Enter option values that are negative, comma-separated (e.g. Hard or 1).</span>';
            if (help) help.textContent = 'Use the option_value from question options.';
        }

        function syncQuestionTypeUi(preserveNegative) {
            var type = getQuestionTypeCode();
            var isText = type === 'TEXT' || type === 'TEXT2';
            var isRating = type === 'RATING';
            var showNegative = !isText && type.length > 0;

            var wrapNeg = document.getElementById('wrapNegativeRules');
            var wrapScale = document.getElementById('wrapRatingScale');
            var field = getNegativeField();
            if (wrapNeg) wrapNeg.style.display = showNegative ? 'block' : 'none';
            if (wrapScale) wrapScale.style.display = isRating ? 'block' : 'none';
            if (field) field.style.display = (type === 'CHOICE' || type === 'CHOICE_MULTI') ? 'block' : 'none';

            if (isText) {
                setNegativeValue([]);
            }

            if (!isRating) {
                var scale = document.getElementById('<%= ddlRatingScale.ClientID %>');
                if (scale && scale.options.length > 0 && !preserveNegative) scale.selectedIndex = 0;
            }

            renderNegativePicker(!!preserveNegative);
        }

        $(document).ready(function () {
            $('#<%= ddlQuestionType.ClientID %>').on('change', function () {
                syncQuestionTypeUi(false);
            });
            $('#<%= ddlRatingScale.ClientID %>').on('change', function () {
                renderNegativePicker(false);
            });
            syncQuestionTypeUi(true);
        });
    </script>
</asp:Content>
