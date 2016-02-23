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
    public partial class CityAdd : EyouSoft.Common.Page.BackPage
    {
        protected string cityName;//城市名称
        protected string method = string.Empty;//当前操作
        protected int cId;
        protected void Page_Load(object sender, EventArgs e)
        {
            PowerControl();
            int proId = Utils.GetInt(Utils.GetFormValue(selProvince.UniqueID));//获取省份Id
            cityName = Utils.GetFormValue("txtCityName");//获取城市名
            cId = Utils.GetInt(Utils.GetQueryStringValue("cId"));//城市Id
            string method = Utils.GetFormValue("hidMethod");//获取当前操作(保存/继续)
            string showMess = "数据保存成功！";//提示消息
            EyouSoft.Model.CompanyStructure.City cityModel = null;
            EyouSoft.BLL.CompanyStructure.City cityBll = new EyouSoft.BLL.CompanyStructure.City();//初始化bll
            //城市Id不为空则添加,否则视为保存
            if (method == "")
            {
                string isExist = Utils.GetFormValue("isExist");//验证城市是否已经存在
                if (isExist == "isExist")
                {
                    string cityNameE = Utils.GetFormValue("cityName");//获取城市名
                    int id = Utils.GetInt(Utils.GetFormValue("cityId"));
                    bool isExistResult = cityBll.IsExists(cityNameE, CurrentUserCompanyID, id);
                    Utils.ResponseMeg(isExistResult, "");
                    return;
                }
                #region 初次加载数据
                //绑定省份下拉框
                IList<EyouSoft.Model.CompanyStructure.Province> proList = new EyouSoft.BLL.CompanyStructure.Province().GetList(CurrentUserCompanyID);
                if (proList != null && proList.Count > 0)
                {
                    selProvince.DataTextField = "ProvinceName";
                    selProvince.DataValueField = "Id";
                    selProvince.DataSource = proList;
                    selProvince.DataBind();
                }
                selProvince.Items.Insert(0, new ListItem("请选择", ""));
                if (cId != 0)//初始化城市信息
                {
                    cityModel = cityBll.GetModel(cId);
                    if (cityModel != null)
                    {
                        cityName = cityModel.CityName;
                        selProvince.Value = cityModel.ProvinceId.ToString();
                    }
                    return;
                }
                #endregion
            }
            else
            {
                #region 保存数据
                if (cityName == "")
                {
                    MessageBox.Show(this, "城市名称不为空！");
                    return;
                }
                bool result = false;
                cityModel = new EyouSoft.Model.CompanyStructure.City();
                cityModel.CityName = cityName;
                cityModel.OperatorId = SiteUserInfo.UserId;
                cityModel.CompanyId = CurrentUserCompanyID;
                cityModel.ProvinceId = proId;
                cityModel.IssueTime = DateTime.Now;
                if (cId != 0)
                {  //修改城市
                    if (cityModel != null)
                    {
                        cityModel.Id = cId;
                        result = cityBll.Update(cityModel);
                    }
                }
                else
                {   //添加城市
                    result = cityBll.Add(cityModel);
                }
                if (!result)
                {
                    showMess = "数据保存失败！";
                }
                //继续添加则刷新页面,否则关闭当前窗口
                if (method == "continue")
                {
                    MessageBox.ShowAndRedirect(this, showMess, "CityAdd.aspx");
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
