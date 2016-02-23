<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SellsSelect.ascx.cs"
    Inherits="Web.UserControl.SellsSelect" %>
<span id="span<%=this.SetPriv %>">
    <input type="text" readonly="readonly" style="background-color: #dadada"  errmsg="请选择<%=this.SetTitle %>!"
        valid="required" id="<%=this.SellsNameClient %>" class="inputtext formsize80"
        name="<%=this.SellsNameClient %>" value="<%=this.SellsName %>">
    <input type="hidden" id="<%=this.SellsIDClient %>" name="<%=this.SellsIDClient %>"
        value="<%=this.SellsID %>" />
    <%if (IsShowSelect)
      { %>
    <a id="<%=this.SetPriv %>_a_btn" style="cursor:pointer" title="<%=this.SetTitle %>" data-width="850" data-height="550"
        class="xuanyong"></a>
    <%} %>
    <span data-class="hideDeptInfo" data-tel="" data-deptid="<%=this.ClientDeptID %>"
        data-deptname="<%=this.ClientDeptName %>"></span></span>

<script type="text/javascript">
    $(function() {
         $("#<%=this.SellsNameClient %>,#<%=this.SetPriv %>_a_btn").click(function() {
                var hideid = $("#<%=this.SellsIDClient %>").attr("id");
                var _data = { callBackFun: '<%=CallBackFun %>', sModel: '<%=this.SMode?"2":"1" %>', pIframeId: "<%=this.ParentIframeID %>",hideId:$("#"+hideid).val() };
                window.parent.Boxy.iframeDialog({
                    title: "销售员",
                    iframeUrl: "/CommonPage/OrderSells.aspx",
                    data: _data,
                    width: "870px",
                    height: "580px"
                });
                
            })
    })  
    
   window["<%=ClientID %>"] = {
        _callBack: function(_data) {
            if (_data) {
                $("#<%=this.SellsNameClient %>").val(_data.text);
                $("#<%=this.SellsIDClient %>").val(_data.value);
            }
        }
    };
</script>

