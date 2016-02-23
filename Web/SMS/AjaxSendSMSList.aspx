<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxSendSMSList.aspx.cs"
    Inherits="Web.SMS.AjaxSendSMSList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<table width="99%" border="0" cellspacing="1" cellpadding="0">
    <tr class="odd">
        <th width="6%" height="30" align="center">
            编号
        </th>
        <th width="15%" align="center">
            发送范围
        </th>
        <th width="15%" align="center">
            发送内容
        </th>
        <th width="20%" align="center">
            发送设置
        </th>
        <th width="15%" align="center">
            操作
        </th>
    </tr>
    <asp:customrepeater id="rptSmsList" runat="server">
                  <ItemTemplate>
                  <tr class="even">
                    <td height="30" align="center"><%# itemIndex++%></td>
                    <td align="center"><%# GetSendInfo((bool)Eval("IsMatchCustomerInfo"),(bool)Eval("IsMatchSupplierInfo"),(bool)Eval("IsMatchDepartmentInfo"), Eval("MobileCode")) %></td>
                    <td align="center"  ><div  style='word-wrap:break-word;width:200px;overflow:hidden; padding-left:3px;'><%# Eval("Content") %> </div> </td>
                    <td align="center"><%# Convert.ToInt32(Eval("FixType")) == 0 ? "固定时间发送 "+((DateTime?)Eval("Time")).Value.ToString("yyyy-MM-dd HH:mm"): Eval("FixType")%></td>
                    <td align="center"><a href="CustomerCare.aspx?sid=<%# Eval("Id") %>"><font class="fblue">修改</font></a> <a href="javascript:;" onclick="return Care.del('<%# Eval("Id") %>')"><font class="fblue">删除</font></a> <a href="javascript:;" onclick="return Care.stop('<%# Eval("Id") %>',this);"><font class="fblue"><%# !(bool)Eval("IsEnabled")?"启用":"停发" %></font></a></td>
                  </tr>
                  </ItemTemplate>
                   <AlternatingItemTemplate>
                  <tr class="odd">
                    <td height="30" align="center"><%# itemIndex++%></td>
                    <td align="center"><%# GetSendInfo((bool)Eval("IsMatchCustomerInfo"),(bool)Eval("IsMatchSupplierInfo"),(bool)Eval("IsMatchDepartmentInfo"), Eval("MobileCode"))%></td>
                    <td align="center"><div  style='word-wrap:break-word;width:200px;overflow:hidden; padding-left:3px;'><%# Eval("Content") %> </div></td>
                    <td align="center"><%# Convert.ToInt32(Eval("FixType")) == 0 ? "固定时间发送 "+((DateTime?)Eval("Time")).Value.ToString("yyyy-MM-dd HH:mm") : Eval("FixType")%></td>
                    <td align="center"><a href="CustomerCare.aspx?sid=<%# Eval("Id") %>"><font class="fblue">修改</font></a> <a href="javascript:;" onclick="return Care.del('<%# Eval("Id") %>')"><font class="fblue">删除</font></a> <a href="javascript:;" onclick="return Care.stop('<%# Eval("Id") %>',this);"><font class="fblue"><%# !(bool)Eval("IsEnabled")?"启用":"停发" %></font></a></td>
                  </tr>
                  </AlternatingItemTemplate>
                  </asp:customrepeater>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td align="right">
            <uc2:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" PageStyleType="NewButton"
                LinkType="2" HrefType="JsHref"></uc2:ExporPageInfoSelect>
        </td>
    </tr>
</table>
