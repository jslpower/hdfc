<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmsDialog.aspx.cs" Inherits="Web.SMS.SmsDialog" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择短语</title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="700" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 20px auto;">
            <tr class="odd">
                <th width="6%" height="20" align="center">
                    <span id="Span1">序号</span>
                </th>
                <th width="17%" height="20" align="center">
                    类型
                </th>
                <th width="77%" height="20" align="center">
                    常用语
                </th>
            </tr>
            <asp:CustomRepeater ID="rptSms" runat="server">
                <ItemTemplate>
                    <tr class="odd">
                        <td width="6%" height="20" align="center" bgcolor="#E3F1FC">
                            <input class="c1" name='r1' type="radio" value='<%# Eval("Id") %>' />
                            <span id="Recommounse_ctl00_lblContentID">
                                <%#itemIndex++ %></span>
                        </td>
                        <td width="17%" height="20" align="center" bgcolor="#E3F1FC">
                            <%# Eval("ClassName") %>
                        </td>
                        <td height="20" align="left" bgcolor="#E3F1FC" class="pandl3" name="nameC">
                            <%# Eval("WordContent") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="odd">
                        <td width="6%" height="20" align="center">
                            <input class="c1" type="radio" name='r1' value='<%# Eval("Id") %> ' />
                            <span id="Recommounse_ctl00_lblContentID">
                                <%#itemIndex++ %></span>
                        </td>
                        <td width="17%" height="20" align="center">
                            <%# Eval("ClassName") %>
                        </td>
                        <td height="20" align="left" class="pandl3" name="nameC">
                            <%# Eval("WordContent") %>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:CustomRepeater>
            <tr class="odd">
                <%-- <td height="20" colspan="2" align="center" bgcolor="#E3F1FC" class="pandl3"><a href="javascript:;" onclick="return selall();">全选</a> <a href="javascript:;" onclick="return opposite();">反选</a> <a href="javascript:;" onclick="return clear1();" >清空</a></td>--%>
                <td height="20" align="right" bgcolor="#E3F1FC" class="pageup" colspan="3">
                    <uc2:ExporPageInfoSelect runat="server" ID="ExportPageInfo1" />
                </td>
            </tr>
            <tr class="odd">
                <td height="30" colspan="9" align="center">
                    <table border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="86" height="40" align="center" class="tjbtn02">
                                <a href="javascript:;" onclick="return selSms();">确认</a>
                            </td>
                            <td width="86" height="40" align="center" class="tjbtn02">
                                <a href="javascript:;" onclick="return dClose();">取消</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script src="/js/jquery-1.4.4.js" type="text/javascript"></script>

    <script type="text/javascript">
        //确认
        function selSms() {
            var smsArr = [];
            $(".c1:checked").each(function() {
                smsArr.push($.trim($(this).parent().siblings("td[name='nameC']").html()));
            });
            var smsArea = window.parent.document.getElementById("textContent");
            smsArea.value = smsArr.toString();
            window.parent.SendSms.fontNum(smsArea);
            dClose();
            return false;
        }
        //取消
        function dClose() {
            window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
            return false;
        }
        //全选
        function selall() {
            $("input:checkbox").attr("checked", true);
        }
        //清空
        function clear1() {
            $("input:checkbox").removeAttr("checked");
            return false;
        }
        //反选
        function opposite() {
            $("input:checkbox").each(function() {
                $(this).attr("checked", !$(this).attr("checked"));
            });
        }
        //
      
        
    </script>

</body>
</html>
