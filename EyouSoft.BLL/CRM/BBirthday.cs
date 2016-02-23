using System;
using System.Collections.Generic;
using EyouSoft.Model.CRM;
using EyouSoft.BLL.CompanyStructure;
using EyouSoft.Model.EnumType.CustomerStructure;

namespace EyouSoft.BLL.CRM
{
    #region 员工生日提醒业务逻辑

    /// <summary>
    /// 员工生日提醒业务逻辑
    /// </summary>
    public class BUserBirthday : BLLBase
    {
        private readonly IDAL.CRM.IBirthday _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.CRM.IBirthday>();

        /// <summary>
        /// 生日中心-生日礼物明细-收到礼物对象类型
        /// </summary>
        private const BirthdayGiftType BirthdayType = BirthdayGiftType.员工;

        /// <summary>
        /// 根据派生类获取基类
        /// </summary>
        /// <param name="model">派生类</param>
        /// <returns>基类</returns>
        private MBirthdayGiftBase GetBaseClass(MUserBirthdayGift model)
        {
            if (model == null) return null;

            return new MBirthdayGiftBase
                {
                    CompanyId = model.CompanyId,
                    Id = model.Id,
                    IssueTime = model.IssueTime,
                    OperatorId = model.OperatorId,
                    Remark = model.Remark,
                    SendGiftTime = model.SendGiftTime
                };
        }

        /// <summary>
        /// 添加礼物明细
        /// </summary>
        /// <param name="model">用户生日礼物实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int AddBirthdayGift(MUserBirthdayGift model)
        {
            if (model == null || model.UserId <= 0 || model.CompanyId <= 0) return 0;

            var tmp = this.GetBaseClass(model);
            tmp.Id = Guid.NewGuid().ToString();
            tmp.IssueTime = DateTime.Now;
            int dalRetCode = _dal.AddBirthdayGift(BirthdayType, model.UserId.ToString(), tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增员工用户生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "新增员工用户生日礼物明细，编号：" + tmp.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改礼物明细
        /// </summary>
        /// <param name="model">用户生日礼物实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int UpdateBirthdayGift(MUserBirthdayGift model)
        {
            if (model == null || model.UserId <= 0 || model.CompanyId <= 0 || string.IsNullOrEmpty(model.Id)) return 0;

            var tmp = this.GetBaseClass(model);
            int dalRetCode = _dal.UpdateBirthdayGift(tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改员工用户生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "修改员工用户生日礼物明细，编号：" + tmp.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteBirthdayGift(params string[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            int dalRetCode = _dal.DeleteBirthdayGift(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除员工用户生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "删除员工用户生日礼物明细，编号：" + this.GetIdsByArr(id) + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 根据编号获取礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 礼物明细
        /// </returns>
        public MUserBirthdayGift GetBirthdayGift(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var tmp = _dal.GetBirthdayGift(id);
            return (MUserBirthdayGift)tmp;
        }

        /// <summary>
        /// 获取某个员工用户收到的礼物明细
        /// </summary>
        /// <param name="userId">员工用户编号</param>
        /// <returns>某个员工用户收到的礼物明细</returns>
        public IList<MUserBirthdayGift> GetBirthdayGiftList(int userId)
        {
            if (userId <= 0) return null;

            var tmp = _dal.GetBirthdayGift(BirthdayType, userId.ToString());

            return (IList<MUserBirthdayGift>)tmp;
        }

        /// <summary>
        /// 获取员工生日提醒列表（seach 有值以seach为准，为null则以days为准）
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">生日查询实体</param>
        /// <returns>员工生日提醒列表</returns>
        public IList<MUserBirthday> GetBirthdayList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            MSearchBirthday seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            int days = 0;
            if (seach == null || (string.IsNullOrEmpty(seach.Name) && !seach.StartBirthday.HasValue && !seach.EndBirthday.HasValue))
            {
                var setting = new CompanySetting().GetSetting(companyId);
                if (setting != null) days = setting.BirthdayReminderDays;
            }

            var tmp = _dal.GetBirthdayList(BirthdayType, companyId, days, pageSize, pageIndex, ref recordCount, seach);
            return (IList<MUserBirthday>)tmp;
        }

        /// <summary>
        /// 获取生日弹窗提醒
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="seach">查询实体</param>
        /// <returns></returns>
        public IList<MBirthdayWindow> GetBirthdayWindow(int companyId, MSearchBirthday seach)
        {
            if (companyId <= 0) return null;

            return _dal.GetBirthdayWindow(companyId, seach);
        }
    }

    #endregion

    #region 导游生日提醒业务逻辑

    /// <summary>
    /// 导游生日提醒业务逻辑
    /// </summary>
    public class BGuideBirthday : BLLBase
    {
        private readonly IDAL.CRM.IBirthday _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.CRM.IBirthday>();

        /// <summary>
        /// 生日中心-生日礼物明细-收到礼物对象类型
        /// </summary>
        private const BirthdayGiftType BirthdayType = BirthdayGiftType.导游;

        /// <summary>
        /// 根据派生类获取基类
        /// </summary>
        /// <param name="model">派生类</param>
        /// <returns>基类</returns>
        private MBirthdayGiftBase GetBaseClass(MGuideBirthdayGift model)
        {
            if (model == null) return null;

            return new MBirthdayGiftBase
            {
                CompanyId = model.CompanyId,
                Id = model.Id,
                IssueTime = model.IssueTime,
                OperatorId = model.OperatorId,
                Remark = model.Remark,
                SendGiftTime = model.SendGiftTime
            };
        }

        /// <summary>
        /// 添加礼物明细
        /// </summary>
        /// <param name="model">导游生日礼物实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int AddBirthdayGift(MGuideBirthdayGift model)
        {
            if (model == null || string.IsNullOrEmpty(model.GuideId) || model.CompanyId <= 0) return 0;

            var tmp = this.GetBaseClass(model);
            tmp.Id = Guid.NewGuid().ToString();
            tmp.IssueTime = DateTime.Now;
            int dalRetCode = _dal.AddBirthdayGift(BirthdayType, model.GuideId, tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增导游生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "新增导游生日礼物明细，编号：" + tmp.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改礼物明细
        /// </summary>
        /// <param name="model">导游生日礼物实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int UpdateBirthdayGift(MGuideBirthdayGift model)
        {
            if (model == null || string.IsNullOrEmpty(model.GuideId) || model.CompanyId <= 0 || string.IsNullOrEmpty(model.Id)) return 0;
            var tmp = this.GetBaseClass(model);
            int dalRetCode = _dal.UpdateBirthdayGift(tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改导游生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "修改导游生日礼物明细，编号：" + tmp.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteBirthdayGift(params string[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            int dalRetCode = _dal.DeleteBirthdayGift(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除导游生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "删除导游生日礼物明细，编号：" + this.GetIdsByArr(id) + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 根据编号获取礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 礼物明细
        /// </returns>
        public MGuideBirthdayGift GetBirthdayGift(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var tmp = _dal.GetBirthdayGift(id);
            return (MGuideBirthdayGift)tmp;
        }

        /// <summary>
        /// 获取某个导游收到的礼物明细
        /// </summary>
        /// <param name="guideId">导游编号</param>
        /// <returns>某个导游收到的礼物明细</returns>
        public IList<MGuideBirthdayGift> GetBirthdayGiftList(string guideId)
        {
            if (string.IsNullOrEmpty(guideId)) return null;

            var tmp = _dal.GetBirthdayGift(BirthdayType, guideId);

            return (IList<MGuideBirthdayGift>)tmp;
        }

        /// <summary>
        /// 获取导游生日提醒列表（seach 有值以seach为准，为null则以days为准）
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">生日查询实体</param>
        /// <returns>导游生日提醒列表</returns>
        public IList<MGuideBirthday> GetBirthdayList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            MSearchBirthday seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            int days = 0;
            if (seach == null || (string.IsNullOrEmpty(seach.Name) && !seach.StartBirthday.HasValue && !seach.EndBirthday.HasValue))
            {
                var setting = new CompanySetting().GetSetting(companyId);
                if (setting != null) days = setting.BirthdayReminderDays;
            }

            var tmp = _dal.GetBirthdayList(BirthdayType, companyId, days, pageSize, pageIndex, ref recordCount, seach);
            return (IList<MGuideBirthday>)tmp;
        }
    }

    #endregion

    #region 组团社生日提醒业务逻辑

    /// <summary>
    /// 组团社生日提醒业务逻辑
    /// </summary>
    public class BContactBirthday : BLLBase
    {
        private readonly IDAL.CRM.IBirthday _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.CRM.IBirthday>();

        /// <summary>
        /// 生日中心-生日礼物明细-收到礼物对象类型
        /// </summary>
        private const BirthdayGiftType BirthdayType = BirthdayGiftType.组团联系人;

        /// <summary>
        /// 根据派生类获取基类
        /// </summary>
        /// <param name="model">派生类</param>
        /// <returns>基类</returns>
        private MBirthdayGiftBase GetBaseClass(MContactBirthdayGift model)
        {
            if (model == null) return null;

            return new MBirthdayGiftBase
            {
                CompanyId = model.CompanyId,
                Id = model.Id,
                IssueTime = model.IssueTime,
                OperatorId = model.OperatorId,
                Remark = model.Remark,
                SendGiftTime = model.SendGiftTime
            };
        }

        /// <summary>
        /// 添加礼物明细
        /// </summary>
        /// <param name="model">组团社联系人生日礼物实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int AddBirthdayGift(MContactBirthdayGift model)
        {
            if (model == null || model.ContactId <= 0 || model.CompanyId <= 0) return 0;

            var tmp = this.GetBaseClass(model);
            tmp.Id = Guid.NewGuid().ToString();
            tmp.IssueTime = DateTime.Now;
            int dalRetCode = _dal.AddBirthdayGift(BirthdayType, model.ContactId.ToString(), tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增组团社联系人生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "新增组团社联系人生日礼物明细，编号：" + tmp.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改礼物明细
        /// </summary>
        /// <param name="model">组团社联系人生日礼物实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int UpdateBirthdayGift(MContactBirthdayGift model)
        {
            if (model == null || model.ContactId <= 0 || model.CompanyId <= 0 || string.IsNullOrEmpty(model.Id)) return 0;
            var tmp = this.GetBaseClass(model);
            int dalRetCode = _dal.UpdateBirthdayGift(tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改组团社联系人生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "修改组团社联系人生日礼物明细，编号：" + tmp.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteBirthdayGift(params string[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            int dalRetCode = _dal.DeleteBirthdayGift(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除组团社联系人生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "删除组团社联系人生日礼物明细，编号：" + this.GetIdsByArr(id) + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 根据编号获取礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 礼物明细
        /// </returns>
        public MContactBirthdayGift GetBirthdayGift(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var tmp = _dal.GetBirthdayGift(id);
            return (MContactBirthdayGift)tmp;
        }

        /// <summary>
        /// 获取某个组团社联系人收到的礼物明细
        /// </summary>
        /// <param name="contactId">组团社联系人编号</param>
        /// <returns>某个组团社联系人收到的礼物明细</returns>
        public IList<MContactBirthdayGift> GetBirthdayGiftList(int contactId)
        {
            if (contactId <= 0) return null;

            var tmp = _dal.GetBirthdayGift(BirthdayType, contactId.ToString());

            return (IList<MContactBirthdayGift>)tmp;
        }

        /// <summary>
        /// 获取组团社联系人生日提醒列表（seach 有值以seach为准，为null则以days为准）
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">生日查询实体</param>
        /// <returns>组团社联系人生日提醒列表</returns>
        public IList<MContactBirthday> GetBirthdayList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            MSearchBirthday seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            int days = 0;
            if (seach == null || (string.IsNullOrEmpty(seach.Name) && !seach.StartBirthday.HasValue && !seach.EndBirthday.HasValue))
            {
                var setting = new CompanySetting().GetSetting(companyId);
                if (setting != null) days = setting.BirthdayReminderDays;
            }

            var tmp = _dal.GetBirthdayList(BirthdayType, companyId, days, pageSize, pageIndex, ref recordCount, seach);
            return (IList<MContactBirthday>)tmp;
        }
    }

    #endregion

    #region 游客生日提醒业务逻辑

    /// <summary>
    /// 游客生日提醒业务逻辑
    /// </summary>
    public class BTravellerBirthday : BLLBase
    {
        private readonly IDAL.CRM.IBirthday _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.CRM.IBirthday>();

        /// <summary>
        /// 生日中心-生日礼物明细-收到礼物对象类型
        /// </summary>
        private const BirthdayGiftType BirthdayType = BirthdayGiftType.游客;

        /// <summary>
        /// 根据派生类获取基类
        /// </summary>
        /// <param name="model">派生类</param>
        /// <returns>基类</returns>
        private MBirthdayGiftBase GetBaseClass(MTravellerBirthdayGift model)
        {
            if (model == null) return null;

            return new MBirthdayGiftBase
            {
                CompanyId = model.CompanyId,
                Id = model.Id,
                IssueTime = model.IssueTime,
                OperatorId = model.OperatorId,
                Remark = model.Remark,
                SendGiftTime = model.SendGiftTime
            };
        }

        /// <summary>
        /// 添加礼物明细
        /// </summary>
        /// <param name="model">游客生日礼物实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int AddBirthdayGift(MTravellerBirthdayGift model)
        {
            if (model == null || string.IsNullOrEmpty(model.TravellerId) || model.CompanyId <= 0) return 0;

            var tmp = this.GetBaseClass(model);
            tmp.Id = Guid.NewGuid().ToString();
            tmp.IssueTime = DateTime.Now;
            int dalRetCode = _dal.AddBirthdayGift(BirthdayType, model.TravellerId, tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增游客生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "新增游客生日礼物明细，编号：" + tmp.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改礼物明细
        /// </summary>
        /// <param name="model">游客生日礼物实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int UpdateBirthdayGift(MTravellerBirthdayGift model)
        {
            if (model == null || string.IsNullOrEmpty(model.TravellerId) || model.CompanyId <= 0 || string.IsNullOrEmpty(model.Id)) return 0;
            var tmp = this.GetBaseClass(model);
            int dalRetCode = _dal.UpdateBirthdayGift(tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改游客生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "修改游客生日礼物明细，编号：" + tmp.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteBirthdayGift(params string[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            int dalRetCode = _dal.DeleteBirthdayGift(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除游客生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "删除游客生日礼物明细，编号：" + this.GetIdsByArr(id) + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 根据编号获取礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 礼物明细
        /// </returns>
        public MTravellerBirthdayGift GetBirthdayGift(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var tmp = _dal.GetBirthdayGift(id);
            return (MTravellerBirthdayGift)tmp;
        }

        /// <summary>
        /// 获取某个游客收到的礼物明细
        /// </summary>
        /// <param name="travellerId">游客编号</param>
        /// <returns>某个游客收到的礼物明细</returns>
        public IList<MTravellerBirthdayGift> GetBirthdayGiftList(string travellerId)
        {
            if (string.IsNullOrEmpty(travellerId)) return null;

            var tmp = _dal.GetBirthdayGift(BirthdayType, travellerId);

            return (IList<MTravellerBirthdayGift>)tmp;
        }

        /// <summary>
        /// 获取游客生日提醒列表（seach 有值以seach为准，为null则以days为准）
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">生日查询实体</param>
        /// <returns>游客生日提醒列表</returns>
        public IList<MTravellerBirthday> GetBirthdayList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            MSearchBirthday seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            int days = 0;
            if (seach == null || (string.IsNullOrEmpty(seach.Name) && !seach.StartBirthday.HasValue && !seach.EndBirthday.HasValue))
            {
                var setting = new CompanySetting().GetSetting(companyId);
                if (setting != null) days = setting.BirthdayReminderDays;
            }

            var tmp = _dal.GetBirthdayList(BirthdayType, companyId, days, pageSize, pageIndex, ref recordCount, seach);
            return (IList<MTravellerBirthday>)tmp;
        }
    }

    #endregion

    #region 地接社生日提醒业务逻辑

    /// <summary>
    /// 地接社生日提醒业务逻辑
    /// </summary>
    public class BGroundBirthday : BLLBase
    {
        private readonly IDAL.CRM.IBirthday _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.CRM.IBirthday>();

        /// <summary>
        /// 生日中心-生日礼物明细-收到礼物对象类型
        /// </summary>
        private const BirthdayGiftType BirthdayType = BirthdayGiftType.地接联系人;

        /// <summary>
        /// 根据派生类获取基类
        /// </summary>
        /// <param name="model">派生类</param>
        /// <returns>基类</returns>
        private MBirthdayGiftBase GetBaseClass(MGroundBirthdayGift model)
        {
            if (model == null) return null;

            return new MBirthdayGiftBase
            {
                CompanyId = model.CompanyId,
                Id = model.Id,
                IssueTime = model.IssueTime,
                OperatorId = model.OperatorId,
                Remark = model.Remark,
                SendGiftTime = model.SendGiftTime
            };
        }

        /// <summary>
        /// 添加礼物明细
        /// </summary>
        /// <param name="model">地接社联系人生日礼物实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int AddBirthdayGift(MGroundBirthdayGift model)
        {
            if (model == null || model.GroundId <= 0 || model.CompanyId <= 0) return 0;

            var tmp = this.GetBaseClass(model);
            tmp.Id = Guid.NewGuid().ToString();
            tmp.IssueTime = DateTime.Now;
            int dalRetCode = _dal.AddBirthdayGift(BirthdayType, model.GroundId.ToString(), tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增地接社联系人生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "新增地接社联系人生日礼物明细，编号：" + tmp.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改礼物明细
        /// </summary>
        /// <param name="model">地接社联系人生日礼物实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int UpdateBirthdayGift(MGroundBirthdayGift model)
        {
            if (model == null || model.GroundId <= 0 || model.CompanyId <= 0 || string.IsNullOrEmpty(model.Id)) return 0;
            var tmp = this.GetBaseClass(model);
            int dalRetCode = _dal.UpdateBirthdayGift(tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改地接社联系人生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "修改地接社联系人生日礼物明细，编号：" + tmp.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteBirthdayGift(params string[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            int dalRetCode = _dal.DeleteBirthdayGift(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除地接社联系人生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "删除地接社联系人生日礼物明细，编号：" + this.GetIdsByArr(id) + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 根据编号获取礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 礼物明细
        /// </returns>
        public MGroundBirthdayGift GetBirthdayGift(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var tmp = _dal.GetBirthdayGift(id);
            return (MGroundBirthdayGift)tmp;
        }

        /// <summary>
        /// 获取某个地接社联系人收到的礼物明细
        /// </summary>
        /// <param name="groundId">地接社联系人编号</param>
        /// <returns>某个地接社联系人收到的礼物明细</returns>
        public IList<MGroundBirthdayGift> GetBirthdayGiftList(int groundId)
        {
            if (groundId <= 0) return null;

            var tmp = _dal.GetBirthdayGift(BirthdayType, groundId.ToString());

            return (IList<MGroundBirthdayGift>)tmp;
        }

        /// <summary>
        /// 获取地接社联系人生日提醒列表（seach 有值以seach为准，为null则以days为准）
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">生日查询实体</param>
        /// <returns>地接社联系人生日提醒列表</returns>
        public IList<MGroundBirthday> GetBirthdayList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            MSearchBirthday seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            int days = 0;
            if (seach == null || (string.IsNullOrEmpty(seach.Name) && !seach.StartBirthday.HasValue && !seach.EndBirthday.HasValue))
            {
                var setting = new CompanySetting().GetSetting(companyId);
                if (setting != null) days = setting.BirthdayReminderDays;
            }

            var tmp = _dal.GetBirthdayList(BirthdayType, companyId, days, pageSize, pageIndex, ref recordCount, seach);
            return (IList<MGroundBirthday>)tmp;
        }
    }

    #endregion

    #region 景点生日提醒业务逻辑

    /// <summary>
    /// 景点生日提醒业务逻辑
    /// </summary>
    public class BSpotBirthday : BLLBase
    {
        private readonly IDAL.CRM.IBirthday _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.CRM.IBirthday>();

        /// <summary>
        /// 生日中心-生日礼物明细-收到礼物对象类型
        /// </summary>
        private const BirthdayGiftType BirthdayType = BirthdayGiftType.景点联系人;

        /// <summary>
        /// 根据派生类获取基类
        /// </summary>
        /// <param name="model">派生类</param>
        /// <returns>基类</returns>
        private MBirthdayGiftBase GetBaseClass(MSpotBirthdayGift model)
        {
            if (model == null) return null;

            return new MBirthdayGiftBase
            {
                CompanyId = model.CompanyId,
                Id = model.Id,
                IssueTime = model.IssueTime,
                OperatorId = model.OperatorId,
                Remark = model.Remark,
                SendGiftTime = model.SendGiftTime
            };
        }

        /// <summary>
        /// 添加礼物明细
        /// </summary>
        /// <param name="model">景点生日礼物实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int AddBirthdayGift(MSpotBirthdayGift model)
        {
            if (model == null || model.SpotId <= 0 || model.CompanyId <= 0) return 0;

            var tmp = this.GetBaseClass(model);
            tmp.Id = Guid.NewGuid().ToString();
            tmp.IssueTime = DateTime.Now;
            int dalRetCode = _dal.AddBirthdayGift(BirthdayType, model.SpotId.ToString(), tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "新增景点联系人生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "新增景点联系人生日礼物明细，编号：" + tmp.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改礼物明细
        /// </summary>
        /// <param name="model">景点生日礼物实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：新增失败；
        /// </returns>
        public int UpdateBirthdayGift(MSpotBirthdayGift model)
        {
            if (model == null || model.SpotId <= 0 || model.CompanyId <= 0 || string.IsNullOrEmpty(model.Id)) return 0;
            var tmp = this.GetBaseClass(model);
            int dalRetCode = _dal.UpdateBirthdayGift(tmp);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "修改景点联系人生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "修改景点联系人生日礼物明细，编号：" + tmp.Id + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteBirthdayGift(params string[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            int dalRetCode = _dal.DeleteBirthdayGift(id);
            if (dalRetCode == 1)
            {
                var log = new Model.CompanyStructure.SysHandleLogs
                {
                    EventTitle = "删除景点联系人生日礼物明细",
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.客户管理_生日中心,
                    EventMessage = "删除景点联系人生日礼物明细，编号：" + this.GetIdsByArr(id) + "。"
                };

                new SysHandleLogs().Add(log);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 根据编号获取礼物明细
        /// </summary>
        /// <param name="id">礼物明细编号</param>
        /// <returns>
        /// 礼物明细
        /// </returns>
        public MSpotBirthdayGift GetBirthdayGift(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var tmp = _dal.GetBirthdayGift(id);
            return (MSpotBirthdayGift)tmp;
        }

        /// <summary>
        /// 获取某个景点联系人收到的礼物明细
        /// </summary>
        /// <param name="spotId">景点联系人编号</param>
        /// <returns>某个景点联系人收到的礼物明细</returns>
        public IList<MSpotBirthdayGift> GetBirthdayGiftList(int spotId)
        {
            if (spotId <= 0) return null;

            var tmp = _dal.GetBirthdayGift(BirthdayType, spotId.ToString());

            return (IList<MSpotBirthdayGift>)tmp;
        }

        /// <summary>
        /// 获取景点联系人生日提醒列表（seach 有值以seach为准，为null则以days为准）
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seach">生日查询实体</param>
        /// <returns>景点联系人生日提醒列表</returns>
        public IList<MSpotBirthday> GetBirthdayList(
            int companyId,
            int pageSize,
            int pageIndex,
            ref int recordCount,
            MSearchBirthday seach)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            int days = 0;
            if (seach == null || (string.IsNullOrEmpty(seach.Name) && !seach.StartBirthday.HasValue && !seach.EndBirthday.HasValue))
            {
                var setting = new CompanySetting().GetSetting(companyId);
                if (setting != null) days = setting.BirthdayReminderDays;
            }

            var tmp = _dal.GetBirthdayList(BirthdayType, companyId, days, pageSize, pageIndex, ref recordCount, seach);
            return (IList<MSpotBirthday>)tmp;
        }
    }

    #endregion
}
