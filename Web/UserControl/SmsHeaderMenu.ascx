<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmsHeaderMenu.ascx.cs"
    Inherits="Web.UserControl.SmsHeaderMenu" %>
<div class="lineprotitlebox">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="15%" nowrap="nowrap">
                <span class="lineprotitle">短信中心</span>
            </td>
            <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;"
                id="smsMap">
            </td>
        </tr>
        <tr>
            <td colspan="2" height="2" bgcolor="#000000">
            </td>
        </tr>
    </table>
</div>
<div class="lineCategorybox" style="height: 50px;">
    <table border="0" cellpadding="0" cellspacing="0" class="xtnav">
        <tr>
            <td width="100" align="center" id="smsTab1">
                <a href="/SMS/SendSms.aspx">发送短信</a>
            </td>
            <td width="100" align="center" id="smsTab3">
                <a href="/SMS/SendHistory.aspx">发送历史</a>
            </td>
            <td width="100" align="center" id="smsTab4">
                <a href="/SMS/CommonSms.aspx">常用短信</a>
            </td>
            <td width="100" align="center" id="smsTab5">
                <a href="/SMS/AccountInfo.aspx">帐户信息</a>
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript">
      $(document).ready(function() {
          $("#smsTab<%=TabIndex %>").addClass("xtnav-on");
          $("#smsMap").html("所在位置：短信中心>>" + $("#smsTab<%=TabIndex %>").find("a").html());
      });
</script>

