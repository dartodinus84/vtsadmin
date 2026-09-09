<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cs_survey_question_company_search.aspx.cs" Inherits="vtsadm.cs_survey_question_company_search" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search Company</title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <link rel="stylesheet" href="Content/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="Content/bower_components/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="Content/dist/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="Content/paginationcs.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-group form-group-sm">
            <div class="input-group input-group-sm">
                <input type="text" id="txtSearchCompany" runat="server" class="form-control" placeholder="Search by Company ID or Name..." />
                <span class="input-group-btn">
                    <button id="CmdSearchCust" runat="server" type="button" class="btn btn-primary btn-xs" onserverclick="CmdSearchCust_Click">
                        <i class="fa fa-search"></i>
                    </button>
                </span>
            </div>
        </div>
        <div class="form-group form-group-sm">
            <asp:Panel runat="server" ScrollBars="Auto" Width="100%">
                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" Font-Size="Small"
                    CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False"
                    EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px"
                    AllowPaging="True" PageSize="5"
                    OnRowDataBound="GridView1_RowDataBound"
                    OnPageIndexChanging="GridView1_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="company_id" HeaderText="Company ID" ItemStyle-Wrap="false" Visible="false"></asp:BoundField>
                        <asp:BoundField DataField="company_nm" HeaderText="Company Name" ItemStyle-Wrap="false"></asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="CmdSelect" runat="server" ToolTip="Select" Text="<i class='fa fa-share'></i>" CssClass="btn btn-success btn-xs" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle ForeColor="#003481" BackColor="White" />
                    <PagerStyle ForeColor="#003481" CssClass="pagination-ys" HorizontalAlign="Left" />
                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                    <AlternatingRowStyle BackColor="#f9f9f9" />
                </asp:GridView>
                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                    <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
