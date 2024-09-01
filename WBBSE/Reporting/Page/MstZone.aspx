<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MstZone.aspx.cs" Inherits="WBBSE.Reporting.Page.MstZone" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>WBBSE Admin :: Zone Report</title>
    <link href="../../vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../vendor/fontawesome-free/css/all.min.css" rel="stylesheet" />
    <script src="../../vendor/jquery/jquery.min.js"></script>
    <script src="../../vendor/bootstrap/js/bootstrap.min.js"></script>
    <script src="../../vendor/fontawesome-free/js/all.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" SizeToReportContent="true"></rsweb:ReportViewer>
        </div>
        <section id="message" class="mt-4" runat="server" visible="false">
            <div class="container">
                <div class="alert alert-danger" style="font-size:20px;">
                    No Record Found <strong><i class="fa fa-exclamation"></i></strong>
                </div>
            </div>
        </section>
    </form>
</body>
</html>

