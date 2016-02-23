<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Boxy.Master" AutoEventWireup="true"
    CodeBehind="CustomerUnitSelect.aspx.cs" Inherits="Web.CommonPage.CustomerUnitSelect" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageBody" runat="server">
    <div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" alt="" />
                </td>
                <td>
                    <div class="searchbox" style="height: 30px;">
                        单位名称：
                        <input type="text" runat="server" class="searchinput inputtext" id="txtCustomer" />
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" alt="" />
                </td>
            </tr>
        </table>
        <div class="tablelist" style="margin-left: 5px">
            <div id="CustomerUnitRequest">
            </div>
            <table width="320" border="0" align="center" cellpadding="0" cellspacing="0" id="TabBtn">
                <tr>
                    <td height="40" align="center">
                    </td>
                    <td height="40" align="center" class="tjbtn02">
                        <a id="btnSave" href="javascript:void(0);">确定</a>
                    </td>
                    <td height="40" align="center" class="tjbtn02">
                        <a href="javascript:void(0);" onclick="parent.Boxy.getIframeDialog('<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide()">
                            关闭</a>
                    </td>
                </tr>
            </table>
            
        </div>
    </div>

    <script type="text/javascript">
        var CustomerUnitSelect = {
            _data: {
                cid: "", //客户编号
                cname: "", //客户名称
                ccname: "", //主要联系人名称
                cctel: "", //主要联系人电话
                ccmobile: "" //只要联系人手机
            },
            _rdata: [], //返回给父页面的对象 格式为 _data 对象的集合,
            SetValue: function() {
                var str = "";
                var index = 0;
                if ('<%= EyouSoft.Common.Utils.GetQueryStringValue("isMore") %>' == "1") {//多选
                    $("#CustomerUnitRequest").find("input[type='checkbox']:checked").each(function() {
                        var tdata = {};
                        tdata.cid = $(this).val();
                        tdata.cname = $(this).attr("data-cname");
                        tdata.ccname = $(this).attr("data-ccname");
                        tdata.cctel = $(this).attr("data-tel");
                        tdata.ccmobile = $(this).attr("data-mobile");
                        CustomerUnitSelect._rdata.push(tdata);
                    })
                }
                else {//单选
                    $("#CustomerUnitRequest").find("input[type='radio']:checked").each(function() {
                        var tdata = {};
                        tdata.cid = $(this).val();
                        tdata.cname = $(this).attr("data-cname");
                        tdata.ccname = $(this).attr("data-ccname");
                        tdata.cctel = $(this).attr("data-tel");
                        tdata.ccmobile = $(this).attr("data-mobile");
                        CustomerUnitSelect._rdata.push(tdata);
                    })
                }
            },
            CustomerId:"",
            CustomerName:"",
            SetNewValue:function(id,name){
                var ndata={};
                ndata.cid=id;
                ndata.cname=name;
                CustomerUnitSelect._rdata.push(ndata);
                //tdata.cname=
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
                            boxyParent[callBack](CustomerUnitSelect._rdata);
                        }
                        else {
                            boxyParent[callBack.split('.')[0]][callBack.split('.')[1]](CustomerUnitSelect._rdata);
                        }
                    }
                    //定义回调
                }
                else {
                    //判断是否存在回调方法
                    if (callBack != null && callBack.length > 0) {
                        if (callBack.indexOf('.') == -1) {
                            window.parent[callBack](CustomerUnitSelect._rdata);
                        }
                        else {
                            window.parent[callBack.split('.')[0]][callBack.split('.')[1]](CustomerUnitSelect._rdata);
                        }
                    }
                    //定义回调
                }
                parent.Boxy.getIframeDialog('<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
            },
            Search: function() {
                var data = {};
                data.iframeId = '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>';
                //父页面传过来的参数 也要带上 
                data.initId = '<%= EyouSoft.Common.Utils.GetQueryStringValue("initId") %>';
                data.callBack = '<%= EyouSoft.Common.Utils.GetQueryStringValue("callBack") %>';
                data.pIframeId = '<%= EyouSoft.Common.Utils.GetQueryStringValue("pIframeId") %>';
                data.isMore = '<%= EyouSoft.Common.Utils.GetQueryStringValue("isMore") %>';
                
                CustomerUnitSelect.GetAjaxData(CustomerUnitSelect.AjaxUrl+"&"+$.param(data));
            } ,
            AjaxUrl:"/CommonPage/CustomerUnitRequest.aspx",
            GetAjaxData: function(url) {
                //AJAX 加载数据
                $("#CustomerUnitRequest").html("<div style='width:100%; text-align:center;'><img src='/images/loadingnew.gif' border='0' align='absmiddle'/>&nbsp;正在加载,请等待....&nbsp;</div>");
                $.newAjax({
                    type: "Get",
                    url: url,
                    cache: false,
                    success: function(result) {
                        $("#CustomerUnitRequest").html(result);
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
        };

        $(document).ready(function() {
            
            $("#<%=txtCustomer.ClientID %>").val($("#<%=txtCustomer.ClientID %>").val());
            
            $("#<%=txtCustomer.ClientID %>").keyup(function(){
                CustomerUnitSelect.AjaxUrl="/CommonPage/CustomerUnitRequest.aspx?cname="+encodeURIComponent($(this).val());
                CustomerUnitSelect.Search();
                return false;
            });
            $("#btnSave").bind("click", function() {
                CustomerUnitSelect.SetValue();
                CustomerUnitSelect.SelectValue();
                return false;
            });
            UnitRequestPage.BintBtn();
        });
    var UnitRequestPage={
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
                UnitRequestPage.Url="/CommonPage/CustomerUnitRequest.aspx?"+$.param(data);
        },
        BintBtn:function(){
            $("#btn_save").unbind("click").live("click",function(){
                UnitRequestPage.GetUrl();
                if(!UnitRequestPage.CheckForm()){
                    return false;
                }
                UnitRequestPage.GoAjax(UnitRequestPage.Url,"add",$(this));
                $("#btnSave").html("保存");
                
            })
            $("#btnxuan").unbind("click").live("click",function(){
                UnitRequestPage.GetUrl();
                if(!UnitRequestPage.CheckForm()){
                    return false;
                }
                UnitRequestPage.GoAjax(UnitRequestPage.Url,"xuanyong",$(this));
                $("#btnxuan").html("保存并选用");
                
            })
        },
        GoAjax: function(url,dotype,obj) {
                //$("#"+$(obj).attr("id")).die("click");
                $("#"+$(obj).attr("id")).unbind("click");
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "json",
                    data: $("#btn_save").closest("form").serialize(),
                    success: function(ret) {
                        if (ret.result != "0") {
                            if(ret.result.split('|').length>1){
                                if(dotype=="xuanyong"){
                                   var id=ret.result.split('|')[0];
                                   var name=ret.result.split('|')[1];
                                    CustomerUnitSelect.SetNewValue(id,name);
                                    CustomerUnitSelect.SelectValue();
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
    
    
    
</asp:Content>
