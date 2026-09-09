<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cs_question_option.aspx.cs" Inherits="vtsadm.cs_question_option" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>CS - Question Option <small>Input</small></h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Customer Satisfaction</a></li>
            <li class="active">Question Option</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-5">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Question Option Form</h3>
                    </div>
                    <div class="box-body">
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                        <asp:HiddenField ID="hfId" runat="server" />
                        <asp:HiddenField ID="hfSurveyQuestionId" runat="server" />

                        <div class="form-group form-group-sm">
                            <label>Survey Question</label>
                            <asp:DropDownList ID="ddlSurveyQuestion" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSurveyQuestion_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Option Value <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Option Label <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtLabel" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Sort Order <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtSort" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
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
                        <h3 class="box-title">List Question Option</h3>
                    </div>
                    <div class="box-body">
                        <asp:GridView ID="gvData" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="false" EmptyDataText="No option data." OnRowCommand="gvData_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="option_value" HeaderText="Value" />
                                <asp:BoundField DataField="option_label" HeaderText="Label" />
                                <asp:BoundField DataField="sort_order" HeaderText="Sort" />
                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server"
                                            CssClass="btn btn-warning btn-xs"
                                            CommandName="EDIT_DATA"
                                            CommandArgument='<%# Eval("question_option_id") %>'
                                            CausesValidation="false">
                                            Edit
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server"
                                            CssClass="btn btn-danger btn-xs"
                                            CommandName="DELETE_DATA"
                                            CommandArgument='<%# Eval("question_option_id") %>'
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

    <script type="text/javascript">
        function confirmDelete() {
            return confirm("Yakin hapus data ini?");
        }
    </script>
</asp:Content>
