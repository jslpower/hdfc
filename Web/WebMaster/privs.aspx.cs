//2011-10-13 汪奇志
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
    /// 子系统权限管理
    /// </summary>
    public partial class _privs : WebmasterPageBase
    {
        int CompanyId = 0;
        int SysId = 0;
        int UserId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyId =Utils.GetInt( Utils.GetQueryStringValue("cid"));
            SysId = Utils.GetInt(Utils.GetQueryStringValue("sysid"));
            UserId = Utils.GetInt(Utils.GetQueryStringValue("uid"));

            var sysInfo = new EyouSoft.BLL.SysStructure.BSys().GetSysInfo(SysId);

            if (sysInfo == null)
            {
                RegisterAlertAndRedirectScript("请求异常。", "systems.aspx");
            }

            ltrSysName.Text = sysInfo.SysName;

            InitPrivs();
        }

        #region private members
        /// <summary>
        /// init privs
        /// </summary>
        void InitPrivs()
        {
            EyouSoft.BLL.SysStructure.BSys bll = new EyouSoft.BLL.SysStructure.BSys();
            var items = bll.GetPrivs1();
            var comPrivs = bll.GetComPrivsInfo(CompanyId);
            bll = null;
            string script = "var comPrivs={0};";

            System.Text.StringBuilder html = new System.Text.StringBuilder();
            EyouSoft.BLL.SysStructure.BSys sysbll = new EyouSoft.BLL.SysStructure.BSys();

            foreach (var menu1 in items)
            {
                html.AppendFormat("<div class=\"p1\"><input type=\"checkbox\" id=\"chk_p_1_{1}\" value=\"{1}\" name=\"chk_p_1\" /><label for=\"chk_p_1_{1}\">{0}</label><!--<span class=\"pcode\">[{1}]</span>--></div>", menu1.Name, menu1.MenuId);

                if (menu1.Menu2s == null || menu1.Menu2s.Count < 1) continue;

                html.Append("<div>");

                int i = 0;
                foreach (var menu2 in menu1.Menu2s)
                {
                    html.Append("<ul class=\"p2\">");
                    html.AppendFormat("<li class=\"p2title\"><input type=\"checkbox\" id=\"chk_p_2_{1}\" value=\"{1}\" name=\"chk_p_2\" /><label for=\"chk_p_2_{1}\">{0}</label><!--<span class=\"pcode\">[{1}]</span>--></li>", menu2.Name, menu2.MenuId);

                    var privs = sysbll.GetPrivs3(menu2.MenuId);

                    if (privs != null && privs.Count > 0)
                    {
                        foreach (var priv in privs)
                        {
                            html.AppendFormat("<li class=\"p3\"><input type=\"checkbox\" id=\"chk_p_3_{1}\" value=\"{1}\" name=\"chk_p_3\"  /><label for=\"chk_p_3_{1}\">{0}</label><span class=\"pcode\">[{1}]</span></li>", priv.Name, priv.PrivsId);
                        }
                    }

                    html.Append("</ul>");

                    if (i % 4 == 3)
                    {
                        html.Append("<ul class=\"p2space\"><li></li></ul>");
                    }
                    i++;
                }

                html.Append("<ul class=\"p2space\"><li></li></ul>");
                html.Append("</div>");
            }

            this.ltrPrivs.Text = html.ToString();

            script = string.Format(script, Newtonsoft.Json.JsonConvert.SerializeObject(comPrivs));

            RegisterScript(script);
        }

        /// <summary>
        /// get privs1
        /// </summary>
        /// <returns></returns>
        private int[] GetPrivs1()
        {
            string[] _privs1 = EyouSoft.Common.Utils.GetFormValues("chk_p_1");
            if (_privs1 == null && _privs1.Length < 1) return null;

            int[] privs1 = new int[_privs1.Length];

            for (int i = 0; i < _privs1.Length; i++)
            {
                privs1[i] = EyouSoft.Common.Utils.GetInt(_privs1[i]);
            }

            return privs1;
        }

        /// <summary>
        /// get privs2
        /// </summary>
        /// <returns></returns>
        private int[] GetPrivs2()
        {
            string[] _privs2 = EyouSoft.Common.Utils.GetFormValues("chk_p_2");
            if (_privs2 == null && _privs2.Length < 1) return null;

            int[] privs2 = new int[_privs2.Length];

            for (int i = 0; i < _privs2.Length; i++)
            {
                privs2[i] = EyouSoft.Common.Utils.GetInt(_privs2[i]);
            }

            return privs2;
        }

        /// <summary>
        /// get privs3
        /// </summary>
        /// <returns></returns>
        private int[] GetPrivs3()
        {
            string[] _privs3 = EyouSoft.Common.Utils.GetFormValues("chk_p_3");
            if (_privs3 == null && _privs3.Length < 1) return null;

            int[] privs3 = new int[_privs3.Length];

            for (int i = 0; i < _privs3.Length; i++)
            {
                privs3[i] = EyouSoft.Common.Utils.GetInt(_privs3[i]);
            }

            return privs3;
        }
        #endregion

        #region protected members
        /// <summary>
        /// btnSetSysPrivs_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSetSysPrivs_Click(object sender, EventArgs e)
        {
            var info = new EyouSoft.Model.SysStructure.MComPrivsInfo();

            info.Privs1 = GetPrivs1();
            info.Privs2 = GetPrivs2();
            info.Privs3 = GetPrivs3();

            EyouSoft.BLL.SysStructure.BSys bll = new EyouSoft.BLL.SysStructure.BSys();
            if (bll.SetComPrivs(SysId, info) == 1)
            {
                RegisterAlertAndReloadScript("设置子系统权限成功");
            }
            else
            {
                RegisterAlertAndReloadScript("设置子系统权限失败");
            }
            bll = null;
        }

        /// <summary>
        /// btnSetAdminPrivsBySys_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSetAdminRoleBySys_Click(object sender, EventArgs e)
        {
            EyouSoft.BLL.SysStructure.BSys bll = new EyouSoft.BLL.SysStructure.BSys();
            int roleId = bll.GetSysRoleId(CompanyId);
            if (bll.SetRoleBySysPrivs(roleId, SysId) == 1)
            {
                RegisterAlertAndReloadScript("设置管理员角色权限为子系统权限成功");
            }
            else
            {
                RegisterAlertAndReloadScript("设置管理员角色权限为子系统权限失败");
            }
            bll = null;
        }

        /// <summary>
        /// btnSetAdminPrivsBySys_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSetAdminPrivsBySys_Click(object sender, EventArgs e)
        {
            EyouSoft.BLL.SysStructure.BSys bll = new EyouSoft.BLL.SysStructure.BSys();
            if (bll.SetUserBySysPrivs(UserId, SysId) == 1)
            {
                RegisterAlertAndReloadScript("设置管理员账号权限为子系统权限成功");
            }
            else
            {
                RegisterAlertAndReloadScript("设置管理员账号权限为子系统权限失败");
            }
            bll = null;
        }

        /// <summary>
        /// btnSetAdminRoleByAdminRole_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSetAdminRoleByAdminRole_Click(object sender, EventArgs e)
        {
            EyouSoft.BLL.SysStructure.BSys bll = new EyouSoft.BLL.SysStructure.BSys();
            int roleId = bll.GetSysRoleId(CompanyId);

            if (bll.SetUserRole(UserId, roleId))
            {
                RegisterAlertAndReloadScript("设置管理员为管理员角色成功");
            }
            else
            {
                RegisterAlertAndReloadScript("设置管理员为管理员角色失败");
            }
        }
        #endregion
    }
}
