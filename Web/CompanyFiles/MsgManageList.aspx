<%@ Page Title="公告通知管理" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="MsgManageList.aspx.cs" Inherits="Web.CompanyFiles.MsgManage" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">公告通知</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;
                        text-align: right;">
                        <b>当前您所在位置</b>>> 公司文件 >> 公告通知
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="lineCategorybox" style="height: 30px;">
        </div>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="center">
                        <asp:PlaceHolder runat="server" ID="plnAddMsg"><a href="/CompanyFiles/MsgAdd.aspx">新
                            增</a></asp:PlaceHolder>
                    </td>
                </tr>
            </table>
        </div>
        <form runat="server" id="form1">
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1" id="liststyle">
                <tr class="odd">
                    <th width="8%">
                        编号
                    </th>
                    <th width="39%">
                        标题
                    </th>
                    <th width="9%">
                        浏览数
                    </th>
                    <th width="9%">
                        发布人
                    </th>
                    <th width="17%">
                        发布时间
                    </th>
                    <th width="10%">
                        操作
                    </th>
                </tr>
                <cc2:CustomRepeater ID="rptInfo" runat="server">
                    <ItemTemplate>
                        <tr class="even" style="height:30px">
                            <td class="pandl3" align="center">
                                <%# itemIndex++%>
                            </td>
                            <td class="pandl3" align="center">
                                <a class="a_SelectMsg" data-id="<%# Eval("Id") %>" title="点击查看信息" href="javascript:void(0);">
                                    <%# Eval("Title")%></a>
                            </td>
                            <td class="pandl3" align="center">
                                <%# Eval("ClickNum")%>
                            </td>
                            <td class="pandl3" align="center">
                                <%# Eval("OperateName")%>
                            </td>
                            <td class="pandl3" align="center">
                                <%# Convert.ToDateTime(Eval("IssueTime")).ToString("yyyy-MM-dd HH:mm:ss")%>
                            </td>
                            <td class="pandl3" align="center">
                                <%# GetHandleHtml(Eval("Id"))%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="odd" style="height:30px">
                            <td class="pandl3" align="center">
                                <%# itemIndex++%>
                            </td>
                            <td class="pandl3" align="center">
                                <a class="a_SelectMsg" data-id="<%# Eval("Id") %>" title="点击查看信息" href="javascript:void(0);">
                                    <%# Eval("Title")%></a>
                            </td>
                            <td class="pandl3" align="center">
                                <%# Eval("ClickNum")%>
                            </td>
                            <td class="pandl3" align="center">
                                <%# Eval("OperateName")%>
                            </td>
                            <td class="pandl3" align="center">
                                <%# Convert.ToDateTime(Eval("IssueTime")).ToString("yyyy-MM-dd HH:mm:ss")%>
                            </td>
                            <td class="pandl3" align="center">
                                <%# GetHandleHtml(Eval("Id"))%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </cc2:CustomRepeater>
                <tr>
                    <td height="30" colspan="7" style="text-align: right" class="pageup">
                        <cc1:ExporPageInfoSelect ID="ExportPageInfo1" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        </form>
    </div>

    <script type="text/javascript">
        var InfoList =
			  {
			      //删除信息
			      del: function(cid) {
			          if (cid == "") {
			              var cidArr = [];
			              var chkAllObj = $("#chkAll");
			              $(".c1:checked").each(function() {
			                  cidArr.push($(this).val());
			              });

			              if (cidArr.length < 1) {
			                  alert("请选择要删除的信息！");
			                  return false;
			              }
			              else {
			                  if (!confirm("你确定要删除所选信息？"))
			                      return false;
			                  cid = cidArr.toString();
			              }
			          }
			          else {
			              if (!confirm("你确定要删除该信息吗？"))
			                  return false;
			          }

			          window.location = "/CompanyFiles/MsgManageList.aspx?method=del&ids=" + cid


			      },
			      selAll: function(tar) {
			          $(":checkbox").attr("checked", $(tar).attr("checked"));
			      },
			      SelectMsg: function(obj) {
			          var id = $(obj).attr("data-id");
			      }
			  }

        $(document).ready(function() {
            tableToolbar.init({ tableContainerSelector: "#liststyle" });
            $("#liststyle").find(".a_SelectMsg").each(function() {
                var Id = $(this).attr("data-id");
                var _data = { Id: Id, returnUrl: window.location.href };
                $(this).attr("href", "/CompanyFiles/NoticeDetail.aspx?" + $.param(_data));
            });
        });
			  
    </script>

</asp:Content>
