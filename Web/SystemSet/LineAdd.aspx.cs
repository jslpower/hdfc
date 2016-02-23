using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SystemSet
{
    public partial class LineAdd : EyouSoft.Common.Page.BackPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Utils.GetQueryStringValue("type");
            string areaid = Utils.GetQueryStringValue("areaid");
            string dotype = Utils.GetQueryStringValue("dotype");
            PowerControl();
            //存在ajax请求
            switch (type)
            {
                case "save":
                    Response.Clear();
                    Response.Write(Save(dotype, areaid));
                    Response.End();
                    break;
            }

            //获得操作ID
            if (!IsPostBack)
            {
                PageInit(areaid, dotype);
            }

        }
        private void PageInit(string areaid, string dotype)
        {
            if (String.Equals(dotype, "update", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(areaid))
            {
                EyouSoft.BLL.CompanyStructure.Area bllarea = new EyouSoft.BLL.CompanyStructure.Area();
                EyouSoft.Model.CompanyStructure.Area model = new EyouSoft.Model.CompanyStructure.Area();
                model = bllarea.GetModel(Utils.GetInt(areaid));
                if (model != null)
                {
                    this.txtAreaName.Text = model.AreaName;
                }
            }
        }

        private string Save(string dotype, string areaid)
        {
            //t为false为编辑，true时为新增
            bool t = String.Equals(dotype, "update", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(areaid) ? false : true;
            EyouSoft.BLL.CompanyStructure.Area bllarea = new EyouSoft.BLL.CompanyStructure.Area();
            EyouSoft.Model.CompanyStructure.Area model = new EyouSoft.Model.CompanyStructure.Area();

            //是否存在相同的线路区域
            string name = this.txtAreaName.Text.Trim();//获取线路区域名称
            int id = Utils.GetInt(areaid);
            bool isExistResult = bllarea.IsExists(name, this.SiteUserInfo.CompanyId, id);
            if (isExistResult)
                return UtilsCommons.AjaxReturnJson("2", "线路区域已存在!");


            if (string.IsNullOrEmpty(this.txtAreaName.Text.Trim()))
            {
                return UtilsCommons.AjaxReturnJson("0", "请填写线路区域!");
            }
            model.AreaName = this.txtAreaName.Text.Trim();
            model.CompanyId = this.SiteUserInfo.CompanyId;
            model.IssueTime = DateTime.Now;
            model.OperatorId = this.SiteUserInfo.UserId;
            string msg = string.Empty;
            if (t)
            {
                if (bllarea.Add(model))
                {
                    return UtilsCommons.AjaxReturnJson("1", "添加成功!");
                }
                else
                {
                    return UtilsCommons.AjaxReturnJson("0", "添加失败!");
                }
            }
            else
            {
                model.Id = Utils.GetInt(areaid);
                if (bllarea.Update(model))
                {
                    return UtilsCommons.AjaxReturnJson("1", "修改成功!");
                }
                else
                {
                    return UtilsCommons.AjaxReturnJson("0", "修改失败!");
                }
            }
        }
        /// <summary>
        /// 权限判断
        /// </summary>
        protected void PowerControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_线路区域栏目))
            {
                this.btn.Visible = false;
            }

        }
    }
}
