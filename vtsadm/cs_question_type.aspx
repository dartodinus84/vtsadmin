<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cs_question_type.aspx.cs" Inherits="vtsadm.cs_question_type" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>CS - Question Type <small>Input</small></h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Customer Satisfaction</a></li>
            <li class="active">Question Type</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-5">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Question Type Form</h3>
                    </div>
                    <div class="box-body">
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>

                        <div class="form-group form-group-sm">
                            <label>Question Type Code <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtQuestionTypeCode" runat="server" CssClass="form-control" MaxLength="30" placeholder="Question Type Code"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Question Type Name <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtQuestionTypeName" runat="server" CssClass="form-control" MaxLength="100" placeholder="Question Type Name"></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" Text="Clear" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>

            <div class="col-md-7">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Question Type</h3>
                    </div>
                    <div class="box-body">
                        <asp:GridView
                            ID="gvData"
                            runat="server"
                            CssClass="table table-bordered table-striped"
                            AutoGenerateColumns="False"
                            EmptyDataText="No question type data."
                            OnRowCommand="gvData_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="question_type_code" HeaderText="Code" />
                                <asp:BoundField DataField="question_type_name" HeaderText="Name" />
                                <asp:TemplateField HeaderText="Active">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblActive"
                                            runat="server"
                                            Text='<%# Convert.ToBoolean(Eval("is_active")) ? "Active" : "Inactive" %>'
                                            CssClass='<%# Convert.ToBoolean(Eval("is_active")) ? "badge bg-success" : "badge bg-danger" %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <div class="btn-group btn-group-xs" role="group" aria-label="Question Type Actions">
                                            <asp:LinkButton
                                                ID="btnEdit"
                                                runat="server"
                                                CssClass="btn btn-warning"
                                                CommandName="EditData"
                                                CommandArgument='<%# Eval("question_type_code") %>'
                                                ToolTip="Edit"
                                                CausesValidation="false">
                                                <i class="fa fa-edit"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton
                                                ID="btnDelete"
                                                runat="server"
                                                CssClass="btn btn-danger"
                                                CommandName="DeleteData"
                                                CommandArgument='<%# Eval("question_type_code") %>'
                                                ToolTip="Delete"
                                                OnClientClick="return confirm('Yakin hapus data ini?');"
                                                CausesValidation="false">
                                                <i class="fa fa-trash"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        function showSuccess(message) {
            var lbl = document.getElementById('<%= lblMessage.ClientID %>');
            if (!lbl) return;
            lbl.style.color = 'Green';
            lbl.innerText = message || '';
        }

        function showError(message) {
            var lbl = document.getElementById('<%= lblMessage.ClientID %>');
            if (!lbl) return;
            lbl.style.color = 'Red';
            lbl.innerText = message || '';
        }
    </script>
</asp:Content>
