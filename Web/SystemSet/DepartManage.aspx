<%@ Page Title="部门管理" Language="C#" MasterPageFile="~/MasterPage/Front.Master" AutoEventWireup="true"
    CodeBehind="DepartManage.aspx.cs" Inherits="Web.SystemSet.DepartManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td width="15%" nowrap="nowrap">
                                <span class="lineprotitle">组织机构</span>
                            </td>
                            <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                             <b>当前您所在位置</b>&gt;&gt; 系统设置 &gt;&gt; 组织机构
                            </td>
                        </tr>
                        <tr>
                            <td height="2" bgcolor="#000000" colspan="2">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="lineCategorybox" style="height: 50px;">
                <table border="0" cellpadding="0" cellspacing="0" class="xtnav">
                    <tr>
                        <td width="100" align="center" class="xtnav-on">
                            <a>部门名称</a>
                        </td>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                            <td width="100" align="center">
                                <a href="/SystemSet/UserList.aspx">部门人员</a>
                            </td>
                        </asp:PlaceHolder>
                    </tr>
                </table>
            </div>
            <a href="javascript:;" onclick="return DM.addD('top','');" style="display: <%=styleDiaplay %>">
                新 增</a>
            <table width="775" border="0" align="center" cellpadding="0" cellspacing="0" style="border: 1px solid #5D8FD1;">
                <%=depStrHTML %>
            </table>
        </div>
        <!-- InstanceEndEditable -->
    </div>
    <div class="clearboth">
    </div>

    <script type="text/javascript">
        //打开弹窗
        var DM = {
            openDialog: function(p_url, p_title, tar_a) {
                Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "640px", height: "380px", tar: tar_a });
            },
            delD: function(dId, tar_a, pId) {
                var canDel = false;
                tableToolbar.ShowConfirmMsg("你确定要删除该部门吗？", function() {
                    $.newAjax(
                      {
                          url: "/SystemSet/DepartManage.aspx",
                          data: { departId: dId, method: "candel" },
                          dataType: "json",
                          cache: false,
                          type: "get",
                          async: false,
                          success: function(result) {
                              if (result.success != "1") {
                                  canDel = false;
                                  alert(result.message);
                              }
                              else {
                                  canDel = true;
                              }

                          },
                          error: function() {
                              tableToolbar._showMsg("操作失败!");
                          }
                      })
                    if (!canDel) {
                        return false;
                    }

                    $.newAjax(
                      {
                          url: "/SystemSet/DepartManage.aspx",
                          data: { departId: dId, method: "del" },
                          dataType: "json",
                          cache: false,
                          type: "get",
                          success: function(result) {
                              if (result.success == "1") {

                                  $(tar_a).closest("tr").remove();
                                  var arrTr = [];
                                  DM.delRe(dId, arrTr);
                                  for (var i in arrTr) {
                                      arrTr[i].remove();
                                  }
                                  tableToolbar._showMsg("删除成功！");

                              }
                          },
                          error: function() {
                              tableToolbar._showMsg("操作失败!");
                          }
                      })
                    return false;
                })
            },
            //修改部门
            updateD: function(dId, tar_a) {
                DM.openDialog("/SystemSet/DepartAdd.aspx?action=edit&departId=" + dId, "修改部门", tar_a);
                return false;
            },
            //添加下级部门
            addD: function(dId, tar_a) {
                DM.openDialog("/SystemSet/DepartAdd.aspx?action=add&parentId=" + dId, "添加部门");
                return false;
            },
            //添加下级部门后回调
            callbackAddD: function(pId) {
                //			            var arrTr = [];
                //			            DM.delRe(pId, arrTr);
                //			            for (var i in arrTr) {
                //			                arrTr[i].remove();
                //			            }
                //			            var tar = $("#strong" + pId).parent().get(0);
                //			            if ($(tar) && $(tar).prev("img").attr("src").indexOf("7") > 0) {

                //			                $(tar).click(function() {
                //			                    DM.getSonD(this, pId);
                //			                });
                //			                $(tar).prev("img").click(function() {
                //			                    DM.getSonD2(this);
                //			                });
                //			            }
                //			            DM.getSonD(tar, pId, true);

            },
            //修改部门后回调
            callbackUpdateD: function(nowId, nowName, updateP) {
                if (updateP == "True") {
                    $("#strong" + nowId).remove();
                }
                else {
                    $("#strong" + nowId).html(nowName);
                }
            }, //点击图片
            getSonD2: function(tar) {
                $(tar).next("a").click();
                return false;
            },
            //获取子部门
            getSonD: function(tar_a, dId, back) {
                var tarObj = $(tar_a);
                var tarImg = tarObj.prev("img");
                if (tarImg.attr("src").indexOf("5") > 0 || back) {
                    tarImg.attr("src", "/images/organization_06.gif");
                }
                else if (tarImg.attr("src").indexOf("6") > 0) {
                    var arrTr = [];
                    DM.delRe(dId, arrTr);
                    for (var i in arrTr) {
                        arrTr[i].remove();
                    }
                    tarImg.attr("src", "/images/organization_05.gif");
                    return false;
                }
                $.newAjax(
                       {
                           url: "/SystemSet/DepartManage.aspx",
                           data: { departId: dId, step: $(tar_a).attr("step") },
                           dataType: "text",
                           cache: false,
                           type: "get",
                           success: function(result) {
                               $("#noDepart").remove();
                               $(tar_a).closest("tr").after(result);
                           },
                           error: function() {
                               tableToolbar._showMsg("操作失败!");
                           }
                       })
            },
            delRe: function(id, arr1) {
                var arr = $("tr[parentid='" + id + "']");
                if (arr) {
                    arr.each(function() {
                        arr1.push($(this));
                        DM.delRe($(this).attr("sid"), arr1);

                    });

                }
            }
        }
			   
    </script>

</asp:Content>
