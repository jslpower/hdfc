using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Webmaster
{
    /// <summary>
    /// 基础权限管理
    /// </summary>
    /// 汪奇志 2012-03-03
    public partial class allprivs : WebmasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.InitAllPrivs();
        }

        #region private members
        /// <summary>
        /// 初始化栏目、模块、权限信息
        /// </summary>
        private void InitAllPrivs()
        {
            System.Text.StringBuilder html = new System.Text.StringBuilder();
            EyouSoft.BLL.SysStructure.BSys bll = new EyouSoft.BLL.SysStructure.BSys();
            var items = bll.GetPrivs1();
            bll = null;

            if (items != null && items.Count > 0)
            {
                foreach (var big in items)
                {
                    html.AppendFormat("<div class=\"pBig\"><input type=\"checkbox\" id=\"chk_p_big_{1}\" value=\"{1}\" name=\"chk_p_big\" /><label for=\"chk_p_big_{1}\">{0}</label><span class=\"pno\">[{1}]</div></span>", big.Name
                        , big.MenuId);

                    if (big.Menu2s == null || big.Menu2s.Count < 1) continue;

                    html.Append("<div>");

                    int i = 0;
                    foreach (var small in big.Menu2s)
                    {
                        html.Append("<ul class=\"pSmall\">");
                        html.AppendFormat("<li class=\"pSmallTitle\"><input type=\"checkbox\" id=\"chk_p_small_{1}\" value=\"{1}\" name=\"chk_p_small\" /><label for=\"chk_p_small_{1}\">{0}</label><span class=\"pno\">[{1}]</span></li>", small.Name
                            , small.MenuId);

                        if (small.Privs != null && small.Privs.Count > 0)
                        {
                            foreach (var permission in small.Privs)
                            {
                                html.AppendFormat("<li class=\"pThird\" title=\"{2}\"><input type=\"checkbox\" id=\"chk_p_third_{1}\" value=\"{1}\" name=\"chk_p_third\"  /><label for=\"chk_p_third_{1}\">{0}</label><span class=\"pno\">[{1}]</span></li>", permission.Name
                                    , permission.PrivsId, permission.Remark);
                            }
                        }

                        html.Append("</ul>");

                        if (i % 4 == 3)
                        {
                            html.Append("<ul class=\"pSmallSpace\"><li></li></ul>");
                        }
                        i++;
                    }

                    html.Append("<ul class=\"pSmallSpace\"><li></li></ul>");
                    html.Append("</div>");
                }
            }

            this.ltrPrivs.Text = html.ToString();

            string script = "var allPrivs={0};";
            script = string.Format(script, Newtonsoft.Json.JsonConvert.SerializeObject(items));
            this.RegisterScript(script);
        }
        #endregion

        #region btn click event
        /// <summary>
        /// btnAddFirst_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddFirst_Click(object sender, EventArgs e)
        {
            var info = new EyouSoft.Model.SysStructure.MSysMenu1Info();
            info.Name = EyouSoft.Common.Utils.GetFormValue("txtFirstName").Trim();
            info.ClassName = EyouSoft.Common.Utils.GetFormValue("radClassName").Trim();

            if (string.IsNullOrEmpty(info.Name))
            {
                this.RegisterAlertAndReloadScript("一级栏目名称不能为空！");
                return;
            }

            /*if (string.IsNullOrEmpty(info.ClassName))
            {
                this.RegisterAlertAndReloadScript("请选择一级栏目小图标！");
                return;
            }*/

            int identityId = new EyouSoft.BLL.SysStructure.BSys().InsertPrivs1(info);

            if (identityId > 0)
            {
                this.RegisterAlertAndReloadScript("一级栏目添加成功！");
                return;
            }
            else
            {
                this.RegisterAlertAndReloadScript("一级栏目添加失败！");
                return;
            }
        }

        /// <summary>
        /// btnAddSecond_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddSecond_Click(object sender, EventArgs e)
        {
            var info = new EyouSoft.Model.SysStructure.MSysMenu2Info();
            info.Name = EyouSoft.Common.Utils.GetFormValue("txtSecondName").Trim();
            info.Url = EyouSoft.Common.Utils.GetFormValue("txtSecondUrl").Trim();
            info.FirstId = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("txtSecondFirst"), 0);

            if (info.FirstId < 1)
            {
                this.RegisterAlertAndReloadScript("请选择第一级栏目！");
                return;
            }

            if (string.IsNullOrEmpty(info.Name))
            {
                this.RegisterAlertAndReloadScript("二级栏目名称不能为空！");
                return;
            }

            if (string.IsNullOrEmpty(info.Url))
            {
                this.RegisterAlertAndReloadScript("二级栏目链接不能为空！");
                return;
            }

            int identityId = new EyouSoft.BLL.SysStructure.BSys().InsertPrivs2(info);

            if (identityId > 0)
            {
                this.RegisterAlertAndReloadScript("二级栏目添加成功！");
                return;
            }
            else
            {
                this.RegisterAlertAndReloadScript("二级栏目添加失败！");
                return;
            }

        }

        /// <summary>
        /// btnAddThird_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddThird_Click(object sender, EventArgs e)
        {
            var info = new EyouSoft.Model.SysStructure.MSysPrivsInfo();
            info.Name = EyouSoft.Common.Utils.GetFormValue("txtPrivsName").Trim();
            info.SecondId = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("txtThirdSecond"));
            int privsType = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("txtPrivsType"), -1);

            if (info.SecondId < 1)
            {
                this.RegisterAlertAndReloadScript("请选择二级栏目！");
                return;
            }

            if (string.IsNullOrEmpty(info.Name))
            {
                
            }

            if (privsType == -1)
            {
                this.RegisterAlertAndReloadScript("请选择权限类别！");
                return;
            }

            info.PrivsType = (EyouSoft.Model.EnumType.SysStructure.PrivsType)privsType;
            int identityId = new EyouSoft.BLL.SysStructure.BSys().InsertPrivs3(info);

            if (identityId > 0)
            {
                this.RegisterAlertAndReloadScript("权限添加成功！");
                return;
            }
            else
            {
                this.RegisterAlertAndReloadScript("权限添加失败！");
                return;
            }
        }
        #endregion
    }
}
