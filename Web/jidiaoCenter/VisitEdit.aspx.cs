using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using EyouSoft.Common;
using System.Collections.Generic;

namespace Web.jidiaoCenter
{
    /// <summary>
    /// 创建人：刘飞
    /// 时间：2012-12-27
    /// 描述：回访新增/修改
    /// </summary>
    public partial class VisitEdit : EyouSoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tourid = Utils.GetQueryStringValue("tourid");
            string visitid = Utils.GetQueryStringValue("visitid");
            string dotype = Utils.GetQueryStringValue("dotype");
            PowerControl();
            switch (dotype)
            {
                case "save":
                    Response.Clear();
                    Response.Write(PageSave(tourid, visitid));
                    Response.End();
                    break;
                case "delete":
                    Response.Clear();
                    Response.Write(DeleteVisit(visitid));
                    Response.End();
                    break;
                default:
                    break;
            }
            if (!IsPostBack)
            {
                PageInit(tourid, visitid, dotype);
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="toutid"></param>
        /// <param name="visitid"></param>
        /// <param name="dotype"></param>
        private void PageInit(string toutid, string visitid, string dotype)
        {
            //绑定导游星级
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.TourStructure.Score));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.Score), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.TourStructure.Score), item);
                ListItem ddlItem = new ListItem();
                ddlItem.Text = text;
                ddlItem.Value = value.ToString();
                this.dptscore.Items.Add(ddlItem);
            }

            this.dptscore.Items.Insert(0, new ListItem("请选择", "0"));


            BindList(toutid);
            EyouSoft.BLL.TourStructure.BTourReturnVisit bll = new EyouSoft.BLL.TourStructure.BTourReturnVisit();
            //绑定列表（未完成）
            if (!string.IsNullOrEmpty(visitid) && dotype == "show")
            {
                EyouSoft.Model.TourStructure.MTourReturnVisit model = bll.GetModel(visitid);
                if (model != null)
                {
                    this.txtContent.Text = model.CustomerOpinion;
                    this.txtName.Text = model.QuanPeiName;
                    this.txtTel.Text = model.QuanPeiPhone;
                    this.txtvisiter.Text = model.Vistor;
                    this.txtvisittime.Text = model.VisitTime.HasValue ? Utils.GetDateTime(model.VisitTime.Value.ToString()).ToString("yyyy-MM-dd") : "";
                    if (this.dptscore.Items.FindByValue(((int)model.Score).ToString()) != null)
                    {
                        dptscore.Items.FindByValue(((int)model.Score).ToString()).Selected = true;
                    }
                }
            }
        }

        private void BindList(string tourid)
        {
            EyouSoft.BLL.TourStructure.BTourReturnVisit bll = new EyouSoft.BLL.TourStructure.BTourReturnVisit();
            IList<EyouSoft.Model.TourStructure.MTourReturnVisit> list = bll.GetList(tourid);
            if (list != null && list.Count > 0)
            {
                this.rptList.DataSource = list;
                this.rptList.DataBind();
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="tourid">团队编号</param>
        /// <param name="visitid">回访编号</param>
        /// <returns></returns>
        private string PageSave(string tourid, string visitid)
        {
            string msg = string.Empty;
            //t 为true 新增，false 修改
            bool t = (visitid != "" ? false : true);
            EyouSoft.BLL.TourStructure.BTourReturnVisit bll = new EyouSoft.BLL.TourStructure.BTourReturnVisit();
            EyouSoft.Model.TourStructure.MTourReturnVisit model = new EyouSoft.Model.TourStructure.MTourReturnVisit();
            model.CustomerOpinion = Utils.GetFormValue(this.txtContent.UniqueID);
            model.IssueTime = DateTime.Now;
            model.OperatorId = SiteUserInfo.UserId;
            model.QuanPeiPhone = Utils.GetFormValue(this.txtTel.UniqueID);
            model.Score = (EyouSoft.Model.EnumType.TourStructure.Score)Utils.GetInt(Utils.GetFormValue(this.dptscore.UniqueID));
            model.TourId = tourid;
            string quanpeiname = Utils.GetFormValue(this.txtName.UniqueID);
            if (string.IsNullOrEmpty(quanpeiname))
            {
                return UtilsCommons.AjaxReturnJson("0", "全陪姓名不能为空!");
            }
            model.QuanPeiName = quanpeiname;
            string visittime = Utils.GetFormValue(this.txtvisittime.UniqueID);
            if (string.IsNullOrEmpty(visittime))
            {
                return UtilsCommons.AjaxReturnJson("0", "回访时间不能为空!");
            }
            model.VisitTime = Utils.GetDateTimeNullable(visittime);
            model.Vistor = Utils.GetFormValue(this.txtvisiter.UniqueID);
            if (t)
            {
                int result = bll.Add(model);
                switch (result)
                {
                    case 0:
                        msg = UtilsCommons.AjaxReturnJson("0", "新增失败!");
                        break;
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "新增成功!");
                        break;
                    default:
                        break;
                }

            }
            else
            {
                model.VisitId = visitid;
                int result = bll.Update(model);
                switch (result)
                {
                    case 0:
                        msg = UtilsCommons.AjaxReturnJson("0", "修改失败!");
                        break;
                    case 1:
                        msg = UtilsCommons.AjaxReturnJson("1", "修改成功!");
                        break;
                    case -1:
                        msg = UtilsCommons.AjaxReturnJson("0", "团队质量评估已反馈不允许修改!");
                        break;
                    default:
                        break;
                }
            }
            return msg;
        }

        private string DeleteVisit(string visitid)
        {
            string msg = string.Empty;
            EyouSoft.BLL.TourStructure.BTourReturnVisit bll = new EyouSoft.BLL.TourStructure.BTourReturnVisit();
            int result = bll.Delete(visitid);
            switch (result)
            {
                case -1:
                    msg = UtilsCommons.AjaxReturnJson("0", "团队质量评估已反馈不允许删除!");
                    break;
                case 0:
                    msg = UtilsCommons.AjaxReturnJson("0", "删除失败!");
                    break;
                case 1:
                    msg = UtilsCommons.AjaxReturnJson("1", "删除成功!");
                    break;
                default:
                    break;
            }
            return msg;
        }

        /// <summary>
        /// 权限判断
        /// </summary>
        protected void PowerControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_回访提醒_新增))
            {
                this.btnsave.Visible = false;
            }
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.计调中心_回访提醒_修改))
            {
                this.btnsave.Visible = false;
            }

        }
    }
}
