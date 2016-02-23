//2011-09-27 汪奇志
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Webmaster
{
    /// <summary>
    /// 子系统域名管理
    /// </summary>
    public partial class _domain : WebmasterPageBase
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        int SysId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            SysId = Utils.GetInt(Utils.GetQueryStringValue("sysid"));

            var sysInfo = new EyouSoft.BLL.SysStructure.BSys().GetSysInfo(SysId);

            if (sysInfo == null)
            {
                RegisterAlertAndRedirectScript("请求异常。", "systems.aspx");
            }

            ltrSysName.Text = sysInfo.SysName;

            InitDomains();
        }

        #region private members
        /// <summary>
        /// 初始化域名信息集合
        /// </summary>
        private void InitDomains()
        {
            string script = "var domains={0};";
            EyouSoft.BLL.SysStructure.BSys bll = new EyouSoft.BLL.SysStructure.BSys();
            IList<EyouSoft.Model.SysStructure.SystemDomain> items = bll.GetDomains(SysId);
            bll = null;

            if (items == null || items.Count < 1)
            {
                RegisterScript(string.Format(script, "[]"));
            }
            else
            {
                RegisterScript(string.Format(script, Newtonsoft.Json.JsonConvert.SerializeObject(items)));
            }
        }

        /// <summary>
        /// get domains
        /// </summary>
        /// <returns></returns>
        private IList<EyouSoft.Model.SysStructure.SystemDomain> GetDomains()
        {
            IList<EyouSoft.Model.SysStructure.SystemDomain> items = new List<EyouSoft.Model.SysStructure.SystemDomain>();

            string[] domains = Utils.GetFormValues("txtDomain");
            string[] urls = Utils.GetFormValues("txtDomainPath");

            for (int i = 0; i < domains.Length; i++)
            {
                if (string.IsNullOrEmpty(domains[i])) continue;
                string s = domains[i].Trim().Replace("http://", "").ToLower();
                if (string.IsNullOrEmpty(s)) continue;

                //验证域名与集合内域名是否重复
                bool isExists=false;
                foreach (var item in items)
                {
                    if (item.Domain == s)
                    {
                        isExists = true;
                        break;
                    }
                }
                if (isExists) continue;

                items.Add(new EyouSoft.Model.SysStructure.SystemDomain()
                {
                    Domain = s,
                    SysId = SysId,
                    Url = urls[i]
                });
            }

            return items;
        }
        #endregion

        #region btnCreate click
        /// <summary>
        /// btnSubmit click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            IList<EyouSoft.Model.SysStructure.SystemDomain> items = GetDomains();
            EyouSoft.BLL.SysStructure.BSys bll=new EyouSoft.BLL.SysStructure.BSys();

            #region validate form
            if (items == null || items.Count < 1)
            {
                RegisterAlertScript("至少要填写一个域名信息。");
                return;
            }            

            IList<string> existss = new List<string>();
            foreach (var item in items)
            {
                existss.Add(item.Domain);
            }

            existss = bll.IsExistsDomains(SysId, existss);

            if (existss != null && existss.Count > 0)
            {
                RegisterAlertScript(string.Format("域名：{0}已经存在。",existss[0]));
                return;
            }
            #endregion

            int setResult = bll.SetSysDomains(SysId, items);

            if (setResult == 1)
            {
                this.RegisterAlertAndReloadScript("域名信息保存成功");
            }
            else
            {
                this.RegisterAlertScript("域名信息保存失败");
            }
        }
        #endregion 
    }
}
