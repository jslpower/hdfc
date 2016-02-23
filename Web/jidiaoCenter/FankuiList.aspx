<%@ Page Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="FankuiList.aspx.cs" Inherits="Web.jidiaoCenter.FankuiList" Title="团队质量反馈" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">计调中心</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            <b>当前您所在位置：</b> &gt;&gt; 计调中心 &gt;&gt; 团队质量反馈
                        </td>
                    </tr>
                    <tr>
                        <td height="2" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
            <tbody>
                <tr>
                    <td width="10" valign="top">
                        <img src="../images/yuanleft.gif">
                    </td>
                    <td>
                        <form method="get" id="form1">
                        <div class="searchbox">
                            团号：
                            <input type="text" size="20" id="tourcode" class="inputtext formsize120" name="tourcode"
                                value='<%=Request.QueryString["tourcode"] %>' />
                            出团时间：
                            <input type="text" size="12" id="starttime" onfocus="WdatePicker()" class="inputtext formsize70"
                                name="starttime" value='<%=Request.QueryString["starttime"] %>' value='<%=Request.QueryString["starttime"] %>' />
                            -
                            <input type="text" size="12" id="endtime" onfocus="WdatePicker()" class="inputtext formsize70"
                                name="endtime" value='<%=Request.QueryString["endtime"] %>' value='<%=Request.QueryString["endtime"] %>' />
                            是否已访：
                            <select name="sltvisit" id="sltvisit" class="inputselect">
                                <option value="-1">请选择</option>
                                <option value="1" <%=Request.QueryString["sltvisit"]=="1"?"selected='selected'":"" %>>是</option>
                                <option value="0" <%=Request.QueryString["sltvisit"]=="0"?"selected='selected'":"" %>>否</option>
                            </select>
                            <input type="submit" value="" class="search-btn" />
                        </div>
                        </form>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif" alt="" />
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="tablelist">
            <table width="100%" cellspacing="1" cellpadding="0" border="0" id="liststyle">
                <tbody>
                    <tr>
                        <th width="36" height="30" align="center">
                            序号
                        </th>
                        <th align="center">
                            团号
                        </th>
                        <th align="center">
                            线路名称
                        </th>
                        <th align="center">
                            出团日期
                        </th>
                        <th align="center">
                            人数
                        </th>
                         <th align="center">
                            地接名称
                        </th>
                        <th align="center">
                            地接导游
                        </th>
                        <th align="center">
                            回访次数
                        </th>
                        <th align="center">
                            质量评估
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptList">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "even" : "odd"%>">
                                <td height="30" align="center">
                                    <%#Container.ItemIndex+1 %>
                                </td>
  
                                <td align="center">
                                    <%#Eval("TourCode")%>&nbsp;<%#GetTourStatus(Eval("TourStatus"))%>
                                </td>
                                <td align="center">
                                    <%#Eval("RouteName")%>
                                </td>
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.GetDateTime(Eval("LDate").ToString()).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <%#Eval("Adults")%>+<%#Eval("Childs")%>+<%#Eval("Accompanys")%>
                                </td>
                                <td align="center">
                                    <span style="display: none;">
                                        <%#GetDijieInfo(Eval("DiJieList"))%>
                                    </span><a class="paopao" data-width="340" href="javascript:;" bt-xtitle="" title="">
                                        <%#Eval("DiJieName")%></a>
                                </td>
                                <td align="center">
                                    <span style="display: none;">
                                        <%#GetGuideInfo(Eval("GuideList"))%>
                                    </span><a class="paopao" data-width="200" href="javascript:;" bt-xtitle="" title="">
                                        <%#Eval("GuideName")%></a>
                                </td>
                                <td align="center">
                                    <a class="toolbar_add" href="javascript:;" data-id='<%#Eval("TourId") %>'><%#Convert.ToInt32(Eval("VisitNum"))==0?"未回访":"回访"+Eval("VisitNum").ToString()+"次"%></a>
                                </td>
                                <td align="center">
                                    <a class="pinggu" data-id='<%#Eval("TourId") %>' data-score='<%#(int)((EyouSoft.Model.EnumType.TourStructure.Score)Eval("Score")) %>'
                                        href="javascript:;"><span class="pandl3">
                                            <%#GetScore(Eval("Score"))%></span></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Literal ID="lbemptymsg" runat="server"></asp:Literal>
                </tbody>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td align="right">
                            <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        $(function() {
           $("#liststyle").find("a[class='paopao']").each(function(){
                 $(this).bt({
                    contentSelector: function() { 
                    return $(this).prev("span").html();
                    },
                    positions: ['bottom'],
                    fill: '#effaff',
                    strokeStyle: '#2a9cd4',
                    noShadowOpts: { strokeStyle: "#2a9cd4" },
                    spikeLength: 5,
                    spikeGirth: 15,
                    overlap: 0,
                    width:$(this).attr("data-width"),
                    centerPointY: 4,
                    cornerRadius: 4,
                    shadow: true,
                    shadowColor: 'rgba(0,0,0,.5)',
                    cssStyles: { color: '#1351a0', 'line-height': '200%' }
                });
            })
            VisitList.BindBtn();
        });
        var VisitList = {
            reload: function() {
                window.location.href = window.location.href;
                return false;
            },
            //添加
            Add: function(tourid) {
                VisitList.ShowBoxy({ iframeUrl: "/jidiaoCenter/VisitEdit.aspx?tourid="+tourid, title: "回访记录", width: "830px", height: "400px" });
            },
            Score:function(score,tourid){
                VisitList.ShowBoxy({ iframeUrl: "/jidiaoCenter/PingGu.aspx?score="+score+"&tourid="+tourid, title: "评估", width: "520px", height: "150px" });
            },
            //显示弹窗
            ShowBoxy: function(data) {
                Boxy.iframeDialog({
                    iframeUrl: data.iframeUrl,
                    title: data.title,
                    modal: true,
                    width: data.width,
                    height: data.height,
                    afterHide: function() { VisitList.reload(); }
                });
            },
            BindBtn: function() {
                //绑定Add事件
                $(".toolbar_add").click(function() {
                    var tourid=$(this).attr("data-id");
                    VisitList.Add(tourid);
                    return false;
                });
                $(".pinggu").click(function(){
                    var score=$(this).attr("data-score");
                    var tourid=$(this).attr("data-id");
                    VisitList.Score(score,tourid);
                })
                tableToolbar.init({
                    tableContainerSelector: "#liststyle", //表格选择器
                    objectName: "行"
                });
            }
        };
    </script>

</asp:Content>
