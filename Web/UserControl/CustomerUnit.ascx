<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerUnit.ascx.cs"
    Inherits="Web.UserControl.CustomerUnit" %>

<script type="text/javascript" src="/JS/json2.js"></script>

<input type="text" id="txtCustomerName" class="formsize120 inputtext" name="txtCustomerName"
    readonly="readonly" style="background-color: #dadada" value="<%= InitCustomerName %>"
    <%=IsRequired?" valid='required' errmsg='请选择组团社!' ":" " %> />
<a href="javascript:void(0);" id="a_SelectCustomer" class="xuanyong"></a>
<input type="hidden" name="<%= HidClientName %>" id="<%= HidClientId %>" value="<%= InitCustomerId %>" />

<script type="text/javascript">
    var CustomerUnit = {
        SelectCallBack: function(args) {
            if (args == null || args.length == 0) return;
            if ("<%= IsMoreSelect %>" == "True") {//多选
                var cids = [], cnames = [];
                for (var i = 0; i < args.length; i++) {
                    cids.push(args[i].cid);
                    cnames.push(args[i].cname);
                }
                $("#<%= HidClientId %>").val(cids.join(','));
                $("#txtCustomerName").val(cnames.join(','));
            }
            else { // 单选
                $("#<%= HidClientId %>").val(args[0].cid);
                $("#txtCustomerName").val(args[0].cname);
            }
        },
        SelectCustomr: function() {
            var url = "/CommonPage/CustomerUnitSelect.aspx?";
            var data = {};
            data.initId = $("#<%= HidClientId %>").val();
            data.callBack = "CustomerUnit.SelectCallBack";
            data.pIframeId = "<%= ParentIframeId %>";
            data.isMore = "<%= IsMoreSelect ? 1 : 0 %>";
            parent.Boxy.iframeDialog({
                iframeUrl: url + $.param(data),
                title: "选择客户单位",
                modal: true,
                width: "820",
                height: "378"
            });
        }
    };
    $(document).ready(function() {
        $("#txtCustomerName,#a_SelectCustomer").bind("click", function() {
            CustomerUnit.SelectCustomr();
            return false;
        });
    });
</script>

