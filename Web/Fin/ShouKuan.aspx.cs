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
    /// 收款登记
    /// </summary>
    public partial class ShouKuan : EyouSoft.Common.Page.BackPage
    {
        /// <summary>
        /// 是否显示删除按钮
        /// </summary>
        protected bool IsShowDel = false;
        /// <summary>
        /// 是否显示修改按钮
        /// </summary>
        protected bool IsShowEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            UploadControl1.CompanyID = CurrentUserCompanyID;

            string save = Utils.GetQueryStringValue("save");
            string action = Utils.GetQueryStringValue("action").ToLower();
            string itemId = Utils.GetQueryStringValue("itemId");
            string sId = Utils.GetQueryStringValue("sId");

            if (!string.IsNullOrEmpty(save))
            {
                Save(action, itemId, sId);
                return;
            }
            if (action == "del")
            {
                DeleteDengJi(action, sId);
                return;
            }
            if (!IsPostBack)
            {
                if (!CheckIdAndType(itemId))
                {
                    Utils.ShowMsgAndCloseBoxy("url错误，请重新打开此窗口！", Utils.GetQueryStringValue("iframeId"), false);
                    return;
                }
                if (!CheckActionAndSId(action, sId))
                {
                    Utils.ShowMsgAndCloseBoxy("url错误，请重新打开此窗口！", Utils.GetQueryStringValue("iframeId"), false);
                    return;
                }
                txtOperator.Value = this.SiteUserInfo.Name;
                hidOperatorId.Value = this.SiteUserInfo.UserId.ToString();

                InitYiShou(itemId);
                this.InitDropDownList();
                if (action == "edit")
                {
                    this.InitShouKuanXinXi(sId);
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
        /// 验证收款项目的信息
        /// </summary>
        /// <param name="itemId">收款项目编号</param>
        /// <returns>
        /// 验证通过返回true；不通过返回false
        /// </returns>
        private bool CheckIdAndType(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///  操作时验证权限
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="sId">收款登记信息编号</param>
        private string CheckPrive(string action, string sId)
        {
            var str = new StringBuilder();
            if (string.IsNullOrEmpty(action) || action == "add")
            {
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_收款新增))
                {
                    str.AppendFormat(
                        "您没有{0}的权限，请联系管理员！",
                        EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_收款新增);
                }
            }
            if (action == "edit")
            {
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_收款修改))
                {
                    str.AppendFormat(
                        "您没有{0}的权限，请联系管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_收款修改);

                }
                if (string.IsNullOrEmpty(sId))
                {
                    str.Append("url错误，请重新打开此窗口！");
                }
            }
            if (action == "del")
            {
                if (!this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_收款删除))
                {
                    str.AppendFormat(
                        "您没有{0}的权限，请联系管理员！", EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_收款删除);

                }
                if (string.IsNullOrEmpty(sId))
                {
                    str.Append("url错误，请重新打开此窗口！");
                }
            }

            return str.ToString();
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化收款列表
        /// </summary>
        /// <param name="itemId">收款项目编号</param>
        private void InitYiShou(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return;

            var model = new EyouSoft.BLL.TourStructure.BTour().GetModel(itemId);
            if (model != null)
            {
                ltrYingShou.Text = this.ToMoneyString(model.SumPrice);
            }
            var list = new EyouSoft.BLL.FinStructure.BShouKuan().GetFinCopeList(itemId);
            rptShouKuan.DataSource = list;
            rptShouKuan.DataBind();

            if (list != null && list.Any())
            {
                ltrYiShou.Text = this.ToMoneyString(list.Sum(t => (t.JinE)));
            }
            if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_收款修改))
            {
                IsShowEdit = true;
            }
            if (this.CheckGrant(EyouSoft.Model.EnumType.PrivsStructure.Privs3.财务管理_应收管理_收款删除))
            {
                IsShowDel = true;
            }
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
        /// <param name="sId">收款信息编号</param>
        private void InitShouKuanXinXi(string sId)
        {
            if (string.IsNullOrEmpty(sId)) return;

            var model = new EyouSoft.BLL.FinStructure.BShouKuan().GetFinCope(sId);

            if (model == null) return;

            txtDate.Value = model.ShouKuanRiQi.ToShortDateString();
            txtOperator.Value = model.ShouKuanRenName;
            hidOperatorId.Value = model.ShouKuanRenId.ToString();
            txtMoney.Value = model.JinE != 0 ? model.JinE.ToString("F2") : string.Empty;
            txtOtherPrice.Value = model.OtherPrice != 0 ? model.OtherPrice.ToString("F2") : string.Empty;
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
        /// <param name="sId">收款登记信息编号</param>
        private void DeleteDengJi(string action, string sId)
        {
            var msg = CheckPrive(action, sId);
            if (!string.IsNullOrEmpty(msg))
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", msg));
                return;
            }

            int r = new EyouSoft.BLL.FinStructure.BShouKuan().DeleteFinCope(sId);

            this.RCWE(UtilsCommons.AjaxReturnJson(r == 1 ? "1" : "0", r == 1 ? "删除成功！" : "删除失败！"));
        }

        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="itemId">收款项目编号</param>
        /// <param name="sId">收款登记信息编号</param>
        private void Save(string action, string itemId, string sId)
        {
            if (!CheckIdAndType(itemId))
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", "url错误，请重新打开此窗口！"));
                return;
            }
            var msg = CheckPrive(action, sId);
            if (!string.IsNullOrEmpty(msg))
            {
                this.RCWE(UtilsCommons.AjaxReturnJson("0", msg));
                return;
            }

            var bll = new EyouSoft.BLL.FinStructure.BShouKuan();
            var model = this.GetFormValue(itemId);
            model.DengJiId = sId;
            model.File = this.GetFileList(model.DengJiId);
            int r = 0;
            if (string.IsNullOrEmpty(action) || action == "add")
            {
                r = bll.AddFinCope(model);
            }
            else if (action == "edit")
            {
                r = bll.UpdateFinCope(model);
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
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "收款总登记之和超过应收！"));
                    break;
                default:
                    this.RCWE(UtilsCommons.AjaxReturnJson("0", "操作失败！"));
                    break;
            }
        }

        /// <summary>
        /// 获取表单值
        /// </summary>
        /// <param name="itemId">收款项目编号</param>
        /// <returns></returns>
        private EyouSoft.Model.FinStructure.MShouKuan GetFormValue(string itemId)
        {
            var model = new EyouSoft.Model.FinStructure.MShouKuan
                {
                    CompanyId = CurrentUserCompanyID,
                    DengJiId = string.Empty,
                    FangShi =
                        (EyouSoft.Model.EnumType.FinStructure.ShouFuKuanFangShi)
                        Utils.GetInt(Utils.GetFormValue(ddlFangShi.UniqueID)),
                    IsKaiPiao = Utils.GetFormValue(ddlKaiPiao.UniqueID) == "1" ? true : false,
                    IssueTime = DateTime.Now,
                    ItemName = string.Empty,
                    JinE = Utils.GetDecimal(Utils.GetFormValue(txtMoney.UniqueID)),
                    OperatorId = this.SiteUserInfo.UserId,
                    OtherPrice = Utils.GetDecimal(Utils.GetFormValue(txtOtherPrice.UniqueID)),
                    ShouKuanBeiZhu = Utils.GetFormValue(txtRemark.UniqueID),
                    ShouKuanRenId = Utils.GetInt(Utils.GetFormValue(hidOperatorId.UniqueID)),
                    ShouKuanRenName = Utils.GetFormValue(txtOperator.UniqueID),
                    ShouKuanRiQi = Utils.GetDateTime(Utils.GetFormValue(txtDate.UniqueID)),
                    Status = EyouSoft.Model.EnumType.FinStructure.KuanXiangStatus.未支付,
                    TourId = itemId,
                    ZhangHuId = Utils.GetFormValue(ddlBankId.UniqueID)
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
