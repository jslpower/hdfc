<%@ Page Title="常用短语_短信中心" Language="C#" MasterPageFile="~/MasterPage/Front.Master"
    AutoEventWireup="true" CodeBehind="CommonSms.aspx.cs" Inherits="Web.SMS.CommonSms" %>

<%@ Register Src="~/UserControl/SmsHeaderMenu.ascx" TagName="headerMenu" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <uc1:headerMenu id="cu_HeaderMenu" runat="server" TabIndex="4" />
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" />
                </td>
                <td height="30" valign="top">
                    <div class="searchbox" style="height: 30px;">
                        关键字：
                        <input type="text" class="searchinput2 inputtext" id="txtKeyWord" runat="server"
                            size="18" />
                        类别：
                        <select runat="server" id="selSmsType" class="inputselect">
                        </select>
                        <a href="javascript:;" onclick="return Sms.search();">
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
                        <a href="javascript:;" onclick="return Sms.add();">新增</a>
                    </td>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Sms.del('');">删除</a>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="10%" align="center" bgcolor="#BDDCF4">
                        全选<input type="checkbox" id="chkAll" onclick="Sms.selAll(this);" />
                    </th>
                    <th width="14%" align="center" bgcolor="#bddcf4">
                        <strong>类型</strong>
                    </th>
                    <th width="64%" align="center" bgcolor="#bddcf4">
                        短信内容
                    </th>
                    <th width="12%" align="center" bgcolor="#bddcf4">
                        <strong>操作</strong>
                    </th>
                </tr>
                <asp:CustomRepeater runat="server" ID="rptSms">
                    <ItemTemplate>
                        <tr>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="checkbox" class="c1" value="<%# Eval("Id") %>" /><%=itemIndex++ %>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("ClassName") %>
                            </td>
                            <td align="left" bgcolor="#e3f1fc">
                                <%# Eval("WordContent") %>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <a href="javascript:;" onclick="return Sms.update('<%# Eval("Id") %>')">修改</a>|<a
                                    href="javascript:;" onclick="return Sms.del('<%# Eval("Id") %>')">删除</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td align="center" bgcolor="#bddcf4">
                                <input type="checkbox" class="c1" value="<%# Eval("Id") %>" /><%=itemIndex++ %>
                            </td>
                            <td align="center" bgcolor="#bddcf4">
                                <%# Eval("ClassName") %>
                            </td>
                            <td align="left" bgcolor="#bddcf4">
                                <%# Eval("WordContent") %>
                            </td>
                            <td align="center" bgcolor="#bddcf4">
                                <a href="javascript:;" onclick="return Sms.update('<%# Eval("Id") %>')">修改</a>|<a
                                    href="javascript:;" onclick="return Sms.del('<%# Eval("Id") %>')">删除</a>
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

    <script type="text/javascript">
        var Sms =
   {
       //打开弹窗
       openDialog: function(p_url, p_title) {
           Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "500px", height: "240px" });
       },
       selAll: function(tar) {
           $("input:checkbox").not($(tar)).attr("checked", $(tar).attr("checked"));

       },
       //添加
       add: function() {
           Sms.openDialog("/SMS/EditCommonSms.aspx", "编辑常用短语");
           return false;
       },
       //修改
       update: function(sid) {
           Sms.openDialog("/SMS/EditCommonSms.aspx?sid=" + sid, "编辑常用短语");
           return false;
       },
       //删除
       del: function(sid) {
           if (sid == "") {
               var sidArr = [];
               var chkAllObj = $("#chkALl");
               $(".c1:checked").each(function() {
                   sidArr.push($(this).val());
               });
               if (sidArr.length < 1) {
                   alert("请选择要删除的短语！");
                   return false;
               }
               else {
                   if (!confirm("你确定要删除所选短语信息？"))
                       return false;
                   sid = sidArr.toString();
               }
           }
           else {
               if (!confirm("你确定要删除该短语信息吗？"))
                   return false;
           }
           $.newAjax(
                      {
                          url: "/SMS/CommonSms.aspx",
                          data: { method: "del", cid: sid },
                          dataType: "json",
                          cache: false,
                          type: "get",
                          success: function(result) {
                              if (result.success == "1") {
                                  alert("删除成功！");
                                  window.location = "/SMS/CommonSms.aspx";
                              }
                              else {
                                  alert("删除失败！");
                              }
                          },
                          error: function() {
                              alert("操作失败!");
                          }
                      })

       },
       //搜索
       search: function() {
           var sKeyWord = encodeURIComponent($("#<%=txtKeyWord.ClientID%>").val());
           var smsType = $("#<%=selSmsType.ClientID%>").val();
           window.location = "/SMS/CommonSms.aspx?smsKeyword=" + sKeyWord + "&smstype=" + smsType;
           return false;
       }

   }
        $(document).ready(function() {
            $("#<%=txtKeyWord.ClientID %>").keydown(function(event) {
                if (event.keyCode == 13) {
                    Sms.search();
                }
            });
        });
    </script>

</asp:Content>
