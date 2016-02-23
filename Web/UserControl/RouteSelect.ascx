<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RouteSelect.ascx.cs"
    Inherits="Web.UserControl.RouteSelect" %>
<div id="divRouteSelect">
    <asp:Literal runat="server" ID="ltrRoute">线路名称：</asp:Literal>
    <input name="txtRouteName" type="text" class="formsize120 inputtext" id="txtRouteName" value="<%= InitRouteName %>" <%if(IsMust){ %><%=NoticeHTML %><%} %> />
    <input type="hidden" id="<%= HidClientId %>" name="<%= HidClientName %>" value="<%= InitRouteId %>" />
    <a title="选用线路" href="javascript:void(0);" id="a_SelectRoute">
        <img src="/images/sanping_04.gif" width="28" height="18" style="vertical-align: top;"
            alt="选用" /></a>
</div>

<script type="text/javascript">
    var RouteSelect = {
        SelectCallBack: function(args) {
            if (args == null || args.length == 0) return;
            if ("<%= IsMoreSelect %>" == "True") {//多选
                var rids = [], rnames = [];
                for (var i = 0; i < args.length; i++) {
                    rids.push(args[i].rid);
                    rnames.push(args[i].rname);
                }
                $("#<%= HidClientId %>").val(rids.join(','));
                $("#txtCustomerName").val(rnames.join(','));
            }
            else { // 单选
                $("#<%= HidClientId %>").val(args[0].rid);
                $("#txtRouteName").val(args[0].rname);
            }
        },
        SelectRoute: function() {
            var url = "/CommonPage/SelectRoute.aspx?";
            var data = {};
            data.initId = $("#<%= HidClientId %>").val();
            data.callBack = "RouteSelect.SelectCallBack";
            data.pIframeId = '<%= EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>';
            data.isMore = "<%= IsMoreSelect ? 1 : 0 %>";
            parent.Boxy.iframeDialog({
                iframeUrl: url + $.param(data),
                title: "选择线路",
                modal: true,
                width: "820",
                height: "378"
            });
        }
    };

    $(document).ready(function() {
        $("#a_SelectRoute").bind("click", function() {
            RouteSelect.SelectRoute();
            return false;
        });
    });
</script>

