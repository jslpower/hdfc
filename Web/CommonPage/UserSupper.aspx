<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSupper.aspx.cs" Inherits="Web.CommonPage.UserSupper" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>供应商选用</title>
    <link href="/Css/style.css" rel="stylesheet" type="text/css" />

    <script src="/JS/jquery-1.4.4.js" type="text/javascript"></script>

    <script src="/JS/bt.min.js" type="text/javascript"></script>

    <script src="/JS/jquery.boxy.js" type="text/javascript"></script>

    <script src="/JS/table-toolbar.js" type="text/javascript"></script>

    <script src="/JS/ValiDatorForm.js" type="text/javascript"></script>

    <!--[if IE]><script src="/js/excanvas.js" type="text/javascript" charset="utf-8"></script><![endif]-->
</head>
<body>
    <form id="form1" method="get">
    <div>
        <table width="99%" align="center" cellpadding="0" cellspacing="0" bgcolor="#e9f4f9"
            style="margin: 0 auto">
            <tr>
                <td width="90%" align="left">
                    <%=type.ToString()%>名称：
                    <input name="txtName" type="text" class="inputtext formsize100" id="txtName" value='<%=Request.QueryString["txtName"]%>' />
                    <select name="slttype" id="slttype" class="inputselect" style="display: none">
                        <%=strsuppertype %>
                    </select>
                    <input type="hidden" name="suppliertype" id="suppliertype" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("suppliertype") %>" />
                    <input type="hidden" name="callback" id="callback" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("callBack") %>" />
                    <input type="hidden" name="iframeid" id="iframeid" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeid") %>" />
                    <input type="hidden" name="pIframeID" id="pIframeID" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("pIframeID") %>" />
                    <input type="hidden" name="hideID" id="hideID" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("hideID") %>" />
                    <input type="hidden" name="aid" id="aid" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("aid") %>" />
                    <input type="hidden" name="isall" id="isall" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("isall") %>" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <div style="margin: 0 auto 0 auto; width: 99%;">
        <div id="AjaxDataList" style="width: 100%; padding-top: 10px">
        </div>
        <table cellspacing="0" cellpadding="0" border="0" align="center" id="TabBtn">
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
    var UseSupplier = {
        AjaxURLg: null,
        isall: '<%=Request.QueryString["isall"] %>',
        type: '<%=(int)type %>',
        tourid: '<%=Request.QueryString["tourid"] %>',
        aid: '<%=Request.QueryString["aid"] %>',
        isadd:'<%=Request.QueryString["isadd"] %>',
        PageInit: function() {
            switch (UseSupplier.type) {
                case "1":
                    this.GetUrl("ground");
                    break;
                case "4":
                    this.GetUrl("scenicspots");
                    break;
                case "3":
                    this.GetUrl("wineshop");
                    break;
                case "2":
                    this.GetUrl("ticket");
                    break;
                case "5":
                    this.GetUrl("other");
                    break;
                case "6":
                    this.GetUrl("guide");
                    break;
            }
        },
        GetUrl: function(type) {
            UseSupplier.AjaxURLg = "/jidiaoCenter/AjaxSupplierRequest.aspx?type=" + type+"&isadd="+UseSupplier.isadd;
        },
        GetAjaxData: function(txtname) {
            //AJAX 加载数据
            $("#AjaxDataList").html("<div style='width:100%; text-align:center;'><img src='/images/loadingnew.gif' border='0' align='absmiddle'/>&nbsp;正在加载,请等待....&nbsp;</div>");

            var para = { name: txtname, callback: $("#callback").val(), iframeId: $("#iframeid").val(), piframeId: $("#pIframeID").val(), aid: UseSupplier.aid, ShowID: $("#hideID").val(), tourid: '<%=EyouSoft.Common.Utils.GetQueryStringValue("tourid") %>', isall: '<%=Request.QueryString["isall"] %>' };
            var url = UseSupplier.AjaxURLg + "&" + $.param(para);
            var suppliertype = "";
            if (UseSupplier.isall == "1") {
                suppliertype = '<%=Request.QueryString["slttype"] %>';
            }
            else {
                suppliertype = Boxy.queryString("suppliertype");
            }
            $.newAjax({
                type: "Get",
                url: url + "&suppliertype=" + suppliertype,
                cache: false,
                success: function(result) {
                    $("#AjaxDataList").html(result);
                    var boxyParentWin = window.parent.Boxy.getIframeWindowByID('<%=Request.QueryString["pIframeID"] %>') || parent;

                    boxyParentWin.$('.Offers').each(function() {
                        var sourceId = $(this).parent().find("input[type='hidden']:eq(0)").val();
                        if (sourceId != null && sourceId != "") {
                            $("#AjaxDataList").find("input[type='radio'][value='" + sourceId + "']").attr("checked", "checked");
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

    $("#slttype").change(function() {
        UseSupplier.type = $("#slttype").val();
    })
    $(function() {
        SupplerRequestPage.BintBtn();
        UseSupplier.PageInit();
        UseSupplier.GetAjaxData("defaultname");
        $("#txtName").keyup(function(){
            UseSupplier.GetAjaxData($(this).val());
        });
        $("#a_btn").click(function() {
            if($("#tblList").find("input[type='radio']:checked").length>0){
                useSupplierPage.SetValue();
                useSupplierPage.SelectValue();
            }
            else{
                parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
            }
            return false;
        })
        if (UseSupplier.isall == "1") {
            $("#slttype").show();
        }
    })
    var useSupplierPage = {
        _dataObj: {},
        selectValue: "",
        selectTxt: "",
        selecttype: "",
        contactname: "",
        contacttel: "",
        contactqq: "",
        SetNewValue:function(data){
             this.selectValue=data.id;
             this.selectTxt=data.name;
             this.contactname = data.contactname;
             this.contacttel = data.contacttel;
             this.contactqq = "";
        },
        SetValue: function() {
            var valueArray = [], txtArray = [],contactname = [], qq = [], tel = [];
            $("#tblList").find("input[type='radio']:checked").each(function() {
                valueArray.push($(this).val());
                txtArray.push($(this).attr("data-show"));
                contactname.push($(this).attr("data-contactname"));
                tel.push($(this).attr("data-tel"));
                qq.push($(this).attr("data-qq"));
            })

            this.selectValue = valueArray.join(',');
            this.selectTxt = txtArray.join(',');
            this.contactname = contactname.join(',');
            this.contacttel = tel.join(',');
            this.contactqq = qq.join(',');
        },
        RadioClickFun: function(args) {
            var rdo = $(args);
            var data = rdo.val().split(',');
            this.selectValue = data[0];
            this.selectTxt = data[1];
            this.contactname = data[2];
            this.contacttel = data[3];
            this.contactqq = data[4];
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
                aid: '<%=Request.QueryString["aid"] %>',
                id: useSupplierPage.selectValue,
                name: useSupplierPage.selectTxt,
                type: '<%=Request.QueryString["suppliertype"] %>',
                contactname: useSupplierPage.contactname,
                contacttel: useSupplierPage.contacttel,
                contactqq: useSupplierPage.contactqq
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
    var SupplerRequestPage={
        Url:"",
        GetUrl:function(){
             var data = {};
                data.iframeId = '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>';
                //父页面传过来的参数 也要带上 
                data.initId = '<%= EyouSoft.Common.Utils.GetQueryStringValue("initId") %>';
                data.callBack = '<%= EyouSoft.Common.Utils.GetQueryStringValue("callBack") %>';
                data.pIframeId = '<%= EyouSoft.Common.Utils.GetQueryStringValue("pIframeId") %>';
                data.isMore = '<%= EyouSoft.Common.Utils.GetQueryStringValue("isMore") %>';
                data.dotype='save';
                data.type= '<%= EyouSoft.Common.Utils.GetQueryStringValue("suppliertype") %>';
                SupplerRequestPage.Url="/jidiaoCenter/AjaxSupplierRequest.aspx?"+$.param(data);
        },
        BintBtn:function(){
            $("#btn_save").live("click",function(){
                SupplerRequestPage.GetUrl();
                if(!SupplerRequestPage.CheckForm()){
                    return false;
                }
                var type='<%= EyouSoft.Common.Utils.GetQueryStringValue("suppliertype") %>';
                $(this).html("正在保存");
                SupplerRequestPage.GoAjax(SupplerRequestPage.Url,"add",type);
               $("#"+$(this).attr("id")).die("click");
            })
            $("#btnxuan").live("click",function(){
                SupplerRequestPage.GetUrl();
                if(!SupplerRequestPage.CheckForm()){
                    return false;
                }
                var type='<%= EyouSoft.Common.Utils.GetQueryStringValue("suppliertype") %>';
                $(this).html("正在选用");
                SupplerRequestPage.GoAjax(SupplerRequestPage.Url,"xuanyong",type);
               $("#"+$(this).attr("id")).die("click");
                
            })
        },
        
        GoAjax: function(url,dotype,suppliertype,obj) {
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    data: $("#btn_save").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result == "1") {
                            if(ret.obj!=null){
                                if(dotype=="xuanyong"){
                                   var id="";
                                   var name="";
                                   var contactname="";
                                   var contacttel="";
                                   var _data={};
                                    if(suppliertype=="导游"){
                                        id=ret.obj.Id;
                                        name=ret.obj.GuideName;
                                        contacttel=ret.obj.Phone;
                                        
                                    }else{
                                        id=ret.obj.Id;
                                        name=ret.obj.UnitName;
                                        if(ret.obj.ContactList!=null){
                                            contactname=ret.obj.ContactList[0].ContactName;
                                            contacttel=ret.obj.ContactList[0].ContactMobile;
                                        }
                                    }
                                   _data.id=id;
                                   _data.name=name;
                                   _data.contactname=contactname;
                                   _data.contacttel=contacttel;
                                    useSupplierPage.SetNewValue(_data);
                                    useSupplierPage.SelectValue();
                                }else{
                                    parent.tableToolbar._showMsg(ret.msg, function() {location.href = location.href; });
                                }
                            }
                            
                        }
                        else {
                            parent.tableToolbar._showMsg(ret.msg);
                        }
                    },
                    error: function() {
                        parent.tableToolbar._showMsg(tableToolbar.errorMsg);
                    }
                });
          },
          Control:function(){
               if($("#CustomerUnitRequest").find("span[id='empty']").length>0){
                   $("#TabBtn").hide();
               }
          },
          CheckForm: function() {
              return ValiDatorForm.validator($("#btn_save").closest("form").get(0), "parent");
          }
    }
    

</script>

