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
                            <label>
                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                Active
                            </label>
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
                                <asp:BoundField DataField="start_date" HeaderText="Start Date" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="end_date" HeaderText="End Date" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="is_active" HeaderText="Active" />
                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
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

    <script type="text/javascript">
        function confirmDelete() {
            return confirm("Yakin ingin hapus data ini?");
        }
    </script>
</asp:Content>
