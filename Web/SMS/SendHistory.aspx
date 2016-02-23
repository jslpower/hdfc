<%@ Page Title="发送历史_短信中心" Language="C#" MasterPageFile="~/MasterPage/Front.Master"
    AutoEventWireup="true" CodeBehind="SendHistory.aspx.cs" Inherits="Web.SMS.SendHistory" %>

<%@ Register Src="~/UserControl/SmsHeaderMenu.ascx" TagName="headerMenu" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <uc1:headerMenu id="cu_HeaderMenu" runat="server" TabIndex="3" />
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" />
                </td>
                <td height="30" valign="top">
                    <div class="searchbox" style="height: 30px;">
                        发送时间：
                        <input class="searchinput2 inputtext" id="txtSendStartDate" runat="server" size="12" onfocus="WdatePicker();" />
                        -
                        <input type="text" class="searchinput2 inputtext" id="txtSendEndDate" runat="server" size="12"
                            onfocus="WdatePicker();" />
                        关键字：
                        <input type="text" class="searchinput2 inputtext" id="txtKeyWord" runat="server" size="20" />
                        发送状态：
                        <select id="selSendState" class="inputselect" runat="server">
                            <option value="0">请选择</option>
                            <option value="1">发送成功</option>
                            <option value="2">发送失败</option>
                        </select>
                        <a href="javascript:;" onclick="return History.search();">
                            <img src="/images/searchbtn.gif" style="vertical-align: top;" /></a>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return History.downExcel();">导出</a>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="10%" align="center" bgcolor="#BDDCF4">
                        编号
                    </th>
                    <th width="12%" align="center" bgcolor="#bddcf4">
                        <strong>发送时间</strong>
                    </th>
                    <th width="12%" align="center" bgcolor="#bddcf4">
                        号码
                    </th>
                    <th width="45%" align="center" bgcolor="#bddcf4">
                        发送内容
                    </th>
                    <th width="11%" align="center" bgcolor="#bddcf4">
                        价格
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        状态
                    </th>
                </tr>
                <asp:CustomRepeater runat="server" ID="rptSendHistory">
                    <ItemTemplate>
                        <tr>
                            <td align="center" bgcolor="#e3f1fc">
                                <%=itemIndex++ %>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Convert.ToDateTime(Eval("SendTime")).ToString("yyyy-MM-dd HH:mm") %>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("MobileNumber") %>
                            </td>
                            <td align="left" bgcolor="#e3f1fc" class="pandl3">
                                <%# Eval("SMSContent") %>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#((decimal)Eval("UseMoeny")).ToString("F2") %>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <strong><font color='<%# Eval("ReturnResult").ToString()=="0"? "#006633":"#FF0000" %>'>
                                    <%# Eval("ReturnResult").ToString()=="0"?"发送成功":"发送失败" %></font></strong>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td align="center" bgcolor="#BDDCF4">
                                <%=itemIndex++ %>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <%# Convert.ToDateTime(Eval("SendTime")).ToString("yyyy-MM-dd HH:mm") %>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <%# Eval("MobileNumber") %>
                            </td>
                            <td align="left" bgcolor="#BDDCF4" class="pandl3">
                                <%# Eval("SMSContent") %>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <%#((decimal)Eval("UseMoeny")).ToString("F2") %>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <strong><font color='<%# Eval("ReturnResult").ToString()=="0"? "#006633":"#FF0000" %>'>
                                    <%# Eval("ReturnResult").ToString()=="0"?"发送成功":"发送失败" %></font></strong>
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
        </div>
    </div>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
    var History =
   {
       //搜索
       search: function(isWhat) {
           var sKeyWord = encodeURIComponent($("#<%=txtKeyWord.ClientID%>").val());
           var sendStartDate = $("#<%=txtSendStartDate.ClientID%>").val();
           var sendEndDate = encodeURIComponent($("#<%=txtSendEndDate.ClientID%>").val());
           var sendState = $("#<%=selSendState.ClientID%>").val();
           var paras="?smsKeyword=" + sKeyWord + "&startdate=" + sendStartDate + "&enddate=" + sendEndDate + "&sendstate=" + sendState;
           
           if (isWhat && isWhat == "print") {
               window.open("/SMS/SendHistoryPrint.aspx"+paras+"&Page=<%=pageIndex %>&recordcount=<%=recordCount %>", "printHistory");
           }
           else if(isWhat&&isWhat=="downexcel")
           {
               window.location = "/SMS/SendHistory.aspx" + paras + "&method=downexcel&recordcount=<%=recordCount %>";
           }
           else {
               window.location = "/SMS/SendHistory.aspx"+paras;
           }
           return false;
       },
       //打印
       printPage: function() {
           return History.search("print");
       },
       //导出
       downExcel: function() {
          return History.search("downexcel");
       }

   }
   $(document).ready(function() {
   $("#<%=txtKeyWord.ClientID %>").keydown(function(event) {
           if (event.keyCode == 13) {
               History.search();
           }
       });
   });
    </script>

</asp:Content>
