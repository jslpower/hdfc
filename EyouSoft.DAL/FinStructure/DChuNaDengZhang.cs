using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Model.FinStructure;
using System.Xml.Linq;

namespace EyouSoft.DAL.FinStructure
{
    /// <summary>
    /// 出纳登账数据访问
    /// </summary>
    public class DChuNaDengZhang : DALBase, IDAL.FinStructure.IChuNaDengZhang
    {
        private readonly Database _db;

        public DChuNaDengZhang()
        {
            _db = this.SystemStore;
        }

        /// <summary>
        /// 添加出纳登账信息（只做财务管理-出纳登账-新增使用）
        /// </summary>
        /// <param name="model">出纳登账实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：添加失败；
        /// </returns>
        public int AddChuNaDengZhang(MChuNaDengZhang model)
        {
            if (model == null || model.CompanyId <= 0 || string.IsNullOrEmpty(model.TourNo) || model.OperatorId <= 0)
                return 0;

            var strSql = new StringBuilder();
            model.DengZhangId = Guid.NewGuid().ToString();
            model.IssueTime = DateTime.Now;
            strSql.Append(@" INSERT INTO [tbl_FinChuNaDengZhang] ([DengZhangId],[CompanyId],[DengZhangType],[DengJiId],[TourNo]
                ,[DengZhangDate],[Price],[BankId],[PayMode],[Reason],[OtherPrice],[IsKaiPiao],[IssueTime],[OperatorId],[IsAuto]
                ,[DengZhangPeople],[DengZhangPeopleId])  
                VALUES (@DengZhangId,@CompanyId,@DengZhangType,@DengJiId,@TourNo
                ,@DengZhangDate,@Price,@BankId,@PayMode,@Reason,@OtherPrice,@IsKaiPiao,@IssueTime,@OperatorId,@IsAuto
                ,@DengZhangPeople,@DengZhangPeopleId); ");
            if (model.File != null && model.File.Any())
            {
                foreach (var t in model.File)
                {
                    if (t == null) continue;

                    strSql.AppendFormat(
                        " INSERT INTO [tbl_FinDengZhangFilePath] ([DengZhangId],[FileName],[FilePath]) VALUES ('{0}','{1}','{2}'); ",
                        model.DengZhangId,
                        t.FileName,
                        t.FilePath);
                }
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "DengZhangId", DbType.AnsiStringFixedLength, model.DengZhangId);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "DengZhangType", DbType.Byte, (int)model.DengZhangType);
            _db.AddInParameter(dc, "DengJiId", DbType.AnsiStringFixedLength, model.DengJiId);
            _db.AddInParameter(dc, "TourNo", DbType.String, model.TourNo);
            _db.AddInParameter(dc, "DengZhangDate", DbType.DateTime, model.DengZhangDate);
            _db.AddInParameter(dc, "Price", DbType.Decimal, model.Price);
            _db.AddInParameter(dc, "BankId", DbType.AnsiStringFixedLength, model.BankId);
            _db.AddInParameter(dc, "PayMode", DbType.Byte, (int)model.PayMode);
            _db.AddInParameter(dc, "Reason", DbType.String, model.Reason);
            _db.AddInParameter(dc, "OtherPrice", DbType.Decimal, model.OtherPrice);
            _db.AddInParameter(dc, "IsKaiPiao", DbType.AnsiStringFixedLength, model.IsKaiPiao ? "1" : "0");
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, model.IssueTime);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "IsAuto", DbType.AnsiStringFixedLength, "0");
            _db.AddInParameter(dc, "DengZhangPeople", DbType.String, model.DengZhangPeople);
            _db.AddInParameter(dc, "DengZhangPeopleId", DbType.Int32, model.DengZhangPeopleId);

            return DbHelper.ExecuteSqlTrans(dc, _db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 修改出纳登账信息
        /// </summary>
        /// <param name="model">出纳登账实体</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：不是用户登记的数据，不能修改；
        /// -2：修改失败；
        /// </returns>
        public int UpdateChuNaDengZhang(MChuNaDengZhang model)
        {
            if (model == null || string.IsNullOrEmpty(model.DengZhangId) || model.CompanyId <= 0 || string.IsNullOrEmpty(model.TourNo)
                || model.OperatorId <= 0)
            {
                return 0;
            }

            var strSql = new StringBuilder();

            #region sql处理

            strSql.Append(" declare @IsAuto char(1); ");
            strSql.Append(" declare @result int; ");
            strSql.Append(" set @result = -1; ");
            strSql.Append(" select @IsAuto = IsAuto from  tbl_FinChuNaDengZhang where [DengZhangId] = @DengZhangId;  ");
            strSql.Append(" if @IsAuto is not null and @IsAuto = '0' ");
            strSql.Append(" begin ");
            strSql.Append(" UPDATE [tbl_FinChuNaDengZhang] SET [TourNo] = @TourNo,[DengZhangDate] = @DengZhangDate,[Price] = @Price,[BankId] = @BankId,[PayMode] = @PayMode,[Reason] = @Reason,[OtherPrice] = @OtherPrice,[IsKaiPiao] = @IsKaiPiao,[DengZhangType] = @DengZhangType,[DengZhangPeople] = @DengZhangPeople,[DengZhangPeopleId] = @DengZhangPeopleId WHERE [DengZhangId] = @DengZhangId; ");
            if (model.File != null && model.File.Any())
            {
                var oldId = (from t in model.File where t.FileId > 0 select t.FileId).ToList();
                if (oldId.Any())
                {
                    strSql.AppendFormat(
                        " insert into tbl_SysDeletedFileQue (FilePath) select FilePath from tbl_FinDengZhangFilePath where DengZhangId = @DengZhangId and Id not in ({0}) ; ",
                        this.GetIdsByArr(oldId.ToArray()));
                    strSql.AppendFormat(
                        " delete from tbl_FinDengZhangFilePath where DengZhangId = @DengZhangId and Id not in ({0}); ",
                        this.GetIdsByArr(oldId.ToArray()));

                    foreach (var t in model.File)
                    {
                        if (t == null) continue;

                        if (oldId.Contains(t.FileId))
                        {
                            strSql.AppendFormat(
                                " if not exists (select 1 from tbl_FinDengZhangFilePath where [DengZhangId] = @DengZhangId and Id = {0} and [FilePath] = '{1}') ",
                                t.FileId,
                                t.FilePath);
                            strSql.Append(" begin ");
                            strSql.AppendFormat(
                                " insert into tbl_SysDeletedFileQue (FilePath) select FilePath from tbl_FinDengZhangFilePath where DengZhangId = @DengZhangId and Id = {0}; ",
                                t.FileId);
                            strSql.Append(" end ");
                            strSql.AppendFormat(
                                " update tbl_FinDengZhangFilePath set [FileName] = '{0}',[FilePath] = '{1}' where [DengZhangId] = @DengZhangId and Id = {2}; ",
                                t.FileName,
                                t.FilePath,
                                t.FileId);
                        }
                        else
                        {
                            strSql.AppendFormat(
                                " INSERT INTO [tbl_FinDengZhangFilePath] ([DengZhangId],[FileName],[FilePath]) VALUES (@DengZhangId,'{0}','{1}'); ",
                                t.FileName,
                                t.FilePath);
                        }
                    }
                }
                else
                {
                    strSql.Append(" insert into tbl_SysDeletedFileQue (FilePath) select FilePath from tbl_FinDengZhangFilePath where DengZhangId = @DengZhangId; ");
                    strSql.Append(" delete from tbl_FinDengZhangFilePath where DengZhangId = @DengZhangId; ");
                    foreach (var t in model.File)
                    {
                        if (t == null) continue;

                        strSql.AppendFormat(
                            " INSERT INTO [tbl_FinDengZhangFilePath] ([DengZhangId],[FileName],[FilePath]) VALUES ('{0}','{1}','{2}'); ",
                            model.DengZhangId,
                            t.FileName,
                            t.FilePath);
                    }
                }
            }
            else
            {
                strSql.Append(" insert into tbl_SysDeletedFileQue (FilePath) select FilePath from tbl_FinDengZhangFilePath where DengZhangId = @DengZhangId; ");
                strSql.Append(" delete from tbl_FinDengZhangFilePath where DengZhangId = @DengZhangId; ");
            }
            strSql.Append(" set @result = 1; ");
            strSql.Append(" end ");
            strSql.Append(" select @result; ");

            #endregion

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "DengZhangId", DbType.AnsiStringFixedLength, model.DengZhangId);
            _db.AddInParameter(dc, "DengZhangType", DbType.Byte, (int)model.DengZhangType);
            _db.AddInParameter(dc, "TourNo", DbType.String, model.TourNo);
            _db.AddInParameter(dc, "DengZhangDate", DbType.DateTime, model.DengZhangDate);
            _db.AddInParameter(dc, "Price", DbType.Decimal, model.Price);
            _db.AddInParameter(dc, "BankId", DbType.AnsiStringFixedLength, model.BankId);
            _db.AddInParameter(dc, "PayMode", DbType.Byte, (int)model.PayMode);
            _db.AddInParameter(dc, "Reason", DbType.String, model.Reason);
            _db.AddInParameter(dc, "OtherPrice", DbType.Decimal, model.OtherPrice);
            _db.AddInParameter(dc, "IsKaiPiao", DbType.AnsiStringFixedLength, model.IsKaiPiao ? "1" : "0");
            _db.AddInParameter(dc, "DengZhangPeople", DbType.String, model.DengZhangPeople);
            _db.AddInParameter(dc, "DengZhangPeopleId", DbType.Int32, model.DengZhangPeopleId);

            object obj = DbHelper.GetSingleBySqlTrans(dc, _db);
            if (obj == null) return -2;

            return Utils.GetInt(obj.ToString());
        }

        /// <summary>
        /// 删除出纳登账信息
        /// </summary>
        /// <param name="id">出纳登账编号</param>
        /// <returns>
        /// 1：成功；
        /// 0：参数错误；
        /// -1：单条删除时，不是用户登记的数据，不能删除；
        /// -2：多条删除时，删除能删除的，不能删除的没有删除；
        /// -3：删除失败；
        /// </returns>
        public int DeleteChuNaDengZhang(params string[] id)
        {
            if (id == null || id.Length <= 0) return 0;

            var strSql = new StringBuilder();

            strSql.Append(" declare @result int; ");
            strSql.Append(" set @result = -3; ");
            if (id.Length == 1)
            {
                strSql.Append(" set @result = -1; ");
                strSql.AppendFormat(
                    " if exists (select 1 from tbl_FinChuNaDengZhang where DengZhangId = '{0}' and IsAuto = '0') ",
                    id[0]);
                strSql.Append(" begin ");
                strSql.AppendFormat(
                    " insert into tbl_SysDeletedFileQue (FilePath) select FilePath from tbl_FinDengZhangFilePath where DengZhangId = '{0}'  ; ",
                    id[0]);
                strSql.AppendFormat(" delete from tbl_FinDengZhangFilePath where DengZhangId = '{0}' ; ", id[0]);
                strSql.AppendFormat(" delete from tbl_FinChuNaDengZhang where DengZhangId = '{0}' and IsAuto = '0' ; ", id[0]);
                strSql.Append(" set @result = 1; ");
                strSql.Append(" end ");
            }
            else
            {
                strSql.Append(" set @result = 1; ");
                strSql.Append(" declare @isExists int; ");
                strSql.AppendFormat(
                    " select @isExists = count(DengZhangId) from tbl_FinChuNaDengZhang where DengZhangId in ({0}) and IsAuto = '1' ",
                    this.GetIdsByArr(id));
                strSql.Append(" if @isExists is not null and @isExists > 0 ");
                strSql.Append(" begin ");
                strSql.Append(" set @result = -2; ");
                strSql.Append(" end ");
                strSql.AppendFormat(
                    " insert into tbl_SysDeletedFileQue (FilePath) select FilePath from tbl_FinDengZhangFilePath where DengZhangId in (select DengZhangId from tbl_FinChuNaDengZhang where  DengZhangId in ({0}) and IsAuto = '0'); ",
                    this.GetIdsByArr(id));
                strSql.AppendFormat(
                    " delete from tbl_FinDengZhangFilePath where DengZhangId in (select DengZhangId from tbl_FinChuNaDengZhang where  DengZhangId in ({0}) and IsAuto = '0'); ",
                    this.GetIdsByArr(id));
                strSql.AppendFormat(
                    " delete from tbl_FinChuNaDengZhang where DengZhangId in ({0}) and IsAuto = '0' ; ",
                    this.GetIdsByArr(id));
            }
            strSql.Append(" select @result; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            object obj = DbHelper.GetSingleBySqlTrans(dc, _db);
            if (obj == null) return -3;

            return Utils.GetInt(obj.ToString(), -3);
        }

        /// <summary>
        /// 获取出纳登账信息
        /// </summary>
        /// <param name="id">出纳登账编号</param>
        /// <returns></returns>
        public MChuNaDengZhang GetChuNaDengZhang(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            MChuNaDengZhang model = null;
            var strSql = new StringBuilder();
            strSql.Append(" SELECT [DengZhangId],[CompanyId],[DengZhangType],[DengJiId],[TourNo],[DengZhangDate],[Price],[BankId],[PayMode],[Reason],[OtherPrice],[IsKaiPiao],[IssueTime],[OperatorId],[IsAuto],[DengZhangPeople],[DengZhangPeopleId] ");
            //strSql.Append(
                //" ,(select a.ContactName from tbl_CompanyUser as a where a.Id = tbl_FinChuNaDengZhang.OperatorId) as OperatorName ");
            strSql.Append(
                " ,(select BankName + '  ' + AccountName + '  ' + BankNo from tbl_CompanyAccount as ca where ca.Id = tbl_FinChuNaDengZhang.BankId) as BankName ");
            strSql.Append(" FROM [tbl_FinChuNaDengZhang] where [DengZhangId] = @DengZhangId; ");
            strSql.Append(" SELECT [Id],[DengZhangId],[FileName],[FilePath] ");
            strSql.Append(" FROM [tbl_FinDengZhangFilePath] where [DengZhangId] = @DengZhangId; ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "DengZhangId", DbType.AnsiStringFixedLength, id);

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                #region 实体赋值

                #region 出纳登账基本信息

                if (dr.Read())
                {
                    model = new MChuNaDengZhang
                        {
                            DengZhangId =
                                dr.IsDBNull(dr.GetOrdinal("DengZhangId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("DengZhangId")),
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            DengJiId =
                                dr.IsDBNull(dr.GetOrdinal("DengJiId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("DengJiId")),
                            TourNo =
                                dr.IsDBNull(dr.GetOrdinal("TourNo"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("TourNo")),
                            Price = dr.IsDBNull(dr.GetOrdinal("Price")) ? 0M : dr.GetDecimal(dr.GetOrdinal("Price")),
                            BankId =
                                dr.IsDBNull(dr.GetOrdinal("BankId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("BankId")),
                            Reason =
                                dr.IsDBNull(dr.GetOrdinal("Reason"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Reason")),
                            OtherPrice =
                                dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                    ? 0M
                                    : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                            OperatorId =
                                dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                            BankName =
                                dr.IsDBNull(dr.GetOrdinal("BankName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("BankName")),
                            DengZhangPeople =
                                dr.IsDBNull(dr.GetOrdinal("DengZhangPeople"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("DengZhangPeople")),
                            DengZhangPeopleId =
                                dr.IsDBNull(dr.GetOrdinal("DengZhangPeopleId"))
                                    ? 0
                                    : dr.GetInt32(dr.GetOrdinal("DengZhangPeopleId"))
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("DengZhangType")))
                        model.DengZhangType =
                            (Model.EnumType.FinStructure.DengJiMode)dr.GetByte(dr.GetOrdinal("DengZhangType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("DengZhangDate"))) model.DengZhangDate = dr.GetDateTime(dr.GetOrdinal("DengZhangDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PayMode")))
                        model.PayMode =
                            (Model.EnumType.FinStructure.ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("PayMode"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao"))) model.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IsAuto"))) model.IsEdit = !this.GetBoolean(dr.GetString(dr.GetOrdinal("IsAuto")));
                }

                #endregion

                #region 出纳登账附件信息

                dr.NextResult();
                if (model != null)
                {
                    model.File = new List<MChuNaDengZhangFile>();
                    while (dr.Read())
                    {
                        model.File.Add(
                            new MChuNaDengZhangFile
                                {
                                    DengZhangId = model.DengZhangId,
                                    FileId = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
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
                }

                #endregion

                #endregion
            }

            return model;
        }

        /// <summary>
        /// 获取出纳登账信息
        /// </summary>
        /// <param name="companyId"> 公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <param name="heJi">合计实体（赋值null或者新实例化一个对象）</param>
        /// <returns></returns>
        public IList<MChuNaDengZhang> GetChuNaDengZhang(int companyId, int pageSize, int pageIndex, ref int recordCount
            , MSearchChuNaDengZhang search,ref MChuNaDengZhangHeJi heJi) 
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            IList<MChuNaDengZhang> list;
            if (heJi == null) heJi = new MChuNaDengZhangHeJi();
            string tableName = "tbl_FinChuNaDengZhang";
            var fields =
                new StringBuilder(
                    " [DengZhangId],[CompanyId],[DengZhangType],[DengJiId],[TourNo],[DengZhangDate],[Price],[BankId],[PayMode],[Reason],[OtherPrice],[IsKaiPiao],[IssueTime],[OperatorId],[IsAuto],DengZhangPeople,DengZhangPeopleId ");
            fields.Append(
                " ,(select BankName + '  ' + AccountName + '  ' + BankNo from tbl_CompanyAccount as ca where ca.Id = tbl_FinChuNaDengZhang.BankId) as BankName ");
            fields.Append(
                " ,(select [Id],[FileName],[FilePath] FROM [tbl_FinDengZhangFilePath] as a where a.[DengZhangId] = tbl_FinChuNaDengZhang.DengZhangId for xml raw,root('Root')) as FileInfo ");
            string orderByStr = " DengZhangDate desc,IssueTime desc ";
            string sumStr = " Sum(Price) as SumPrice,Sum(OtherPrice) as SumOtherPrice ";
            var strWhere = new StringBuilder();
            strWhere.AppendFormat(" CompanyId = {0} ", companyId);

            #region 条件处理

            if (search != null)
            {
                if (search.DengZhangType.HasValue)
                {
                    strWhere.AppendFormat(" and DengZhangType = {0} ", (int)search.DengZhangType.Value);
                }
                if (search.StartTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',DengZhangDate) >= 0 ", search.StartTime.Value.ToShortDateString());
                }
                if (search.EndTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',DengZhangDate) <= 0 ", search.EndTime.Value.ToShortDateString());
                }
            }

            #endregion

            list = new List<MChuNaDengZhang>();
            using (IDataReader dr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fields.ToString()
                , strWhere.ToString(), orderByStr, sumStr))
            {
                #region 实体赋值

                while (dr.Read())
                {
                    var model = new MChuNaDengZhang
                        {
                            DengZhangId =
                                dr.IsDBNull(dr.GetOrdinal("DengZhangId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("DengZhangId")),
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            DengJiId =
                                dr.IsDBNull(dr.GetOrdinal("DengJiId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("DengJiId")),
                            TourNo =
                                dr.IsDBNull(dr.GetOrdinal("TourNo"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("TourNo")),
                            Price = dr.IsDBNull(dr.GetOrdinal("Price")) ? 0M : dr.GetDecimal(dr.GetOrdinal("Price")),
                            BankId =
                                dr.IsDBNull(dr.GetOrdinal("BankId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("BankId")),
                            Reason =
                                dr.IsDBNull(dr.GetOrdinal("Reason"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("Reason")),
                            OtherPrice =
                                dr.IsDBNull(dr.GetOrdinal("OtherPrice"))
                                    ? 0M
                                    : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                            OperatorId =
                                dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                            BankName =
                                dr.IsDBNull(dr.GetOrdinal("BankName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("BankName")),
                            DengZhangPeople =
                                dr.IsDBNull(dr.GetOrdinal("DengZhangPeople"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("DengZhangPeople")),
                            DengZhangPeopleId =
                                dr.IsDBNull(dr.GetOrdinal("DengZhangPeopleId"))
                                    ? 0
                                    : dr.GetInt32(dr.GetOrdinal("DengZhangPeopleId"))
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("DengZhangType")))
                        model.DengZhangType =
                            (Model.EnumType.FinStructure.DengJiMode)dr.GetByte(dr.GetOrdinal("DengZhangType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("DengZhangDate"))) model.DengZhangDate = dr.GetDateTime(dr.GetOrdinal("DengZhangDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PayMode")))
                        model.PayMode =
                            (Model.EnumType.FinStructure.ShouFuKuanFangShi)dr.GetByte(dr.GetOrdinal("PayMode"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IsKaiPiao"))) model.IsKaiPiao = this.GetBoolean(dr.GetString(dr.GetOrdinal("IsKaiPiao")));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime"))) model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IsAuto"))) model.IsEdit = !this.GetBoolean(dr.GetString(dr.GetOrdinal("IsAuto")));
                    if (!dr.IsDBNull(dr.GetOrdinal("FileInfo"))) model.File = this.GetFileByXml(dr.GetString(dr.GetOrdinal("FileInfo")));

                    list.Add(model);
                }

                #endregion

                #region 合计处理

                dr.NextResult();
                if (dr.Read())
                {
                    if (!dr.IsDBNull(dr.GetOrdinal("SumPrice"))) heJi.SumPrice = dr.GetDecimal(dr.GetOrdinal("SumPrice"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SumOtherPrice"))) heJi.SumOtherPrice = dr.GetDecimal(dr.GetOrdinal("SumOtherPrice"));
                }

                #endregion
            }

            return list;
        }

        /// <summary>
        /// 根据sqlxml生成出纳登账附件信息集合
        /// </summary>
        /// <param name="xml">sqlxml</param>
        /// <returns></returns>
        private IList<MChuNaDengZhangFile> GetFileByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;

            var xRoot = XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            if (xRows == null || !xRows.Any()) return null;

            return (from t in xRows
                    where t != null
                    select new MChuNaDengZhangFile
                        {
                            FileId = Utils.GetInt(Utils.GetXAttributeValue(t, "Id")),
                            FileName = Utils.GetXAttributeValue(t, "FileName"),
                            FilePath = Utils.GetXAttributeValue(t, "FilePath")
                        }).ToList();
        }
    }
}
