<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="po_add_details_pojo.aspx.cs" Inherits="vtsadm.po_add_details_pojo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add PO Detail</title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <link rel="stylesheet" href="Content/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="Content/bower_components/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="Content/dist/css/AdminLTE.min.css" />

    <style type="text/css">
        html, body {
            background: #fff;
            margin: 0;
            padding: 0;
            overflow: auto;
        }

        .detail-form {
            padding: 16px 18px 12px;
        }

        .po-summary {
            background: #f7f9fc;
            border: 1px solid #e8ecf1;
            border-radius: 3px;
            color: #555;
            font-size: 12px;
            margin-bottom: 14px;
            padding: 8px 12px;
        }

        .po-summary strong {
            color: #003481;
            font-size: 13px;
            margin-left: 4px;
        }

        .form-group {
            margin-bottom: 12px;
        }

        .form-group label {
            color: #444;
            font-size: 12px;
            font-weight: 600;
            margin-bottom: 4px;
        }

        .form-control {
            border-color: #d2d6de;
            border-radius: 3px;
            box-shadow: none;
            height: 32px;
        }

        .form-control:focus {
            border-color: #003481;
        }

        .form-actions {
            border-top: 1px solid #f0f0f0;
            margin-top: 4px;
            padding-top: 12px;
            text-align: right;
        }

        .form-actions .btn {
            min-width: 82px;
        }

        .form-actions .btn + .btn {
            margin-left: 6px;
        }

        .alert-box {
            margin-bottom: 0;
            margin-top: 10px;
        }

        .alert-box:empty {
            display: none;
        }
    </style>

    <script type="text/javascript" src="Content/bower_components/jquery/dist/jquery.min.js"></script>
    <script type="text/javascript" src="Content/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" runat="server" id="txtCustID" />
        <input type="hidden" runat="server" id="txtPoID" />

        <div class="detail-form">
            <div class="po-summary">
                PO ID : <strong><span id="lblPoIDDisplay" runat="server">-</span></strong>
            </div>

            <div class="row">
                <div class="col-xs-4">
                    <div class="form-group">
                        <label>Device Group</label>
                        <asp:DropDownList ID="CmbDeviceGroupID" AutoPostBack="true" OnSelectedIndexChanged="CmbDeviceGroupID_TextChanged" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-xs-4">
                    <div class="form-group">
                        <label>Device Type</label>
                        <asp:DropDownList ID="CmbDeviceTypeID" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-xs-4">
                    <div class="form-group">
                        <label>Quantity</label>
                        <input type="text" id="txtQuantity" runat="server" class="form-control input-sm" placeholder="Qty ..." onkeypress="return event.charCode >= 48 && event.charCode <= 57" />
                    </div>
                </div>
            </div>

            <div id="lblMsg" runat="server" class="alert-box"></div>

            <div class="form-actions">
                <asp:Button ID="Button1" CssClass="btn btn-default btn-sm" runat="server" OnClick="CmdClearDetail_Click" Text="Reset" />
                <asp:Button ID="Button2" CssClass="btn btn-primary btn-sm" runat="server" OnClick="CmdSaveDetail_Click" Text="Save" />
            </div>
        </div>

        <div class="modal modal-open fade" id="modal-messagebox" style="display: none;">
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

        <script type="text/javascript">
            (function () {
                var lbMsg = document.getElementById('lblMsg');
                if (!lbMsg || lbMsg.innerHTML.trim() === '') {
                    return;
                }

                if (lbMsg.innerHTML.indexOf('Success') >= 0) {
                    lbMsg.className = 'alert-box alert alert-success alert-dismissible';
                    lbMsg.innerHTML = '<button type="button" class="close" data-dismiss="alert"><span>&times;</span></button>' + lbMsg.innerHTML;
                } else {
                    lbMsg.className = 'alert-box alert alert-danger alert-dismissible';
                    lbMsg.innerHTML = '<button type="button" class="close" data-dismiss="alert"><span>&times;</span></button>' + lbMsg.innerHTML;
                }

                window.setTimeout(function () {
                    $(lbMsg).fadeTo(500, 0, function () {
                        $(this).slideUp(300, function () {
                            this.innerHTML = '';
                            this.className = 'alert-box';
                            $(this).show().css('opacity', 1);
                        });
                    });
                }, 3500);
            })();
        </script>
    </form>
</body>
</html>
