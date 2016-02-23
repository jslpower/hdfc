<%@ Page Title="客户列表_短信中心" Language="C#" MasterPageFile="~/MasterPage/Front.Master"
    AutoEventWireup="true" CodeBehind="SmsCustomerList.aspx.cs" Inherits="Web.SMS.SmsCustomerList" %>

<%@ Register Src="~/UserControl/SmsHeaderMenu.ascx" TagName="headerMenu" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <uc1:headerMenu id="cu_HeaderMenu" runat="server" TabIndex="2" />
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
                        <input type="text" class="searchinput2 inputtext" id="txtMobile" runat="server" size="12" />
                        客户分类：
                        <select name="select" runat="server" id="selCustType" class="inputselect">
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
                    <th width="8%" align="center" bgcolor="#BDDCF4">
                        全选<input type="checkbox" id="chkAll" onclick="Sms.selAll(this);" />
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
                        客户分类
                    </th>
                    <th width="18%" align="center" bgcolor="#bddcf4">
                        备注
                    </th>
                    <th width="9%" align="center" bgcolor="#bddcf4">
                        <strong>操作</strong>
                    </th>
                </tr>
                <asp:CustomRepeater runat="server" ID="rptCustomer">
                    <ItemTemplate>
                        <tr>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="checkbox" class="c1" value="<%# Eval("Id") %>" />
                                <%# itemIndex++%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("MobileNumber")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("CustomerCompanyName")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("CustomerContactName")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("ClassName")%>
                            </td>
                            <td align="left" bgcolor="#e3f1fc" class="pandl3">
                                <%# Eval("ReMark")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <a href="javascript:;" onclick="return Sms.update('<%# Eval("Id") %>')">修改</a>|<a
                                    href="javascript:;" onclick="return Sms.del('<%# Eval("Id") %>')">删除</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td align="center" bgcolor="#BDDCF4">
                                <input type="checkbox" class="c1" value="<%# Eval("Id") %>" />
                                <%# itemIndex++%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <%# Eval("MobileNumber")%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <%# Eval("CustomerCompanyName")%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <%# Eval("CustomerContactName")%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <%# Eval("ClassName")%>
                            </td>
                            <td align="left" bgcolor="#BDDCF4" class="pandl3">
                                <%# Eval("ReMark")%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <a href="javascript:;" onclick="return Sms.update('<%# Eval("Id") %>')">修改</a><a
                                    href="javascript:;" onclick="return Sms.del('<%# Eval("Id") %>')">|删除</a>
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
           Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "600px", height: "260px" });
       },
       selAll: function(tar) {
           $("input:checkbox").not($(tar)).attr("checked", $(tar).attr("checked"));

       },
       //添加
       add: function() {
           Sms.openDialog("/SMS/EditSmsCustomer.aspx", "编辑客户");
           return false;
       },
       //修改
       update: function(cid) {
           Sms.openDialog("/SMS/EditSmsCustomer.aspx?custid=" + cid, "编辑客户");
           return false;
       },
       //删除
       del: function(cid) {
           if (cid == "") {
               var cidArr = [];
               var chkAllObj = $("#chkALl");
               $(".c1:checked").each(function() {
                   cidArr.push($(this).val());
               });
               if (cidArr.length < 1) {
                   alert("请选择要删除的客户！");
                   return false;
               }
               else {
                   if (!confirm("你确定要删除所选客户信息？"))
                       return false;
                   cid = cidArr.toString();
               }
           }
           else {
               if (!confirm("你确定要删除该客户信息吗？"))
                   return false;
           }
           $.newAjax(
                      {
                          url: "/SMS/SmsCustomerList.aspx",
                          data: { method: "del", custid: cid },
                          dataType: "json",
                          cache: false,
                          type: "get",
                          success: function(result) {
                              if (result.success == "1") {
                                  alert("删除成功！");
                                  window.location = "/SMS/SmsCustomerList.aspx";
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
           var userName = encodeURIComponent($("#<%=txtUserName.ClientID%>").val());
           var companyName = encodeURIComponent($("#<%=txtCompanyName.ClientID%>").val());
           var mobile = encodeURIComponent($("#<%=txtMobile.ClientID%>").val());
           var custType = $("#<%=selCustType.ClientID%>").val();
           window.location = "/SMS/SmsCustomerList.aspx?username=" + encodeURIComponent(userName) + "&companyName=" + encodeURIComponent(companyName) + "&mobile=" + encodeURIComponent(mobile) + "&custType=" + custType;
           return false;
       }

   }
        $(document).ready(function() {
            $("#<%=txtCompanyName.ClientID %>,#<%=txtUserName.ClientID %>,#<%=txtMobile.ClientID %>").keydown(function(event) {
                if (event.keyCode == 13) {
                    Sms.search();
                }
            });
        });
    </script>

</asp:Content>
