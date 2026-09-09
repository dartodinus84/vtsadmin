<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="customer.aspx.cs" Inherits="vtsadm.customer" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Customer           
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Customer</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Customer ID</label>
                            <asp:TextBox ID="txtCustomerID" runat="server" class="form-control" placeholder="Skip for new customer ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Full Name</label>
                            <asp:TextBox ID="txtFullName" runat="server" class="form-control" placeholder="Full Name (Please using title for corporate customer, ex: PT. xxxx) ..." required="required"></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Business Fields <span style="color:red">*</span></label>
                            <asp:DropDownList ID="CmbBusinessField" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbBusinessField_TextChanged"></asp:DropDownList>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Sub Business Fields <span style="color:red">*</span></label>
                            <asp:DropDownList ID="CmbBusinessSubField" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                        </div>

                        <!--
                        <div class="form-group form-group-sm">
                            <label>Business Fields</label>
                            <textarea id="txtBusinessFields" runat="server" class="form-control" placeholder="Business Fields ..." rows="2" required="required"></textarea>
                        </div>
                        -->

                        <div class="form-group form-group-sm">
                            <label>Operational Area</label>
                            <textarea id="txtOperationalArea" runat="server" class="form-control" placeholder="Operational Area ..." rows="2" required="required"></textarea>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Vehicle Type</label>
                            <textarea id="txtVehicleType" runat="server" class="form-control" placeholder="Vehicle Type ..." rows="2" required="required"></textarea>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Identity Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Customer Type</label>
                            <asp:DropDownList ID="CmbCustTypeID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Service Type</label>
                            <asp:DropDownList ID="CmbServiceTypeID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>ID Type</label>
                            <asp:DropDownList ID="CmbIDType" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>ID Number</label>
                            <asp:TextBox ID="txtIDNo" runat="server" class="form-control" placeholder="ID Number ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Branch Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <asp:DropDownList ID="CmbBranchID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbBranchID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Address</label>
                            <asp:TextBox ID="txtBranchAddress" runat="server" class="form-control" placeholder="Branch Address ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>


                <div class="nav-tabs-custom" id="div_details">
                    <ul class="nav nav-tabs pull-right">
                        <li><a href="#tab_4-4" data-toggle="tab">PIC 4</a></li>
                        <li><a href="#tab_3-3" data-toggle="tab">PIC 3</a></li>
                        <li><a href="#tab_2-2" data-toggle="tab">PIC 2</a></li>
                        <li class="active"><a href="#tab_1-1" data-toggle="tab">PIC 1</a></li>
                        <li class="pull-left header">Details Information</li>
                    </ul>
                    <div class="tab-content active">
                        <div class="tab-pane" id="tab_4-4">
                            <div class="form-group form-group-sm">
                                <label>PIC Name 4</label>
                                <asp:TextBox ID="txtPICName4" runat="server" class="form-control" placeholder="PIC Name 4 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>PIC Position 4</label>
                                <asp:TextBox ID="txtPICPosition4" runat="server" class="form-control" placeholder="PIC Position 4 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Mobile Phone 4</label>
                                <asp:TextBox ID="txtMobilePhone4" runat="server" class="form-control" placeholder="Mobile Phone 4 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Office Phone 4</label>
                                <asp:TextBox ID="txtOfficePhone4" runat="server" class="form-control" placeholder="Office Phone 4 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Email 4</label>
                                <asp:TextBox ID="txtEmail4" runat="server" class="form-control" placeholder="Email 4 ..."></asp:TextBox>
                            </div>
                        </div>
                        <div class="tab-pane" id="tab_3-3">
                            <div class="form-group form-group-sm">
                                <label>PIC Name 3</label>
                                <asp:TextBox ID="txtPICName3" runat="server" class="form-control" placeholder="PIC Name 3 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>PIC Position 3</label>
                                <asp:TextBox ID="txtPICPosition3" runat="server" class="form-control" placeholder="PIC Position 3 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Mobile Phone 3</label>
                                <asp:TextBox ID="txtMobilePhone3" runat="server" class="form-control" placeholder="Mobile Phone 3 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Office Phone 3</label>
                                <asp:TextBox ID="txtOfficePhone3" runat="server" class="form-control" placeholder="Office Phone 3 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Email 3</label>
                                <asp:TextBox ID="txtEmail3" runat="server" class="form-control" placeholder="Email 3 ..."></asp:TextBox>
                            </div>
                        </div>
                        <div class="tab-pane" id="tab_2-2">
                            <div class="form-group form-group-sm">
                                <label>PIC Name 2</label>
                                <asp:TextBox ID="txtPICName2" runat="server" class="form-control" placeholder="PIC Name 2 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>PIC Position 2</label>
                                <asp:TextBox ID="txtPICPosition2" runat="server" class="form-control" placeholder="PIC Position 2 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Mobile Phone 2</label>
                                <asp:TextBox ID="txtMobilePhone2" runat="server" class="form-control" placeholder="Mobile Phone 2 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Office Phone 2</label>
                                <asp:TextBox ID="txtOfficePhone2" runat="server" class="form-control" placeholder="Office Phone 2 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Email 2</label>
                                <asp:TextBox ID="txtEmail2" runat="server" class="form-control" placeholder="Email 2 ..."></asp:TextBox>
                            </div>
                        </div>
                        <div class="tab-pane active" id="tab_1-1">
                            <div class="form-group form-group-sm">
                                <label>PIC Name 1</label>
                                <asp:TextBox ID="txtPICName1" runat="server" class="form-control" placeholder="PIC Name 1 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>PIC Position 1</label>
                                <asp:TextBox ID="txtPICPosition1" runat="server" class="form-control" placeholder="PIC Position 1 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Mobile Phone 1</label>
                                <asp:TextBox ID="txtMobilePhone1" runat="server" class="form-control" placeholder="Mobile Phone 1 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Office Phone 1</label>
                                <asp:TextBox ID="txtOfficePhone1" runat="server" class="form-control" placeholder="Office Phone 1 ..."></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Email 1</label>
                                <asp:TextBox ID="txtEmail1" runat="server" class="form-control" placeholder="Email 1 ..."></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <%--                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Details Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>PIC Name 1</label>
                            <asp:TextBox ID="txtPICName1" runat="server" class="form-control" placeholder="PIC Name 1 ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>PIC Position 1</label>
                            <asp:TextBox ID="txtPICPosition1" runat="server" class="form-control" placeholder="PIC Position 1 ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Mobile Phone 1</label>
                            <asp:TextBox ID="txtMobilePhone1" runat="server" class="form-control" placeholder="Mobile Phone 1 ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>PIC Name 2</label>
                            <asp:TextBox ID="txtPICName2" runat="server" class="form-control" placeholder="PIC Name 2 ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>PIC Position 2</label>
                            <asp:TextBox ID="txtPICPosition2" runat="server" class="form-control" placeholder="PIC Position 2 ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Mobile Phone 2</label>
                            <asp:TextBox ID="txtMobilePhone2" runat="server" class="form-control" placeholder="Mobile Phone 2 ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>PIC Name 3</label>
                            <asp:TextBox ID="txtPICName3" runat="server" class="form-control" placeholder="PIC Name 3 ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>PIC Position 3</label>
                            <asp:TextBox ID="txtPICPosition3" runat="server" class="form-control" placeholder="PIC Position 3 ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Mobile Phone 3</label>
                            <asp:TextBox ID="txtMobilePhone3" runat="server" class="form-control" placeholder="Mobile Phone 3 ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>PIC Name 4</label>
                            <asp:TextBox ID="txtPICName4" runat="server" class="form-control" placeholder="PIC Name 4 ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>PIC Position 4</label>
                            <asp:TextBox ID="txtPICPosition4" runat="server" class="form-control" placeholder="PIC Position 4 ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Mobile Phone 4</label>
                            <asp:TextBox ID="txtMobilePhone4" runat="server" class="form-control" placeholder="Mobile Phone 4 ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Office Phone</label>
                            <asp:TextBox ID="txtOfficePhone" runat="server" class="form-control" placeholder="Office Phone ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" class="form-control" placeholder="Email ..."></asp:TextBox>
                        </div>
                    </div>
                </div>--%>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Address Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Address</label>
                            <textarea id="txtAddress" runat="server" class="form-control" placeholder="Address ..." rows="2"></textarea>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Billing Address</label>
                            <textarea id="txtBillingAddr" runat="server" class="form-control" placeholder="Billing Address ..." rows="2"></textarea>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tax Address</label>
                            <textarea id="txtTaxAddr" runat="server" class="form-control" placeholder="Tax Address ..." rows="2"></textarea>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Shipping Address</label>
                            <textarea id="txtShippingAddr" runat="server" class="form-control" placeholder="Shipping Address ..." rows="2"></textarea>
                        </div>
                    </div>
                </div>

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Ducument Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Start Date</label>
                            <asp:TextBox ID="txtPeriodStart" TextMode="Date" runat="server" class="form-control" placeholder="Date Arrival ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>End Date</label>
                            <asp:TextBox ID="txtPeriodEnd" TextMode="Date" runat="server" class="form-control" placeholder="Date Arrival ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Doc Name</label>
                            <asp:TextBox ID="textAttachName" runat="server" class="form-control" placeholder="Doc Name"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Doc Desc</label>
                            <asp:TextBox ID="textAttachDesc" runat="server" class="form-control" placeholder="Doc Desc"></asp:TextBox>
                        </div>

                        

                    </div>

                    <div class="box-footer">
                        <button id="CmdUpload" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload">Upload Doc</button>
                    </div>

                </div>

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Others Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Group ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtCustGroupID" runat="server" class="form-control" placeholder="Group ID ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button2" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-customer"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Marketing Name</label>
                            <asp:DropDownList ID="CmbMarketing" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>IT Support</label>
                            <asp:DropDownList ID="CmbITS" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>IT Outbound</label>
                            <asp:DropDownList ID="CmbITOutbound" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Support Area</label>
                            <asp:DropDownList ID="CmbSupportAreaID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Sync VMS</label>
                            <select id="txtSyncFms" runat="server" class="form-control">
                                <option value="0">No</option>
                                <option value="1">Yes</option>
                            </select>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button type="button" id="CmdClear" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdDoc" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-document').modal('show');return false;" Text="Documents" />
                        <asp:Button ID="CmdServer" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-server').modal('show');return false;" Text="Servers" />
                        <asp:Button ID="CmdUser" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-user').modal('show');return false;" Text="User" />
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="return validateCustomerBusinessFields();" Text="Submit" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Customer</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by name ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="CmdSearch" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearch_ServerClick"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True"
                                    PageSize="5" OnRowCommand="GridView2_RowCommand" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="CustID" HeaderText="Customer ID" ItemStyle-Wrap="false" SortExpression="CustID"></asp:BoundField>
                                        <asp:BoundField DataField="FullName" HeaderText="Full Name" ItemStyle-Wrap="false" SortExpression="FullName"></asp:BoundField>
                                        <asp:BoundField DataField="IDType" HeaderText="ID Type" ItemStyle-Wrap="false" SortExpression="IDType"></asp:BoundField>
                                        <asp:BoundField DataField="IDNumber" HeaderText="ID Number" ItemStyle-Wrap="false" SortExpression="IDNumber"></asp:BoundField>
                                        <asp:BoundField DataField="CustTypeDesc" HeaderText="Cust Type Desc" ItemStyle-Wrap="false" SortExpression="CustTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="BranchName" HeaderText="Branch Name" ItemStyle-Wrap="false" SortExpression="BranchName"></asp:BoundField>
                                        <asp:BoundField DataField="SyncFms" HeaderText="SyncFms" ItemStyle-Wrap="false" SortExpression="SyncFms"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <%--<asp:ButtonField ControlStyle-CssClass="btn btn-block btn-primary btn-xs" Text="Delete" ButtonType="Image" CommandName="Delete"></asp:ButtonField>--%>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BranchAddress" HeaderText="Branch Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BranchID" HeaderText="Branch ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="CustTypeID" HeaderText="Cust Type ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BillingAddress" HeaderText="Billing Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TaxAddress" HeaderText="Tax Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ShipmentAddress" HeaderText="Shipment Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PICName1" HeaderText="PIC Name 1" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PICPosition1" HeaderText="PIC Position 1" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PICName2" HeaderText="PIC Name 2" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PICPosition2" HeaderText="PIC Position 2" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PICName3" HeaderText="PIC Name 3" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PICPosition3" HeaderText="PIC Position 3" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PICName4" HeaderText="PIC Name 4" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PICPosition4" HeaderText="PIC Position 4" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="OfficePhone1" HeaderText="Office Phone 1" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="OfficePhone2" HeaderText="Office Phone 2" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="OfficePhone3" HeaderText="Office Phone 3" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="OfficePhone4" HeaderText="Office Phone 4" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MobilePhone1" HeaderText="Mobile Phone 1" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MobilePhone2" HeaderText="Mobile Phone 2" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MobilePhone3" HeaderText="Mobile Phone 3" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MobilePhone4" HeaderText="Mobile Phone 4" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Email1" HeaderText="Email 1" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Email2" HeaderText="Email 2" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Email3" HeaderText="Email 3" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Email4" HeaderText="Email 4" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MarketingID" HeaderText="Marketing ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="CustGroupID" HeaderText="Group ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UserID_Server" HeaderText="Group ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UserName_Server" HeaderText="Group ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="sPass_Server" HeaderText="Group ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BusinessFields" HeaderText="Business Fields" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="OperationalArea" HeaderText="Operational Area" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="VehicleType" HeaderText="Vehicle Type" ItemStyle-Wrap="false"></asp:BoundField>

                                        <asp:BoundField DataField="AttachName" HeaderText="Attach Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="AttachDesc" HeaderText="Attach Desc" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="AttachUrl" HeaderText="Attach Url" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PeriodStart" HeaderText="PeriodStart" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PeriodEnd" HeaderText="PeriodEnd" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ServiceTypeID" HeaderText="Service Type" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ITS" HeaderText="IT Support" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ITOutbound" HeaderText="IT Outbound" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BusinessFieldID" HeaderText="BusinessFieldID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="SupAreaID" HeaderText="SupAreaID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BusinessSubFieldID" HeaderText="BusinessSubFieldID" ItemStyle-Wrap="false"></asp:BoundField>
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
        </div>

        <div class="modal fade" id="modal-submit" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure ?</h6>
                    </div>
                    <div class="modal-footer">
                        <img src="Content/ajax-loader2.gif" style="display: none;" id="iload" />
                        <button type="button" class="btn btn-default btn-confirmasi" runat="server" onclick="$('.btn-confirmasi').attr('disabled','disabled');$('button.close').hide();$('#iload').show();" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
                        <button type="button" class="btn btn-primary btn-confirmasi" onclick="$('#modal-submit').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-delete">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete Customer ID :&nbsp;</h6>
                        <label id="LblCustID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtCustIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-customer">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Customer Group</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="customer_customer_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-document">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Document Attachment</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto" Height="350px">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="100" OnRowDataBound="GridView1_RowDataBound">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="DocID" HeaderText="Doc ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DocName" HeaderText="Doc Name" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Check" HeaderStyle-CssClass="gv-header-center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk1" runat="server" Enabled="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="IsAttach" HeaderText="IsAttach" ItemStyle-Wrap="false"></asp:BoundField>
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
                                    <asp:Label ID="LblPagingDoc" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-server">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Server Registered</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto" Height="350px">
                                <asp:GridView ID="GridView3" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="100" OnRowDataBound="GridView3_RowDataBound">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="ServerID" HeaderText="Server ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ServerName" HeaderText="Server Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Mis_CustID" HeaderText="Company ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Check" HeaderStyle-CssClass="gv-header-center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk1" runat="server" Enabled="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="IsCheck" HeaderText="IsCheck" ItemStyle-Wrap="false"></asp:BoundField>
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
                                    <asp:Label ID="LblPagingServer" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-upload">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Upload</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="customer_upload_doc.aspx" style="width: 100%; border: none; height: 460px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal modal-open fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Info Box</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" id="div_comment" runat="server">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-user">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">User Information</h4>
                    </div>
                    <div class="modal-body" style="background-color: #ecf0f5">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">User Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <input type="hidden" id="txtOldPassword" runat="server" />
                                        <div class="form-group form-group-sm">
                                            <label>User ID</label>
                                            <asp:TextBox ID="txtUserID" runat="server" class="form-control" placeholder="User ID ..."></asp:TextBox>
                                        </div>

                                        <div class="form-group form-group-sm" id="divNewUserID" runat="server" style="display: none">
                                            <label>New User ID</label>
                                            <asp:TextBox ID="txtNewUserID" runat="server" class="form-control" placeholder="User ID ..."></asp:TextBox>
                                            <small class="text-muted">Isi hanya jika ingin mengganti User ID saat update.</small>
                                        </div>

                                        <div class="form-group form-group-sm">
                                            <label>User Name</label>
                                            <asp:TextBox ID="txtUserName" runat="server" class="form-control" placeholder="User Name ..."></asp:TextBox>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Password</label>
                                            <asp:TextBox ID="txtPassword" runat="server" class="form-control" placeholder="Password ..." TextMode="Password" autocomplete="new-password"></asp:TextBox>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Confirm Password</label>
                                            <asp:TextBox ID="txtConfirmPassword" runat="server" class="form-control" placeholder="Confirm Password ..." TextMode="Password" autocomplete="new-password"></asp:TextBox>
                                        </div>
                                        <div class="alert alert-info" style="margin-top: 10px; margin-bottom: 10px;">
                                            <strong>Ketentuan Password:</strong><br />
                                            Password harus terdiri dari 6-40 karakter dan wajib mengandung:
                                            <ul style="margin-top: 8px; margin-bottom: 8px; padding-left: 18px;">
                                                <li>Huruf besar (A-Z)</li>
                                                <li>Huruf kecil (a-z)</li>
                                                <li>Angka (0-9)</li>
                                                <li>Simbol/karakter khusus (contoh: @ # $ % ! *)</li>
                                            </ul>
                                            Password tidak boleh:
                                            <ul style="margin-top: 8px; margin-bottom: 8px; padding-left: 18px;">
                                                <li>Menggunakan urutan karakter beruntun (contoh: 1234, abcd)</li>
                                                <li>Mengandung karakter yang sama lebih dari 2 kali berturut-turut (contoh: aaa, 111)</li>
                                                <li>Sama dengan 12 password terakhir yang pernah digunakan</li>
                                            </ul>
                                            Contoh password yang benar:
                                            <ul style="margin-top: 8px; margin-bottom: 0; padding-left: 18px;">
                                                <li>Test#123</li>
                                                <li>Secure!789</li>
                                            </ul>
                                        </div>
                                        <div id="admDivCheck" runat="server" class="modal-footer" style="display: none">
                                            <button type="button" id="CmdUpdateUser" class="btn btn-primary" runat="server" onserverclick="CmdUpdateUser_ServerClick">Update User</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function postCustGroupChild(sCustID) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtCustGroupID').value = sCustID;
                $('#modal-customer').modal('hide');
            }
        }

        function confirmDelete(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblCustID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtCustIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete").modal('show');
            }
        }

        function validateCustomerBusinessFields() {
            var cmbBiz = document.getElementById('ContentPlaceHolder1_CmbBusinessField');
            var cmbSub = document.getElementById('ContentPlaceHolder1_CmbBusinessSubField');
            var cmdSubmit = document.getElementById('ContentPlaceHolder1_CmdSubmit');
            var isInsert = !cmdSubmit || (cmdSubmit.value || '').toUpperCase() === 'SUBMIT';
            if (!cmbBiz || cmbBiz.value === '' || cmbBiz.value === '[Select]') {
                alert('Business Fields wajib diisi.');
                if (cmbBiz) cmbBiz.focus();
                return false;
            }
            if (isInsert && (!cmbSub || cmbSub.disabled || cmbSub.value === '' || cmbSub.value === '[Select]')) {
                alert('Sub Business Fields wajib diisi.');
                if (cmbSub && !cmbSub.disabled) cmbSub.focus();
                return false;
            }
            $('#modal-submit').modal('show');
            return false;
        }

        function endRequest(sender, args) {
            $('#modal-messagebox').on('hidden.bs.modal', function () {
                document.body.style.paddingRight = '0px';
            });
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
                $('.modal-backdrop').remove();
                $('#modal-messagebox').modal('show');
            }
        }
        endRequest();
    </script>
</asp:Content>
