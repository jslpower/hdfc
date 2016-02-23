<%@ Page Title="城市管理" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="CityManage.aspx.cs" Inherits="Web.SystemSet.CityManage" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineprotitlebox">
        <table width="100%" cellspacing="0" cellpadding="0" border="0">
            <tbody>
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">基础设置</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        <b>当前您所在位置</b>&gt;&gt; 系统设置 &gt;&gt; 基础设置
                    </td>
                </tr>
                <tr>
                    <td height="2" bgcolor="#000000" colspan="2">
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="height: 50px;" class="lineCategorybox">
        <table cellspacing="0" cellpadding="0" border="0" class="xtnav">
            <tbody>
                <tr>
                    <td width="100" align="center" class="xtnav-on">
                        <a>城市管理</a>
                    </td>
                    <td width="100" align="center">
                        <a href="/SystemSet/LineManage.aspx">线路区域管理</a>
                    </td>
                    <td width="100" align="center">
                        <a href="/SystemSet/YinHangZhangHu.aspx">公司账户</a>
                    </td>
                    <td width="100" align="center">
                        <a href="/SystemSet/SaleArea.aspx">销售地区管理</a>
                    </td>
                    <td width="100" align="center">
                        <a href="/SystemSet/CarFlight.aspx">车次航班管理</a>
                    </td>
                   <td width="100" align="center">
                        <a href="/SystemSet/Rating.aspx">信用评级管理</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="btnbox-xt">
        <table cellspacing="0" cellpadding="0" border="0" align="left" style="margin-left: 8px;">
            <tbody>
                <tr>
                    <td width="91">
                        <a href="javascript:;" onclick="return CityManage.openDialog('ProvinceAdd.aspx','添加省份');">
                            添加省份</a>
                    </td>
                    <td width="91">
                        <a href="javascript:;" onclick="return CityManage.openDialog('CityAdd.aspx','添加省份');">
                            添加城市</a>
                    </td>
                </tr>
                <tr>
                    <td height="30" align="left" colspan="3">
                        <font color="#FF0000"><strong>注：点击地区名称可进行修改</strong></font>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <table width="99%" cellspacing="1" cellpadding="0" border="0" align="left">
        <tbody>
            <tr>
                <td width="36" height="30" bgcolor="#BDDCF4" align="center">
                    <strong>编号</strong>
                </td>
                <td bgcolor="#BDDCF4" align="center">
                    <strong>省份名称</strong>
                </td>
                <td width="20%" bgcolor="#BDDCF4" align="center">
                    <strong>城市名称</strong>
                </td>
                <%--<th width="20%" bgcolor="#BDDCF4" align="center">
                    常用城市
                </th>--%>
                <td width="20%" bgcolor="#BDDCF4" align="center">
                    <strong>操作</strong>
                </td>
            </tr>
            <%=proAndCityHtml%>
            <tr>
                <td valign="middle" height="30" bgcolor="#FFFFFF" align="center" class="pageup" colspan="5">
                    <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>

    <script type="text/javascript">
        var CityManage =
    {
        //打开弹窗
        openDialog: function(p_url, p_title, p_height) {
            Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "550px", height: p_height });
        },
        updateCity: function(cId) {
            CityManage.openDialog("/SystemSet/CityAdd.aspx?cId=" + cId, "修改城市", "150px");
            return false;
        },
        updatePro: function(pId) {
            CityManage.openDialog("/SystemSet/ProvinceAdd.aspx?proId=" + pId, "修改省份", "120px");
            return false;
        },
        //删除城市
        delCity: function(cityId, tar) {
            tableToolbar.ShowConfirmMsg("你是否确认要删除该城市？", function() {
                CityManage.doAjax("delCity", cityId);
            });

        },
        //删除省份
        delPro: function(proId, tar) {
            tableToolbar.ShowConfirmMsg("删除省份将会删除该省份下的所有城市，是否确认删除？", function() {
                CityManage.doAjax("delPro", proId);
            });

        },
        //设置为常用城市
        setCity: function(cId, tar) {
            var cityObj = $(tar);
            var ischecked = cityObj.attr("checked");
            var mess = ischecked ? "你是否要设置该城市为常用城市？" : "你是否要取消该城市为常用城市？";
            if (!confirm(mess)) {
                cityObj.attr("checked", !ischecked);
                return false;
            }
            CityManage.doAjax(cityObj.attr("isFav"), cId, cityObj);
        },
        //统一Ajax调用
        doAjax: function(doType, cid, tarp) {
            $.newAjax(
              {
                  url: "/SystemSet/CityManage.aspx",
                  data: { method: doType, id: cid },
                  dataType: "json",
                  cache: false,
                  type: "get",
                  success: function(result) {
                      if (result.success == '1') {
                          if (doType == "True" || doType == "False") {
                              if (doType == "False") { tarp.attr("isFav", "True"); tableToolbar._showMsg("已取消常用城市！"); }
                              else { tarp.attr("isFav", "False"); tableToolbar._showMsg("已设置为常用城市！"); }
                          }
                          else {
                              if (doType == "delCity" || doType == "delPro") {
                                  tableToolbar._showMsg("删除成功！", function() { window.location = "CityManage.aspx"; });
                              }
                          }
                      }
                      else {
                          tableToolbar._showMsg(result.message);
                      }
                  }
              });
        }
    }
    </script>

</asp:Content>
