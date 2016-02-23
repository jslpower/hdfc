<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendDetailDialog.aspx.cs"
    Inherits="Web.SMS.SendDetailDialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发送详情</title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <style>
        table
        {
            margin: auto;
            margin-top: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <%=detailMess%>
    </div>
    </form>
</body>
</html>
