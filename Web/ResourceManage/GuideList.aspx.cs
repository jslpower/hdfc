using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.ResourceManage
{
    public partial class GuideList : EyouSoft.Common.Page.BackPage
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

            //Ajax for delete 
            string act = Utils.GetQueryStringValue("act");
            if (!string.IsNullOrEmpty(act))
            {
                if (act.Equals("areadel"))
                {
                    Response.Clear();
                    Response.Write(Del());
                    Response.End();
                }
            }


            if (!IsPostBack)
            {
                Bind();
            }

        }

        protected void Bind()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);

            EyouSoft.BLL.SourceStructure.BGuideSupplier bll = new EyouSoft.BLL.SourceStructure.BGuideSupplier();
            string guideName = Utils.GetQueryStringValue("txtGuideName");
            string gysName = Utils.GetQueryStringValue("txtGysName");
            IList<EyouSoft.Model.SourceStructure.MPageGuide> list = bll.GetList(SiteUserInfo.CompanyId, pageSize, pageIndex, ref recordCount, guideName,gysName);

            //绑定数据
            if (list != null && list.Count > 0)
            {
                UtilsCommons.Paging(pageSize, ref pageIndex, recordCount);
                this.rpGuide.DataSource = list;
                this.rpGuide.DataBind();
            }
            else
            {
                Literal1.Text = "<tr align=\"center\"> <td colspan=\"7\">没有相关数据</td></tr>";
            }

            BindPage();
        }

        /// <summary>
        /// 获取附件路径
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetFilePath(object obj)
        {
            string file = "<a href='javascript:void(0)'><img src='/images/fujian_bg.gif' alt='' width='15' height='14' /></a>";

            if (obj != null)
            {
                IList<EyouSoft.Model.SourceStructure.MFileInfo> list = obj as IList<EyouSoft.Model.SourceStructure.MFileInfo>;
                if (list != null && list.Count > 0)
                {
                    file = string.Format("<a target='_blank' href='{0}'> <img src='/images/fujian_bg.gif' alt='' width='15' height='14' /></a>", list.FirstOrDefault().FilePath);
                }
            }
            return file;
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        private string Del()
        {
            string msg = null;
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_导游_删除))
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "无删除权限！");
            }
            EyouSoft.BLL.SourceStructure.BGuideSupplier bll = new EyouSoft.BLL.SourceStructure.BGuideSupplier();
            string stid = Utils.GetFormValue("tid");
            int flg = bll.Delete(stid);
            if (flg == 1)
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("1", "删除成功！");
            }
            else if (flg == -1)
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "确认件安排过该导游 不允许删除！");
            }
            else
            {
                msg = EyouSoft.Common.UtilsCommons.AjaxReturnJson("0", "删除失败！");
            }


            return msg;
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


        #region 权限判断
        private void PowControl()
        {
            if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_导游_栏目))
            {
                Utils.ResponseNoPermit(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_导游_栏目, true);
            }
            add = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_导游_新增);
            update = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_导游_修改);
            del = this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.供应商管理_导游_删除);
        }
        #endregion



    }
}
