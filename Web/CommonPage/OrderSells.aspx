<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderSells.aspx.cs" Inherits="Web.CommonPage.OrderSells" %>

<%@ Import Namespace="EyouSoft.Model.CompanyStructure" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Css/style.css" rel="stylesheet" type="text/css" />
    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />

    <script src="/Js/jquery-1.4.4.js" type="text/javascript"></script>

    <style type="text/css">
        #tblList
        {
            border-collapse: collapse;
        }
        #tblList td
        {
            border: 1px #b8c5ce solid;
            padding: 0 3px;
            width: 25%;
        }
    </style>
</head>
<body style="background: #e9f4f9;">
    <div class="alertbox-outbox02">
        <table width="99%" cellspacing="0" cellpadding="0" class="alertboxbk1" border="0"
            style="margin: 0 auto; border-collapse: collapse;">
            <tbody>
                <tr>
                    <td height="30">
                        姓名：
                        <input type="text" id="userName" name="userName" class="inputtext formsize80" value='<%=Request.QueryString["userName"] %>' />
                        <%if (EyouSoft.Common.Utils.GetQueryStringValue("sModel") != "1")
                          { %>
                        &nbsp;&nbsp;已选择：<span id="spanSelectMore"></span>
                        <%} %>
                    </td>
                </tr>
                <tr>
                    <td>
                       <div id="DivSellerList"></div>
                       
                    </td>
                </tr>
            </tbody>
        </table>
        <table cellspacing="0" cellpadding="0" border="0" align="center">
            <tbody>
                <tr>
                    <td width="76" height="40" align="center" class="tjbtn02">
                        <a href="javascript:;" id="a_btn">选用</a>
                    </td>
                    <td width="76" height="40" align="center" class="tjbtn02">
                        <a href="javascript:;" onclick="window.parent.Boxy.getIframeDialog('<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();">
                            关闭</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <script type="text/javascript">
        var OrderSellsPage = {
            selectValue: "",
            selectTxt: "",
            deptID: "",
            deptName: "",
            ContactTel:"",
            aid: '<%=Request.QueryString["id"] %>',
            parentWindow: null, //要赋值的页面的window对象
            iframeID: '<%=Request.QueryString["iframeId"]%>', //当前弹窗ID
            pIframeID: '<%=Request.QueryString["pIframeId"]%>', //父级弹窗ID
            SetValue: function() {
                var valueArray = new Array();
                var txtArray = new Array();
                var deptIdArray = new Array();
                var deptNameArray = new Array();
                var TelArray=new Array();
                if ('<%=Request.QueryString["sModel"]%>' != '1') {
                    $("#spanSelectMore").find("input[name='contactID']:checked").each(function() {
                        valueArray.push($.trim($(this).val()));
                        txtArray.push($.trim($(this).next().html()));
                        deptIdArray.push($(this).next().attr("data-deptID"));
                        deptNameArray.push($(this).next().attr("data-deptName"));
                        TelArray.push($(this).next().attr("data-tel"));
                    })
                }
                else {
                    $("#DivSellerList").find("input[name='contactID']:checked").each(function() {
                        valueArray.push($.trim($(this).val()));
                        txtArray.push($.trim($(this).next().html()));
                        deptIdArray.push($(this).next().attr("data-deptID"));
                        deptNameArray.push($(this).next().attr("data-deptName"));
                        TelArray.push($(this).next().attr("data-tel"));
                    })
                }
                OrderSellsPage.selectValue = valueArray.join(',');
                OrderSellsPage.selectTxt = txtArray.join(',');
                OrderSellsPage.deptID = deptIdArray.join(',');
                OrderSellsPage.deptName = deptNameArray.join(',');
                OrderSellsPage.ContactTel=TelArray.join(',');
            },
            AjaxUrl:"/jidiaoCenter/AjaxSelerRequest.aspx?",
            GetAjaxData: function(url) {
                //AJAX 加载数据
                $("#DivSellerList").html("<div style='width:100%; text-align:center;'><img src='/images/loadingnew.gif' border='0' align='absmiddle'/>&nbsp;正在加载,请等待....&nbsp;</div>");
                $.newAjax({
                    type: "Get",
                    url: url,
                    cache: false,
                    success: function(result) {
                        $("#DivSellerList").html(result);
                        var boxyParentWin = window.parent.Boxy.getIframeWindowByID('<%=Request.QueryString["pIframeID"] %>') || parent;
                        boxyParentWin.$('.Offers').each(function() {
                            var sourceId = $(this).parent().find("input[type='hidden']:eq(0)").val();
                            if (sourceId != null && sourceId != "") {
                                $("#DivSellerList").find("input[type='radio'][value='" + sourceId + "']").attr("checked", "checked");
                                return false;
                            }
                        })
                        var data = {
                            id: '<%=EyouSoft.Common.Utils.GetQueryStringValue("hideId") %>'
                        }
                        //报账页面 团队支出 选用 2次选中功能
                        //选中单选钮
                        $(":radio[value='" + data.id + "']").attr("checked", "checked");
                    }
                });
            },
            SearchFun: function(key) {
                var data = { id: '<%=Request.QueryString["id"] %>', iframeId: '<%=Request.QueryString["iframeId"]%>', pIframeId: '<%=Request.QueryString["pIframeId"]%>', callBackFun: '<%=Request.QueryString["callBackFun"] %>', sModel: '<%=Request.QueryString["sModel"]%>', userName: '',aid:'<%=Request.QueryString["aid"] %>'};
                data.userName = key;
                OrderSellsPage.GetAjaxData(OrderSellsPage.AjaxUrl+$.param(data));
            },
            BtnBind: function() {
                $("#a_btn").click(function() {
                    OrderSellsPage.SetValue();

                    var data = { id: '<%=Request.QueryString["id"] %>', value: OrderSellsPage.selectValue, text: OrderSellsPage.selectTxt, hide: '<%=Request.QueryString["hide"] %>', show: '<%=Request.QueryString["show"] %>', deptID: OrderSellsPage.deptID, deptName: OrderSellsPage.deptName,aid: '<%=Request.QueryString["aid"] %>',tel:OrderSellsPage.ContactTel };
                    //根据父级是否为弹窗传值
                    var func = '<%=Request.QueryString["callBackFun"] %>';
                    var piframeId='<%=Request.QueryString["pIframeID"] %>';
                   var boxyParent = window.parent.Boxy.getIframeWindow(piframeId) || window.parent.Boxy.getIframeWindowByID(piframeId);
                    if (func.indexOf('.') == -1) {
                        boxyParent[func](data);
                    } else {
                        boxyParent[func.split('.')[0]][func.split('.')[1]](data);
                    }
                    parent.Boxy.getIframeDialog(OrderSellsPage.iframeID).hide();
                    return false;
                })
              
            }
        }

        $(function() {
             //获得需要赋值页面的window 对象
            if (OrderSellsPage.pIframeID) {
                OrderSellsPage.parentWindow = window.parent.Boxy.getIframeWindow(OrderSellsPage.pIframeID) || window.parent.Boxy.getIframeWindowByID(OrderSellsPage.pIframeID);
            }
            else {
                OrderSellsPage.parentWindow = parent.window;
            }
            $("#userName").focus();
            $("#userName").keyup(function(){
                OrderSellsPage.SearchFun($(this).val());
            })
            //初始化绑定事件
            OrderSellsPage.BtnBind();
        }) 
    </script>

</body>
</html>
