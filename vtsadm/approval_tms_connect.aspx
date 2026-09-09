<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="approval_tms_connect.aspx.cs" Inherits="vtsadm.approval_tms_connect" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Approval TMC Connect</h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Approval TMC Connect</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Approval TMC Connect</h3>
                    </div>
                    <div class="box-body">
                        <div style="display: flex; flex-direction: row; flex-wrap: wrap; align-items: center;">
                            <div class="form-group form-group-sm col-xl-6 col-lg-6 col-md-12 col-sm-12 col-xs-12">
                                <label for="txtSearch">Search</label>
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Company / Legal Name / Brand Name ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm col-xl-6 col-lg-6 col-md-12 col-sm-12 col-xs-12">
                                <label for="CmdSearch" style="display: block;">&nbsp;</label>
                                <button id="CmdSearch" runat="server" type="button" class="btn btn-primary btn-sm" onclick="if(typeof showOverlay==='function'){showOverlay();}" onserverclick="CmdSearch_Click">
                                    <i class="fa fa-search"></i> Search
                                </button>
                                <button id="CmdReset" runat="server" type="button" class="btn btn-default btn-sm" onclick="if(typeof showOverlay==='function'){showOverlay();}" onserverclick="CmdReset_Click">
                                    <i class="fa fa-refresh"></i> Reset
                                </button>
                            </div>
                        </div>

                        <div class="form-group form-group-sm" id="div_comment" runat="server"></div>

                        <div class="form-group form-group-sm table-responsive">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="ID" Visible="false"></asp:BoundField>
                                        <asp:TemplateField HeaderText="No" ItemStyle-Width="45px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="LblNo" runat="server" Text='<%# (GridView1.PageIndex * GridView1.PageSize) + Container.DisplayIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="customer_company_nm" HeaderText="Company Name" NullDisplayText="-"></asp:BoundField>
                                        <asp:BoundField DataField="company_id" HeaderText="Company ID" NullDisplayText="-"></asp:BoundField>
                                        <asp:BoundField DataField="pic" HeaderText="PIC" NullDisplayText="-"></asp:BoundField>
                                        <asp:BoundField DataField="address" HeaderText="Address" NullDisplayText="-" ItemStyle-CssClass="address-wrap"></asp:BoundField>
                                        <asp:BoundField DataField="phoneno" HeaderText="Phone No" NullDisplayText="-"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Email">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="LnkEmail" runat="server" Text="-"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="legal_name" HeaderText="Legal Name" NullDisplayText="-"></asp:BoundField>
                                        <asp:BoundField DataField="brand_name" HeaderText="Brand Name" NullDisplayText="-"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Status Verifikasi" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="LblStatus" runat="server" CssClass="label"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdActivate" runat="server" CssClass="btn btn-success btn-xs" CommandName="ACTIVATE" CommandArgument='<%# Eval("id") %>' Text="Aktivasi" OnClientClick="return confirmActivate(this);"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPagingParam" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <style>
        .address-wrap {
            white-space: normal !important;
            word-break: break-word;
            max-width: 240px;
        }
    </style>

    <script type="text/javascript">
        function confirmActivate(btn) {
            if (!confirm('Apakah anda yakin ingin mengaktifkan data ini?')) {
                return false;
            }

            if (btn.getAttribute('data-submitted') === '1') {
                return false;
            }

            btn.setAttribute('data-submitted', '1');
            btn.classList.add('disabled');
            btn.innerHTML = "<i class='fa fa-spinner fa-spin'></i> Processing";

            if (typeof showOverlay === 'function') {
                showOverlay();
            }

            return true;
        }
    </script>
</asp:Content>
