using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit;
using System.Data.Common;
using System.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.TourStructure
{

    #region 确认件登记短信提醒组团社数据访问

    /// <summary>
    /// 确认件登记短信提醒组团社数据访问
    /// </summary>
    public class DSMSTourCustomer : DALBase, IDAL.TourStructure.ISMSTourCustomer
    {
        #region 初始化db

        private Database _db = null;

        /// <summary>
        /// 初始化_db
        /// </summary>
        public DSMSTourCustomer()
        {
            _db = base.SystemStore;
        }

        #endregion

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">确认件登记短信提醒组团社实体</param>
        /// <returns>1成功；其他失败</returns>
        public int Add(Model.TourStructure.MSMSTourCustomer model)
        {
            if (model == null || string.IsNullOrEmpty(model.TourId)) return 0;

            var sql = new StringBuilder("INSERT INTO [SMS_CustomerCarefor]([CompanyId],[MobileCode],[IsMatchCustomerInfo],[IsMatchSupplierInfo],[IsMatchDepartmentInfo],[Content],[Time],[FixType],[ChannelId],[IsEnabled],[OperatorId],[IssueTime])");
            sql.Append("values(@CompanyId,@MobileCode,@IsMatchCustomerInfo,@IsMatchSupplierInfo,@IsMatchDepartmentInfo,@Content,@Time,@FixType,@ChannelId,@IsEnabled,@OperatorId,@IssueTime);");
            sql.Append(" select @SmsId = @@identity; ");
            sql.Append(" INSERT INTO [SMS_CustomerCareforTour] ([TourId],[SmsId]) VALUES (@TourId,isnull(@SmsId,0)) ");
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(cmd, "ChannelId", DbType.Int32, model.ChannelId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "Content", DbType.String, model.Content);
            this._db.AddInParameter(cmd, "FixType", DbType.Int32, (int)model.FixType);
            this._db.AddInParameter(cmd, "IsEnabled", DbType.StringFixedLength, model.IsEnabled ? '1' : '0');
            this._db.AddInParameter(cmd, "IsMatchCustomerInfo", DbType.StringFixedLength, model.IsMatchCustomerInfo ? '1' : '0');
            this._db.AddInParameter(cmd, "IsMatchDepartmentInfo", DbType.StringFixedLength, model.IsMatchDepartmentInfo ? '1' : '0');
            this._db.AddInParameter(cmd, "IsMatchSupplierInfo", DbType.StringFixedLength, model.IsMatchSupplierInfo ? '1' : '0');
            this._db.AddInParameter(cmd, "IsSeded", DbType.StringFixedLength, model.IsSeded ? '1' : '0');
            this._db.AddInParameter(cmd, "IssueTime", DbType.DateTime, model.IssueTime);
            this._db.AddInParameter(cmd, "MobileCode", DbType.String, model.MobileCode);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "Time", DbType.DateTime, model.Time);

            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(cmd, "SmsId", DbType.Int32, 0);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">确认件登记短信提醒组团社实体</param>
        /// <returns>1成功；其他失败</returns>
        public int Update(Model.TourStructure.MSMSTourCustomer model)
        {
            if (model == null || model.Id <= 0) return 0;

            var sql = new StringBuilder("update [SMS_CustomerCarefor] set [MobileCode]=@MobileCode,[IsMatchCustomerInfo]=@IsMatchCustomerInfo,");
            sql.Append(" [IsMatchSupplierInfo]=@IsMatchSupplierInfo,[IsMatchDepartmentInfo]=@IsMatchDepartmentInfo,[Content]=@Content,[Time]=@Time,[FixType]=@FixType,[ChannelId]=@ChannelId ");
            sql.AppendFormat(" where id={0}", model.Id);
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(cmd, "MobileCode", DbType.String, model.MobileCode);
            this._db.AddInParameter(cmd, "IsMatchCustomerInfo", DbType.StringFixedLength, model.IsMatchCustomerInfo ? '1' : '0');
            this._db.AddInParameter(cmd, "IsMatchDepartmentInfo", DbType.StringFixedLength, model.IsMatchDepartmentInfo ? '1' : '0');
            this._db.AddInParameter(cmd, "IsMatchSupplierInfo", DbType.StringFixedLength, model.IsMatchSupplierInfo ? '1' : '0');
            this._db.AddInParameter(cmd, "Content", DbType.String, model.Content);
            this._db.AddInParameter(cmd, "Time", DbType.DateTime, model.Time);
            this._db.AddInParameter(cmd, "FixType", DbType.Int32, (int)model.FixType);
            this._db.AddInParameter(cmd, "ChannelId", DbType.Int32, model.ChannelId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? 1 : -1;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="smsId">短信提醒编号</param>
        /// <returns>1成功；其他失败</returns>
        public int Delete(string tourId, params int[] smsId)
        {
            if (string.IsNullOrEmpty(tourId) || smsId == null || smsId.Length < 1) return 0;

            var sql = new StringBuilder(" DELETE FROM SMS_CustomerCarefor where id in (@CareForId); ");
            sql.Append(" delete from SMS_CustomerCareforTour where TourId = @TourId and SmsId in(@CareForId) ; ");
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());

            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);
            this._db.AddInParameter(cmd, "CareForId", DbType.Int32, GetIdsByArr(smsId));

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? 1 : -1;
        }

        /// <summary>
        /// 获取确认件登记短信提醒组团社实体
        /// </summary>
        /// <param name="smsId">短信提醒编号</param>
        /// <returns></returns>
        public Model.TourStructure.MSMSTourCustomer GetModel(int smsId)
        {
            if (smsId <= 0) return null;

            var strSql = new StringBuilder();
            strSql.Append(
                " SELECT [Id],[CompanyId],[MobileCode],[IsMatchCustomerInfo],[IsMatchSupplierInfo],[IsMatchDepartmentInfo],[Content],[Time],[FixType],[ChannelId],[IsEnabled],[OperatorId],[IssueTime],[IsSeded],b.TourId,(SELECT TourCode FROM tbl_Tour AS c WHERE c.TourId = b.TourId) AS  TourCode FROM [SMS_CustomerCarefor] AS a INNER JOIN SMS_CustomerCareforTour AS b ON a.Id = b.SmsId ");
            strSql.Append(" where Id = @Id ");

            DbCommand cmd = this._db.GetSqlStringCommand(strSql.ToString());

            this._db.AddInParameter(cmd, "Id", DbType.Int32, smsId);

            Model.TourStructure.MSMSTourCustomer model = null;
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (dr.Read())
                {
                    model = new Model.TourStructure.MSMSTourCustomer
                        {
                            ChannelId =
                                dr.IsDBNull(dr.GetOrdinal("ChannelId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ChannelId")),
                            CompanyId =
                                dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            Content =
                                dr.IsDBNull(dr.GetOrdinal("Content")) ? "" : dr.GetString(dr.GetOrdinal("Content")),
                            FixType =
                                dr.IsDBNull(dr.GetOrdinal("FixType"))
                                    ? EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.无
                                    : (EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime)
                                      Enum.Parse(
                                          typeof(EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime
                                          ),
                                          dr.GetInt32(dr.GetOrdinal("FixType")).ToString()),
                            Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                            IsEnabled =
                                dr.IsDBNull(dr.GetOrdinal("IsEnabled"))
                                    ? false
                                    : dr.GetString(dr.GetOrdinal("IsEnabled")) == "1" ? true : false,
                            IsMatchCustomerInfo =
                                dr.IsDBNull(dr.GetOrdinal("IsMatchCustomerInfo"))
                                    ? false
                                    : dr.GetString(dr.GetOrdinal("IsMatchCustomerInfo")) == "1" ? true : false,
                            IsMatchDepartmentInfo =
                                dr.IsDBNull(dr.GetOrdinal("IsMatchDepartmentInfo"))
                                    ? false
                                    : dr.GetString(dr.GetOrdinal("IsMatchDepartmentInfo")) == "1" ? true : false,
                            IsMatchSupplierInfo =
                                dr.IsDBNull(dr.GetOrdinal("IsMatchSupplierInfo"))
                                    ? false
                                    : dr.GetString(dr.GetOrdinal("IsMatchSupplierInfo")) == "1" ? true : false,
                            IsSeded =
                                dr.IsDBNull(dr.GetOrdinal("IsSeded"))
                                    ? false
                                    : dr.GetString(dr.GetOrdinal("IsSeded")) == "1" ? true : false,
                            IssueTime =
                                dr.IsDBNull(dr.GetOrdinal("IssueTime"))
                                    ? DateTime.Parse("2011-01-28")
                                    : dr.GetDateTime(dr.GetOrdinal("IssueTime")),
                            MobileCode =
                                dr.IsDBNull(dr.GetOrdinal("MobileCode"))
                                    ? ""
                                    : dr.GetString(dr.GetOrdinal("MobileCode")),
                            OperatorId =
                                dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                            Time =
                                dr.IsDBNull(dr.GetOrdinal("Time"))
                                    ? DateTime.Parse("2011-01-28")
                                    : dr.GetDateTime(dr.GetOrdinal("Time")),
                            TourId = dr.IsDBNull(dr.GetOrdinal("TourId")) ? "" : dr.GetString(dr.GetOrdinal("TourId")),
                            TourNo =
                                dr.IsDBNull(dr.GetOrdinal("TourCode")) ? "" : dr.GetString(dr.GetOrdinal("TourCode"))
                        };
                }
            }

            return model;
        }

        /// <summary>
        /// 获取确认件登记短信提醒组团社列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">查询实体</param>
        /// <returns></returns>
        public IList<Model.TourStructure.MSMSTourCustomer> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount, Model.TourStructure.MSearchSMSTourCustomer search)
        {
            if (companyId <= 0 || pageIndex <= 0 || pageSize <= 1) return null;

            string fields = " [Id],[CompanyId],[MobileCode],[IsMatchCustomerInfo],[IsMatchSupplierInfo],[IsMatchDepartmentInfo],[Content],[Time],[FixType],[ChannelId],[IsEnabled],[OperatorId],[IssueTime],[IsSeded],TourId,TourCode ";
            string tableName = " SELECT [Id],[CompanyId],[MobileCode],[IsMatchCustomerInfo],[IsMatchSupplierInfo],[IsMatchDepartmentInfo],[Content],[Time],[FixType],[ChannelId],[IsEnabled],[OperatorId],[IssueTime],[IsSeded],b.TourId,(SELECT TourCode FROM tbl_Tour AS c WHERE c.TourId = b.TourId) AS  TourCode FROM [SMS_CustomerCarefor] AS a INNER JOIN SMS_CustomerCareforTour AS b ON a.Id = b.SmsId ";
            string orderBy = " IssueTime desc ";
            var strWhere = new StringBuilder();
            strWhere.AppendFormat(" CompanyId = {0} ", companyId);

            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.TourId))
                {
                    strWhere.AppendFormat(" and TourId = '{0}' ", Toolkit.Utils.ToSqlLike(search.TourId));
                }
            }

            var list = new List<Model.TourStructure.MSMSTourCustomer>();
            using (IDataReader dr = DbHelper.ExecuteReader2(_db, pageSize, pageIndex, ref recordCount, tableName, fields, strWhere.ToString(), orderBy, string.Empty))
            {
                while (dr.Read())
                {
                    list.Add(
                        new Model.TourStructure.MSMSTourCustomer
                            {
                                ChannelId =
                                    dr.IsDBNull(dr.GetOrdinal("ChannelId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("ChannelId")),
                                CompanyId =
                                    dr.IsDBNull(dr.GetOrdinal("CompanyId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                                Content =
                                    dr.IsDBNull(dr.GetOrdinal("Content")) ? "" : dr.GetString(dr.GetOrdinal("Content")),
                                FixType =
                                    dr.IsDBNull(dr.GetOrdinal("FixType"))
                                        ? EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime.无
                                        : (EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime)
                                          Enum.Parse(
                                              typeof(
                                              EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime),
                                              dr.GetInt32(dr.GetOrdinal("FixType")).ToString()),
                                Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                                IsEnabled =
                                    dr.IsDBNull(dr.GetOrdinal("IsEnabled"))
                                        ? false
                                        : dr.GetString(dr.GetOrdinal("IsEnabled")) == "1" ? true : false,
                                IsMatchCustomerInfo =
                                    dr.IsDBNull(dr.GetOrdinal("IsMatchCustomerInfo"))
                                        ? false
                                        : dr.GetString(dr.GetOrdinal("IsMatchCustomerInfo")) == "1" ? true : false,
                                IsMatchDepartmentInfo =
                                    dr.IsDBNull(dr.GetOrdinal("IsMatchDepartmentInfo"))
                                        ? false
                                        : dr.GetString(dr.GetOrdinal("IsMatchDepartmentInfo")) == "1" ? true : false,
                                IsMatchSupplierInfo =
                                    dr.IsDBNull(dr.GetOrdinal("IsMatchSupplierInfo"))
                                        ? false
                                        : dr.GetString(dr.GetOrdinal("IsMatchSupplierInfo")) == "1" ? true : false,
                                IsSeded =
                                    dr.IsDBNull(dr.GetOrdinal("IsSeded"))
                                        ? false
                                        : dr.GetString(dr.GetOrdinal("IsSeded")) == "1" ? true : false,
                                IssueTime =
                                    dr.IsDBNull(dr.GetOrdinal("IssueTime"))
                                        ? DateTime.Parse("2011-01-28")
                                        : dr.GetDateTime(dr.GetOrdinal("IssueTime")),
                                MobileCode =
                                    dr.IsDBNull(dr.GetOrdinal("MobileCode"))
                                        ? ""
                                        : dr.GetString(dr.GetOrdinal("MobileCode")),
                                OperatorId =
                                    dr.IsDBNull(dr.GetOrdinal("OperatorId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                                Time =
                                    dr.IsDBNull(dr.GetOrdinal("Time"))
                                        ? DateTime.Parse("2011-01-28")
                                        : dr.GetDateTime(dr.GetOrdinal("Time")),
                                TourId =
                                    dr.IsDBNull(dr.GetOrdinal("TourId")) ? "" : dr.GetString(dr.GetOrdinal("TourId")),
                                TourNo =
                                    dr.IsDBNull(dr.GetOrdinal("TourCode"))
                                        ? ""
                                        : dr.GetString(dr.GetOrdinal("TourCode"))
                            });
                }
            }

            return list;
        }
    }

    #endregion

    public class DTour : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.TourStructure.ITour
    {
        #region 初始化db
        private Database _db = null;

        /// <summary>
        /// 初始化_db
        /// </summary>
        public DTour()
        {
            _db = base.SystemStore;
        }
        #endregion


        #region ITour 成员

        /// <summary>
        /// 确认件登记
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// 0:添加失败 1:添加成功
        /// </returns>
        public int Add(EyouSoft.Model.TourStructure.MTourInfo model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_Add");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "TourType", DbType.Byte, (int)model.TourType);
            this._db.AddInParameter(cmd, "RouteId", DbType.AnsiStringFixedLength, model.RouteId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, model.RouteName);
            this._db.AddInParameter(cmd, "IsRouteHuiTian", DbType.AnsiStringFixedLength, model.IsRouteHuiTian == true ? 1 : 0);
            this._db.AddInParameter(cmd, "LDate", DbType.DateTime, model.LDate.Value);
            this._db.AddInParameter(cmd, "RDate", DbType.DateTime, model.RDate.Value);
            this._db.AddInParameter(cmd, "SaleId", DbType.Int32, model.SaleId);
            this._db.AddInParameter(cmd, "IsMonth", DbType.AnsiStringFixedLength, model.IsMonth == true ? 1 : 0);
            this._db.AddInParameter(cmd, "MonthTime", DbType.DateTime, model.MonthTime);
            this._db.AddInParameter(cmd, "Adults", DbType.Int32, model.Adults);
            this._db.AddInParameter(cmd, "Childs", DbType.Int32, model.Childs);
            this._db.AddInParameter(cmd, "Accompanys", DbType.Int32, model.Accompanys);
            this._db.AddInParameter(cmd, "BuyCompanyId", DbType.AnsiStringFixedLength, model.BuyCompanyId);
            this._db.AddInParameter(cmd, "SumPrice", DbType.Currency, model.SumPrice);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "TourStatus", DbType.Byte, (int)model.TourStatus);
            this._db.AddInParameter(cmd, "IsChuPiao", DbType.AnsiStringFixedLength, model.IsChuPiao == true ? 1 : 0);

            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);

            this._db.AddInParameter(cmd, "TourDiJie", DbType.Xml, CreateTourDiJieXml(model.DiJieList));
            this._db.AddInParameter(cmd, "TourGuide", DbType.Xml, CreateTourGuideXml(model.GuideList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));


            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 确认件修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// -1:确认登记件财务已操作结束 无法修改
        ///	 1:成功 0：失败
        /// </returns>
        public int Update(EyouSoft.Model.TourStructure.MTourInfo model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_Update");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "TourType", DbType.Byte, (int)model.TourType);
            this._db.AddInParameter(cmd, "RouteId", DbType.AnsiStringFixedLength, model.RouteId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, model.RouteName);
            this._db.AddInParameter(cmd, "IsRouteHuiTian", DbType.AnsiStringFixedLength, model.IsRouteHuiTian == true ? 1 : 0);
            this._db.AddInParameter(cmd, "LDate", DbType.DateTime, model.LDate.Value);
            this._db.AddInParameter(cmd, "RDate", DbType.DateTime, model.RDate.Value);
            this._db.AddInParameter(cmd, "SaleId", DbType.Int32, model.SaleId);
            this._db.AddInParameter(cmd, "IsMonth", DbType.AnsiStringFixedLength, model.IsMonth == true ? 1 : 0);
            this._db.AddInParameter(cmd, "MonthTime", DbType.DateTime, model.MonthTime);
            this._db.AddInParameter(cmd, "Adults", DbType.Int32, model.Adults);
            this._db.AddInParameter(cmd, "Childs", DbType.Int32, model.Childs);
            this._db.AddInParameter(cmd, "Accompanys", DbType.Int32, model.Accompanys);
            this._db.AddInParameter(cmd, "BuyCompanyId", DbType.AnsiStringFixedLength, model.BuyCompanyId);
            this._db.AddInParameter(cmd, "SumPrice", DbType.Currency, model.SumPrice);
            this._db.AddInParameter(cmd, "IsChuPiao", DbType.AnsiStringFixedLength, model.IsChuPiao == true ? 1 : 0);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);

            this._db.AddInParameter(cmd, "TourDiJie", DbType.Xml, CreateTourDiJieXml(model.DiJieList));
            this._db.AddInParameter(cmd, "TourGuide", DbType.Xml, CreateTourGuideXml(model.GuideList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 确认件 财务操作结束  退回计调操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0</returns>
        public int Update(string TourId, bool IsEnd)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_End");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, TourId);
            this._db.AddInParameter(cmd, "IsEnd", DbType.AnsiStringFixedLength, IsEnd ? 1 : 0);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 财务保存操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// -1:只有未处理的订单才能审核
        /// 1:成功 0：失败
        /// </returns>
        public int Update_Fin(EyouSoft.Model.TourStructure.MTourInfo model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_Fin_Update");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "RouteId", DbType.AnsiStringFixedLength, model.RouteId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, model.RouteName);
            this._db.AddInParameter(cmd, "IsRouteHuiTian", DbType.AnsiStringFixedLength, model.IsRouteHuiTian == true ? 1 : 0);
            this._db.AddInParameter(cmd, "LDate", DbType.DateTime, model.LDate.Value);
            this._db.AddInParameter(cmd, "RDate", DbType.DateTime, model.RDate.Value);
            this._db.AddInParameter(cmd, "SaleId", DbType.Int32, model.SaleId);
            this._db.AddInParameter(cmd, "IsMonth", DbType.AnsiStringFixedLength, model.IsMonth == true ? 1 : 0);
            this._db.AddInParameter(cmd, "MonthTime", DbType.DateTime, model.MonthTime);
            this._db.AddInParameter(cmd, "Adults", DbType.Int32, model.Adults);
            this._db.AddInParameter(cmd, "Childs", DbType.Int32, model.Childs);
            this._db.AddInParameter(cmd, "Accompanys", DbType.Int32, model.Accompanys);
            this._db.AddInParameter(cmd, "BuyCompanyId", DbType.AnsiStringFixedLength, model.BuyCompanyId);

            this._db.AddInParameter(cmd, "SumPrice", DbType.Currency, model.SumPrice);
            this._db.AddInParameter(cmd, "IsChuPiao", DbType.AnsiStringFixedLength, model.IsChuPiao == true ? 1 : 0);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);

            this._db.AddInParameter(cmd, "RebatePeople", DbType.Int32, model.RebatePeople);
            this._db.AddInParameter(cmd, "RebatePrice", DbType.Currency, model.RebatePrice);
            this._db.AddInParameter(cmd, "Profit", DbType.Currency, model.Profit);

            this._db.AddInParameter(cmd, "ConfirmOperatorId", DbType.Int32, model.ConfirmOperatorId);
            this._db.AddInParameter(cmd, "ConfirmTime", DbType.DateTime, model.ConfirmTime);

            this._db.AddInParameter(cmd, "TourDiJie", DbType.Xml, CreateTourDiJieXml(model.DiJieList));
            this._db.AddInParameter(cmd, "TourGuide", DbType.Xml, CreateTourGuideXml(model.GuideList));
            this._db.AddInParameter(cmd, "File", DbType.Xml, CreateFileXml(model.FileList));

            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }


        /// <summary>
        /// 修改团队状态
        /// </summary>
        /// <param name="TourId"></param>
        /// <param name="TourStatus"></param>
        /// <returns>
        /// -1:团队操作已结束 1:成功 0：失败
        /// </returns>
        public int Update(string TourId, EyouSoft.Model.EnumType.TourStructure.TourStatus TourStatus)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_Update_Status");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, TourId);
            this._db.AddInParameter(cmd, "TourStatus", DbType.Byte, (int)TourStatus);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        #endregion

        /// <summary>
        /// 删除确认件
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns>
        /// -1:做过地接安排不允许删除
        ///-2:做过出票或退票安排不允许删除
        ///-3:财务审核后不能删除
        ///-4:操作结束不能删除
        ///1:成功 0：失败		
        /// </returns>
        public int Delete(string TourId)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_Delete");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, TourId);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }





        /// <summary>
        /// 强制删除确认件
        /// </summary>
        /// <param name="TourId">团号</param>
        /// <returns></returns>
        public int Delete_(string TourId)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_Delete_Power");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, TourId);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, _db);
            return Convert.ToInt32(_db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.MTourInfo GetModel(string TourId)
        {
            EyouSoft.Model.TourStructure.MTourInfo model = null;
            StringBuilder query = new StringBuilder();
            query.Append(" select * from view_Tour ");
            query.AppendFormat(" where TourId='{0}'", TourId);

            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.MTourInfo();
                    model.TourId = dr.GetString(dr.GetOrdinal("TourId"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.TourCode = !dr.IsDBNull(dr.GetOrdinal("TourCode")) ? dr.GetString(dr.GetOrdinal("TourCode")) : null;
                    model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)dr.GetByte(dr.GetOrdinal("TourType"));
                    model.RouteId = !dr.IsDBNull(dr.GetOrdinal("RouteId")) ? dr.GetString(dr.GetOrdinal("RouteId")) : null;
                    model.RouteName = !dr.IsDBNull(dr.GetOrdinal("RouteName")) ? dr.GetString(dr.GetOrdinal("RouteName")) : null;
                    model.LDate = (DateTime?)dr.GetDateTime(dr.GetOrdinal("LDate"));
                    model.RDate = (DateTime?)dr.GetDateTime(dr.GetOrdinal("RDate"));
                    model.SaleId = dr.GetInt32(dr.GetOrdinal("SaleId"));
                    model.SaleName = !dr.IsDBNull(dr.GetOrdinal("SaleName")) ? dr.GetString(dr.GetOrdinal("SaleName")) : null;
                    model.IsMonth = dr.GetString(dr.GetOrdinal("IsMonth")) == "1";
                    model.MonthTime = !dr.IsDBNull(dr.GetOrdinal("MonthTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("MonthTime")) : null;
                    model.Adults = dr.GetInt32(dr.GetOrdinal("Adults"));
                    model.Childs = dr.GetInt32(dr.GetOrdinal("Childs")); ;
                    model.Accompanys = dr.GetInt32(dr.GetOrdinal("Accompanys"));
                    model.BuyCompanyId = dr.GetString(dr.GetOrdinal("BuyCompanyId"));
                    model.BuyCompnayName = dr.GetString(dr.GetOrdinal("BuyCompanyName"));
                    model.SumPrice = dr.GetDecimal(dr.GetOrdinal("SumPrice"));
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    model.TourStatus = (EyouSoft.Model.EnumType.TourStructure.TourStatus)dr.GetByte(dr.GetOrdinal("TourStatus"));
                    model.IsChuPiao = dr.GetString(dr.GetOrdinal("IsChuPiao")) == "1";
                    model.IssueTime = (DateTime?)dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.CheckMoney = dr.GetDecimal(dr.GetOrdinal("CheckMoney"));
                    model.ReturnMoney = dr.GetDecimal(dr.GetOrdinal("ReturnMoney"));
                    model.ReceivedMoney = dr.GetDecimal(dr.GetOrdinal("ReceivedMoney"));
                    model.RefundMoney = dr.GetDecimal(dr.GetOrdinal("RefundMoney"));

                    model.RebatePeople = dr.GetInt32(dr.GetOrdinal("RebatePeople"));
                    model.RebatePrice = dr.GetDecimal(dr.GetOrdinal("RebatePrice"));
                    model.Profit = dr.GetDecimal(dr.GetOrdinal("Profit"));

                    model.ConfirmOperatorId = dr.GetInt32(dr.GetOrdinal("ConfirmOperatorId"));
                    model.ConfirmTime = !dr.IsDBNull(dr.GetOrdinal("ConfirmTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("ConfirmTime")) : null;
                    model.IsEnd = dr.GetString(dr.GetOrdinal("IsEnd")) == "1";


                    model.DiJieList = new List<EyouSoft.Model.TourStructure.MTourDiJie>();
                    model.DiJieList = !dr.IsDBNull(dr.GetOrdinal("TourDiJie")) ? GetTourDiJieByXml(dr.GetString(dr.GetOrdinal("TourDiJie"))) : null;

                    model.GuideList = new List<EyouSoft.Model.TourStructure.MTourGuide>();
                    model.GuideList = !dr.IsDBNull(dr.GetOrdinal("TourGuide")) ? GetTourGuideByXml(dr.GetString(dr.GetOrdinal("TourGuide"))) : null;

                    model.FileList = new List<EyouSoft.Model.TourStructure.MFile>();
                    model.FileList = !dr.IsDBNull(dr.GetOrdinal("TourFile")) ? GetFileByXml(dr.GetString(dr.GetOrdinal("TourFile"))) : null;
                    model.Remark = !dr.IsDBNull(dr.GetOrdinal("Remark")) ? dr.GetString(dr.GetOrdinal("Remark")) : null;
                }
            }
            return model;

        }

        /// <summary>
        /// 获取当日登记团，散个数
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="SanTour"></param>
        /// <param name="Tour"></param>
        public void GetTodayTour(int companyId, ref int SanTour, ref int Tour)
        {
            StringBuilder query = new StringBuilder();
            query.AppendFormat("select count(1) as SanTour  from tbl_Tour where CompanyId={0} and TourType={1} and datediff(day,getdate(),IssueTime)=0 and IsDelete='0' ", companyId, (int)EyouSoft.Model.EnumType.TourStructure.TourType.散);

            query.AppendFormat("select count(1) as Tour from tbl_Tour where CompanyId={0} and TourType={1} and datediff(day,getdate(),IssueTime)=0 and IsDelete='0' ", companyId, (int)EyouSoft.Model.EnumType.TourStructure.TourType.团);

            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (dr.Read())
                {
                    SanTour = dr.GetInt32(dr.GetOrdinal("SanTour"));
                }

                dr.NextResult();

                if (dr.Read())
                {
                    Tour = dr.GetInt32(dr.GetOrdinal("Tour"));
                }
            }

        }

        /// <summary>
        /// 根据公司编号，团号获取团队列表(只取团队编号，和团队号) 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="TourCode"></param>
        /// <param name="TourStatus">团队状态的数组</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MTour> GetList(int companyId, string TourCode, params int[] TourStatus)
        {
            IList<EyouSoft.Model.TourStructure.MTour> list = new List<EyouSoft.Model.TourStructure.MTour>();

            StringBuilder query = new StringBuilder();
            query.Append(" select TourId,TourCode from tbl_Tour ");
            query.AppendFormat(" where CompanyId='{0}' and TourCode like '%{1}%' and  IsDelete='0' ", companyId, TourCode);

            if (TourStatus != null && TourStatus.Length != 0)
            {
                query.AppendFormat(" and TourStatus in ({0})", GetIdsByArr(TourStatus));
            }

            DbCommand cmd = this._db.GetSqlStringCommand(query.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.TourStructure.MTour model = new EyouSoft.Model.TourStructure.MTour();
                    model.TourId = dr.GetString(dr.GetOrdinal("TourId"));
                    model.TourCode = dr.GetString(dr.GetOrdinal("TourCode"));
                    list.Add(model);
                }
            }

            return list;

        }

        /// <summary>
        /// 分页获取确认件登记的列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MPageTour> GetList(
         int companyId,
         int pageSize,
         int pageIndex,
         ref int recordCount,
         EyouSoft.Model.TourStructure.MSearchTour search)
        {
            IList<EyouSoft.Model.TourStructure.MPageTour> list = new List<EyouSoft.Model.TourStructure.MPageTour>();

            StringBuilder fileds = new StringBuilder();
            fileds.Append("TourId,TourType,TourCode,LDate,RDate,IssueTime,MonthTime,SumPrice,ReturnMoney,CheckMoney,TourStatus,IsMonth,BuyCompanyId,");
            fileds.Append("(select ContactName from tbl_CompanyUser where Id=tbl_Tour.SaleId) as SaleName,");//销售员
            fileds.Append("(select ContactName from tbl_CompanyUser where Id=tbl_Tour.OperatorId )as Planer,");//计调员
            fileds.Append("RouteName,Adults,Childs,Accompanys,");
            fileds.Append("RebatePeople,RebatePrice,Profit,");
            fileds.Append("(select CustomerName from tbl_Customer where Id=tbl_Tour.BuyCompanyId)as BuyCompanyName,");//组团社名称
            fileds.Append("(select count(1) as num from tbl_Tour as a where a.BuyCompanyId=tbl_Tour.BuyCompanyId and datediff(year,getdate(),a.IssueTime)=0 and a.IsDelete='0'  )as BuyCompanyCount,");//组团社今年交易的次数
            //fileds.Append("(select [Name],qq,Tel,Mobile,BirthDay,Remark from tbl_CustomerContactInfo where CustomerId=tbl_Tour.BuyCompanyId for xml raw,root('Root')) as ContactInfo,");//组团社浮动的paopao组团社联系人
            fileds.Append("(select * from  ");
            fileds.Append("((select ContactName as [Name],null as qq,Phone as Tel,Mobile,null as BirthDay from tbl_Customer where Id=tbl_Tour.BuyCompanyId) ");
            fileds.Append(" union ");
            fileds.Append("(select [Name],qq,Tel,Mobile,BirthDay from tbl_CustomerContactInfo where CustomerId=tbl_Tour.BuyCompanyId) ) as t ");
            fileds.Append("for xml raw,root('Root')) as ContactInfo,");


            fileds.Append("(select (select ContactName from tbl_CompanyUser where Id=tbl_PlanPiao.TicketerId) as Ticketer,Interval,TrafficTime,TrafficNumber,(select cs.UnitName from tbl_CompanySupplier as cs where cs.Id = tbl_PlanPiao.GysId) as GysName,GysId from tbl_PlanPiao where TourId=tbl_Tour.TourId order by IssueTime asc for xml raw,root('Root')) as TicketerInfo,");//出票人信息
            //地接社信息
            fileds.Append("(select (select UnitName from tbl_CompanySupplier where Id=tbl_PlanDiJie.GysId ) as DiJieName,GysId,");//地接社名称
            fileds.Append("(select top 1 ContactName,ContactTel,QQ  from tbl_SupplierContact where SupplierId=tbl_PlanDiJie.GysId for xml path,elements) as DiJieContactInfo");//地接社联系人信息
            fileds.Append(" from tbl_PlanDiJie  where TourId =tbl_Tour.TourId for xml path,elements,root('Root')) as DiJieInfo,");//地接社名称

            fileds.Append("(select (select GuideName from tbl_SupplierGuide where Id=tbl_TourGuide.GuideId) as GuideName,Phone from tbl_TourGuide where TourId=tbl_Tour.TourId for xml raw,root('Root')) as GuideInfo, ");//导游信息

            fileds.Append("(select CollectionRefundAmount,CollectionRefundDate,(select BankNo from tbl_CompanyAccount where Id=tbl_FinCope.BankId) as BankNo,");//银行卡号
            fileds.Append("CollectionRefundOperator,IsKaiPiao from tbl_FinCope where CollectionItem=0 and CollectionId=tbl_Tour.TourId for xml raw,root('Root')) as CollectionInfo");//收款人

            string tableName = "tbl_Tour";

            StringBuilder query = new StringBuilder();
            query.AppendFormat(" CompanyId={0} and IsDelete='0' ", companyId);


            if (search != null)
            {
                bool tmp = search.IssueBeginDate.HasValue || search.IssueEndDate.HasValue || search.LBeginDate.HasValue
                           || search.LEndDate.HasValue || !string.IsNullOrEmpty(search.DiJieShe)
                           || !string.IsNullOrEmpty(search.Planer) || !string.IsNullOrEmpty(search.RouteName)
                           || !string.IsNullOrEmpty(search.SaleName);
                bool tmp1 = !string.IsNullOrEmpty(search.TourCode) || !string.IsNullOrEmpty(search.ZuTuanShe)
                           || search.IsMonth.HasValue || search.IsChuPiao.HasValue || search.IsClean.HasValue
                           || search.IsEnd.HasValue;
                if (tmp || tmp1)
                {

                }
                else
                {
                    //财务操作结束后，该团就自动隐藏，用搜索可查询到
                    query.Append(" and IsEnd='0' ");
                }


                if (!string.IsNullOrEmpty(search.TourCode))
                {
                    query.AppendFormat(" and TourCode like '%{0}%' ", search.TourCode);
                }
                if (!string.IsNullOrEmpty(search.RouteName))
                {
                    query.AppendFormat(" and RouteName like '%{0}%' ", search.RouteName);
                }

                if (!string.IsNullOrEmpty(search.SaleName))
                {
                    query.AppendFormat(" and exists(select 1 from tbl_CompanyUser where Id=tbl_Tour.SaleId and ContactName like '%{0}%' )", search.SaleName);
                }

                if (search.LBeginDate.HasValue)
                {

                    query.AppendFormat(" and  datediff(day,LDate,'{0}')<=0 ", search.LBeginDate.Value);
                }

                if (search.LEndDate.HasValue)
                {
                    query.AppendFormat(" and  datediff(day,LDate,'{0}')>=0 ", search.LEndDate.Value);
                }

                if (search.IssueBeginDate.HasValue)
                {

                    query.AppendFormat(" and  datediff(day,IssueTime,'{0}')<=0 ", search.IssueBeginDate.Value);
                }

                if (search.IssueEndDate.HasValue)
                {

                    query.AppendFormat(" and  datediff(day,IssueTime,'{0}')>=0 ", search.IssueEndDate.Value);
                }

                if (!string.IsNullOrEmpty(search.Planer))
                {

                    query.AppendFormat(" and exists(select 1 from tbl_CompanyUser where Id=tbl_Tour.OperatorId and ContactName like '%{0}%' )", search.Planer);
                }

                if (!string.IsNullOrEmpty(search.ZuTuanShe))
                {
                    query.AppendFormat(" and exists(select 1 from tbl_Customer where Id=tbl_Tour.BuyCompanyId and CustomerName like '%{0}%' )", search.ZuTuanShe);
                }

                if (!string.IsNullOrEmpty(search.DiJieShe))
                {
                    query.AppendFormat(" and exists(select 1 from tbl_PlanDiJie inner join tbl_CompanySupplier on tbl_PlanDiJie.GysId=tbl_CompanySupplier.Id where TourId=tbl_Tour.TourId and UnitName like '%{0}%') ", search.DiJieShe);
                }

                if (search.IsMonth.HasValue)
                {
                    query.AppendFormat(" and IsMonth='{0}' ", search.IsMonth.Value ? 1 : 0);
                }

                if (search.IsClean.HasValue)
                {
                    if (search.IsClean.Value)
                    {
                        query.Append(" and  SumPrice+ReturnMoney-CheckMoney=0");
                    }
                    else
                    {
                        query.Append(" and  SumPrice+ReturnMoney-CheckMoney<>0");
                    }
                }

                if (search.IsChuPiao.HasValue)
                {
                    query.AppendFormat(" and IsChuPiao='{0}' ", search.IsChuPiao.Value ? 1 : 0);
                }


                if (search.TourType.HasValue)
                {
                    query.AppendFormat(" and TourType={0} ", (int)search.TourType.Value);
                }

                if (search.IsEnd.HasValue)
                {
                    query.AppendFormat(" and IsEnd='{0}' ", search.IsEnd.Value ? 1 : 0);

                }
                if (search.TourStatus.HasValue)
                {
                    query.AppendFormat(" and TourStatus={0} ", (int)search.TourStatus.Value);
                }
            }
            else
            {
                //财务操作结束后，该团就自动隐藏，用搜索可查询到
                query.Append(" and IsEnd='0' ");
            }

            string OrderByString = "IsChuTuan asc,LDate desc";
            if (search != null)
            {
                switch (search.OrderByTourState)
                {
                    case 1:
                        OrderByString = " TourStatus asc, " + OrderByString;
                        break;
                    case 2:
                        OrderByString += " TourStatus desc, " + OrderByString;
                        break;
                    default:
                        OrderByString = "TourStatus asc,IsChuTuan asc,LDate desc";
                        break;
                }
            }

            using (IDataReader dr = DbHelper.ExecuteReader1(
                  this._db,
                  pageSize,
                  pageIndex,
                  ref recordCount,
                  tableName,
                  fileds.ToString(),
                  query.ToString(),
                  OrderByString,
                  null))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.TourStructure.MPageTour model = new EyouSoft.Model.TourStructure.MPageTour();
                    model.TourId = dr.GetString(dr.GetOrdinal("TourId"));
                    model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType?)dr.GetByte(dr.GetOrdinal("TourType"));
                    model.TourCode = dr.GetString(dr.GetOrdinal("TourCode"));
                    model.Planer = !dr.IsDBNull(dr.GetOrdinal("Planer")) ? dr.GetString(dr.GetOrdinal("Planer")) : null;
                    model.RouteName = !dr.IsDBNull(dr.GetOrdinal("RouteName")) ? dr.GetString(dr.GetOrdinal("RouteName")) : null;
                    model.Adults = dr.GetInt32(dr.GetOrdinal("Adults"));
                    model.Childs = dr.GetInt32(dr.GetOrdinal("Childs"));
                    model.Accompanys = dr.GetInt32(dr.GetOrdinal("Accompanys"));
                    model.BuyCompnayName = !dr.IsDBNull(dr.GetOrdinal("BuyCompanyName")) ? dr.GetString(dr.GetOrdinal("BuyCompanyName")) : null;
                    model.SumPrice = dr.GetDecimal(dr.GetOrdinal("SumPrice"));
                    model.ReturnMoney = dr.GetDecimal(dr.GetOrdinal("ReturnMoney"));
                    model.CheckMoney = dr.GetDecimal(dr.GetOrdinal("CheckMoney"));
                    model.LDate = (DateTime?)dr.GetDateTime(dr.GetOrdinal("LDate"));
                    model.RDate = (DateTime?)dr.GetDateTime(dr.GetOrdinal("RDate"));
                    model.MonthTime = !dr.IsDBNull(dr.GetOrdinal("MonthTime")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("MonthTime")) : null;
                    model.IssueTime = (DateTime?)dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.TourStatus = (EyouSoft.Model.EnumType.TourStructure.TourStatus)dr.GetByte(dr.GetOrdinal("TourStatus"));

                    model.CustomerContactList = new List<EyouSoft.Model.CRM.MCustomerContact>();
                    model.CustomerContactList = !dr.IsDBNull(dr.GetOrdinal("ContactInfo")) ? GetCustomerContactByXml(dr.GetString(dr.GetOrdinal("ContactInfo"))) : null;

                    model.PlanTicketList = new List<EyouSoft.Model.PlanStructure.MPlanTicket>();
                    model.PlanTicketList = !dr.IsDBNull(dr.GetOrdinal("TicketerInfo")) ? GetPlanTicketByXml(dr.GetString(dr.GetOrdinal("TicketerInfo"))) : null;

                    model.DiJieList = new List<EyouSoft.Model.TourStructure.MTourDiJie>();
                    model.DiJieList = !dr.IsDBNull(dr.GetOrdinal("DiJieInfo")) ? GetTourDiJieByXml(dr.GetString(dr.GetOrdinal("DiJieInfo"))) : null;

                    model.GuideList = new List<EyouSoft.Model.TourStructure.MTourGuide>();
                    model.GuideList = !dr.IsDBNull(dr.GetOrdinal("GuideInfo")) ? GetTourGuideByXml(dr.GetString(dr.GetOrdinal("GuideInfo"))) : null;

                    model.ShouKuanList = new List<EyouSoft.Model.FinStructure.MShouKuan>();
                    model.ShouKuanList = !dr.IsDBNull(dr.GetOrdinal("CollectionInfo")) ? GetShouKuanByXml(dr.GetString(dr.GetOrdinal("CollectionInfo"))) : null;

                    model.SaleName = !dr.IsDBNull(dr.GetOrdinal("SaleName")) ? dr.GetString(dr.GetOrdinal("SaleName")) : null;
                    model.RebatePeople = dr.GetInt32(dr.GetOrdinal("RebatePeople"));
                    model.RebatePrice = dr.GetDecimal(dr.GetOrdinal("RebatePrice"));
                    model.Profit = dr.GetDecimal(dr.GetOrdinal("Profit"));

                    model.IsMonth = !dr.IsDBNull(dr.GetOrdinal("IsMonth")) ? (dr.GetString(dr.GetOrdinal("IsMonth")) == "1") : false;

                    model.BuyCompanyCount = dr.GetInt32(dr.GetOrdinal("BuyCompanyCount"));

                    model.BuyCompanyId = dr.IsDBNull(dr.GetOrdinal("BuyCompanyId"))
                                             ? string.Empty
                                             : dr.GetString(dr.GetOrdinal("BuyCompanyId"));

                    list.Add(model);

                }
            }
            return list;
        }






        #region 私有方法
        /// <summary>
        /// 获取团队地接的xml
        /// </summary>
        /// <param name="list"></param>
        /// <param name="TourId"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        private string CreateTourDiJieXml(IList<EyouSoft.Model.TourStructure.MTourDiJie> list)
        {
            if (list == null) return null;
            if (list.Count == 0) return null;
            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<Root>");
            foreach (EyouSoft.Model.TourStructure.MTourDiJie model in list)
            {
                xmlDoc.Append("<TourDiJie ");
                xmlDoc.AppendFormat("Id=\"{0}\" ", !string.IsNullOrEmpty(model.Id) ? model.Id : Guid.NewGuid().ToString());
                xmlDoc.AppendFormat("DiJieId=\"{0}\" ", model.DiJieId);
                xmlDoc.AppendFormat("Name=\"{0}\" ", Utils.ReplaceXmlSpecialCharacter(model.Name));
                xmlDoc.AppendFormat("Phone=\"{0}\" ", model.Phone);
                xmlDoc.AppendFormat("QQ=\"{0}\" ", model.QQ);
                xmlDoc.Append(" />");
            }
            xmlDoc.Append("</Root>");
            return xmlDoc.ToString();
        }

        /// <summary>
        /// 获取团队导游的xml
        /// </summary>
        /// <param name="list"></param>
        /// <param name="TourId"></param>
        /// <returns></returns>
        private string CreateTourGuideXml(IList<EyouSoft.Model.TourStructure.MTourGuide> list)
        {
            if (list == null) return null;
            if (list.Count == 0) return null;
            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<Root>");
            foreach (EyouSoft.Model.TourStructure.MTourGuide model in list)
            {
                xmlDoc.Append("<TourGuide ");
                xmlDoc.AppendFormat("Id=\"{0}\" ", !string.IsNullOrEmpty(model.Id) ? model.Id : Guid.NewGuid().ToString());
                xmlDoc.AppendFormat("GuideId=\"{0}\" ", model.GuideId);
                xmlDoc.AppendFormat("Phone=\"{0}\" ", model.Phone);
                xmlDoc.AppendFormat("GuideName=\"{0}\" ", model.GuideName);
                xmlDoc.Append(" />");
            }
            xmlDoc.Append("</Root>");
            return xmlDoc.ToString();
        }

        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="TourId"></param>
        /// <returns></returns>
        private string CreateFileXml(IList<EyouSoft.Model.TourStructure.MFile> list)
        {
            if (list == null) return null;
            if (list.Count == 0) return null;
            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<Root>");
            foreach (EyouSoft.Model.TourStructure.MFile model in list)
            {
                xmlDoc.Append("<File ");
                xmlDoc.AppendFormat("Id=\"{0}\" ", !string.IsNullOrEmpty(model.Id) ? model.Id : Guid.NewGuid().ToString());
                xmlDoc.AppendFormat("FileName=\"{0}\" ", model.FileName);
                xmlDoc.AppendFormat("FilePath=\"{0}\" ", model.FilePath);
                xmlDoc.Append(" />");
            }
            xmlDoc.Append("</Root>");
            return xmlDoc.ToString();
        }

        /// <summary>
        /// 根据xml获取团队地接集合
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.MTourDiJie> GetTourDiJieByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            xml = xml.Replace("&lt;", "<").Replace("&gt;", ">");
            IList<EyouSoft.Model.TourStructure.MTourDiJie> list = new List<EyouSoft.Model.TourStructure.MTourDiJie>();

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);
            System.Xml.XmlNode root = doc.SelectSingleNode("Root");
            if (root != null)
            {
                foreach (System.Xml.XmlNode node in root.ChildNodes)
                {
                    EyouSoft.Model.TourStructure.MTourDiJie model = new EyouSoft.Model.TourStructure.MTourDiJie();
                    model.DiJieName = node["DiJieName"] != null ? node["DiJieName"].InnerText : null;
                    model.DiJieId = node["GysId"] != null ? node["GysId"].InnerText : string.Empty;

                    if (node["DiJieContactInfo"] != null && node["DiJieContactInfo"].HasChildNodes)
                    {
                        foreach (System.Xml.XmlNode child in node["DiJieContactInfo"].ChildNodes)
                        {
                            model.Name = child["ContactName"] != null ? child["ContactName"].InnerText : null;
                            model.Phone = child["ContactTel"] != null ? child["ContactTel"].InnerText : null;
                            model.QQ = child["QQ"] != null ? child["QQ"].InnerText : null;
                        }
                    }

                    list.Add(model);
                }

            }

            return list;

            //System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            //var xRows = Utils.GetXElements(xRoot, "row");
            //foreach (var xRow in xRows)
            //{
            //    EyouSoft.Model.TourStructure.MTourDiJie model = new EyouSoft.Model.TourStructure.MTourDiJie();
            //    model.Id = Utils.GetXAttributeValue(xRow, "Id");
            //    model.DiJieId = Utils.GetXAttributeValue(xRow, "DiJieId");
            //    model.DiJieName = Utils.GetXAttributeValue(xRow, "DiJieName");
            //    model.Name = Utils.GetXAttributeValue(xRow, "Name");
            //    model.Phone = Utils.GetXAttributeValue(xRow, "Phone");
            //    model.QQ = Utils.GetXAttributeValue(xRow, "QQ");
            //    list.Add(model);
            //}


        }

        /// <summary>
        /// 根据xml获取团队导游
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.MTourGuide> GetTourGuideByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.TourStructure.MTourGuide> list = new List<EyouSoft.Model.TourStructure.MTourGuide>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.TourStructure.MTourGuide model = new EyouSoft.Model.TourStructure.MTourGuide();
                model.Id = Utils.GetXAttributeValue(xRow, "Id");
                model.GuideName = Utils.GetXAttributeValue(xRow, "GuideName");
                model.Phone = Utils.GetXAttributeValue(xRow, "Phone");
                model.GuideId = Utils.GetXAttributeValue(xRow, "GuideId");
                list.Add(model);
            }
            return list;

        }

        /// <summary>
        /// 根据xml获取文件
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.MFile> GetFileByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.TourStructure.MFile> list = new List<EyouSoft.Model.TourStructure.MFile>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.TourStructure.MFile model = new EyouSoft.Model.TourStructure.MFile();
                model.Id = Utils.GetXAttributeValue(xRow, "Id");
                model.FileName = Utils.GetXAttributeValue(xRow, "FileName");
                model.FilePath = Utils.GetXAttributeValue(xRow, "FilePath");
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 根据xml获取客户单位联系人
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.CRM.MCustomerContact> GetCustomerContactByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.CRM.MCustomerContact> list = new List<EyouSoft.Model.CRM.MCustomerContact>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.CRM.MCustomerContact model = new EyouSoft.Model.CRM.MCustomerContact();
                model.Name = Utils.GetXAttributeValue(xRow, "Name");
                model.Qq = Utils.GetXAttributeValue(xRow, "qq");
                model.Tel = Utils.GetXAttributeValue(xRow, "Tel");
                model.Mobile = Utils.GetXAttributeValue(xRow, "Mobile");
                model.BirthDay = Utils.GetDateTimeNullable(Utils.GetXAttributeValue(xRow, "BirthDay"));
                model.Remark = Utils.GetXAttributeValue(xRow, "Remark");
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 根据xml获取票务安排信息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.PlanStructure.MPlanTicket> GetPlanTicketByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.PlanStructure.MPlanTicket> list = new List<EyouSoft.Model.PlanStructure.MPlanTicket>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.PlanStructure.MPlanTicket model = new EyouSoft.Model.PlanStructure.MPlanTicket();
                model.Ticketer = Utils.GetXAttributeValue(xRow, "Ticketer");
                model.Interval = Utils.GetXAttributeValue(xRow, "Interval");
                model.TrafficTime = Utils.GetDateTimeNullable(Utils.GetXAttributeValue(xRow, "TrafficTime"));
                model.TrafficNumber = Utils.GetXAttributeValue(xRow, "TrafficNumber");
                model.GysName = Utils.GetXAttributeValue(xRow, "GysName");
                model.GysId = Utils.GetXAttributeValue(xRow, "GysId");

                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// 根据xml获取收款信息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.FinStructure.MShouKuan> GetShouKuanByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.FinStructure.MShouKuan> list = new List<EyouSoft.Model.FinStructure.MShouKuan>();
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                EyouSoft.Model.FinStructure.MShouKuan model = new EyouSoft.Model.FinStructure.MShouKuan();
                model.JinE = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "CollectionRefundAmount"));
                model.ShouKuanRiQi = Utils.GetDateTime(Utils.GetXAttributeValue(xRow, "CollectionRefundDate"));
                model.ShouKuanRenName = Utils.GetXAttributeValue(xRow, "CollectionRefundOperator");
                model.ZhangHuCode = Utils.GetXAttributeValue(xRow, "BankNo");
                model.IsKaiPiao = this.GetBoolean(Utils.GetXAttributeValue(xRow, "IsKaiPiao"));
                list.Add(model);
            }

            return list;
        }




        #endregion
    }
}
