<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cs_rating_scale.aspx.cs" Inherits="vtsadm.cs_rating_scale" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>CS - Rating Scale
            <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Customer Satisfaction</a></li>
            <li class="active">Rating Scale</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-5">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Rating Scale Form</h3>
                    </div>
                    <div class="box-body">
                        <asp:HiddenField ID="hfId" runat="server" />

                        <div class="form-group form-group-sm">
                            <label>Scale Name</label>
                            <asp:TextBox ID="txtScaleName" runat="server" CssClass="form-control" />
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Min Value</label>
                            <asp:TextBox ID="txtMin" runat="server" CssClass="form-control" />
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Max Value</label>
                            <asp:TextBox ID="txtMax" runat="server" CssClass="form-control" />
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Active</label><br />
                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" UseSubmitBehavior="false" />
                    </div>
                </div>
            </div>

            <div class="col-md-7">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Rating Scale</h3>
                    </div>
                    <div class="box-body">
                        <asp:Label ID="lblMessage" runat="server" />
                        <br />
                        <br />

                        <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped" OnRowCommand="gvData_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="rating_scale_id" HeaderText="ID" Visible="false" />
                                <asp:BoundField DataField="scale_name" HeaderText="Scale Name" />
                                <asp:BoundField DataField="min_value" HeaderText="Min" />
                                <asp:BoundField DataField="max_value" HeaderText="Max" />
                                <asp:BoundField DataField="is_active" HeaderText="Active" />
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server"
                                            CssClass="btn btn-warning btn-xs"
                                            CommandName="EDIT_DATA"
                                            CausesValidation="false"
                                            CommandArgument='<%# Eval("rating_scale_id") %>'>
                                            Edit
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server"
                                            CssClass="btn btn-danger btn-xs"
                                            CommandName="DELETE_DATA"
                                            CausesValidation="false"
                                            CommandArgument='<%# Eval("rating_scale_id") %>'
                                            OnClientClick="return confirmDelete();">
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
