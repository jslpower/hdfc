<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerDialog.aspx.cs"
    Inherits="Web.SMS.CustomerDialog" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        //搜索
        function searchCust() {
            var userName = encodeURIComponent($("#<%=txtUserName.ClientID%>").val());
            var companyName = encodeURIComponent($("#<%=txtCompanyName.ClientID%>").val());
            var mobile = encodeURIComponent($("#<%=txtMobile.ClientID%>").val());

            window.location = '/SMS/CustomerDialog.aspx?iframeId=<%=Request.QueryString["iframeId"] %>&txtMobiles=<%= EyouSoft.Common.Utils.GetQueryStringValue("txtMobiles") %>&username=' + userName + "&companyName=" + companyName + "&mobile=" + mobile;
            return false;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" />
                </td>
                <td height="30" valign="top">
                    <div class="searchbox" style="height: 30px;">
                        单位名称：
                        <input type="text" class="searchinput2 inputtext" id="txtCompanyName" runat="server"
                            size="18" />
                        姓名：
                        <input type="text" class="searchinput2 inputtext" id="txtUserName" size="12" runat="server" />
                        手机：
                        <input type="text" class="searchinput2 inputtext" id="txtMobile" runat="server" size="12" />&nbsp;
                        <a href="javascript:;" onclick="return searchCust();">
                            <img src="/images/searchbtn.gif" style="vertical-align: top;" /></a>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="8%" align="center" bgcolor="#BDDCF4">
                        全选<input type="checkbox" id="chkAll" onclick="return selAll(this);" />
                    </th>
                    <th width="11%" align="center" bgcolor="#bddcf4">
                        <strong>手机号码</strong>
                    </th>
                    <th width="23%" align="center" bgcolor="#bddcf4">
                        单位名称
                    </th>
                    <th width="13%" align="center" bgcolor="#bddcf4">
                        姓名
                    </th>
                    <th width="18%" align="center" bgcolor="#bddcf4">
                        地址
                    </th>
                    <th width="18%" align="center" bgcolor="#bddcf4">
                        备注
                    </th>
                </tr>
                <asp:CustomRepeater runat="server" ID="rptCustomer">
                    <ItemTemplate>
                        <tr>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="checkbox" class="c1" value='<%#  Eval("Mobile") %>' />
                                <%# itemIndex++%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("Mobile")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("CustomerName")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("ContactName")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("Address")%>
                            </td>
                            <td align="left" bgcolor="#e3f1fc" class="pandl3">
                                <%# Eval("Remark")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td align="center" bgcolor="#BDDCF4">
                                <input type="checkbox" class="c1" value='<%#  Eval("Mobile") %>' />
                                <%# itemIndex++%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <%# Eval("Mobile")%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <%# Eval("CustomerName")%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <%# Eval("ContactName")%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <%# Eval("Address")%>
                            </td>
                            <td align="left" bgcolor="#BDDCF4" class="pandl3">
                                <%# Eval("Remark")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:CustomRepeater>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="right" class="pageup">
                        <uc2:ExporPageInfoSelect runat="server" ID="ExportPageInfo1" />
                    </td>
                </tr>
            </table>
            <table border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="78" height="40" align="center" class="tjbtn02">
                    </td>
                    <td width="86" height="40" align="center" class="tjbtn02">
                        <a href="javascript:;" onclick="return SendSure();">确认</a>
                    </td>
                    <td width="86" height="40" align="center" class="tjbtn02">
                        <a href="javascript:;" onclick="return DiaClose();">取消</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>

    <script src="/js/jquery-1.4.4.js" type="text/javascript"></script>

    <script type="text/javascript">
        //发送所有
        function SendAll() {
            window.parent.document.getElementById("sendAll").value = "SendAll";
            DiaClose();
        }
        //全选
        function selAll(tar) {
            var isChk = $(tar).attr("checked");
            $(".c1:checkbox").attr("checked", isChk);
        }
        //发送选中
        function SendSure() {
            var cidArr = [];
            $(".c1:checked").each(function() {
                var mobile = $.trim($(this).val());
                if (mobile != "") {
                    cidArr.push(mobile);
                }
            });
            var mobileText = window.parent.document.getElementById('<%= EyouSoft.Common.Utils.GetQueryStringValue("txtMobiles") %>');
            if (cidArr.length > 0) {
                mobileText.value = cidArr.toString();
            }
            DiaClose();
        }
        //关闭
        function DiaClose() {
            window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()
        }
         
    </script>

</body>
</html>
