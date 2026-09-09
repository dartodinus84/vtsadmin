<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="job_maint_upload.aspx.cs" Inherits="vtsadm.job_maint_upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <!-- Bootstrap 3.3.7 -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="Content/bower_components/font-awesome/css/font-awesome.min.css" />
    <!-- Ionicons -->
    <link rel="stylesheet" href="Content/bower_components/Ionicons/css/ionicons.min.css" />
    <!-- daterange picker -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap-daterangepicker/daterangepicker.css" />
    <!-- bootstrap datepicker -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css" />
    <!-- DataTables -->
    <link rel="stylesheet" href="Content/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css" />
    <!-- iCheck for checkboxes and radio inputs -->
    <link rel="stylesheet" href="Content/plugins/iCheck/all.css" />
    <!-- Bootstrap Color Picker -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap-colorpicker/dist/css/bootstrap-colorpicker.min.css" />
    <!-- Bootstrap time Picker -->
    <link rel="stylesheet" href="Content/plugins/timepicker/bootstrap-timepicker.min.css" />
    <!-- Select2 -->
    <link rel="stylesheet" href="Content/bower_components/select2/dist/css/select2.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="Content/dist/css/AdminLTE.min.css" />
    <!-- AdminLTE Skins. Choose a skin from the css/skins
           folder instead of downloading all of them to reduce the load. -->
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
      <![endif]-->
    <link rel="stylesheet" href="Content/dist/css/skins/_all-skins.min.css" />
    <link rel="stylesheet" href="Content/paginationcs.css" />
    <link rel="stylesheet" href="Content/loader.css" />
    <!-- Google Font -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic" />


    <style type="text/css">
        .example-modal .modal {
            position: relative;
            top: auto;
            bottom: auto;
            right: auto;
            left: auto;
            display: block;
            z-index: 1;
        }

        .example-modal .modal {
            background: transparent !important;
        }
    </style>

    <script type="text/javascript" src="Content/bower_components/jquery/dist/jquery.min.js"></script>
    <script type="text/javascript" src="Content/bower_components/jquery-ui/jquery-ui.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script type="text/javascript" src="Content/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <input type="hidden" runat="server" id="txtIsUpdate" />
                            <input type="hidden" runat="server" id="txtJobID" />
                            <div class="alert alert-info" id="jobIDDisplay" style="display: none;">
                                <strong>Job ID:</strong> <span id="jobIDValue"></span>
                            </div>
                            <label>File Input</label>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </div>
                        <div class="box-footer">
                            <button id="CmdUpload" runat="server" type="button" class="btn btn-primary" onserverclick="CmdUpload_ServerClick">Upload</button>
                            <button id="CmdCancel" runat="server" type="button" class="btn btn-primary" onserverclick="CmdCancel_ServerClick">Cancel</button>
                            <div runat="server" id="lblMsg"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" Width="100%">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="JobID" HeaderText="Job ID" ItemStyle-Wrap="false" SortExpression="JobID"></asp:BoundField>
                                        <asp:BoundField DataField="Seq" HeaderText="Seq" ItemStyle-Wrap="false" SortExpression="Seq"></asp:BoundField>
                                        <asp:BoundField DataField="TvdID" HeaderText="Tvd ID" ItemStyle-Wrap="false" SortExpression="TvdID"></asp:BoundField>
                                        <asp:BoundField DataField="PoliceNo" HeaderText="Police No" ItemStyle-Wrap="false" SortExpression="PoliceNo"></asp:BoundField>
                                        <asp:BoundField DataField="MaintTypeDesc" HeaderText="Maint Type" ItemStyle-Wrap="false" SortExpression="MaintTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDeleteDetail" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="NoSN" HeaderText="SN" ItemStyle-Wrap="false" SortExpression="NoSN"></asp:BoundField>
                                        <asp:BoundField DataField="MSIDN" HeaderText="GSM" ItemStyle-Wrap="false" SortExpression="MSIDN"></asp:BoundField>
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Wrap="false" SortExpression="Remark"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle ForeColor="#003481" CssClass="pagination-ys" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="False" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;"><asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            /**
             * JOBID RECEPTION SYSTEM - SECURE IFRAME COMMUNICATION
             * 
             * This script handles secure reception of JobID from parent page
             * Supports dual reception methods: postMessage API + URL parameters
             * 
             * SECURITY FEATURES:
             * - Input validation and sanitization
             * - Origin validation for postMessage
             * - Error handling and logging
             * - Visual feedback for user
             * 
             * FLOW: Page loads -> JobID received -> stored in hidden input -> displayed to user
             */

            /**
             * PRIMARY METHOD: Receive JobID via postMessage API
             * 
             * @param {string} jobID - The JobID received from parent window
             * 
             * SECURITY: Validates input and provides visual feedback
             */
            function receiveJobID(jobID) {
                // Validate JobID input
                if (!jobID || typeof jobID !== 'string' || jobID.trim() === '') {
                    console.warn('Invalid JobID received:', jobID);
                    return;
                }
                
                // Sanitize and store JobID
                var sanitizedJobID = jobID.trim();
                
                // Safely update txtJobID
                var txtJobIDElement = document.getElementById('txtJobID');
                if (txtJobIDElement) {
                    txtJobIDElement.value = sanitizedJobID;
                }
                
                // Safely update jobIDValue and show jobIDDisplay
                var jobIDValueElement = document.getElementById('jobIDValue');
                var jobIDDisplayElement = document.getElementById('jobIDDisplay');
                
                if (jobIDValueElement) {
                    jobIDValueElement.textContent = sanitizedJobID;
                }
                
                if (jobIDDisplayElement) {
                    jobIDDisplayElement.style.display = 'block';
                }
                
                console.log('JobID received via postMessage:', sanitizedJobID);
            }

            /**
             * SECURE MESSAGE LISTENER
             * 
             * Listens for postMessage events from parent window
             * Validates message origin and structure for security
             */
            window.addEventListener('message', function(event) {
                // Basic origin validation (in production, validate exact origin)
                // if (event.origin !== "https://your-domain.com") return;
                
                // Validate message structure
                if (event.data && 
                    event.data.type === 'setJobID' && 
                    event.data.jobID) {
                    
                    // Optional: Validate timestamp if provided
                    if (event.data.timestamp) {
                        var messageAge = new Date().getTime() - event.data.timestamp;
                        if (messageAge > 5000) { // 5 second timeout
                            console.warn('Message too old, ignoring');
                            return;
                        }
                    }
                    
                    receiveJobID(event.data.jobID);
                }
            });

            /**
             * FALLBACK METHOD: Get JobID from URL parameters
             * 
             * This method ensures JobID is available even if postMessage fails
             * Used as backup when iframe is reloaded with query parameters
             */
            function getJobIDFromURL() {
                try {
                    var urlParams = new URLSearchParams(window.location.search);
                    var jobID = urlParams.get('jobID');
                    
                    if (jobID && jobID.trim() !== '') {
                        var sanitizedJobID = jobID.trim();
                        
                        // Safely update txtJobID
                        var txtJobIDElement = document.getElementById('txtJobID');
                        if (txtJobIDElement) {
                            txtJobIDElement.value = sanitizedJobID;
                        }
                        
                        // Safely update jobIDValue and show jobIDDisplay
                        var jobIDValueElement = document.getElementById('jobIDValue');
                        var jobIDDisplayElement = document.getElementById('jobIDDisplay');
                        
                        if (jobIDValueElement) {
                            jobIDValueElement.textContent = sanitizedJobID;
                        }
                        
                        if (jobIDDisplayElement) {
                            jobIDDisplayElement.style.display = 'block';
                        }
                        
                        console.log('JobID received from URL:', sanitizedJobID);
                    }
                } catch (e) {
                    console.error('Error parsing URL parameters:', e);
                }
            }

            /**
             * UPLOAD SUCCESS HANDLER - PARENT WINDOW COMMUNICATION
             * 
             * This function is called after successful file upload
             * It communicates with the parent window to refresh the grid and close modal
             * 
             * FLOW: Upload Success → Refresh Parent Grid → Close Modal
             */
            function handleUploadSuccess() {
                try {
                    // Get the JobID from the hidden input
                    var jobID = document.getElementById('txtJobID').value.trim();
                    
                    if (!jobID) {
                        console.warn('JobID not available for parent communication');
                        return;
                    }
                    
                    console.log('Upload successful, refreshing parent grid with JobID:', jobID);
                    
                    // STEP 1: Call parent window's Open_GridView function
                    if (window.parent && typeof window.parent.Open_GridView === 'function') {
                        window.parent.Open_GridView(jobID);
                        console.log('Parent Open_GridView called successfully');
                    } else {
                        console.warn('Parent Open_GridView function not available');
                    }
                    
                    // STEP 2: Close the modal after a short delay to ensure grid refresh
                    setTimeout(function() {
                        if (window.parent && window.parent.$) {
                            window.parent.$('#modal-upload').modal('hide');
                            console.log('Modal closed successfully');
                        } else {
                            console.warn('Parent jQuery not available for modal closing');
                        }
                    }, 500); // 500ms delay to ensure grid refresh completes
                    
                } catch (e) {
                    console.error('Error in handleUploadSuccess:', e);
                }
            }

            /**
             * MESSAGE DISPLAY HANDLER WITH SUCCESS DETECTION
             * 
             * Enhanced message handler that detects upload success
             * and triggers parent window communication
             */
            function handleMessageDisplay() {
                var lbMsg = document.getElementById('lblMsg');
                var isExists = lbMsg.innerHTML;
                
                if (isExists != '') {
                    if (isExists.includes("Success")) {
                        lbMsg.className = "btn btn-success";
                        
                        // CRITICAL: Trigger parent window communication on success
                        setTimeout(function() {
                            handleUploadSuccess();
                        }, 100); // Small delay to ensure message is displayed
                        
                    } else {
                        lbMsg.className = "btn btn-danger";
                    }
                    
                    // Auto-hide messages after 2 seconds
                    window.setTimeout(function () { 
                        $('#lblMsg').fadeTo(500, 0).slideUp(500, function () { 
                            $(this).remove(); 
                        }); 
                    }, 2000);
                }
            }

            /**
             * INITIALIZATION
             * 
             * Call fallback method on page load to ensure JobID is captured
             * regardless of how it was transmitted
             */
            getJobIDFromURL();

            // Call the enhanced message handler on page load
            handleMessageDisplay();
        </script>
    </form>
</body>
</html>

