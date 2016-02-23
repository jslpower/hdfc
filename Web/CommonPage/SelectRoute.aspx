<%@ Page Title="线路选用页面" Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="SelectRoute.aspx.cs" Inherits="Web.CommonPage.SelectRoute" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <div class="mainbody">
        <table width="1000" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" alt="" />
                </td>
                <td>
                    <div class="searchbox">
                        线路名称：
                        <input name="txt_xianluName" type="text" id="txt_xianluName" class="inputtext searchinput"
                            value="<%= EyouSoft.Common.Utils.GetQueryStringValue("rname") %>" />
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" alt="" />
                </td>
            </tr>
        </table>
        <div id="AjaxDiv"></div>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:void(0);" id="selectxl">选用线路</a>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">

        var SelectRoutePage = {
            _data: {
                rid: "",    //线路Id
                rname: ""   //线路名称
            },
            AjaxUrl:"/jidiaoCenter/AjaxRouteRequest.aspx?",
            GetAjaxData: function(url) {
                //AJAX 加载数据
                $("#AjaxDiv").html("<div style='width:100%; text-align:center;'><img src='/images/loadingnew.gif' border='0' align='absmiddle'/>&nbsp;正在加载,请等待....&nbsp;</div>");
                $.newAjax({
                    type: "Get",
                    url: url,
                    cache: false,
                    success: function(result) {
                        $("#AjaxDiv").html(result);
                        var boxyParentWin = window.parent.Boxy.getIframeWindowByID('<%=Request.QueryString["pIframeID"] %>') || parent;
                        boxyParentWin.$('.Offers').each(function() {
                            var sourceId = $(this).parent().find("input[type='hidden']:eq(0)").val();
                            if (sourceId != null && sourceId != "") {
                                $("#AjaxDiv").find("input[type='radio'][value='" + sourceId + "']").attr("checked", "checked");
                                return false;
                            }

                        })
                        var data = {
                            id: '<%=EyouSoft.Common.Utils.GetQueryStringValue("hideID") %>'
                        }
                        //报账页面 团队支出 选用 2次选中功能
                        //选中单选钮
                        $(":radio[value='" + data.id + "']").attr("checked", "checked");
                    }
                });
            },
            _rdata: [], //返回给父页面的对象 格式为 _data 对象的集合,
            SetValue: function() {
                var str = "";
                var index = 0;
                if ("<%= IsSelectMore %>" == "1") {//多选
                    $("#AjaxDiv").find("input[type='checkbox']:checked").each(function() {
                        var tdata = {};
                        tdata.rid = $(this).val();
                        tdata.rname = $(this).attr("data-name")

                        SelectRoutePage._rdata.push(tdata);
                    })
                }
                else {//单选
                    $("#AjaxDiv").find("input[type='radio']:checked").each(function() {
                        var tdata = {};
                        tdata.rid = $(this).val();
                        tdata.rname = $(this).attr("data-name")

                        SelectRoutePage._rdata.push(tdata);
                    })
                }
            },
            SelectValue: function() {

                var callBack = '<%= EyouSoft.Common.Utils.GetQueryStringValue("callBack") %>';
                var pIframeID = '<%= EyouSoft.Common.Utils.GetQueryStringValue("pIframeId") %>';
                //根据父级是否为弹窗传值
                if (pIframeID != "" && pIframeID.length > 0) {
                    
                    //定义父级弹窗
                    var boxyParent = window.parent.Boxy.getIframeWindow(pIframeID) || window.parent.Boxy.getIframeWindowByID(pIframeID);
                    //判断是否存在回调方法
                    if (callBack != null && callBack.length > 0) {
                        if (callBack.indexOf('.') == -1) {
                            boxyParent[callBack](SelectRoutePage._rdata);
                        }
                        else {
                            boxyParent[callBack.split('.')[0]][callBack.split('.')[1]](SelectRoutePage._rdata);
                        }
                    }
                    //定义回调
                }
                else {
                    //判断是否存在回调方法
                    if (callBack != null && callBack.length > 0) {
                        
                        if (callBack.indexOf('.') == -1) {
                            window.parent[callBack](SelectRoutePage._rdata);
                        }
                        else {
                             
                            window.parent[callBack.split('.')[0]][callBack.split('.')[1]](SelectRoutePage._rdata);
                        }
                    }
                    //定义回调
                }
                parent.Boxy.getIframeDialog('<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
            },
            search: function() {
                var data = {};
                data.rname = $("#txt_xianluName").val();
                data.iframeId = '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>';
                //父页面传过来的参数 也要带上 
                data.initId = '<%= EyouSoft.Common.Utils.GetQueryStringValue("initId") %>';
                data.callBack = '<%= EyouSoft.Common.Utils.GetQueryStringValue("callBack") %>';
                data.pIframeId = '<%= EyouSoft.Common.Utils.GetQueryStringValue("pIframeId") %>';
                data.isMore = '<%= EyouSoft.Common.Utils.GetQueryStringValue("isMore") %>';
                SelectRoutePage.GetAjaxData(SelectRoutePage.AjaxUrl+$.param(data));
            }
        };

        $(document).ready(function() {
            $("#txt_xianluName").keyup(function() {
                SelectRoutePage.search();
                return false;
            });
            $("#selectxl").click(function() {
                SelectRoutePage.SetValue();
                SelectRoutePage.SelectValue();
                return false;
            });
        });        
    </script>

</asp:Content>
