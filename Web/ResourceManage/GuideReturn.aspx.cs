using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.ResourceManage
{
    public partial class WebForm1 : EyouSoft.Common.Page.BackPage
    {
        #region 设置分页变量
        protected int pageIndex = 1;
        protected int recordCount;
        protected int pageSize = 10;
        #endregion

        #region 权限变量
        protected bool add = false;//新增
        protected bool update = false;//修改
        protected bool del = false;//删除
        protected bool back = false;//反馈 
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            PowControl();

            //ajax for delete
            string type = Utils.GetQueryStringValue("Type");
            if (!string.IsNullOrEmpty(type))
            {
                if (type.Equals("del"))
                {
                    Response.Clear();
                    Response.Write(Delete());
                    Response.End();
                }

                if (type.Equals("save"))
                {
                    Response.Clear();
                    Response.Write(Save());
                    Response.End();
                }
            }

            if (!IsPostBack)
            {
                BindFanKuiType();
                Bind();
            }
        }


        protected void Bind()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);

            EyouSoft.BLL.SourceStructure.BGuideSupplier bll = new EyouSoft.BLL.SourceStructure.BGuideSupplier();
            string guideId = Utils.GetQueryStringValue("tid");
            this.hdguideId.Value = guideId;

            IList<EyouSoft.Model.SourceStructure.MGuideFanKui> list = bll.GetList(guideId, pageSize, pageIndex, ref recordCount);
            //绑定数据
            UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
            this.rpGuideReturn.DataSource = list;
            this.rpGuideReturn.DataBind();

            BindPage();
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion

        #region 绑定反馈类型
        /// <summary>
        /// 绑定反馈类型
        /// </summary>
        protected void BindFanKuiType()
        {
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.SourceStructure.FanKuiType));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.SourceStructure.FanKuiType), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.SourceStructure.FanKuiType), item);
                ListItem ddlItem = new ListItem();
                ddlItem.Text = text;
                ddlItem.Value = value.ToString();
                this.ddlFanKuiType.Items.Add(ddlItem);
            }
        }
        #endregion


        /// <summary>
        /// 保存或修改信息
        /// </summary>
        private string Save()
        {

            string msg = string.Empty;
            EyouSoft.BLL.SourceStructure.BGuideSupplier bll = new EyouSoft.BLL.SourceStructure.BGuideSupplier();

            EyouSoft.Model.SourceStructure.MGuideFanKui model = new EyouSoft.Model.SourceStructure.MGuideFanKui();
            model.Id = Utils.GetInt(Utils.GetFormValue(this.hdfankuiId.UniqueID));
            model.GuideId = Utils.GetFormValue(this.hdguideId.UniqueID);
            model.FanKuiType = (EyouSoft.Model.EnumType.SourceStructure.FanKuiType)Utils.GetInt(Utils.GetFormValue(this.ddlFanKuiType.UniqueID));
            model.FanKuiTime = Utils.GetDateTimeNullable(Utils.GetFormValue(this.txtFanKuiTime.UniqueID));
            model.FanKuiRemark = Utils.GetFormValue(this.txtFanKuiRemark.UniqueID);


            if (model.Id == 0)
            {
                if (bll.Add(model) == 1)
                {
                    msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("1", "添加成功！");
                }
                else
                {
                    msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "添加失败！");
                }
            }
            else
            {
                if (bll.Update(model) == 1)
                {
                    msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("1", "修改成功！");
                }
                else
                {
                    msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "修改失败！");
                }
            }

            return msg;


        }

        #region 删除操作
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        private string Delete()
        {
            string msg = null;
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_导游_导游反馈删除))
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "无删除权限！");
            }
            int id = Utils.GetInt(Utils.GetFormValue("id"));
            EyouSoft.BLL.SourceStructure.BGuideSupplier bll = new EyouSoft.BLL.SourceStructure.BGuideSupplier();
            if (bll.Delete(id) != 0)
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("1", "删除成功！");
            }
            else
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "删除成功！");
            }
            return msg;
        }
        #endregion

        #region 权限判断
        private void PowControl()
        {
            add = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_导游_导游反馈新增);
            update = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_导游_导游反馈修改);
            del = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_导游_导游反馈删除);
        }

        #endregion


    }
}
