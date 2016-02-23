<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectTour.aspx.cs" Inherits="Web.jidiaoCenter.SelectTour" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>选择团号</title>
   <link href="/Css/style.css" rel="stylesheet" type="text/css" />

    <script src="/JS/jquery-1.4.4.js" type="text/javascript"></script>

    <script src="/JS/jquery.boxy.js" type="text/javascript"></script>

    <script src="/JS/table-toolbar.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" method="get">
    <div>
        <table width="99%" align="center" cellpadding="0" cellspacing="0" bgcolor="#e9f4f9"
            style="margin: 0 auto">
            <tr>
                <td width="90%" align="left">
                    团号：
                    <input name="txtTourCode" type="text" class="inputtext formsize100" id="txtTourCode" value='<%=Request.QueryString["txtTourCode"]%>' />
                    <input type="hidden" name="callback" id="callback" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("callBack") %>" />
                    <input type="hidden" name="iframeid" id="iframeid" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeid") %>" />
                    <input type="hidden" name="pIframeID" id="pIframeID" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("pIframeID") %>" />
                    <input type="hidden" name="hideID" id="hideID" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("hideID") %>" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <div style="margin: 0 auto 0 auto; width: 99%;">
        <div id="AjaxTourList" style="width: 100%; padding-top: 10px">
        </div>
        <table cellspacing="0" cellpadding="0" border="0" align="center">
            <tbody>
                <tr>
                    <td width="76" height="40" align="center" class="tjbtn02">
                        <a href="javascript:;" id="a_btn" runat="server">选用</a>
                    </td>
                    <td width="76" height="40" align="center" class="tjbtn02">
                        <a href="javascript:;" onclick="window.parent.Boxy.getIframeDialog('<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();">
                            关闭</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</body>
</html>

<script type="text/javascript">
    var SelectTour = {
        AjaxURLg: "/jidiaoCenter/AjaxSelectTourRequest.aspx?",
        type:'<%=Request.QueryString["type"] %>',
        GetAjaxData: function(url,tourcode) {
            //AJAX 加载数据
            $("#AjaxTourList").html("<div style='width:100%; text-align:center;'><img src='/images/loadingnew.gif' border='0' align='absmiddle'/>&nbsp;正在加载,请等待....&nbsp;</div>");

            var para = {tourcode:tourcode,type:SelectTour.type, callback: $("#callback").val(), iframeId: $("#iframeid").val(), piframeId: $("#pIframeID").val(), ShowID: $("#hideID").val() };
            var url = SelectTour.AjaxURLg + "&" + $.param(para);
            $.newAjax({
                type: "Get",
                url: url,
                cache: false,
                success: function(result) {
                    $("#AjaxTourList").html(result);
                    var boxyParentWin = window.parent.Boxy.getIframeWindowByID('<%=Request.QueryString["pIframeID"] %>') || parent;

                    boxyParentWin.$('.Offers').each(function() {
                        var sourceId = $(this).parent().find("input[type='hidden']:eq(0)").val();
                        if (sourceId != null && sourceId != "") {
                            $("#AjaxTourList").find("input[type='radio'][value='" + sourceId + "']").attr("checked", "checked");
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
        }
    }

    $(function() {
        SelectTour.GetAjaxData(SelectTour.AjaxURLg,"");
        $("#txtTourCode").keyup(function(){
            SelectTour.GetAjaxData(SelectTour.AjaxURLg,$(this).val());
        });
        $("#a_btn").click(function() {
            if($("#AjaxTourList").find("input[type='radio']:checked").length>0){
                useSupplierPage.SetValue();
                useSupplierPage.SelectValue();
            }
            else{
                parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
            }
            return false;
        })
    })
    var useSupplierPage = {
        _dataObj: {},
        selectValue: "",
        selectTxt: "",
        SetValue: function() {
            var valueArray = [], txtArray = [];
            $("#AjaxTourList").find("input[type='radio']:checked").each(function() {
                valueArray.push($(this).val());
                txtArray.push($(this).attr("data-show"));
            })

            this.selectValue = valueArray.join(',');
            this.selectTxt = txtArray.join(',');
        },
        RadioClickFun: function(args) {
            var rdo = $(args);
            var data = rdo.val().split(',');
            this.selectValue = data[0];
            this.selectTxt = data[1];
            this.SelectValue();
        },
        SelectValue: function() {
            var data = {
                callBack: Boxy.queryString("callBack"),
                hideID: Boxy.queryString("hideID"),
                iframeID: Boxy.queryString("iframeId"),
                pIframeID: '<%=Request.QueryString["pIframeID"] %>'
            }
            var args = {
                tourid: useSupplierPage.selectValue,
                tourcode: useSupplierPage.selectTxt
            }
            //根据父级是否为弹窗传值
            if (data.pIframeID != "" && data.pIframeID.length > 0) {
                //定义父级弹窗
                var boxyParent = window.parent.Boxy.getIframeWindow(data.pIframeID) || window.parent.Boxy.getIframeWindowByID(data.pIframeID);
                //判断是否存在回调方法
                if (data.callBack != null && data.callBack.length > 0) {
                    if (data.callBack.indexOf('.') == -1) {
                        boxyParent[data.callBack](args);
                    }
                    else {
                        boxyParent[data.callBack.split('.')[0]][data.callBack.split('.')[1]](args);
                    }
                }
                //定义回调
            }
            else {
                //判断是否存在回调方法
                if (data.callBack != null && data.callBack.length > 0) {
                    if (data.callBack.indexOf('.') == -1) {
                        window.parent[data.callBack](args);
                    }
                    else {  
                        window.parent[data.callBack.split('.')[0]][data.callBack.split('.')[1]](args);
                    }
                }
                //定义回调
            }
            parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
        }
    }
    

</script>
