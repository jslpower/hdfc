using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SystemSet
{
    public partial class RatingAdd : EyouSoft.Common.Page.BackPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Utils.GetQueryStringValue("type");
            string ratingid = Utils.GetQueryStringValue("ratingid");
            string dotype = Utils.GetQueryStringValue("dotype");
            PowerControl();
            //存在ajax请求
            switch (type)
            {
                case "save":
                    Response.Clear();
                    Response.Write(Save(dotype, ratingid));
                    Response.End();
                    break;
            }

            //获得操作ID
            if (!IsPostBack)
            {
                PageInit(ratingid, dotype);
            }

        }
        private void PageInit(string ratingid, string dotype)
        {
            if (String.Equals(dotype, "update", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(ratingid))
            {
                EyouSoft.BLL.CompanyStructure.Rating bllRating = new EyouSoft.BLL.CompanyStructure.Rating();
                EyouSoft.Model.CompanyStructure.Rating model = new EyouSoft.Model.CompanyStructure.Rating();
                model = bllRating.GetModel(Utils.GetInt(ratingid));
                if (model != null)
                {
                    this.txtRatingName.Text = model.RatingName;
                }
            }
        }

        private string Save(string dotype, string ratingid)
        {
            //t为false为编辑，true时为新增
            bool t = String.Equals(dotype, "update", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(ratingid) ? false : true;
            EyouSoft.BLL.CompanyStructure.Rating bllRating = new EyouSoft.BLL.CompanyStructure.Rating();
            EyouSoft.Model.CompanyStructure.Rating model = new EyouSoft.Model.CompanyStructure.Rating();

            //是否存在相同的客户信用等级
            string name = this.txtRatingName.Text.Trim();//客户信用等级名称
            int id = Utils.GetInt(ratingid);
            bool isExistResult = bllRating.IsExists(name, this.SiteUserInfo.CompanyId, id);
            if (isExistResult)
                return UtilsCommons.AjaxReturnJson("2", "客户信用等级已存在!");


            if (string.IsNullOrEmpty(this.txtRatingName.Text.Trim()))
            {
                return UtilsCommons.AjaxReturnJson("0", "请填写客户信用等级!");
            }
            model.RatingName = this.txtRatingName.Text.Trim();
            model.CompanyId = this.SiteUserInfo.CompanyId;
            model.IssueTime = DateTime.Now;
            model.OperatorId = this.SiteUserInfo.UserId;
            string msg = string.Empty;
            if (t)
            {
                if (bllRating.Add(model))
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
                model.Id = Utils.GetInt(ratingid);
                if (bllRating.Update(model))
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
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.系统设置_基础设置_信用等级栏目))
            {
                this.btn.Visible = false;
            }

        }
    }
}
