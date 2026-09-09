<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_customer2.aspx.cs" Inherits="vtsadm.dashboard_customer2" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .icon {
            top: 0px !important;
            font-size: 40px !important;
        }

        .inner {
            height: 80px !important;
            padding-top: 2px !important;
        }



        .small-box-footer {
            border-bottom-left-radius: 11px;
            border-bottom-right-radius: 11px;
        }

        .small-box {
            border-radius: 11px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .box {
            border-radius: 11px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .widget-user-2 .widget-user-header {
            border-top-left-radius: 11px;
            border-top-right-radius: 11px;
            height: 60px;
            padding-top: 10px;
        }

        .box-header {
            font-size: 16px;
        }

        li-format {
            height: 45px !important;
        }

        .package{
            max-height: 288px;
            overflow-y: scroll; 
        }

        .scrollbar{
            /*margin-left: 30px;*/
            float: left;
            height: 288px;
            /*width: 65px;*/
            width: 100%;
            /*background: #F5F5F5;*/
            overflow-y: scroll;
            /*margin-bottom: 25px;*/
        }

        .force-overflow{
            min-height: 288px;
        }
    </style>

    <style>
      .select,
      #locale {
        width: 100%;
      }
      .like {
        margin-right: 10px;
      }
    </style>


    <section class="content-header">
        <h1>Dashboard
        <small>Customer</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="#"><i class="fa fa-dashboard"></i>Customer</a></li>
        </ol>
    </section>


    <section class="content">
        <div class="row">
            <div class="col-lg-4">
                <div class="small-box bg-blue-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblTotalCustomer" runat="server"></label>
                        </h3>
                        <p>Total Customer</p>
                    </div>                    
                    <div class="icon">
                        <i class="fa fa-users"></i>
                    </div>
                    <a href="rpt_dashboard_customer.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="small-box bg-yellow-gradient">
                    <div class="inner">
                        <div class="row">
                            <div class="col-lg-4" style="padding-bottom:0; margin-bottom:0;">
                                <h3>
                                    <label id="lblJmlTMS" runat="server"></label>
                                </h3>
                                <p class="no-margin">Customer TMS</p>
                            </div>
                            <div class="col-lg-6" style="padding-left:0; padding-right:0;">
                                <table class="table table-bordered no-margin no-padding">
                                    <tr>
                                        <th class="text-center bg-green">
                                            Active
                                        </th>
                                        <th class="text-center bg-red">
                                            Block
                                        </th>
                                    </tr>
                                    <tr>
                                        <td class="text-center">
                                            <label id="lbl_tms_active" runat="server" class="no-margin"></label>
                                        </td>
                                        <td class="text-center">
                                            <label id="lbl_tms_block" runat="server" class="no-margin"></label>
                                        </td>
                                    </tr>
                                </table>                                
                            </div>
                        </div>
                    </div>
                    <div class="icon">
                        <i class="fa fa-server"></i>
                    </div>
                    <a href="rpt_dashboard_customer.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblJmlIGO" runat="server"></label>
                        </h3>
                        <p>Customer IGO Tracker</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-server"></i>
                    </div>
                    <a href="rpt_dashboard_customer.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="box">
                    <div class="box-body">
                        <div class="scrollbar" id="style-1">
                            <div class="force-overflow">
                                <table class="table table-bordered" id="tbBranch">
                                    <tr>
                                        <th class="text-center bg-blue">
                                            Branch Name
                                        </th>
                                        <th class="text-center bg-green">
                                            TMS Active
                                        </th>
                                        <th class="text-center bg-red">
                                            TMS Block
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>Jakarta Branch</td>
                                        <td class="text-center"><label id="lblJktActive" runat="server"></label></td>
                                        <td class="text-center"><label id="lblJktBlock" runat="server"></label></td>
                                    </tr>
                                    <tr>
                                        <td>Surabaya Branch</td>
                                        <td class="text-center"><label id="lblSbyActive" runat="server"></label></td>
                                        <td class="text-center"><label id="lblSbyBlock" runat="server"></label></td>
                                    </tr>
                                    <tr>
                                        <td>Medan Branch</td>
                                        <td class="text-center"><label id="lblMdnActive" runat="server"></label></td>
                                        <td class="text-center"><label id="LblMdnBlock" runat="server"></label></td>
                                    </tr>
                                    <tr>
                                        <td>Padang Branch</td>
                                        <td class="text-center"><label id="lblPdgActive" runat="server"></label></td>
                                        <td class="text-center"><label id="lblPdgBlock" runat="server"></label></td>
                                    </tr>
                                    <tr>
                                        <td>Semarang Branch</td>
                                        <td class="text-center"><label id="lblSmrActive" runat="server"></label></td>
                                        <td class="text-center"><label id="lblSmrBlock" runat="server"></label></td>
                                    </tr>
                                    <tr>
                                        <td>Palembang Branch</td>
                                        <td class="text-center"><label id="lblPlmbActive" runat="server"></label></td>
                                        <td class="text-center"><label id="lblPlmbBlock" runat="server"></label></td>
                                    </tr>
                                    <tr>
                                        <td>Yogyakarta Branch</td>
                                        <td class="text-center"><label id="lblYgtActive" runat="server"></label></td>
                                        <td class="text-center"><label id="lblYgtBlock" runat="server"></label></td>
                                    </tr>
                                </table>
                            </div>
                        </div>                        
                    </div>
                    <div class="small-box bg-teal-gradient" style="border-top-left-radius:0; border-top-right-radius:0;">
                        <a href="rpt_dashboard_customer_by_branch.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="box">
                    <div class="box-body">
                        <table class="table table-bordered">
                            <tr>
                                <th class="text-center bg-blue">
                                    Unit Range
                                </th>
                                <th class="text-center bg-yellow">
                                    Customer
                                </th>
                                <th class="text-center bg-teal">
                                    JO Training Closed
                                </th>
                            </tr>
                            <tr>
                                <td>1-2 UNIT</td>
                                <td class="text-center"><label id="unit12" runat="server"></label></td>
                                <td class="text-center"><label id="joTrng12" runat="server"></label></td>
                            </tr>
                            <tr>
                                <td>3-5 UNIT</td>
                                <td class="text-center"><label id="unit35" runat="server"></label></td>
                                <td class="text-center"><label id="joTrng35" runat="server"></label></td>
                            </tr>
                            <tr>
                                <td>6-10 UNIT</td>
                                <td class="text-center"><label id="unit610" runat="server"></label></td>
                                <td class="text-center"><label id="joTrng610" runat="server"></label></td>
                            </tr>
                            <tr>
                                <td>11-20 UNIT</td>
                                <td class="text-center"><label id="unit1120" runat="server"></label></td>
                                <td class="text-center"><label id="joTrng1120" runat="server"></label></td>
                            </tr>
                            <tr>
                                <td>21-30 UNIT</td>
                                <td class="text-center"><label id="unit2130" runat="server"></label></td>
                                <td class="text-center"><label id="joTrng2130" runat="server"></label></td>
                            </tr>
                            <tr>
                                <td>>30 UNIT</td>
                                <td class="text-center"><label id="unit30" runat="server"></label></td>
                                <td class="text-center"><label id="joTrng30" runat="server"></label></td>
                            </tr>
                        </table>
                    </div>
                    <div class="small-box bg-teal-gradient" style="border-top-left-radius:0; border-top-right-radius:0;">
                        <a href="rpt_dashboard_customer_by_jumlah_unit.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="box">
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True"
                                    PageSize="5" OnRowCommand="GridView2_RowCommand" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="PackageName" HeaderText="Package Name" ItemStyle-Wrap="false" SortExpression="PackageName"></asp:BoundField>
                                        <asp:BoundField DataField="jml" HeaderText="Jumlah Customer" ItemStyle-Wrap="false" SortExpression="jml"></asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="Black" />
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-pie-chart"></i>
                        CUSTOMER EASYGO
                    </div>
                    <div class="box-body">
                        <div id="chartCustomerEasygo" style="height: 200px;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-pie-chart"></i>
                        CUSTOMER TMS STATUS
                    </div>
                    <div class="box-body">
                        <div id="chartCustomerTMSstatus" style="height: 200px;"></div>
                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-pie-chart"></i>
                        CUSTOMER TMS
                    </div>
                    <div class="box-body">
                        <div id="chartCustomerTMSrange" style="height: 200px;"></div>
                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-pie-chart"></i>
                        CUSTOMER BRANCH
                    </div>
                    <div class="box-body">
                        <div id="chartCustomerBranch" style="height: 200px;">
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-default">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title">Modal Detail</h4>
                    </div>
                    <div class="modal-body">
                      
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-leftz" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

    </section>
    <script src="Scripts/js/dashboard/dashboard_customer.js"></script>
    <script src="https://canvasjs.com/assets/script/jquery-1.11.1.min.js"></script>
    <script src="https://canvasjs.com/assets/script/jquery.canvasjs.min.js"></script>
</asp:Content>