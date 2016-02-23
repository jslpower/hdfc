using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;

namespace Web.Fin
{
    /// <summary>
    /// 付款登记
    /// </summary>
    public partial class FuKuan : EyouSoft.Common.Page.BackPage
    {
        /// <summary>
        /// 是否显示删除按钮
        /// </summary>
        protected bool IsShowDel = false;

        /// <summary>
        /// 付款项目类型集合  dj：地接；pwc：票务出票；pwt：票务退票；
        /// </summary>
        private readonly string[] _itemTypes = new[] { "dj", "pwc", "pwt" };

        protected void Page_Load(object sender, EventArgs e)
        {
            UploadControl1.CompanyID = CurrentUserCompanyID;

            string save = Utils.GetQueryStringValue("save");//是否保存
            string action = Utils.GetQueryStringValue("action").ToLower();//新增、修改、删除
            string itemId = Utils.GetQueryStringValue("itemId");//付款项目编号
            string strType = Utils.GetQueryStringValue("type").ToLower();//付款项目类型
            string dId = Utils.GetQueryStringValue("dId");//付款登记编号

            if (!string.IsNullOrEmpty(save))
            {
                Save(action, itemId, dId, strType);
                return;
            }
            if (action == "del")
            {
                DeleteDengJi(action, dId, strType);
                return;
            }

            if (!IsPostBack)
            {
                if (!CheckIdAndType(itemId, strType))
                {
                    Utils.ShowMsgAndCloseBoxy("url错误，请重新打开此窗口！", Utils.GetQueryStringValue("iframeId"), false);
                    return;
                }
                if (!CheckActionAndSId(action, dId))
                {
                    Utils.ShowMsgAndCloseBoxy("url错误，请重新打开此窗口！", Utils.GetQueryStringValue("iframeId"), false);
                    return;
                }
                txtOperator.Value = this.SiteUserInfo.Name;
                hidOperatorId.Value = this.SiteUserInfo.UserId.ToString();

                CheckIsShowDel(strType);
                InitYiFu(itemId, strType);
                this.InitDropDownList();
                if (action == "edit")
                {
                    this.InitFuKuanXinXi(dId, strType);
                }
            }
        }

        #region 验证函数

        /// <summary>
        /// 验证操作类型和编号，以及权限
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="sId">编号</param>
        /// <returns></returns>
        private bool CheckActionAndSId(string action, string sId)
        {
            bool r = true;
            if (action == "edit")
            {
                if (string.IsNullOrEmpty(sId))
                {
                    r = false;
                }
            }

            return r;
        }

        /// <summary>
        /// 验证付款项目的信息
        /// </summary>
        /// <param name="itemId">付款项目编号</param>
        /// <param name="type">付款项目类型</param>
        /// <returns>
        /// 验证通过返回true；不通过返回false
        /// </returns>
        private bool CheckIdAndType(string itemId, string type)
        {
            if (string.IsNullOrEmpty(itemId) || string.IsNullOrEmpty(type) || !_itemTypes.Contains(type))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 根据操作类型和付款项目类型获取权限
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="type">付款项目类型</param>
        /// <returns></returns>
        private EyouSoft.Model.EnumType.PrivsStructure.Privs3 GetHandlePriv(string action, string type)
        {
            int priv = 0;
            if (string.IsNullOrEmpty(action) || action == "add")
            {
                switch (type)
                {
                    case "dj":
                        priv = (int)EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应付管理_地接付款新增;
                        break;
                    case "pwc":
                    case "pwt":
                        priv = (int)EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应付管理_票务付款新增;
                        break;
                }
            }
            if (action == "edit")
            {
                switch (type)
                {
                    case "dj":
                        priv = (int)EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应付管理_地接付款修改;
                        break;
                    case "pwc":
                    case "pwt":
                        priv = (int)EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应付管理_票务付款修改;
                        break;
                }
            }
            if (action == "del")
            {
                switch (type)
                {
                    case "dj":
                        priv = (int)EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应付管理_地接付款删除;
                        break;
                    case "pwc":
                    case "pwt":
                        priv = (int)EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应付管理_票务付款删除;
                        break;
                }
            }

            return (EyouSoft.Model.EnumType.PrivsStructure.Privs3)priv;
        }

        /// <summary>
        ///  操作时验证权限
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="sId">付款登记信息编号</param>
        /// <param name="type">付款登记项目类型</param>
        private string CheckPrive(string action, string sId, string type)
        {
            var str = new StringBuilder();
            if (!this.CheckGrant(this.GetHandlePriv(action, type)))
            {
                str.AppendFormat(
                    "您没有{0}的权限，请联系管理员！",
                    this.GetHandlePriv(action, type));
            }
            if (action == "edit")
            {
                if (string.IsNullOrEmpty(sId))
                {
                    str.Append("url错误，请重新打开此窗口！");
                }
            }
            if (action == "del")
            {
                if (string.IsNullOrEmpty(sId))
                {
                    str.Append("url错误，请重新打开此窗口！");
                }
            }

            return str.ToString();
        }

        /// <summary>
        /// 是否显示删除按钮
        /// </summary>
        /// <param name="type"></param>
        private void CheckIsShowDel(string type)
        {
            if (this.CheckGrant(this.GetHandlePriv("del", type)))
            {
                IsShowDel = true;
                return;
            }

            IsShowDel = false;
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化收款列表
        /// </summary>
        /// <param name="itemId">收款项目编号</param>
        /// <param name="type">付款项目类型</param>
        private void InitYiFu(string itemId, string type)
        {
            ltrYingFu.Text = this.ToMoneyString(this.GetYingFu(itemId, type));
            decimal yiShou = 0M;
            object list = this.GetDataSource(itemId, type, ref yiShou);
            ltrYiFu.Text = this.ToMoneyString(yiShou);
            rptFuKuan.DataSource = list;
            rptFuKuan.DataBind();
        }

        /// <summary>
        /// 获取数据源信息
        /// </summary>
        /// <param name="itemId">付款项目编号</param>
        /// <param name="type">付款项目类型</param>
        /// <param name="yiShou">已收</param>
        /// <returns></returns>
        private object GetDataSource(string itemId, string type, ref decimal yiShou)
        {
            yiShou = 0;
            object list = null;

            switch (type)
            {
                case "dj":
                    var tmpdj = new EyouSoft.BLL.FinStructure.BDiJieFuKuan().GetFinCopeList(itemId);
                    if (tmpdj != null && tmpdj.Any())
                    {
                        yiShou = tmpdj.Sum(t => (t.JinE));
                    }
                    list = tmpdj;
                    break;
                case "pwc":
                    var tmppwc =
                        new EyouSoft.BLL.FinStructure.BPiaoFuKuan().GetFinCopeList(
                            EyouSoft.Model.EnumType.PlanStructure.TicketMode.出票, itemId);
                    if (tmppwc != null && tmppwc.Any())
                    {
                        yiShou = tmppwc.Sum(t => (t.JinE));
                    }
                    list = tmppwc;
                    break;
                case "pwt":
                    var tmppwt =
                        new EyouSoft.BLL.FinStructure.BPiaoFuKuan().GetFinCopeList(
                            EyouSoft.Model.EnumType.PlanStructure.TicketMode.退票, itemId);
                    if (tmppwt != null && tmppwt.Any())
                    {
                        yiShou = tmppwt.Sum(t => (t.JinE));
                    }
                    list = tmppwt;
                    break;
            }

            return list;
        }

        /// <summary>
        /// 获取应付金额
        /// </summary>
        /// <param name="itemId">付款项目编号</param>
        /// <param name="type">付款项目类型</param>
        /// <returns></returns>
        private decimal GetYingFu(string itemId, string type)
        {
            decimal r = 0M;
            switch (type)
            {
                case "dj":
                    var tmpdj = new EyouSoft.BLL.PlanStructure.BPlanDiJie().GetModel(itemId);
                    if (tmpdj != null) r = tmpdj.SumPrice;
                    break;
                case "pwc":
                case "pwt":
                    var tmppw = new EyouSoft.BLL.PlanStructure.BPlanTicket().GetModel(itemId);
                    if (tmppw != null) r = tmppw.SumPrice;
                    break;
            }
            return r;
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitDropDownList()
        {
            var list = new EyouSoft.BLL.CompanyStructure.BYinHangZhangHu().GetZhangHus(CurrentUserCompanyID);
            if (list != null && list.Any())
            {
                foreach (var t in list)
                {
                    if (t == null) continue;
                    ddlBankId.Items.Add(new ListItem(t.BankName + " " + t.AccountName + " " + t.BankNo, t.Id));
                }
            }
            ddlBankId.Items.Insert(0, new ListItem("请选择", "-1"));

            ddlFangShi.DataSource = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi));
            ddlFangShi.DataTextField = "Text";
            ddlFangShi.DataValueField = "Value";
            ddlFangShi.DataBind();
        }

        /// <summary>
        /// 初始化收款信息
        /// </summary>
        /// <param name="dId">付款登记信息编号</param>
        /// <param name="type">付款项目类型</param>
        private void InitFuKuanXinXi(string dId, string type)
        {
            if (string.IsNullOrEmpty(dId)) return;

            EyouSoft.Model.FinStructure.MKuanBase model = null;

            switch (type)
            {
                case "dj":
                    model = this.GetBaseModel(new EyouSoft.BLL.FinStructure.BDiJieFuKuan().GetFinCope(dId), type);
                    break;
                case "pwc":
                case "pwt":
                    model = this.GetBaseModel(new EyouSoft.BLL.FinStructure.BPiaoFuKuan().GetFinCope(dId), type);
                    break;
            }

            if (model == null) return;

            txtDate.Value = model.ShouKuanRiQi.ToShortDateString();
            txtOperator.Value = model.ShouKuanRenName;
            hidOperatorId.Value = model.ShouKuanRenId.ToString();
            txtMoney.Value = model.JinE != 0 ? model.JinE.ToString("F2") : string.Empty;
            txtRemark.Value = model.ShouKuanBeiZhu;

            if (ddlBankId.Items.FindByValue(model.ZhangHuId) != null)
                ddlBankId.Items.FindByValue(model.ZhangHuId).Selected = true;
            if (ddlFangShi.Items.FindByValue(Convert.ToInt32(model.FangShi).ToString()) != null)
                ddlFangShi.Items.FindByValue(Convert.ToInt32(model.FangShi).ToString()).Selected = true;
            if (ddlKaiPiao.Items.FindByValue(model.IsKaiPiao ? "1" : "0") != null)
                ddlKaiPiao.Items.FindByValue(model.IsKaiPiao ? "1" : "0").Selected = true;

            if (model.File != null)
            {
                UploadControl1.YuanFiles = (from c in model.File
                                            where c != null
                                            select
                                                new UserControl.MFileInfo
                                                {
                                                    FileId = c.FileId.ToString(),
                                                    FileName = c.FileName,
                                                    FilePath = c.FilePath
                                                }).ToList();
            }
        }

        /// <summary>
        /// 获取base信息
        /// </summary>
        /// <param name="obj">子类</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private EyouSoft.Model.FinStructure.MKuanBase GetBaseModel(object obj, string type)
        {
            if (obj == null) return null;

            switch (type)
            {
                case "dj":
                    var tmpdj = (EyouSoft.Model.FinStructure.MDiJieFuKuan)obj;
                    return new EyouSoft.Model.FinStructure.MKuanBase
                        {
                            CompanyId = tmpdj.CompanyId,
                            DengJiId = tmpdj.DengJiId,
                            FangShi = tmpdj.FangShi,
                            File = tmpdj.File,
                            IsKaiPiao = tmpdj.IsKaiPiao,
                            IssueTime = tmpdj.IssueTime,
                            ItemName = tmpdj.ItemName,
                            JinE = tmpdj.JinE,
                            OperatorId = tmpdj.OperatorId,
                            OtherPrice = tmpdj.OtherPrice,
                            ShouKuanBeiZhu = tmpdj.ShouKuanBeiZhu,
                            ShouKuanRenId = tmpdj.ShouKuanRenId,
                            ShouKuanRenName = tmpdj.ShouKuanRenName,
                            ShouKuanRiQi = tmpdj.ShouKuanRiQi,
                            Status = tmpdj.Status,
                            ZhangHuCode = tmpdj.ZhangHuCode,
                            ZhangHuId = tmpdj.ZhangHuId
                        };
                case "pwc":
                case "pwt":
                    var tmppw = (EyouSoft.Model.FinStructure.MPiaoFuKuan)obj;
                    return new EyouSoft.Model.FinStructure.MKuanBase
                    {
                        CompanyId = tmppw.CompanyId,
                        DengJiId = tmppw.DengJiId,
                        FangShi = tmppw.FangShi,
                        File = tmppw.File,
                        IsKaiPiao = tmppw.IsKaiPiao,
                        IssueTime = tmppw.IssueTime,
                        ItemName = tmppw.ItemName,
                        JinE = tmppw.JinE,
                        OperatorId = tmppw.OperatorId,
                        OtherPrice = tmppw.OtherPrice,
                        ShouKuanBeiZhu = tmppw.ShouKuanBeiZhu,
                        ShouKuanRenId = tmppw.ShouKuanRenId,
                        ShouKuanRenName = tmppw.ShouKuanRenName,
                        ShouKuanRiQi = tmppw.ShouKuanRiQi,
                        Status = tmppw.Status,
                        ZhangHuCode = tmppw.ZhangHuCode,
                        ZhangHuId = tmppw.ZhangHuId
                    };
            }

            return null;
        }

        #endregion

        #region  前台方法

        /// <summary>
        /// 获取附件下载链接
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetFilePath(object obj)
        {
            if (obj == null) return string.Empty;

            var list = (IList<EyouSoft.Model.FinStructure.MKuanFile>)obj;
            if (!list.Any()) return string.Empty;

            return
                string.Format(
                    "<a title=\"点击下载\" target=\"_blank\" href=\"{0}\"><img alt=\"{1}\" src=\"/images/fujian_bg.gif\" width=\"15\" height=\"14\" /></a>",
                    list[0].FilePath,
                    list[0].FileName);
        }

        #endregion

        #region 按钮事件

        /// <summary>
        /// 删除登记信息
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="dId">付款登记信息编号</param>
        /// <param name="type">付款项目类型</param>
        private void DeleteDengJi(string action, string dId, string type)
        {
            var msg = CheckPrive(action, dId, type);
            if (!string.IsNullOrEmpty(msg))
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", msg));
                return;
            }

            int r = 0;

            switch (type)
            {
                case "dj":
                    r = new EyouSoft.BLL.FinStructure.BDiJieFuKuan().DeleteFinCope(dId);
                    break;
                case "pwc":
                case "pwt":
                    r = new EyouSoft.BLL.FinStructure.BPiaoFuKuan().DeleteFinCope(dId);
                    break;
            }

            this.RCWE(UtilsCommons.AjaxReturnJson(r == 1 ? "1" : "0", r == 1 ? "删除成功！" : "删除失败！"));
        }

        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="itemId">收款项目编号</param>
        /// <param name="dId">收款登记信息编号</param>
        /// <param name="type">付款项目类型</param>
        private void Save(string action, string itemId, string dId, string type)
        {
            if (!CheckIdAndType(itemId, type))
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", "url错误，请重新打开此窗口！"));
                return;
            }
            var msg = CheckPrive(action, dId, type);
            if (!string.IsNullOrEmpty(msg))
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", msg));
                return;
            }

            var model = this.GetFormValue(dId);
            int r = 0;

            if (string.IsNullOrEmpty(action) || action == "add")
            {
                switch (type)
                {
                    case "dj":
                        r = new EyouSoft.BLL.FinStructure.BDiJieFuKuan().AddFinCope(
                                (EyouSoft.Model.FinStructure.MDiJieFuKuan)this.GetModelByType(itemId, type, model));
                        break;
                    case "pwc":
                    case "pwt":
                        r = new EyouSoft.BLL.FinStructure.BPiaoFuKuan().AddFinCope(
                                (EyouSoft.Model.FinStructure.MPiaoFuKuan)this.GetModelByType(itemId, type, model));
                        break;
                }
            }
            else if (action == "edit")
            {
                switch (type)
                {
                    case "dj":
                        r = new EyouSoft.BLL.FinStructure.BDiJieFuKuan().UpdateFinCope(
                                (EyouSoft.Model.FinStructure.MDiJieFuKuan)this.GetModelByType(itemId, type, model));
                        break;
                    case "pwc":
                    case "pwt":
                        r = new EyouSoft.BLL.FinStructure.BPiaoFuKuan().UpdateFinCope(
                                (EyouSoft.Model.FinStructure.MPiaoFuKuan)this.GetModelByType(itemId, type, model));
                        break;
                }
            }

            switch (r)
            {
                case 0:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "操作失败！"));
                    break;
                case 1:
                    this.RCWE(UtilsCommons.AjaxReturnJson("1", "操作成功！"));
                    break;
                case -1:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "付款总登记之和超过应付！"));
                    break;
                default:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "操作失败！"));
                    break;
            }
        }

        /// <summary>
        /// 根据类型生成对应的model
        /// </summary>
        /// <param name="itemId">收款项目编号</param>
        /// <param name="type">付款项目类型</param>
        /// <param name="kuanBase">基类信息</param>
        /// <returns></returns>
        private object GetModelByType(string itemId, string type, EyouSoft.Model.FinStructure.MKuanBase kuanBase)
        {
            switch (type)
            {
                case "dj":
                    return new EyouSoft.Model.FinStructure.MDiJieFuKuan
                        {
                            CompanyId = kuanBase.CompanyId,
                            DengJiId = kuanBase.DengJiId,
                            FangShi = kuanBase.FangShi,
                            DiJiePlanId = itemId,
                            File = kuanBase.File,
                            IsKaiPiao = kuanBase.IsKaiPiao,
                            IssueTime = kuanBase.IssueTime,
                            ItemName = kuanBase.ItemName,
                            JinE = kuanBase.JinE,
                            OperatorId = kuanBase.OperatorId,
                            OtherPrice = kuanBase.OtherPrice,
                            ShouKuanBeiZhu = kuanBase.ShouKuanBeiZhu,
                            ShouKuanRenId = kuanBase.ShouKuanRenId,
                            ShouKuanRenName = kuanBase.ShouKuanRenName,
                            ShouKuanRiQi = kuanBase.ShouKuanRiQi,
                            Status = kuanBase.Status,
                            ZhangHuCode = kuanBase.ZhangHuCode,
                            ZhangHuId = kuanBase.ZhangHuId
                        };
                case "pwc":
                case "pwt":
                    return new EyouSoft.Model.FinStructure.MPiaoFuKuan
                        {
                            CompanyId = kuanBase.CompanyId,
                            DengJiId = kuanBase.DengJiId,
                            FangShi = kuanBase.FangShi,
                            PiaoPlanId = itemId,
                            TicketType =
                                type == "pwc"
                                    ? EyouSoft.Model.EnumType.PlanStructure.TicketMode.出票
                                    : EyouSoft.Model.EnumType.PlanStructure.TicketMode.退票,
                            File = kuanBase.File,
                            IsKaiPiao = kuanBase.IsKaiPiao,
                            IssueTime = kuanBase.IssueTime,
                            ItemName = kuanBase.ItemName,
                            JinE = kuanBase.JinE,
                            OperatorId = kuanBase.OperatorId,
                            OtherPrice = kuanBase.OtherPrice,
                            ShouKuanBeiZhu = kuanBase.ShouKuanBeiZhu,
                            ShouKuanRenId = kuanBase.ShouKuanRenId,
                            ShouKuanRenName = kuanBase.ShouKuanRenName,
                            ShouKuanRiQi = kuanBase.ShouKuanRiQi,
                            Status = kuanBase.Status,
                            ZhangHuCode = kuanBase.ZhangHuCode,
                            ZhangHuId = kuanBase.ZhangHuId
                        };
            }

            return null;
        }

        /// <summary>
        /// 获取表单值
        /// </summary>
        /// <param name="dId">收款登记信息编号</param>
        /// <returns></returns>
        private EyouSoft.Model.FinStructure.MKuanBase GetFormValue(string dId)
        {
            var model = new EyouSoft.Model.FinStructure.MKuanBase
            {
                CompanyId = CurrentUserCompanyID,
                DengJiId = dId,
                FangShi =
                    (EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi)
                    Utils.GetInt(Utils.GetFormValue(ddlFangShi.UniqueID)),
                IsKaiPiao = Utils.GetFormValue(ddlKaiPiao.UniqueID) == "1" ? true : false,
                IssueTime = DateTime.Now,
                ItemName = string.Empty,
                JinE = Utils.GetDecimal(Utils.GetFormValue(txtMoney.UniqueID)),
                OperatorId = this.SiteUserInfo.UserId,
                ShouKuanBeiZhu = Utils.GetFormValue(txtRemark.UniqueID),
                ShouKuanRenId = Utils.GetInt(Utils.GetFormValue(hidOperatorId.UniqueID)),
                ShouKuanRenName = Utils.GetFormValue(txtOperator.UniqueID),
                ShouKuanRiQi = Utils.GetDateTime(Utils.GetFormValue(txtDate.UniqueID)),
                Status = EyouSoft.Model.EnumType.FinStructure.KuanXiangStatus.未支付,
                ZhangHuId = Utils.GetFormValue(ddlBankId.UniqueID),
                File = this.GetFileList(dId)
            };

            return model;
        }

        /// <summary>
        /// 获取上传的文件
        /// </summary>
        /// <param name="dengJiId">收款登记编号</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.FinStructure.MKuanFile> GetFileList(string dengJiId)
        {

            IList<EyouSoft.Model.FinStructure.MKuanFile> list = null;

            var fileNew = UploadControl1.Files;
            var fileOld = UploadControl1.YuanFiles;
            if (fileNew != null && fileNew.Any())
            {
                list = (from c in fileNew
                        where c != null
                        select
                            new EyouSoft.Model.FinStructure.MKuanFile
                            {
                                DengJiId = dengJiId,
                                FileId = Utils.GetInt(c.FileId),
                                FileName = c.FileName,
                                FilePath = c.FilePath
                            }).ToList();
            }
            else
            {
                if (fileOld != null && fileOld.Any())
                {
                    list = (from c in fileOld
                            where c != null
                            select
                                new EyouSoft.Model.FinStructure.MKuanFile
                                {
                                    DengJiId = dengJiId,
                                    FileId = Utils.GetInt(c.FileId),
                                    FileName = c.FileName,
                                    FilePath = c.FilePath
                                }).ToList();
                }
            }

            return list;
        }

        #endregion
    }
}
