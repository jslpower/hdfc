using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;
using System.Data;
using System.Data.Common;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Model.EnumType.FinStructure;
using EyouSoft.Model.FinStructure;
using EyouSoft.Model.EnumType.PlanStructure;

namespace EyouSoft.DAL.FinStructure
{
    /// <summary>
    /// 收支登记数据访问
    /// </summary>
    public class DShouKuan : DALBase, IDAL.FinStructure.IShouKuan
    {
        private readonly Database _db;

        public DShouKuan()
        {
            _db = this.SystemStore;
        }

        /// <summary>
        /// 添加收付款登记
        /// </summary>
        /// <param name="type">收付款登记类型</param>
        /// <param name="id">收付款登记项目编号</param>
        /// <param name="model">收付款基类</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：收付款总登记和超过应收（应付）；
        /// -2：添加失败
        /// </returns>
        public int AddFinCope(KuanXiangType type, string id, MKuanBase model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.ShouKuanRenName)
                || string.IsNullOrEmpty(model.ZhangHuId) || model.OperatorId <= 0
                || (type != KuanXiangType.其它收入 && type != KuanXiangType.其它支出 && string.IsNullOrEmpty(id)))
            {
                return 0;
            }

            model.DengJiId = Guid.NewGuid().ToString();
            model.IssueTime = DateTime.Now;
            model.Status = KuanXiangStatus.未支付;

            var strSql = new StringBuilder();

            #region sql处理

            strSql.Append(" declare @result int; ");
            strSql.Append(" declare @ZongMoney money; ");
            strSql.Append(" declare @CheckMoney money; ");
            strSql.Append(" declare @ChuNaDangZhangId char(36); ");
            strSql.Append(" set @ZongMoney = 0; ");
            strSql.Append(" set @CheckMoney = 0; ");
            strSql.Append(" set @ChuNaDangZhangId = NewId(); ");
            if (type == KuanXiangType.其它收入 || type == KuanXiangType.其它支出)
            {
                strSql.Append(" set @result = -2; ");
                strSql.Append(" set @ZongMoney = @CollectionRefundAmount; ");
                strSql.Append(" set @CheckMoney = 0; ");
            }
            else
            {
                strSql.Append(" set @result = -1; ");
                strSql.Append(
                    " select @CheckMoney = IsNull(Sum(CollectionRefundAmount),0) from tbl_FinCope where CollectionId = @CollectionId and CollectionItem = @CollectionItem; ");
                switch (type)
                {
                    case KuanXiangType.计划收款:
                        strSql.Append(" select @ZongMoney = IsNull(SumPrice,0) from tbl_Tour where TourId = @CollectionId and IsDelete = '0'; ");
                        break;
                    case KuanXiangType.出票支出:
                    case KuanXiangType.退票支出:
                        strSql.AppendFormat(
                            " select @ZongMoney = IsNull(SumPrice,0) from tbl_PlanPiao where PlanId = @CollectionId and TicketStatus = {0} ; ",
                            (int)TicketStatus.已出票);
                        break;
                    case KuanXiangType.地接支出付款:
                        strSql.AppendFormat(
                            " select @ZongMoney = IsNull(SumPrice,0) from tbl_PlanDiJie where PlanId = @CollectionId and DiJieStatus = {0} ; ",
                            (int)DiJieStatus.已审核);
                        break;
                }
            }
            strSql.Append(" if @CheckMoney + @CollectionRefundAmount <= @ZongMoney ");
            strSql.Append(" begin ");
            strSql.Append(" INSERT INTO [tbl_FinCope] ([Id],[CompanyId],[CollectionId],[CollectionItem],[CollectionRefundDate],[CollectionRefundOperatorID],[CollectionRefundOperator],[CollectionRefundAmount],[CollectionRefundMode],[CollectionRefundMemo],[OtherPrice],[IsKaiPiao],[BankId],[Status],[OperatorId],[IssueTime],[CollectionItemName]) VALUES (@Id,@CompanyId,@CollectionId,@CollectionItem,@CollectionRefundDate,@CollectionRefundOperatorID,@CollectionRefundOperator,@CollectionRefundAmount,@CollectionRefundMode,@CollectionRefundMemo,@OtherPrice,@IsKaiPiao,@BankId,@Status,@OperatorId,@IssueTime,@CollectionItemName); ");
            if (model.File != null && model.File.Any())
            {
                foreach (var t in model.File)
                {
                    if (t == null) continue;

                    strSql.AppendFormat(
                        " INSERT INTO [tbl_FinCopeFilePath] ([DengJiId],[FileName],[FilePath]) VALUES (@Id,'{0}','{1}'); ",
                        Utils.ToSqlLike(t.FileName),
                        Utils.ToSqlLike(t.FilePath));
                }
            }
            if (type == KuanXiangType.计划收款)
            {
                //收款时维护确认件表收款相关字段
                strSql.AppendLine();
                strSql.AppendLine(this.GetTourShouRuSql("@CollectionId"));
            }

            #region 写出纳登账信息

            strSql.AppendLine(" declare @DengZhangType tinyint; ");
            if (type == KuanXiangType.计划收款 || type == KuanXiangType.其它收入)
            {
                strSql.AppendFormat(" set @DengZhangType = {0}; ", (int)DengJiMode.收款);
                strSql.AppendLine();
            }
            else
            {
                strSql.AppendFormat(" set @DengZhangType = {0}; ", (int)DengJiMode.退款);
                strSql.AppendLine();
            }
            strSql.AppendLine(" declare @TourNo nvarchar(100); ");
            if (type == KuanXiangType.计划收款)
            {
                strSql.AppendLine(" select @TourNo = IsNull(TourCode,'') from tbl_Tour where tbl_Tour.TourId = @CollectionId; ");
            }
            else if (type == KuanXiangType.地接支出付款)
            {
                strSql.AppendLine(" select @TourNo = IsNull(TourCode,'') from tbl_Tour where tbl_Tour.TourId = (select top 1 tbl_PlanDiJie.TourId from tbl_PlanDiJie where tbl_PlanDiJie.PlanId = @CollectionId); ");
            }
            else if (type == KuanXiangType.出票支出 || type == KuanXiangType.退票支出)
            {
                strSql.AppendLine(" select @TourNo = IsNull(TourCode,'') from tbl_Tour where tbl_Tour.TourId = (select top 1 tbl_PlanPiao.TourId from tbl_PlanPiao where tbl_PlanPiao.PlanId = @CollectionId); ");
            }
            else//其他收入支出没有团号
            {
                strSql.AppendLine(" set @TourNo = ''; ");
            }
            strSql.AppendLine(@" INSERT INTO [tbl_FinChuNaDengZhang] ([DengZhangId],[CompanyId],[DengZhangType],[DengJiId],[TourNo]
                ,[DengZhangDate],[Price],[BankId],[PayMode],[Reason],[OtherPrice],[IsKaiPiao],[IssueTime],[OperatorId],[IsAuto]
                ,[DengZhangPeople],[DengZhangPeopleId])  
                VALUES (@ChuNaDangZhangId,@CompanyId,@DengZhangType,@Id,@TourNo
                ,@CollectionRefundDate,@CollectionRefundAmount,@BankId,@CollectionRefundMode,isnull(@CollectionRefundMemo,'') + isnull(@CollectionItemName,''),@OtherPrice,@IsKaiPiao,@IssueTime,@OperatorId,'1'
                ,@CollectionRefundOperator,@CollectionRefundOperatorID); ");
            //附件信息
            if (model.File != null && model.File.Any())
            {
                foreach (var t in model.File)
                {
                    if (t == null) continue;

                    strSql.AppendFormat(
                        " INSERT INTO [tbl_FinDengZhangFilePath] ([DengZhangId],[FileName],[FilePath]) VALUES (@ChuNaDangZhangId,'{0}','{1}'); ",
                        Utils.ToSqlLike(t.FileName),
                        Utils.ToSqlLike(t.FilePath));
                }
            }

            #endregion

            strSql.Append(" set @result = 1; ");
            strSql.Append(" end ");
            strSql.Append(" select @result; ");

            #endregion

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, model.DengJiId);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "CollectionId", DbType.AnsiStringFixedLength, id);
            _db.AddInParameter(dc, "CollectionItem", DbType.Byte, (int)type);
            _db.AddInParameter(dc, "CollectionRefundDate", DbType.DateTime, model.ShouKuanRiQi);
            _db.AddInParameter(dc, "CollectionRefundOperatorID", DbType.Int32, model.ShouKuanRenId);
            _db.AddInParameter(dc, "CollectionRefundOperator", DbType.String, model.ShouKuanRenName);
            _db.AddInParameter(dc, "CollectionRefundAmount", DbType.Decimal, model.JinE);
            _db.AddInParameter(dc, "CollectionRefundMode", DbType.Byte, (int)model.FangShi);
            _db.AddInParameter(dc, "CollectionRefundMemo", DbType.String, model.ShouKuanBeiZhu);
            _db.AddInParameter(dc, "OtherPrice", DbType.Decimal, model.OtherPrice);
            _db.AddInParameter(dc, "IsKaiPiao", DbType.AnsiStringFixedLength, model.IsKaiPiao ? "1" : "0");
            _db.AddInParameter(dc, "BankId", DbType.AnsiStringFixedLength, model.ZhangHuId);
            _db.AddInParameter(dc, "Status", DbType.Byte, (int)model.Status);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, model.IssueTime);
            _db.AddInParameter(dc, "CollectionItemName", DbType.String, model.ItemName);

            object obj = DbHelper.GetSingleBySqlTrans(dc, _db);

            if (obj == null) return -2;

            return Utils.GetInt(obj.ToString(), -2);
        }

        /// <summary>
        /// 修改收付款登记
        /// </summary>
        /// <param name="model">收付款基类</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：收付款总登记和超过应收（应付）；
        /// -2：修改失败；
        /// </returns>
        public int UpdateFinCope(MKuanBase model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.ShouKuanRenName)
                || string.IsNullOrEmpty(model.ZhangHuId) || model.OperatorId <= 0
                || string.IsNullOrEmpty(model.DengJiId))
            {
                return 0;
            }

            var strSql = new StringBuilder();

            #region sql处理

            strSql.Append(" declare @result int; ");
            strSql.Append(" declare @ZongMoney money; ");
            strSql.Append(" declare @CheckMoney money; ");
            strSql.Append(" declare @ItemType TINYINT; ");
            strSql.Append(" declare @ItemId char(36); ");
            strSql.AppendLine(" declare @cndzId char(36); ");
            strSql.AppendLine(
                " select top 1 @cndzId = isnull(DengZhangId,'') from tbl_FinChuNaDengZhang where [DengJiId] = @Id ; ");
            strSql.Append(" set @ZongMoney = 0; ");
            strSql.Append(" set @CheckMoney = 0; ");
            strSql.Append(" set @ItemType = 0; ");
            strSql.Append(" set @ItemId = ''; ");
            strSql.Append(" set @result = -2; ");
            strSql.Append(" select @ItemId = CollectionId, @ItemType = CollectionItem from tbl_FinCope where Id = @Id; ");
            strSql.Append(" if @ItemType is not null ");
            strSql.Append(" begin ");
            strSql.AppendFormat(" if @ItemType = {0} or @ItemType = {1} ", (int)KuanXiangType.其它收入, (int)KuanXiangType.其它支出);
            strSql.Append(" begin ");
            strSql.Append(" set @result = -2; ");
            strSql.Append(" set @ZongMoney = @CollectionRefundAmount; ");
            strSql.Append(" set @CheckMoney = 0; ");
            strSql.Append(" end ");
            strSql.Append(" else ");
            strSql.Append(" begin ");
            strSql.Append(" if @ItemId is not null and len(@ItemId) > 0 ");
            strSql.Append(" begin ");
            strSql.Append(" set @result = -1; ");
            strSql.Append(
                " select @CheckMoney = IsNull(Sum(CollectionRefundAmount),0) from tbl_FinCope where CollectionId = @ItemId and CollectionItem = @ItemType and Id <> @Id; ");
            strSql.AppendFormat(
                " if @ItemType = {0} begin select @ZongMoney = IsNull(SumPrice,0) from tbl_Tour where TourId = @ItemId and IsDelete = '0'; end ",
                (int)KuanXiangType.计划收款);
            strSql.AppendFormat(
                " if @ItemType = {0} or @ItemType = {1} begin select @ZongMoney = IsNull(SumPrice,0) from tbl_PlanPiao where PlanId = @ItemId and TicketStatus = {2} ; end ",
                (int)KuanXiangType.出票支出,
                (int)KuanXiangType.退票支出,
                (int)TicketStatus.已出票);
            strSql.AppendFormat(
                " if @ItemType = {0} begin select @ZongMoney = IsNull(SumPrice,0) from tbl_PlanDiJie where PlanId = @ItemId and DiJieStatus = {1} ; end ",
                (int)KuanXiangType.地接支出付款,
                (int)DiJieStatus.已审核);
            strSql.Append(" end ");
            strSql.Append(" end ");
            strSql.Append(" end ");
            strSql.Append(" if @CheckMoney + @CollectionRefundAmount <= @ZongMoney ");
            strSql.Append(" begin ");
            strSql.Append(
                " UPDATE [tbl_FinCope] SET [CollectionRefundDate] = @CollectionRefundDate,[CollectionRefundOperatorID] = @CollectionRefundOperatorID,[CollectionRefundOperator] = @CollectionRefundOperator,[CollectionRefundAmount] = @CollectionRefundAmount,[CollectionRefundMode] = @CollectionRefundMode,[CollectionRefundMemo] = @CollectionRefundMemo,[OtherPrice] = @OtherPrice,[IsKaiPiao] = @IsKaiPiao,[BankId] = @BankId,[CollectionItemName] = @CollectionItemName WHERE [Id] = @Id; ");
            if (model.File != null && model.File.Any())
            {
                var oldId = (from c in model.File where c.FileId > 0 select c.FileId).ToArray();
                if (oldId.Length > 0)
                {
                    strSql.AppendFormat(
                        " insert into tbl_SysDeletedFileQue (FilePath) select FilePath from tbl_FinCopeFilePath where DengJiId = @Id and Id not in ({0}); ",
                        this.GetIdsByArr(oldId));
                    strSql.AppendFormat(
                        " delete from tbl_FinCopeFilePath where DengJiId = @Id and Id not in ({0}); ",
                        this.GetIdsByArr(oldId));
                }
                else
                {
                    strSql.AppendFormat(
                        " insert into tbl_SysDeletedFileQue (FilePath) select FilePath from tbl_FinCopeFilePath where DengJiId = @Id ; ");
                    strSql.AppendFormat(
                        " delete from tbl_FinCopeFilePath where DengJiId = @Id; ");
                }

                foreach (var t in model.File)
                {
                    if (t == null) continue;

                    if (t.FileId > 0)
                    {
                        strSql.AppendFormat(
                        " if exists (select 1 from tbl_FinCopeFilePath where DengJiId = @Id and Id = {0}) ", t.FileId);
                        strSql.Append(" begin ");
                        strSql.AppendFormat(
                            " if exists (select 1 from tbl_FinCopeFilePath where DengJiId = @Id and Id = {0} and FilePath <> '{1}') ",
                            t.FileId,
                            Utils.ToSqlLike(t.FilePath));
                        strSql.Append(" begin ");
                        strSql.AppendFormat(
                            " insert into tbl_SysDeletedFileQue (FilePath) select FilePath from tbl_FinCopeFilePath where DengJiId = @Id and Id = {0}; ",
                            t.FileId);
                        strSql.Append(" end ");
                        strSql.AppendFormat(
                            " update tbl_FinCopeFilePath set FileName = '{0}',FilePath = '{1}' where DengJiId = @Id and Id = {2}; ",
                            Utils.ToSqlLike(t.FileName),
                            Utils.ToSqlLike(t.FilePath),
                            t.FileId);
                        strSql.Append(" end ");
                    }
                    else
                    {
                        strSql.AppendFormat(
                        " INSERT INTO [tbl_FinCopeFilePath] ([DengJiId],[FileName],[FilePath]) VALUES (@Id,'{0}','{1}'); ",
                        Utils.ToSqlLike(t.FileName),
                        Utils.ToSqlLike(t.FilePath));
                    }
                }
            }
            else
            {
                strSql.Append(
                    " insert into tbl_SysDeletedFileQue (FilePath) select FilePath from tbl_FinCopeFilePath where DengJiId = @Id; ");
                strSql.Append(" delete from tbl_FinCopeFilePath where DengJiId = @Id; ");
            }
            strSql.AppendFormat(" if @ItemType = {0} ", (int)KuanXiangType.计划收款);
            strSql.Append(" begin ");
            //收款时维护确认件表收款相关字段
            strSql.AppendLine();
            strSql.AppendLine(this.GetTourShouRuSql("@ItemId"));
            strSql.Append(" end ");

            #region 修改出纳登账信息

            strSql.AppendLine(
                " UPDATE [tbl_FinChuNaDengZhang] SET [DengZhangPeopleId] = @CollectionRefundOperatorID,[DengZhangPeople] = @CollectionRefundOperator,[DengZhangDate] = @CollectionRefundDate,[Price] = @CollectionRefundAmount,[BankId] = @BankId,[PayMode] = @CollectionRefundMode,[Reason] = isnull(@CollectionRefundMemo,'') + isnull(@CollectionItemName,''),[OtherPrice] = @OtherPrice,[IsKaiPiao] = @IsKaiPiao WHERE [DengJiId] = @Id; ");
            //附件信息
            if (model.File != null && model.File.Any())
            {
                strSql.AppendLine(" delete from [tbl_FinDengZhangFilePath] where [DengZhangId] = @cndzId; ");
                foreach (var t in model.File)
                {
                    if (t == null) continue;

                    strSql.AppendFormat(
                        " INSERT INTO [tbl_FinDengZhangFilePath] ([DengZhangId],[FileName],[FilePath]) VALUES (@cndzId,'{0}','{1}'); ",
                        Utils.ToSqlLike(t.FileName),
                        Utils.ToSqlLike(t.FilePath));
                }
            }
            else
            {
                strSql.AppendLine(" delete from [tbl_FinDengZhangFilePath] where [DengZhangId] = @cndzId; ");
            }

            #endregion

            strSql.Append(" set @result = 1; ");
            strSql.Append(" end ");
            strSql.Append(" select @result; ");


            #endregion

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, model.DengJiId);
            _db.AddInParameter(dc, "CollectionRefundDate", DbType.DateTime, model.ShouKuanRiQi);
            _db.AddInParameter(dc, "CollectionRefundOperatorID", DbType.Int32, model.ShouKuanRenId);
            _db.AddInParameter(dc, "CollectionRefundOperator", DbType.String, model.ShouKuanRenName);
            _db.AddInParameter(dc, "CollectionRefundAmount", DbType.Decimal, model.JinE);
            _db.AddInParameter(dc, "CollectionRefundMode", DbType.Byte, (int)model.FangShi);
            _db.AddInParameter(dc, "CollectionRefundMemo", DbType.String, model.ShouKuanBeiZhu);
            _db.AddInParameter(dc, "OtherPrice", DbType.Decimal, model.OtherPrice);
            _db.AddInParameter(dc, "IsKaiPiao", DbType.AnsiStringFixedLength, model.IsKaiPiao ? "1" : "0");
            _db.AddInParameter(dc, "BankId", DbType.AnsiStringFixedLength, model.ZhangHuId);
            _db.AddInParameter(dc, "CollectionItemName", DbType.String, model.ItemName);

            object obj = DbHelper.GetSingleBySqlTrans(dc, _db);

            if (obj == null) return -2;

            return Utils.GetInt(obj.ToString(), -2);
        }

        /// <summary>
        /// 删除收付款登记
        /// </summary>
        /// <param name="id">收付款登记编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：删除失败；
        /// </returns>
        public int DeleteFinCope(string id)
        {
            if (string.IsNullOrEmpty(id)) return 0;

            var strSql = new StringBuilder();
            strSql.Append(" declare @ItemId char(36); ");
            strSql.Append(" declare @ItemType tinyint; ");
            strSql.Append(" declare @CheckMoney money; ");
            strSql.Append(" set @CheckMoney = 0; ");
            strSql.Append(" select @ItemId = CollectionId,@ItemType = CollectionItem from tbl_FinCope where Id = @Id; ");
            strSql.Append(
                " insert into tbl_SysDeletedFileQue (FilePath) select FilePath from tbl_FinCopeFilePath where DengJiId = @Id; ");
            strSql.Append(" delete from tbl_FinCopeFilePath where DengJiId = @Id; ");
            strSql.AppendFormat(" delete from tbl_FinCope where Id = @Id; ");
            //维护计划的收款情况
            strSql.AppendFormat(
                " if @ItemType is not null and @ItemType = {0} and @ItemId is not null and len(@ItemId) > 0 ",
                (int)KuanXiangType.计划收款);
            strSql.Append(" begin ");
            strSql.Append(this.GetTourShouRuSql("@ItemId"));
            strSql.Append(" end ");
            //删除出纳登账信息
            strSql.AppendLine(" delete from [tbl_FinDengZhangFilePath] where [DengZhangId] = (select top 1 DengZhangId from tbl_FinChuNaDengZhang where [DengJiId] = @Id); ");
            strSql.AppendLine(" delete from tbl_FinChuNaDengZhang where DengJiId = @Id; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, id);

            return DbHelper.ExecuteSqlTrans(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 获取收付款登记信息
        /// </summary>
        /// <param name="id">收付款登记编号</param>
        /// <returns></returns>
        public object GetFinCope(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            MShouKuan shouKuan = null;
            MDiJieFuKuan diJieFuKuan = null;
            MPiaoFuKuan piaoFuKuan = null;
            MQiTaShouKuan qiTaShouKuan = null;
            MQiTaFuKuan qiTaFuKuan = null;
            KuanXiangType type = KuanXiangType.计划收款;
            var strSql = new StringBuilder();

            strSql.AppendLine(" select  ");
            strSql.AppendLine(" [Id],[CompanyId],[CollectionId],[CollectionItem],[CollectionRefundDate],[CollectionRefundOperatorID],[CollectionRefundOperator],[CollectionRefundAmount],[CollectionRefundMode],[CollectionRefundMemo],[OtherPrice],[IsKaiPiao],[BankId],[Status],[OperatorId],[IssueTime],[CollectionItemName] ");
            strSql.AppendLine(" ,(select BankName + '  ' + AccountName + '  ' + BankNo from tbl_CompanyAccount as ca where ca.Id = tbl_FinCope.BankId) as BankName ");
            strSql.AppendLine(" from tbl_FinCope where Id = @Id; ");
            strSql.AppendLine(" select  ");
            strSql.AppendLine(" [Id],DengJiId,[FileName],[FilePath] ");
            strSql.AppendLine(" from tbl_FinCopeFilePath where DengJiId = @Id ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, id);

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    type = (KuanXiangType)dr.GetByte(dr.GetOrdinal("CollectionItem"));

                    #region 基本信息赋值

                    switch (type)
                    {
                        case KuanXiangType.计划收款:
                            shouKuan = new MShouKuan
                                {
                                    DengJiId =
                                        dr.IsDBNull(dr.GetOrdinal("Id"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("Id")),
                                    CompanyId =
                                        dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                            ? 0
                                            : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                    TourId =
                                        dr.IsDBNull(dr.GetOrdinal("CollectionId"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("CollectionId")),
                                    ShouKuanRenId =
                                        dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                            ? 0
                                            : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                    ShouKuanRenName =
                                        dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                    JinE =
                                        dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                            ? 0M
                                            : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                    FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                    ShouKuanBeiZhu =
                                        dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                    OtherPrice =
                                        dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                            ? 0M
                                            : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                    ZhangHuId =
                                        dr.IsDBNull(dr.GetOrdinal("BankId"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("BankId")),
                                    Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                    OperatorId =
                                        dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                            ? 0
                                            : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                    ZhangHuCode =
                                        dr.IsDBNull(dr.GetOrdinal("BankName"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("BankName")),
                                    ItemName =
                                        dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                            ? string.Empty
                                            : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                                };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                shouKuan.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                shouKuan.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                shouKuan.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            break;
                        case KuanXiangType.出票支出:
                            piaoFuKuan = new MPiaoFuKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                PiaoPlanId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                piaoFuKuan.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                piaoFuKuan.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                piaoFuKuan.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            piaoFuKuan.TicketType = TicketMode.出票;
                            break;
                        case KuanXiangType.地接支出付款:
                            diJieFuKuan = new MDiJieFuKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                DiJiePlanId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                diJieFuKuan.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                diJieFuKuan.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                diJieFuKuan.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            break;
                        case KuanXiangType.退票支出:
                            piaoFuKuan = new MPiaoFuKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                PiaoPlanId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                piaoFuKuan.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                piaoFuKuan.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                piaoFuKuan.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            piaoFuKuan.TicketType = TicketMode.退票;
                            break;
                        case KuanXiangType.其它收入:
                            qiTaShouKuan = new MQiTaShouKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                qiTaShouKuan.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                qiTaShouKuan.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                qiTaShouKuan.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            break;
                        case KuanXiangType.其它支出:
                            qiTaFuKuan = new MQiTaFuKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                qiTaFuKuan.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                qiTaFuKuan.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                qiTaFuKuan.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            break;
                    }

                    #endregion
                }

                #region 附件信息

                dr.NextResult();
                while (dr.Read())
                {
                    switch (type)
                    {
                        case KuanXiangType.出票支出:
                        case KuanXiangType.退票支出:
                            if (piaoFuKuan != null)
                            {
                                if (piaoFuKuan.File == null) piaoFuKuan.File = new List<MKuanFile>();
                                piaoFuKuan.File.Add(
                                    new MKuanFile
                                        {
                                            DengJiId =
                                                dr.IsDBNull(dr.GetOrdinal("DengJiId"))
                                                    ? string.Empty
                                                    : dr.GetString(dr.GetOrdinal("DengJiId")),
                                            FileId =
                                                dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                                            FileName =
                                                dr.IsDBNull(dr.GetOrdinal("FileName"))
                                                    ? string.Empty
                                                    : dr.GetString(dr.GetOrdinal("FileName")),
                                            FilePath =
                                                dr.IsDBNull(dr.GetOrdinal("FilePath"))
                                                    ? string.Empty
                                                    : dr.GetString(dr.GetOrdinal("FilePath"))
                                        });
                            }
                            break;
                        case KuanXiangType.地接支出付款:
                            if (diJieFuKuan != null)
                            {
                                if (diJieFuKuan.File == null) diJieFuKuan.File = new List<MKuanFile>();
                                diJieFuKuan.File.Add(
                                    new MKuanFile
                                    {
                                        DengJiId =
                                            dr.IsDBNull(dr.GetOrdinal("DengJiId"))
                                                ? string.Empty
                                                : dr.GetString(dr.GetOrdinal("DengJiId")),
                                        FileId =
                                            dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                                        FileName =
                                            dr.IsDBNull(dr.GetOrdinal("FileName"))
                                                ? string.Empty
                                                : dr.GetString(dr.GetOrdinal("FileName")),
                                        FilePath =
                                            dr.IsDBNull(dr.GetOrdinal("FilePath"))
                                                ? string.Empty
                                                : dr.GetString(dr.GetOrdinal("FilePath"))
                                    });
                            }
                            break;
                        case KuanXiangType.计划收款:
                            if (shouKuan != null)
                            {
                                if (shouKuan.File == null) shouKuan.File = new List<MKuanFile>();
                                shouKuan.File.Add(
                                    new MKuanFile
                                    {
                                        DengJiId =
                                            dr.IsDBNull(dr.GetOrdinal("DengJiId"))
                                                ? string.Empty
                                                : dr.GetString(dr.GetOrdinal("DengJiId")),
                                        FileId =
                                            dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                                        FileName =
                                            dr.IsDBNull(dr.GetOrdinal("FileName"))
                                                ? string.Empty
                                                : dr.GetString(dr.GetOrdinal("FileName")),
                                        FilePath =
                                            dr.IsDBNull(dr.GetOrdinal("FilePath"))
                                                ? string.Empty
                                                : dr.GetString(dr.GetOrdinal("FilePath"))
                                    });
                            }
                            break;
                        case KuanXiangType.其它收入:
                            if (qiTaShouKuan != null)
                            {
                                if (qiTaShouKuan.File == null) qiTaShouKuan.File = new List<MKuanFile>();
                                qiTaShouKuan.File.Add(
                                    new MKuanFile
                                    {
                                        DengJiId =
                                            dr.IsDBNull(dr.GetOrdinal("DengJiId"))
                                                ? string.Empty
                                                : dr.GetString(dr.GetOrdinal("DengJiId")),
                                        FileId =
                                            dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                                        FileName =
                                            dr.IsDBNull(dr.GetOrdinal("FileName"))
                                                ? string.Empty
                                                : dr.GetString(dr.GetOrdinal("FileName")),
                                        FilePath =
                                            dr.IsDBNull(dr.GetOrdinal("FilePath"))
                                                ? string.Empty
                                                : dr.GetString(dr.GetOrdinal("FilePath"))
                                    });
                            }
                            break;
                        case KuanXiangType.其它支出:
                            if (qiTaFuKuan != null)
                            {
                                if (qiTaFuKuan.File == null) qiTaFuKuan.File = new List<MKuanFile>();
                                qiTaFuKuan.File.Add(
                                    new MKuanFile
                                    {
                                        DengJiId =
                                            dr.IsDBNull(dr.GetOrdinal("DengJiId"))
                                                ? string.Empty
                                                : dr.GetString(dr.GetOrdinal("DengJiId")),
                                        FileId =
                                            dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                                        FileName =
                                            dr.IsDBNull(dr.GetOrdinal("FileName"))
                                                ? string.Empty
                                                : dr.GetString(dr.GetOrdinal("FileName")),
                                        FilePath =
                                            dr.IsDBNull(dr.GetOrdinal("FilePath"))
                                                ? string.Empty
                                                : dr.GetString(dr.GetOrdinal("FilePath"))
                                    });
                            }
                            break;
                    }
                }

                #endregion

            }

            switch (type)
            {
                case KuanXiangType.出票支出:
                case KuanXiangType.退票支出:
                    return piaoFuKuan;
                case KuanXiangType.地接支出付款:
                    return diJieFuKuan;
                case KuanXiangType.计划收款:
                    return shouKuan;
                case KuanXiangType.其它收入:
                    return qiTaShouKuan;
                case KuanXiangType.其它支出:
                    return qiTaFuKuan;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取收付款登记信息
        /// </summary>
        /// <param name="type">收付款登记类型</param>
        /// <param name="id">收付款登记项目编号</param>
        /// <returns></returns>
        public object GetFinCopeList(KuanXiangType type, string id)
        {
            if (type != KuanXiangType.其它收入 && type != KuanXiangType.其它支出 && string.IsNullOrEmpty(id)) return null;

            IList<MShouKuan> shouKuan = null;
            IList<MDiJieFuKuan> diJieFuKuan = null;
            IList<MPiaoFuKuan> piaoFuKuan = null;
            IList<MQiTaShouKuan> qiTaShouKuan = null;
            IList<MQiTaFuKuan> qiTaFuKuan = null;
            var strSql = new StringBuilder();

            strSql.AppendLine(" select ");
            strSql.AppendLine(
                " [Id],[CompanyId],[CollectionId],[CollectionItem],[CollectionRefundDate],[CollectionRefundOperatorID],[CollectionRefundOperator],[CollectionRefundAmount],[CollectionRefundMode],[CollectionRefundMemo],[OtherPrice],[IsKaiPiao],[BankId],[Status],[OperatorId],[IssueTime],[CollectionItemName] ");
            strSql.AppendLine(" ,(select BankName + '  ' + AccountName + '  ' + BankNo from tbl_CompanyAccount as ca where ca.Id = tbl_FinCope.BankId) as BankName ");
            strSql.AppendLine(
                " ,(select Id,DengJiId,FileName,FilePath from tbl_FinCopeFilePath where tbl_FinCopeFilePath.DengJiId = tbl_FinCope.Id for xml raw,root('Root')) as FileList ");
            strSql.Append(" from tbl_FinCope where CollectionItem = @CollectionItem ");
            if (type != KuanXiangType.其它收入 && type != KuanXiangType.其它支出)
            {
                strSql.AppendFormat(" and CollectionId = '{0}' ", id);
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "CollectionItem", DbType.Byte, (int)type);

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                #region 实体赋值

                while (dr.Read())
                {
                    switch (type)
                    {
                        case KuanXiangType.计划收款:
                            if (shouKuan == null) shouKuan = new List<MShouKuan>();
                            var sk = new MShouKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                TourId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                sk.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                sk.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                sk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("FileList")))
                            {
                                sk.File = this.GetFileBySqlXml(dr.GetString(dr.GetOrdinal("FileList")));
                            }

                            shouKuan.Add(sk);
                            break;
                        case KuanXiangType.出票支出:
                            if (piaoFuKuan == null) piaoFuKuan = new List<MPiaoFuKuan>();
                            var pfk = new MPiaoFuKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                PiaoPlanId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                pfk.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                pfk.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                pfk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("FileList")))
                            {
                                pfk.File = this.GetFileBySqlXml(dr.GetString(dr.GetOrdinal("FileList")));
                            }
                            pfk.TicketType = TicketMode.出票;

                            piaoFuKuan.Add(pfk);
                            break;
                        case KuanXiangType.退票支出:
                            if (piaoFuKuan == null) piaoFuKuan = new List<MPiaoFuKuan>();
                            var tpfk = new MPiaoFuKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                PiaoPlanId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                tpfk.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                tpfk.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                tpfk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("FileList")))
                            {
                                tpfk.File = this.GetFileBySqlXml(dr.GetString(dr.GetOrdinal("FileList")));
                            }
                            tpfk.TicketType = TicketMode.退票;

                            piaoFuKuan.Add(tpfk);
                            break;
                        case KuanXiangType.地接支出付款:
                            if (diJieFuKuan == null) diJieFuKuan = new List<MDiJieFuKuan>();
                            var djfk = new MDiJieFuKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                DiJiePlanId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                djfk.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                djfk.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                djfk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("FileList")))
                            {
                                djfk.File = this.GetFileBySqlXml(dr.GetString(dr.GetOrdinal("FileList")));
                            }

                            diJieFuKuan.Add(djfk);
                            break;
                        case KuanXiangType.其它收入:
                            if (qiTaShouKuan == null) qiTaShouKuan = new List<MQiTaShouKuan>();
                            var qtsk = new MQiTaShouKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                qtsk.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                qtsk.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                qtsk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("FileList")))
                            {
                                qtsk.File = this.GetFileBySqlXml(dr.GetString(dr.GetOrdinal("FileList")));
                            }

                            qiTaShouKuan.Add(qtsk);
                            break;
                        case KuanXiangType.其它支出:
                            if (qiTaFuKuan == null) qiTaFuKuan = new List<MQiTaFuKuan>();
                            var qtfk = new MQiTaFuKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                qtfk.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                qtfk.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                qtfk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("FileList")))
                            {
                                qtfk.File = this.GetFileBySqlXml(dr.GetString(dr.GetOrdinal("FileList")));
                            }

                            qiTaFuKuan.Add(qtfk);
                            break;
                    }
                }

                #endregion
            }

            switch (type)
            {
                case KuanXiangType.出票支出:
                case KuanXiangType.退票支出:
                    return piaoFuKuan;
                case KuanXiangType.地接支出付款:
                    return diJieFuKuan;
                case KuanXiangType.计划收款:
                    return shouKuan;
                case KuanXiangType.其它收入:
                    return qiTaShouKuan;
                case KuanXiangType.其它支出:
                    return qiTaFuKuan;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取收付款登记信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="type">收付款登记类型</param>
        /// <param name="search">其他收入支出查询实体</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public object GetFinCopeList(int companyId, KuanXiangType type, int pageSize, int pageIndex, ref int recordCount, MSearchOther search)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 0)
            {
                return null;
            }

            string tableName = "tbl_FinCope";
            var strWhere = new StringBuilder();
            var fields = new StringBuilder();
            string orderByStr = " CollectionRefundDate desc,IssueTime desc ";

            #region sql处理

            fields.Append(
                " [Id],[CompanyId],[CollectionId],[CollectionItem],[CollectionRefundDate],[CollectionRefundOperatorID],[CollectionItemName],[CollectionRefundOperator],[CollectionRefundAmount],[CollectionRefundMode],[CollectionRefundMemo],[OtherPrice],[IsKaiPiao],[BankId],[Status],[OperatorId],[IssueTime] ");
            fields.Append(" ,(select BankName + '  ' + AccountName + '  ' + BankNo from tbl_CompanyAccount as ca where ca.Id = tbl_FinCope.BankId) as BankName ");
            fields.Append(
                " ,(select Id,DengJiId,FileName,FilePath from tbl_FinCopeFilePath where tbl_FinCopeFilePath.DengJiId = tbl_FinCope.Id for xml raw,root('Root')) as FileList ");
            strWhere.AppendFormat(" [CompanyId] = {0} and CollectionItem = {1} ", companyId, (int)type);
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.ShouZhiXingMu))
                {
                    strWhere.AppendFormat(
                        " and CollectionItemName like '%{0}%' ", Utils.ToSqlLike(search.ShouZhiXingMu));
                }
                if (search.StartTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',CollectionRefundDate) >= 0 ", search.StartTime.Value.ToShortDateString());
                }
                if (search.EndTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',CollectionRefundDate) <= 0 ", search.EndTime.Value.ToShortDateString());
                }
            }

            #endregion

            IList<MShouKuan> shouKuan = null;
            IList<MDiJieFuKuan> diJieFuKuan = null;
            IList<MPiaoFuKuan> piaoFuKuan = null;
            IList<MQiTaShouKuan> qiTaShouKuan = null;
            IList<MQiTaFuKuan> qiTaFuKuan = null;

            using (IDataReader dr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fields.ToString()
                , strWhere.ToString(), orderByStr, string.Empty))
            {
                #region 实体赋值

                while (dr.Read())
                {
                    switch (type)
                    {
                        case KuanXiangType.计划收款:
                            if (shouKuan == null) shouKuan = new List<MShouKuan>();
                            var sk = new MShouKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                TourId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                sk.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                sk.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                sk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("FileList")))
                            {
                                sk.File = this.GetFileBySqlXml(dr.GetString(dr.GetOrdinal("FileList")));
                            }

                            shouKuan.Add(sk);
                            break;
                        case KuanXiangType.出票支出:
                            if (piaoFuKuan == null) piaoFuKuan = new List<MPiaoFuKuan>();
                            var pfk = new MPiaoFuKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                PiaoPlanId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                pfk.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                pfk.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                pfk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("FileList")))
                            {
                                pfk.File = this.GetFileBySqlXml(dr.GetString(dr.GetOrdinal("FileList")));
                            }
                            pfk.TicketType = TicketMode.出票;

                            piaoFuKuan.Add(pfk);
                            break;
                        case KuanXiangType.退票支出:
                            if (piaoFuKuan == null) piaoFuKuan = new List<MPiaoFuKuan>();
                            var tpfk = new MPiaoFuKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                PiaoPlanId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                tpfk.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                tpfk.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                tpfk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("FileList")))
                            {
                                tpfk.File = this.GetFileBySqlXml(dr.GetString(dr.GetOrdinal("FileList")));
                            }
                            tpfk.TicketType = TicketMode.退票;

                            piaoFuKuan.Add(tpfk);
                            break;
                        case KuanXiangType.地接支出付款:
                            if (diJieFuKuan == null) diJieFuKuan = new List<MDiJieFuKuan>();
                            var djfk = new MDiJieFuKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                DiJiePlanId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                djfk.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                djfk.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                djfk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("FileList")))
                            {
                                djfk.File = this.GetFileBySqlXml(dr.GetString(dr.GetOrdinal("FileList")));
                            }

                            diJieFuKuan.Add(djfk);
                            break;
                        case KuanXiangType.其它收入:
                            if (qiTaShouKuan == null) qiTaShouKuan = new List<MQiTaShouKuan>();
                            var qtsk = new MQiTaShouKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                qtsk.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                qtsk.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                qtsk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("FileList")))
                            {
                                qtsk.File = this.GetFileBySqlXml(dr.GetString(dr.GetOrdinal("FileList")));
                            }

                            qiTaShouKuan.Add(qtsk);
                            break;
                        case KuanXiangType.其它支出:
                            if (qiTaFuKuan == null) qiTaFuKuan = new List<MQiTaFuKuan>();
                            var qtfk = new MQiTaFuKuan
                            {
                                DengJiId =
                                    dr.IsDBNull(dr.GetOrdinal("Id"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("Id")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                ShouKuanRenId =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperatorID"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CollectionRefundOperatorID")),
                                ShouKuanRenName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundOperator"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundOperator")),
                                JinE =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundAmount"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("CollectionRefundAmount")),
                                FangShi = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("CollectionRefundMode")),
                                ShouKuanBeiZhu =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionRefundMemo"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionRefundMemo")),
                                OtherPrice =
                                    dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                                ZhangHuId =
                                    dr.IsDBNull(dr.GetOrdinal("BankId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankId")),
                                Status = (KuanXiangStatus)dr.GetByte(dr.GetOrdinal("Status")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                ZhangHuCode =
                                    dr.IsDBNull(dr.GetOrdinal("BankName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("BankName")),
                                ItemName =
                                    dr.IsDBNull(dr.GetOrdinal("CollectionItemName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CollectionItemName"))
                            };
                            if (!dr.IsDBNull(dr.GetOrdinal("CollectionRefundDate")))
                            {
                                qtfk.ShouKuanRiQi = dr.GetDateTime(dr.GetOrdinal("CollectionRefundDate"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao")))
                            {
                                qtfk.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                            {
                                qtfk.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("FileList")))
                            {
                                qtfk.File = this.GetFileBySqlXml(dr.GetString(dr.GetOrdinal("FileList")));
                            }

                            qiTaFuKuan.Add(qtfk);
                            break;
                    }
                }

                #endregion
            }

            switch (type)
            {
                case KuanXiangType.出票支出:
                case KuanXiangType.退票支出:
                    return piaoFuKuan;
                case KuanXiangType.地接支出付款:
                    return diJieFuKuan;
                case KuanXiangType.计划收款:
                    return shouKuan;
                case KuanXiangType.其它收入:
                    return qiTaShouKuan;
                case KuanXiangType.其它支出:
                    return qiTaFuKuan;
                default:
                    return null;
            }
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
        public IList<MDiJieList> GetDiJieList(int companyId, int pageSize, int pageIndex, ref int recordCount
            , MSearchDiJieList search, MPlanHeJi heJi)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 0)
            {
                return null;
            }

            if (heJi == null) heJi = new MPlanHeJi();
            var tableName = new StringBuilder();
            string orderByStr = " DiJieStatus asc, IssueTime desc, LDate desc ";
            string sumStr = " Sum(YingFuKuan) as SumYingFuKuan,Sum(YiFuKuan) as SumYiFuKuan ";
            string fields = " * ";

            #region tableName处理

            tableName.AppendLine(" SELECT PlanId,CompanyId,TourId,GysId,YingFuKuan,DiJieStatus,TourCode,RouteName,LDate,Adults,Childs,Accompanys,GSYName,PlanOperatorName,YiFuKuan,ContactList,IssueTime,TourStatus,TourType FROM ( ");
            tableName.AppendLine(" SELECT pdj.PlanId,pdj.CompanyId,pdj.TourId,pdj.GysId,pdj.SumPrice AS YingFuKuan,pdj.DiJieStatus,pdj.IssueTime,pdj.IsMonth ");
            tableName.AppendLine(" ,t.TourCode,t.RouteName,t.LDate,t.Adults,t.Childs,t.Accompanys,t.TourStatus,t.TourType ");
            tableName.AppendLine(" ,cs.UnitName AS GSYName ");
            tableName.AppendLine(" ,cu.ContactName AS PlanOperatorName ");
            tableName.AppendFormat(
                " ,(SELECT ISNULL(SUM(fc.CollectionRefundAmount),0) FROM tbl_FinCope AS fc WHERE fc.CollectionId = pdj.PlanId AND fc.CollectionItem = {0} AND fc.[Status] = {1}) AS YiFuKuan ",
                (int)KuanXiangType.地接支出付款,
                (int)KuanXiangStatus.未支付);
            tableName.AppendLine();
            tableName.AppendLine(
                " ,(SELECT sc.ContactName,sc.ContactTel,sc.QQ FROM tbl_SupplierContact AS sc WHERE sc.SupplierId = pdj.GysId FOR XML RAW,ROOT('Root')) AS ContactList ");
            tableName.AppendLine(" FROM tbl_PlanDiJie pdj LEFT JOIN tbl_Tour t ON pdj.TourId = t.TourId ");
            tableName.AppendLine(" LEFT JOIN tbl_CompanySupplier cs ON pdj.GysId = cs.Id ");
            tableName.AppendLine(" LEFT JOIN tbl_CompanyUser cu ON t.OperatorId = cu.Id ");
            tableName.AppendFormat(
                " where pdj.DiJieStatus in ({0},{2}) and pdj.CompanyId = {1} ",
                (int)DiJieStatus.已确认,
                companyId,
                (int)DiJieStatus.已审核);
            tableName.AppendLine();
            tableName.AppendLine(" ) as a ");
            tableName.AppendLine(" where 1 = 1 ");
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.TourNo))
                {
                    tableName.AppendFormat(" and TourCode like '%{0}%' ", Utils.ToSqlLike(search.TourNo));
                    tableName.AppendLine();
                }
                if (search.StartLeaveDate.HasValue)
                {
                    tableName.AppendFormat(
                        " and datediff(dd,'{0}',LDate) >= 0 ", search.StartLeaveDate.Value.ToShortDateString());
                    tableName.AppendLine();
                }
                if (search.EndLeaveDate.HasValue)
                {
                    tableName.AppendFormat(
                        " and datediff(dd,'{0}',LDate) <= 0 ", search.EndLeaveDate.Value.ToShortDateString());
                    tableName.AppendLine();
                }
                if (search.IsCheck.HasValue)
                {
                    tableName.AppendFormat(
                        search.IsCheck.Value ? " and DiJieStatus = {0} " : " and DiJieStatus <> {0} ",
                        (int)DiJieStatus.已审核);
                    tableName.AppendLine();
                }
                if (search.IsJieQing.HasValue)
                {
                    tableName.AppendLine(
                        search.IsJieQing.Value ? " and YingFuKuan = YiFuKuan " : " and YingFuKuan <> YiFuKuan ");
                }
                if (search.IsYueJie.HasValue)
                {
                    tableName.AppendFormat(" and IsMonth = '{0}' ", search.IsYueJie.Value ? "1" : "0");
                    tableName.AppendLine();
                }
                if (search.TourType.HasValue)
                {
                    tableName.AppendFormat(" and TourType = {0} ", (int)search.TourType.Value);
                    tableName.AppendLine();
                }
            }
            else
            {
                tableName.AppendLine(" and YingFuKuan <> YiFuKuan ");
            }

            #endregion

            IList<MDiJieList> list = new List<MDiJieList>();
            using (IDataReader dr = DbHelper.ExecuteReader2(_db, pageSize, pageIndex, ref recordCount, tableName.ToString()
                , fields, string.Empty, orderByStr, sumStr))
            {
                while (dr.Read())
                {
                    #region 实体赋值

                    var tmp = new MDiJieList
                        {
                            PlanId =
                                dr.IsDBNull(dr.GetOrdinal("PlanId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("PlanId")),
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            TourId =
                                dr.IsDBNull(dr.GetOrdinal("TourId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("TourId")),
                            TourCode =
                                dr.IsDBNull(dr.GetOrdinal("TourCode"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("TourCode")),
                            RouteName =
                                dr.IsDBNull(dr.GetOrdinal("RouteName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("RouteName")),
                            Adults = dr.IsDBNull(dr.GetOrdinal("Adults")) ? 0 : dr.GetInt32(dr.GetOrdinal("Adults")),
                            Childs = dr.IsDBNull(dr.GetOrdinal("Childs")) ? 0 : dr.GetInt32(dr.GetOrdinal("Childs")),
                            Accompanys =
                                dr.IsDBNull(dr.GetOrdinal("Accompanys")) ? 0 : dr.GetInt32(dr.GetOrdinal("Accompanys")),
                            GysId =
                                dr.IsDBNull(dr.GetOrdinal("GysId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("GysId")),
                            GysName =
                                dr.IsDBNull(dr.GetOrdinal("GSYName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("GSYName")),
                            PlanPeopleName =
                                dr.IsDBNull(dr.GetOrdinal("PlanOperatorName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("PlanOperatorName")),
                            YingFuKuan =
                                dr.IsDBNull(dr.GetOrdinal("YingFuKuan"))
                                    ? 0M
                                    : dr.GetDecimal(dr.GetOrdinal("YingFuKuan")),
                            YiFuKuan =
                                dr.IsDBNull(dr.GetOrdinal("YiFuKuan")) ? 0M : dr.GetDecimal(dr.GetOrdinal("YiFuKuan")),
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("LDate")))
                    {
                        tmp.LDate = dr.GetDateTime(dr.GetOrdinal("LDate"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactList")))
                    {
                        tmp.Contact = this.GetContactBySqlXml(dr.GetString(dr.GetOrdinal("ContactList")));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("DiJieStatus")))
                    {
                        tmp.State = (DiJieStatus)dr.GetByte(dr.GetOrdinal("DiJieStatus"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("TourStatus")))
                    {
                        tmp.TourState =
                            (Model.EnumType.TourStructure.TourStatus)dr.GetByte(dr.GetOrdinal("TourStatus"));
                    }

                    #endregion

                    list.Add(tmp);
                }

                dr.NextResult();
                if (dr.Read())
                {
                    if (!dr.IsDBNull(dr.GetOrdinal("SumYingFuKuan")))
                    {
                        heJi.YingFuKuan = dr.GetDecimal(dr.GetOrdinal("SumYingFuKuan"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("SumYiFuKuan")))
                    {
                        heJi.YiFuKuan = dr.GetDecimal(dr.GetOrdinal("SumYiFuKuan"));
                    }
                }
            }

            return list;
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
        public IList<MPiaoList> GetPiaoList(int companyId, int pageSize, int pageIndex, ref int recordCount
            , MSearchPiaoList search, MPlanHeJi heJi)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 0)
            {
                return null;
            }

            if (heJi == null) heJi = new MPlanHeJi();
            var tableName = new StringBuilder();
            string orderByStr = " TicketStatus asc, LDate desc,IssueTime desc ";
            string sumStr = " Sum(YingFuKuan) as SumYingFuKuan,Sum(YiFuKuan) as SumYiFuKuan ";
            string fields = " * ";

            #region tableName处理

            tableName.AppendLine(" select  ");
            tableName.AppendLine(" PlanId,CompanyId,TicketMode,TourId,Adults,Childs,TrafficNumber,Interval,TicketerId,GysId,TicketStatus,PayType,YingFuKuan,TourCode,LDate,TicketName,GysName,YiFuKuan,IssueTime,TourStatus,TourType ");
            tableName.AppendLine(" from ( ");
            tableName.AppendLine(" select ");
            tableName.AppendLine(" pp.PlanId,pp.CompanyId,pp.TicketMode,pp.TourId,pp.Adults,pp.Childs,pp.TrafficNumber,pp.Interval,pp.TicketerId,pp.GysId,pp.TicketStatus,pp.PayType,pp.SumPrice AS YingFuKuan,pp.IssueTime,pp.IsMonth ");
            tableName.AppendLine(" ,t.TourCode,t.LDate,t.TourStatus,t.TourType ");
            tableName.AppendLine(" ,cu.ContactName AS TicketName ");
            tableName.AppendLine(" ,cs.UnitName AS GysName ");
            tableName.AppendFormat(
                " ,(SELECT ISNULL(SUM(fc.CollectionRefundAmount),0) FROM tbl_FinCope AS fc WHERE fc.CollectionId = pp.PlanId AND fc.CollectionItem in ({0},{1}) AND fc.[Status] = {2}) AS YiFuKuan ",
                (int)KuanXiangType.出票支出,
                (int)KuanXiangType.退票支出,
                (int)KuanXiangStatus.未支付);
            tableName.AppendLine();
            tableName.AppendLine(" FROM tbl_PlanPiao AS pp LEFT JOIN tbl_Tour t ON pp.TourId = t.TourId ");
            tableName.AppendLine(" LEFT JOIN tbl_CompanyUser cu ON pp.TicketerId = cu.Id ");
            tableName.AppendLine(" LEFT JOIN tbl_CompanySupplier cs ON pp.GysId = cs.Id ");
            tableName.AppendFormat(
                " WHERE pp.CompanyId = {0}  ", companyId);
            tableName.AppendLine();
            tableName.AppendLine(" ) as a ");
            tableName.AppendLine(" where 1 = 1 ");
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.TourNo))
                {
                    tableName.AppendFormat(" and TourCode like '%{0}%' ", Utils.ToSqlLike(search.TourNo));
                    tableName.AppendLine();
                }
                if (search.StartLeaveDate.HasValue)
                {
                    tableName.AppendFormat(
                        " and datediff(dd,'{0}',LDate) >= 0 ", search.StartLeaveDate.Value.ToShortDateString());
                    tableName.AppendLine();
                }
                if (search.EndLeaveDate.HasValue)
                {
                    tableName.AppendFormat(
                        " and datediff(dd,'{0}',LDate) <= 0 ", search.EndLeaveDate.Value.ToShortDateString());
                    tableName.AppendLine();
                }
                if (search.IsCheck.HasValue)
                {
                    tableName.AppendFormat(
                        search.IsCheck.Value ? " and TicketStatus = {0} " : " and TicketStatus <> {0} ",
                        (int)TicketStatus.已出票);
                    tableName.AppendLine();
                }
                if (search.IsJieQing.HasValue)
                {
                    tableName.AppendLine(
                        search.IsJieQing.Value ? " and YingFuKuan = YiFuKuan " : " and YingFuKuan <> YiFuKuan ");
                }
                if (search.IsYueJie.HasValue)
                {
                    tableName.AppendFormat(" and IsMonth = '{0}' ", search.IsYueJie.Value ? "1" : "0");
                    tableName.AppendLine();
                }

                if (!string.IsNullOrEmpty(search.GysName))
                {
                    tableName.AppendFormat(" and GysName like '%{0}%' ", search.GysName);
                    tableName.AppendLine();
                }
                if (search.TourType.HasValue)
                {
                    tableName.AppendFormat(" and TourType = {0} ", (int)search.TourType.Value);
                    tableName.AppendLine();
                }
            }
            else
            {
                tableName.AppendLine(" and YingFuKuan <> YiFuKuan ");
            }

            #endregion

            IList<MPiaoList> list = new List<MPiaoList>();
            using (IDataReader dr = DbHelper.ExecuteReader2(_db, pageSize, pageIndex, ref recordCount, tableName.ToString()
                , fields, string.Empty, orderByStr, sumStr))
            {
                while (dr.Read())
                {
                    #region 实体赋值

                    var tmp = new MPiaoList
                        {
                            PlanId =
                                dr.IsDBNull(dr.GetOrdinal("PlanId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("PlanId")),
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            TourId =
                                dr.IsDBNull(dr.GetOrdinal("TourId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("TourId")),
                            TourCode =
                                dr.IsDBNull(dr.GetOrdinal("TourCode"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("TourCode")),
                            Adults = dr.IsDBNull(dr.GetOrdinal("Adults")) ? 0 : dr.GetInt32(dr.GetOrdinal("Adults")),
                            Childs = dr.IsDBNull(dr.GetOrdinal("Childs")) ? 0 : dr.GetInt32(dr.GetOrdinal("Childs")),
                            TrafficNumber =
                                dr.IsDBNull(dr.GetOrdinal("TrafficNumber"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("TrafficNumber")),
                            Interval =
                                dr.IsDBNull(dr.GetOrdinal("Interval"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Interval")),
                            ChuPiaoRenId =
                                dr.IsDBNull(dr.GetOrdinal("TicketerId")) ? 0 : dr.GetInt32(dr.GetOrdinal("TicketerId")),
                            ChuPiaoRenName =
                                dr.IsDBNull(dr.GetOrdinal("TicketName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("TicketName")),
                            GysId =
                                dr.IsDBNull(dr.GetOrdinal("GysId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("GysId")),
                            GysName =
                                dr.IsDBNull(dr.GetOrdinal("GysName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("GysName")),
                            YingFuKuan =
                                dr.IsDBNull(dr.GetOrdinal("YingFuKuan"))
                                    ? 0M
                                    : dr.GetDecimal(dr.GetOrdinal("YingFuKuan")),
                            YiFuKuan =
                                dr.IsDBNull(dr.GetOrdinal("YiFuKuan")) ? 0M : dr.GetDecimal(dr.GetOrdinal("YiFuKuan")),
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("LDate")))
                    {
                        tmp.LDate = dr.GetDateTime(dr.GetOrdinal("LDate"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("PayType")))
                    {
                        tmp.PayType = (ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("PayType"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("TicketMode")))
                    {
                        tmp.TicketMode = (TicketMode)dr.GetByte(dr.GetOrdinal("TicketMode"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("TicketStatus")))
                    {
                        tmp.State = (TicketStatus)dr.GetByte(dr.GetOrdinal("TicketStatus"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("TourStatus")))
                    {
                        tmp.TourState =
                            (Model.EnumType.TourStructure.TourStatus)dr.GetByte(dr.GetOrdinal("TourStatus"));
                    }

                    #endregion

                    list.Add(tmp);
                }

                dr.NextResult();
                if (dr.Read())
                {
                    if (!dr.IsDBNull(dr.GetOrdinal("SumYingFuKuan")))
                    {
                        heJi.YingFuKuan = dr.GetDecimal(dr.GetOrdinal("SumYingFuKuan"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("SumYiFuKuan")))
                    {
                        heJi.YiFuKuan = dr.GetDecimal(dr.GetOrdinal("SumYiFuKuan"));
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 获取维护团队收入的Sql
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private string GetTourShouRuSql(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return string.Empty;

            var strSql = new StringBuilder();
            strSql.AppendLine();
            strSql.AppendLine(" declare @tmpIsCheckMoney money; set @tmpIsCheckMoney = 0; ");
            strSql.AppendLine(" declare @tmpIsAllMoney money; set @tmpIsAllMoney = 0; ");
            strSql.AppendFormat(
                " select @tmpIsCheckMoney = IsNull(Sum(CollectionRefundAmount),0) from tbl_FinCope where CollectionId = {0} and CollectionItem = {1} and [Status] = {2}; ",
                tourId,
                (int)KuanXiangType.计划收款,
                (int)KuanXiangStatus.未支付);
            strSql.AppendLine();
            strSql.AppendFormat(
                " select @tmpIsAllMoney = IsNull(Sum(CollectionRefundAmount),0) from tbl_FinCope where CollectionId = {0} and CollectionItem = {1}; ",
                tourId,
                (int)KuanXiangType.计划收款);
            strSql.AppendLine();
            strSql.AppendLine(" if @tmpIsCheckMoney is not null and @tmpIsAllMoney is not null ");
            strSql.AppendLine(" begin ");
            strSql.AppendFormat(
                " update tbl_Tour set CheckMoney = @tmpIsCheckMoney,ReceivedMoney = @tmpIsAllMoney where TourId = {0}; ",
                tourId);
            strSql.AppendLine();
            strSql.AppendLine(" end ");

            return strSql.ToString();
        }

        /// <summary>
        /// 根据sqlxml获取附件信息
        /// </summary>
        /// <param name="xml">sqlxml</param>
        /// <returns></returns>
        private IList<MKuanFile> GetFileBySqlXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;

            var xRoot = XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            if (xRows == null || !xRows.Any()) return null;

            return (from c in xRows
                    where c != null
                    select
                        new MKuanFile
                            {
                                FileId = Utils.GetInt(Utils.GetXAttributeValue(c, "Id")),
                                DengJiId = Utils.GetXAttributeValue(c, "DengJiId"),
                                FileName = Utils.GetXAttributeValue(c, "FileName"),
                                FilePath = Utils.GetXAttributeValue(c, "FilePath")
                            }).ToList();
        }

        /// <summary>
        /// 根据sqlxml获取供应商联系人信息集合
        /// </summary>
        /// <param name="xml">sqlxml</param>
        /// <returns></returns>
        private IList<Model.SourceStructure.MSupplierContact> GetContactBySqlXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;

            var xRoot = XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            if (xRows == null || !xRows.Any()) return null;

            return (from c in xRows
                    where c != null
                    select
                        new Model.SourceStructure.MSupplierContact
                            {
                                ContactName = Utils.GetXAttributeValue(c, "ContactName"),
                                ContactTel = Utils.GetXAttributeValue(c, "ContactTel"),
                                QQ = Utils.GetXAttributeValue(c, "QQ")
                            }).ToList();
        }
    }
}
