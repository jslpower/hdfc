using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SystemSet
{
    public partial class ProvinceAdd : EyouSoft.Common.Page.BackPage
    {
        protected string provinceName = string.Empty;//省份名
        protected int pId;
        protected void Page_Load(object sender, EventArgs e)
        {
            PowerControl();
            provinceName = Utils.GetFormValue("txtProvinceName");//获取省份
            pId = Utils.GetInt(Utils.GetQueryStringValue("proId"));//省份Id
            string method = Utils.GetFormValue("hidMethod");//获取当前操作(保存/继续)
            string showMess = "数据保存成功";//提示消息
            EyouSoft.Model.CompanyStructure.Province proModel = null;
            EyouSoft.BLL.CompanyStructure.Province proBll = new EyouSoft.BLL.CompanyStructure.Province();//初始化bll
            //当前操作为空则初始化
            if (method == "")
            {
                string isExist = Utils.GetFormValue("isExist");//验证城市是否已经存在
                if (isExist == "isExist")
                {
                    string pNameE = Utils.GetFormValue("pName");//获取城市名
                    int id = Utils.GetInt(Utils.GetFormValue("pId"));
                    bool isExistResult = proBll.IsExists(pNameE, CurrentUserCompanyID, id);
                    Utils.ResponseMeg(isExistResult, "");
                    return;
                }
                #region 初次加载数据
                if (pId != 0)
                {
                    proModel = proBll.GetModel(pId);//获取省份实体
                    if (proModel != null)
                    {
                        provinceName = proModel.ProvinceName;
                    }
                    return;
                }
                #endregion
            }
            else
            {
                #region 保存
                if (provinceName == "")
                {
                    MessageBox.Show(this, "省份不为空！");
                    return;
                }
                proModel = new EyouSoft.Model.CompanyStructure.Province();
                proModel.ProvinceName = provinceName;
                proModel.CompanyId = CurrentUserCompanyID;
                proModel.IssueTime = DateTime.Now;
                bool result = false;
                if (pId != 0)
                {
                    proModel.Id = pId;
                    result = proBll.Update(proModel);//更新省份
                }
                else
                {
                    result = proBll.Add(proModel);//添加省份
                }
                if (!result)
                {
                    showMess = "数据保存失败！";
                }
                //继续添加则刷新页面,否则关闭当前窗口
                if (method == "continue")//继续添加重定向当前页，否则关闭当前窗口
                {
                    MessageBox.ShowAndRedirect(this, showMess, "ProvinceEdit.aspx");
                }
                else
                {
                    MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/SystemSet/CityManage.aspx';window.parent.Boxy.getIframeDialog('{1}').hide();", showMess, Utils.GetQueryStringValue("iframeId")));
                }
                #endregion
            }
        }
        /// <summary>
        /// 权限判断
        /// </summary>
        protected void PowerControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_城市管理栏目))
            {
                this.btnsave.Visible = false;
            }

        }
    }
}
