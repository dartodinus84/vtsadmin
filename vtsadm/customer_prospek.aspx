<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="customer_prospek.aspx.cs" Inherits="vtsadm.customer_prospek" EnableEventValidation="false" %>

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
                            <label>Full Name</label>
                            <asp:TextBox ID="txtCustomerID" runat="server" class="form-control" placeholder="Skip for new customer ..." required="required" type="hidden"></asp:TextBox>
                            <asp:TextBox ID="txtFullName" runat="server" class="form-control" placeholder="Full Name (Please using title for corporate customer, ex: PT. xxxx) ..." required="required"></asp:TextBox>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Address</label>
                            <textarea id="txtAddress" runat="server" class="form-control" placeholder="Address ..." rows="2"></textarea>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Business Fields <span style="color:red">*</span></label>
                            <asp:DropDownList ID="CmbBusinessField" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbBusinessField_TextChanged"></asp:DropDownList>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Sub Business Fields <span style="color:red">*</span></label>
                            <asp:DropDownList ID="CmbBusinessSubField" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                        </div>

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
            </div>
            
            <div class="col-md-6">
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

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Details Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>PIC Name</label>
                            <asp:TextBox ID="txtPICName" runat="server" class="form-control" placeholder="PIC Name ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>PIC Position</label>
                            <asp:TextBox ID="txtPICPosition" runat="server" class="form-control" placeholder="PIC Position  ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Mobile Phone</label>
                            <asp:TextBox ID="txtMobilePhone" runat="server" class="form-control" placeholder="Mobile Phone  ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Office Phone</label>
                            <asp:TextBox ID="txtOfficePhone" runat="server" class="form-control" placeholder="Office Phone  ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" class="form-control" placeholder="Email  ..."></asp:TextBox>
                        </div>
                    </div>

                    <div class="box-footer">
                        <button type="button" id="CmdClear" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="return validateCustomerBusinessFields();" Text="Submit" />
                        <button type="button" class="btn btn-info" data-toggle="modal" data-target="#modal-upload-excel">
                            <i class="fa fa-file-excel-o"></i>&nbsp;Upload Excel
                        </button>
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
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                       
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="BranchAddress" HeaderText="Branch Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BranchID" HeaderText="Branch ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="CustTypeID" HeaderText="Cust Type ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        
                                        <asp:BoundField DataField="PICName" HeaderText="PIC Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PICPosition" HeaderText="PIC Position" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="OfficePhone" HeaderText="Office Phone" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MobilePhone" HeaderText="Office Phone" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="OperationalArea" HeaderText="Operational Area" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="VehicleType" HeaderText="Vehicle Type" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BusinessFields" HeaderText="Business Fields" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BusinessFieldID" HeaderText="BusinessFieldID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ServiceTypeID" HeaderText="Service Type" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BusinessSubFieldID" HeaderText="BusinessSubFieldID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BusinessSubFields" HeaderText="Sub Business Fields" ItemStyle-Wrap="false"></asp:BoundField>
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

        <div class="modal fade" id="modal-upload-excel" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                        <h4 class="modal-title">Upload Customer Prospek (Excel)</h4>
                    </div>
                    <div class="modal-body">
                        <p class="text-muted small">
                            Upload file <strong>.xlsx</strong> dengan kolom:
                            BranchID, CustTypeID, IDType, IDNumber, FullName, Address, PICName, PICPosition,
                            OfficePhone, MobilePhone, Email, BusinessFieldID, BusinessSubFieldID, OperationalArea, VehicleType, ServiceTypeID.
                            <br />
                            <strong>BusinessFieldID</strong> dan <strong>BusinessSubFieldID</strong> wajib diisi dengan <strong>ID</strong> (bukan nama).
                            Sub Field harus milik Business Field yang sama. Lihat sheet lookup di template.
                        </p>
                        <div id="DivUploadError" runat="server" class="alert alert-danger" role="alert" style="display: none;"></div>
                        <div class="form-group form-group-sm">
                            <asp:FileUpload ID="FileUploadExcel" runat="server" CssClass="form-control" accept=".xlsx" />
                        </div>
                        <div class="form-group form-group-sm">
                            <asp:Button ID="CmdUploadExcel" runat="server" CssClass="btn btn-primary" Text="Upload &amp; Import"
                                CausesValidation="false" UseSubmitBehavior="true" formnovalidate="formnovalidate"
                                OnClick="CmdUploadExcel_Click" />
                            <asp:Button ID="CmdDownloadTemplate" runat="server" CssClass="btn btn-default" Text="Download Template"
                                CausesValidation="false" UseSubmitBehavior="true" formnovalidate="formnovalidate"
                                OnClick="CmdDownloadTemplate_Click" />
                        </div>
                        <asp:Panel runat="server" ScrollBars="Auto" Height="250" ID="PanelUploadResult">
                            <asp:GridView ID="GridViewUploadResult" runat="server" CssClass="table table-bordered table-condensed"
                                AutoGenerateColumns="False" EnableViewState="false"
                                EmptyDataText="Upload file Excel lalu klik Upload &amp; Import."
                                OnRowDataBound="GridViewUploadResult_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="RowNo" HeaderText="#" />
                                    <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                                    <asp:BoundField DataField="IDNumber" HeaderText="ID Number" />
                                    <asp:BoundField DataField="BusinessFieldID" HeaderText="Business Field" />
                                    <asp:BoundField DataField="BusinessSubFieldID" HeaderText="Sub Business Field" />
                                    <asp:BoundField DataField="StatusText" HeaderText="Status" />
                                    <asp:BoundField DataField="Reason" HeaderText="Reason" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function validateCustomerBusinessFields() {
            var cmbBiz = document.getElementById('ContentPlaceHolder1_CmbBusinessField');
            var cmbSub = document.getElementById('ContentPlaceHolder1_CmbBusinessSubField');
            if (!cmbBiz || cmbBiz.value === '' || cmbBiz.value === '[Select]') {
                alert('Business Fields wajib diisi.');
                if (cmbBiz) cmbBiz.focus();
                return false;
            }
            if (!cmbSub || cmbSub.disabled || cmbSub.value === '' || cmbSub.value === '[Select]') {
                alert('Sub Business Fields wajib diisi.');
                if (cmbSub && !cmbSub.disabled) cmbSub.focus();
                return false;
            }
            $('#modal-submit').modal('show');
            return false;
        }

        function confirmDelete(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblCustID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtCustIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete").modal('show');
            }
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
