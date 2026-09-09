<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cs_survey_category.aspx.cs" Inherits="vtsadm.cs_survey_category" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>CS - Survey Category <small>Input</small></h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Customer Satisfaction</a></li>
            <li class="active">Survey Category</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Survey Category Information</h3>
                    </div>
                    <div class="box-body">
                        <asp:HiddenField ID="hfId" runat="server" />
                        <div id="div_comment" class="form-group form-group-sm" runat="server"></div>
                        <div class="form-group form-group-sm">
                            <label>Category Code</label>
                            <asp:TextBox ID="txtCategoryCode" runat="server" CssClass="form-control" placeholder="Code ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Category Name</label>
                            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" placeholder="Name ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Description</label>
                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Description ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Active</label><br />
                            <asp:CheckBox ID="chkIsActive" runat="server" />
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="btnCancel" CssClass="btn btn-default" runat="server" Text="Clear" OnClick="btnCancel_Click" CausesValidation="false" />
                        <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" UseSubmitBehavior="false" />
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Survey Category</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control pull-right" placeholder="Search by name ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="btnSearch" runat="server" type="submit" class="btn btn-primary" onserverclick="btnSearch_ServerClick"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <asp:GridView ID="gvData" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" EmptyDataText="No items to display" DataKeyNames="survey_category_id" OnRowCommand="gvData_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="survey_category_id" HeaderText="ID" />
                                <asp:BoundField DataField="category_code" HeaderText="Code" />
                                <asp:BoundField DataField="category_name" HeaderText="Name" />
                                <asp:BoundField DataField="description" HeaderText="Description" />
                                <asp:BoundField DataField="is_active" HeaderText="Active" />
                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-warning btn-xs" CommandName="EditData" CommandArgument='<%# Eval("survey_category_id") %>' CausesValidation="false">Edit</asp:LinkButton>
                                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-xs" CommandName="DeleteData" CommandArgument='<%# Eval("survey_category_id") %>' OnClientClick="return confirm('Yakin hapus data ini?');" CausesValidation="false">Delete</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Label ID="LblPaging" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <div class="modal fade" id="modalForm" tabindex="-1" role="dialog" aria-hidden="true"></div>

    <script type="text/javascript" id="js-close-modal">
        function closeModalFix() {
            const modalEl = document.getElementById('modalForm');

            if (!modalEl) return;

            const modalInstance = window.bootstrap && bootstrap.Modal ? bootstrap.Modal.getInstance(modalEl) : null;
            if (modalInstance) {
                modalInstance.hide();
            }

            document.body.classList.remove('modal-open');
            document.body.style = '';

            document.querySelectorAll('.modal-backdrop').forEach(function (el) { el.remove(); });
        }

        function showSuccess(message) {
            showMessage('success', message);
        }

        function showError(message) {
            showMessage('danger', message);
        }

        function showMessage(type, message) {
            var container = document.getElementById('ContentPlaceHolder1_div_comment');
            if (!container) return;

            var safeMessage = (message || '').toString();
            container.innerHTML = "<div class='alert alert-" + type + "' role='alert'>" +
                "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
                "<span aria-hidden='true'>&times;</span></button>" + safeMessage + "</div>";
        }
    </script>
</asp:Content>
