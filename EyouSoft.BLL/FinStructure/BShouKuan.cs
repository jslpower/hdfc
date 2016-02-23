using System.Collections.Generic;
using EyouSoft.Model.EnumType.FinStructure;
using EyouSoft.BLL.CompanyStructure;
using EyouSoft.Model.EnumType.PlanStructure;

namespace EyouSoft.BLL.FinStructure
{
    #region 财务管理应收管理业务逻辑

    /// <summary>
    /// 财务管理应收管理业务逻辑
    /// </summary>
    public class BShouKuan : BLLBase
    {
        private readonly IDAL.FinStructure.IShouKuan _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.FinStructure.IShouKuan>();

        /// <summary>
        /// 生成基类实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Model.FinStructure.MKuanBase GetBaseModel(Model.FinStructure.MShouKuan model)
        {
            return new Model.FinStructure.MKuanBase
                {
                    CompanyId = model.CompanyId,
                    DengJiId = model.DengJiId,
                    FangShi = model.FangShi,
                    File = model.File,
                    IsKaiPiao = model.IsKaiPiao,
                    IssueTime = model.IssueTime,
                    ItemName = model.ItemName,
                    JinE = model.JinE,
                    OperatorId = model.OperatorId,
                    OtherPrice = model.OtherPrice,
                    ShouKuanBeiZhu = model.ShouKuanBeiZhu,
                    ShouKuanRenId = model.ShouKuanRenId,
                    ShouKuanRenName = model.ShouKuanRenName,
                    ShouKuanRiQi = model.ShouKuanRiQi,
                    Status = model.Status,
                    ZhangHuCode = model.ZhangHuCode,
                    ZhangHuId = model.ZhangHuId
                };
        }

        /// <summary>
        /// 添加收款登记
        /// </summary>
        /// <param name="model">收款登记实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：收款总登记和超过应收；
        /// -2：添加失败
        /// </returns>
        public int AddFinCope(Model.FinStructure.MShouKuan model)
        {
            if (model == null || string.IsNullOrEmpty(model.TourId) || model.CompanyId <= 0
                || string.IsNullOrEmpty(model.ShouKuanRenName) || string.IsNullOrEmpty(model.ZhangHuId)
                || model.OperatorId <= 0)
            {
                return 0;
            }

            var tmp = this.GetBaseModel(model);
            int dalRetCode = _dal.AddFinCope(KuanXiangType.计划收款, model.TourId, tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增收款登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应收管理,
                    EventMessage = "新增收款登记，编号：" + tmp.DengJiId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改收款登记
        /// </summary>
        /// <param name="model">收款登记</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：收款总登记和超过应收；
        /// -2：修改失败；
        /// </returns>
        public int UpdateFinCope(Model.FinStructure.MShouKuan model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.ShouKuanRenName)
                || string.IsNullOrEmpty(model.ZhangHuId) || model.OperatorId <= 0
                || string.IsNullOrEmpty(model.DengJiId))
            {
                return 0;
            }

            int dalRetCode = _dal.UpdateFinCope(this.GetBaseModel(model));
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改收款登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应收管理,
                    EventMessage = "修改收款登记，编号：" + model.DengJiId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除收款登记
        /// </summary>
        /// <param name="id">收款登记编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteFinCope(string id)
        {
            if (string.IsNullOrEmpty(id)) return 0;

            int dalRetCode = _dal.DeleteFinCope(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除收款登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应收管理,
                    EventMessage = "删除收款登记，编号：" + id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 获取收款登记信息
        /// </summary>
        /// <param name="id">收款登记编号</param>
        /// <returns></returns>
        public Model.FinStructure.MShouKuan GetFinCope(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return (Model.FinStructure.MShouKuan)_dal.GetFinCope(id);
        }

        /// <summary>
        /// 获取收款登记信息
        /// </summary>
        /// <param name="id">收款登记项目编号(团队编号)</param>
        /// <returns></returns>
        public IList<Model.FinStructure.MShouKuan> GetFinCopeList(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return (IList<Model.FinStructure.MShouKuan>)_dal.GetFinCopeList(KuanXiangType.计划收款, id);
        }
    }

    #endregion

    #region 财务管理应付管理地接付款业务逻辑

    /// <summary>
    /// 财务管理应付管理地接付款业务逻辑
    /// </summary>
    public class BDiJieFuKuan : BLLBase
    {
        private readonly IDAL.FinStructure.IShouKuan _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.FinStructure.IShouKuan>();

        /// <summary>
        /// 生成基类实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Model.FinStructure.MKuanBase GetBaseModel(Model.FinStructure.MDiJieFuKuan model)
        {
            return new Model.FinStructure.MKuanBase
            {
                CompanyId = model.CompanyId,
                DengJiId = model.DengJiId,
                FangShi = model.FangShi,
                File = model.File,
                IsKaiPiao = model.IsKaiPiao,
                IssueTime = model.IssueTime,
                ItemName = model.ItemName,
                JinE = model.JinE,
                OperatorId = model.OperatorId,
                OtherPrice = model.OtherPrice,
                ShouKuanBeiZhu = model.ShouKuanBeiZhu,
                ShouKuanRenId = model.ShouKuanRenId,
                ShouKuanRenName = model.ShouKuanRenName,
                ShouKuanRiQi = model.ShouKuanRiQi,
                Status = model.Status,
                ZhangHuCode = model.ZhangHuCode,
                ZhangHuId = model.ZhangHuId
            };
        }

        /// <summary>
        /// 添加地接付款登记
        /// </summary>
        /// <param name="model">地接付款登记实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：付款总登记和超过应付；
        /// -2：添加失败
        /// </returns>
        public int AddFinCope(Model.FinStructure.MDiJieFuKuan model)
        {
            if (model == null || string.IsNullOrEmpty(model.DiJiePlanId) || model.CompanyId <= 0
                || string.IsNullOrEmpty(model.ShouKuanRenName)
                || model.OperatorId <= 0)
            {
                return 0;
            }

            var tmp = this.GetBaseModel(model);
            int dalRetCode = _dal.AddFinCope(KuanXiangType.地接支出付款, model.DiJiePlanId, tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增地接付款登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应付管理,
                    EventMessage = "新增地接付款登记，编号：" + tmp.DengJiId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改地接付款登记
        /// </summary>
        /// <param name="model">地接付款登记实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：付款总登记和超过应付；
        /// -2：修改失败；
        /// </returns>
        public int UpdateFinCope(Model.FinStructure.MDiJieFuKuan model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.ShouKuanRenName)
                || model.OperatorId <= 0
                || string.IsNullOrEmpty(model.DengJiId))
            {
                return 0;
            }

            int dalRetCode = _dal.UpdateFinCope(this.GetBaseModel(model));
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改地接付款登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应付管理,
                    EventMessage = "修改地接付款登记，编号：" + model.DengJiId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除地接付款登记
        /// </summary>
        /// <param name="id">地接付款登记编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteFinCope(string id)
        {
            if (string.IsNullOrEmpty(id)) return 0;

            int dalRetCode = _dal.DeleteFinCope(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除地接付款登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应付管理,
                    EventMessage = "删除地接付款登记，编号：" + id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 获取地接付款登记
        /// </summary>
        /// <param name="id">地接付款登记编号</param>
        /// <returns></returns>
        public Model.FinStructure.MDiJieFuKuan GetFinCope(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return (Model.FinStructure.MDiJieFuKuan)_dal.GetFinCope(id);
        }

        /// <summary>
        /// 获取地接付款登记信息
        /// </summary>
        /// <param name="id">地接付款登记项目编号(地接安排编号)</param>
        /// <returns></returns>
        public IList<Model.FinStructure.MDiJieFuKuan> GetFinCopeList(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return (IList<Model.FinStructure.MDiJieFuKuan>)_dal.GetFinCopeList(KuanXiangType.地接支出付款, id);
        }

        /// <summary>
        /// 获取财务管理应付管理地接列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <param name="heJi">合计实体</param>
        /// <returns></returns>
        public IList<Model.FinStructure.MDiJieList> GetDiJieList(int companyId, int pageSize, int pageIndex, ref int recordCount
            , Model.FinStructure.MSearchDiJieList search, Model.FinStructure.MPlanHeJi heJi)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 0) return null;

            return _dal.GetDiJieList(companyId, pageSize, pageIndex, ref recordCount, search, heJi);
        }
    }

    #endregion

    #region 财务管理应付管理票务付款业务逻辑

    /// <summary>
    /// 财务管理应付管理票务付款业务逻辑
    /// </summary>
    public class BPiaoFuKuan : BLLBase
    {
        private readonly IDAL.FinStructure.IShouKuan _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.FinStructure.IShouKuan>();

        /// <summary>
        /// 生成基类实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Model.FinStructure.MKuanBase GetBaseModel(Model.FinStructure.MPiaoFuKuan model)
        {
            return new Model.FinStructure.MKuanBase
            {
                CompanyId = model.CompanyId,
                DengJiId = model.DengJiId,
                FangShi = model.FangShi,
                File = model.File,
                IsKaiPiao = model.IsKaiPiao,
                IssueTime = model.IssueTime,
                ItemName = model.ItemName,
                JinE = model.JinE,
                OperatorId = model.OperatorId,
                OtherPrice = model.OtherPrice,
                ShouKuanBeiZhu = model.ShouKuanBeiZhu,
                ShouKuanRenId = model.ShouKuanRenId,
                ShouKuanRenName = model.ShouKuanRenName,
                ShouKuanRiQi = model.ShouKuanRiQi,
                Status = model.Status,
                ZhangHuCode = model.ZhangHuCode,
                ZhangHuId = model.ZhangHuId
            };
        }

        /// <summary>
        /// 添加票务付款登记
        /// </summary>
        /// <param name="model">票务付款登记实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：付款总登记和超过应付；
        /// -2：添加失败
        /// </returns>
        public int AddFinCope(Model.FinStructure.MPiaoFuKuan model)
        {
            if (model == null || string.IsNullOrEmpty(model.PiaoPlanId) || model.CompanyId <= 0
                || string.IsNullOrEmpty(model.ShouKuanRenName)
                || model.OperatorId <= 0)
            {
                return 0;
            }

            var tmp = this.GetBaseModel(model);
            int dalRetCode = _dal.AddFinCope(
                model.TicketType == TicketMode.出票 ? KuanXiangType.出票支出 : KuanXiangType.退票支出, model.PiaoPlanId, tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                    {
                        EventTitle = "新增票务" + (model.TicketType == TicketMode.出票 ? "出票" : "退票") + "付款登记",
                        ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应付管理,
                        EventMessage =
                            "新增票务" + (model.TicketType == TicketMode.出票 ? "出票" : "退票") + "付款登记，编号：" + tmp.DengJiId + "。"
                    };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改票务付款登记
        /// </summary>
        /// <param name="model">票务付款登记实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：付款总登记和超过应付；
        /// -2：修改失败；
        /// </returns>
        public int UpdateFinCope(Model.FinStructure.MPiaoFuKuan model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.ShouKuanRenName)
                || model.OperatorId <= 0
                || string.IsNullOrEmpty(model.DengJiId))
            {
                return 0;
            }

            int dalRetCode = _dal.UpdateFinCope(this.GetBaseModel(model));
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改票务付款登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应付管理,
                    EventMessage = "修改票务付款登记，编号：" + model.DengJiId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除票务付款登记
        /// </summary>
        /// <param name="id">票务付款登记编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteFinCope(string id)
        {
            if (string.IsNullOrEmpty(id)) return 0;

            int dalRetCode = _dal.DeleteFinCope(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除票务付款登记",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_应付管理,
                    EventMessage = "删除票务付款登记，编号：" + id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 获取票务付款登记
        /// </summary>
        /// <param name="id">票务付款登记编号</param>
        /// <returns></returns>
        public Model.FinStructure.MPiaoFuKuan GetFinCope(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return (Model.FinStructure.MPiaoFuKuan)_dal.GetFinCope(id);
        }

        /// <summary>
        /// 获取票务付款登记
        /// </summary>
        /// <param name="id">票务付款登记项目编号(机票安排编号)</param>
        /// <param name="type">票务类型(出票、退票)</param>
        /// <returns></returns>
        public IList<Model.FinStructure.MPiaoFuKuan> GetFinCopeList(TicketMode type, string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return
                (IList<Model.FinStructure.MPiaoFuKuan>)
                _dal.GetFinCopeList(type == TicketMode.出票 ? KuanXiangType.出票支出 : KuanXiangType.退票支出, id);
        }

        /// <summary>
        /// 获取财务管理应付管理票务列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <param name="heJi">合计实体</param>
        /// <returns></returns>
        public IList<Model.FinStructure.MPiaoList> GetPiaoList(int companyId, int pageSize, int pageIndex, ref int recordCount
            , Model.FinStructure.MSearchPiaoList search, Model.FinStructure.MPlanHeJi heJi)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 0) return null;

            return _dal.GetPiaoList(companyId, pageSize, pageIndex, ref recordCount, search, heJi);
        }
    }

    #endregion

    #region 财务管理其他收入业务逻辑

    /// <summary>
    /// 财务管理其他收入业务逻辑
    /// </summary>
    public class BQiTaShouKuan : BLLBase
    {
        private readonly IDAL.FinStructure.IShouKuan _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.FinStructure.IShouKuan>();

        /// <summary>
        /// 生成基类实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Model.FinStructure.MKuanBase GetBaseModel(Model.FinStructure.MQiTaShouKuan model)
        {
            return new Model.FinStructure.MKuanBase
            {
                CompanyId = model.CompanyId,
                DengJiId = model.DengJiId,
                FangShi = model.FangShi,
                File = model.File,
                IsKaiPiao = model.IsKaiPiao,
                IssueTime = model.IssueTime,
                ItemName = model.ItemName,
                JinE = model.JinE,
                OperatorId = model.OperatorId,
                OtherPrice = model.OtherPrice,
                ShouKuanBeiZhu = model.ShouKuanBeiZhu,
                ShouKuanRenId = model.ShouKuanRenId,
                ShouKuanRenName = model.ShouKuanRenName,
                ShouKuanRiQi = model.ShouKuanRiQi,
                Status = model.Status,
                ZhangHuCode = model.ZhangHuCode,
                ZhangHuId = model.ZhangHuId
            };
        }

        /// <summary>
        /// 添加其他收入
        /// </summary>
        /// <param name="model">其他收入实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：收款总登记和超过应收；
        /// -2：添加失败
        /// </returns>
        public int AddFinCope(Model.FinStructure.MQiTaShouKuan model)
        {
            if (model == null || model.CompanyId <= 0
                || string.IsNullOrEmpty(model.ShouKuanRenName)
                || model.OperatorId <= 0)
            {
                return 0;
            }

            var tmp = this.GetBaseModel(model);
            int dalRetCode = _dal.AddFinCope(KuanXiangType.其它收入, string.Empty, tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                    {
                        EventTitle = "新增其他收入",
                        ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_其他收入,
                        EventMessage = "新增其他收入，编号：" + tmp.DengJiId + "。"
                    };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改其他收入
        /// </summary>
        /// <param name="model">其他收入实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：收款总登记和超过应收；
        /// -2：修改失败；
        /// </returns>
        public int UpdateFinCope(Model.FinStructure.MQiTaShouKuan model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.ShouKuanRenName)
                || model.OperatorId <= 0
                || string.IsNullOrEmpty(model.DengJiId))
            {
                return 0;
            }

            int dalRetCode = _dal.UpdateFinCope(this.GetBaseModel(model));
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改其他收入",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_其他收入,
                    EventMessage = "修改其他收入，编号：" + model.DengJiId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除其他收入
        /// </summary>
        /// <param name="id">其他收入编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteFinCope(string id)
        {
            if (string.IsNullOrEmpty(id)) return 0;

            int dalRetCode = _dal.DeleteFinCope(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除其他收入",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_其他收入,
                    EventMessage = "删除其他收入，编号：" + id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 获取其他收入
        /// </summary>
        /// <param name="id">其他收入编号</param>
        /// <returns></returns>
        public Model.FinStructure.MQiTaShouKuan GetFinCope(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return (Model.FinStructure.MQiTaShouKuan)_dal.GetFinCope(id);
        }

        /// <summary>
        /// 获取其他收入
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="search">其他收入查询实体</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public IList<Model.FinStructure.MQiTaShouKuan> GetFinCopeList(int companyId, int pageSize, int pageIndex, ref int recordCount
            , Model.FinStructure.MSearchOther search)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 0) return null;

            return
                (IList<Model.FinStructure.MQiTaShouKuan>)
                _dal.GetFinCopeList(companyId, KuanXiangType.其它收入, pageSize, pageIndex, ref recordCount, search);
        }
    }

    #endregion

    #region 财务管理其他支出业务逻辑

    /// <summary>
    /// 财务管理其他支出业务逻辑
    /// </summary>
    public class BQiTaFuKuan : BLLBase
    {
        private readonly IDAL.FinStructure.IShouKuan _dal =
            Component.Factory.ComponentFactory.CreateDAL<IDAL.FinStructure.IShouKuan>();

        /// <summary>
        /// 生成基类实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Model.FinStructure.MKuanBase GetBaseModel(Model.FinStructure.MQiTaFuKuan model)
        {
            return new Model.FinStructure.MKuanBase
            {
                CompanyId = model.CompanyId,
                DengJiId = model.DengJiId,
                FangShi = model.FangShi,
                File = model.File,
                IsKaiPiao = model.IsKaiPiao,
                IssueTime = model.IssueTime,
                ItemName = model.ItemName,
                JinE = model.JinE,
                OperatorId = model.OperatorId,
                OtherPrice = model.OtherPrice,
                ShouKuanBeiZhu = model.ShouKuanBeiZhu,
                ShouKuanRenId = model.ShouKuanRenId,
                ShouKuanRenName = model.ShouKuanRenName,
                ShouKuanRiQi = model.ShouKuanRiQi,
                Status = model.Status,
                ZhangHuCode = model.ZhangHuCode,
                ZhangHuId = model.ZhangHuId
            };
        }

        /// <summary>
        /// 添加其他支出
        /// </summary>
        /// <param name="model">其他支出实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：付款总登记和超过应付；
        /// -2：添加失败
        /// </returns>
        public int AddFinCope(Model.FinStructure.MQiTaFuKuan model)
        {
            if (model == null || model.CompanyId <= 0
                || string.IsNullOrEmpty(model.ShouKuanRenName)
                || model.OperatorId <= 0)
            {
                return 0;
            }

            var tmp = this.GetBaseModel(model);
            int dalRetCode = _dal.AddFinCope(KuanXiangType.其它支出, string.Empty, tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增其他支出",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_其他支出,
                    EventMessage = "新增其他支出，编号：" + tmp.DengJiId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改其他支出
        /// </summary>
        /// <param name="model">其他支出实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：付款总登记和超过应付；
        /// -2：修改失败；
        /// </returns>
        public int UpdateFinCope(Model.FinStructure.MQiTaFuKuan model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.ShouKuanRenName)
                || model.OperatorId <= 0
                || string.IsNullOrEmpty(model.DengJiId))
            {
                return 0;
            }

            int dalRetCode = _dal.UpdateFinCope(this.GetBaseModel(model));
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改其他支出",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_其他支出,
                    EventMessage = "修改其他支出，编号：" + model.DengJiId + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除其他支出
        /// </summary>
        /// <param name="id">其他支出编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteFinCope(string id)
        {
            if (string.IsNullOrEmpty(id)) return 0;

            int dalRetCode = _dal.DeleteFinCope(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除其他支出",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.财务管理_其他支出,
                    EventMessage = "删除其他支出，编号：" + id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 获取其他支出
        /// </summary>
        /// <param name="id">其他支出编号</param>
        /// <returns></returns>
        public Model.FinStructure.MQiTaFuKuan GetFinCope(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return (Model.FinStructure.MQiTaFuKuan)_dal.GetFinCope(id);
        }

        /// <summary>
        /// 获取其他收入
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="search">其他收入查询实体</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public IList<Model.FinStructure.MQiTaFuKuan> GetFinCopeList(int companyId, int pageSize, int pageIndex, ref int recordCount
            , Model.FinStructure.MSearchOther search)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 0) return null;

            return
                (IList<Model.FinStructure.MQiTaFuKuan>)
                _dal.GetFinCopeList(companyId, KuanXiangType.其它支出, pageSize, pageIndex, ref recordCount, search);
        }
    }

    #endregion
}
