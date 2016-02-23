using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Model.TongJiStructure;
using System.Xml.Linq;

namespace EyouSoft.DAL.TongJiStructure
{
    /// <summary>
    /// 统计数据访问
    /// </summary>
    public class DTongJi : DALBase, IDAL.TongJiStructure.ITongJi
    {
        private readonly Database _db;

        public DTongJi()
        {
            _db = this.SystemStore;
        }

        /// <summary>
        /// 获取团散统计列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">团散统计查询实体</param>
        /// <param name="heJi">团散统计合计实体(应用传null或者实例化一个就可以了，不用具体赋值)</param>
        /// <returns></returns>
        public IList<MTourAndSan> GetTourAndSan(int companyId, int pageSize, int pageIndex, ref int recordCount
            , MSearchTourAndSan search, MTourAndSanHeJi heJi)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            if (heJi == null) heJi = new MTourAndSanHeJi();

            IList<MTourAndSan> list;
            string tableName = "view_TourAndCustomer";
            string fields = " [CustomerId],[LDate],[TourCode],[Adults],[Childs],[Accompanys],[RouteName],[SumPrice],[Profit],[YongJin],[CustomerName],[CustomerProviceId],[CustomerCityId],[OperatorName],[SaleName],[DiJieZhiChu],[JiPiaoZhiChu],[IssueTime],[SaleId],[OperatorId],[TourId],[ReceivedMoney] ";
            fields +=
                " ,(SELECT TicketType,Interval,TrafficTime,TrafficNumber FROM tbl_PlanPiao AS tpp WHERE tpp.TourId = view_TourAndCustomer.TourId FOR XML RAW,ROOT('Root')) AS TicketInfo ";
            string orderByStr = " IssueTime desc ";
            string sumStr = " Sum(Adults) as SumAdults,Sum(Childs) as SumChilds,Sum(Accompanys) as SumAccompanys,Sum(SumPrice) as ZongShouRu,Sum(DiJieZhiChu) as SumDiJieZhiChu,Sum(JiPiaoZhiChu) as SumJiPiaoZhiChu,Sum(YongJin) as ZongYongJin,Sum(Profit) as ZongMaoLi,Sum(ReceivedMoney) as SumReceivedMoney ";
            var strWhere = new StringBuilder();
            strWhere.AppendFormat(" CompanyId = {0} ", companyId);

            #region sql条件处理

            if (search != null)
            {
                if (search.ProvinceId != null && search.ProvinceId.Any())
                {
                    if (search.ProvinceId.Length == 1)
                    {
                        strWhere.AppendFormat(" and CustomerProviceId = {0} ", search.ProvinceId[0]);
                    }
                    else
                    {
                        strWhere.AppendFormat(" and CustomerProviceId in ({0}) ", this.GetIdsByArr(search.ProvinceId));
                    }
                }
                if (search.CityId != null && search.CityId.Any())
                {
                    if (search.CityId.Length == 1)
                    {
                        strWhere.AppendFormat(" and CustomerCityId = {0} ", search.CityId[0]);
                    }
                    else
                    {
                        strWhere.AppendFormat(" and CustomerCityId in ({0}) ", this.GetIdsByArr(search.CityId));
                    }
                }
                if (search.StartLeaveDate.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',LDate) >= 0 ", search.StartLeaveDate.Value.ToShortDateString());
                }
                if (search.EndLeaveDate.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',LDate) <= 0 ", search.EndLeaveDate.Value.ToShortDateString());
                }
                if (search.StartOrderDate.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',IssueTime) >= 0 ", search.StartOrderDate.Value.ToShortDateString());
                }
                if (search.EndOrderDate.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',IssueTime) <= 0 ", search.EndOrderDate.Value.ToShortDateString());
                }
                if (!string.IsNullOrEmpty(search.CustomerId))
                {
                    strWhere.AppendFormat(" and CustomerId = '{0}' ", Utils.ToSqlLike(search.CustomerId));
                }
                if (!string.IsNullOrEmpty(search.CustomerName))
                {
                    strWhere.AppendFormat(" and CustomerName like '%{0}%' ", Utils.ToSqlLike(search.CustomerName));
                }
                if (search.OperatorId > 0)
                {
                    strWhere.AppendFormat(" and OperatorId = {0} ", search.OperatorId);
                }
                if (!string.IsNullOrEmpty(search.OperatorName))
                {
                    strWhere.AppendFormat(" and OperatorName like '%{0}%' ", Utils.ToSqlLike(search.OperatorName));
                }
                if (search.SaleId > 0)
                {
                    strWhere.AppendFormat(" and SaleId = {0} ", search.SaleId);
                }
                if (!string.IsNullOrEmpty(search.SaleName))
                {
                    strWhere.AppendFormat(" and SaleName like '%{0}%' ", Utils.ToSqlLike(search.SaleName));
                }
                if (search.TourType.HasValue)
                {
                    strWhere.AppendFormat(" and TourType = {0} ", (int)search.TourType.Value);
                }
            }

            #endregion

            list = new List<MTourAndSan>();
            using (IDataReader dr = DbHelper.ExecuteReader1(_db, pageSize, pageIndex, ref recordCount, tableName, fields
                , strWhere.ToString(), orderByStr, sumStr))
            {
                #region 实体赋值

                while (dr.Read())
                {
                    var model = new MTourAndSan
                        {
                            TourId =
                                dr.IsDBNull(dr.GetOrdinal("TourId"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("TourId")),
                            CustomerName =
                                dr.IsDBNull(dr.GetOrdinal("CustomerName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("CustomerName")),
                            PlanName =
                                dr.IsDBNull(dr.GetOrdinal("OperatorName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("OperatorName")),
                            SaleName =
                                dr.IsDBNull(dr.GetOrdinal("SaleName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("SaleName")),
                            TourNo =
                                dr.IsDBNull(dr.GetOrdinal("TourCode"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("TourCode")),
                            Adults = dr.IsDBNull(dr.GetOrdinal("Adults")) ? 0 : dr.GetInt32(dr.GetOrdinal("Adults")),
                            Childs = dr.IsDBNull(dr.GetOrdinal("Childs")) ? 0 : dr.GetInt32(dr.GetOrdinal("Childs")),
                            Accompanys =
                                dr.IsDBNull(dr.GetOrdinal("Accompanys")) ? 0 : dr.GetInt32(dr.GetOrdinal("Accompanys")),
                            RouteName =
                                dr.IsDBNull(dr.GetOrdinal("RouteName"))
                                    ? string.Empty
                                    : dr.GetString(dr.GetOrdinal("RouteName")),
                            ShouRuSumPrice =
                                dr.IsDBNull(dr.GetOrdinal("SumPrice")) ? 0M : dr.GetDecimal(dr.GetOrdinal("SumPrice")),
                            DiJieZhiChu =
                                dr.IsDBNull(dr.GetOrdinal("DiJieZhiChu"))
                                    ? 0M
                                    : dr.GetDecimal(dr.GetOrdinal("DiJieZhiChu")),
                            JiPiaoZhiChu =
                                dr.IsDBNull(dr.GetOrdinal("JiPiaoZhiChu"))
                                    ? 0M
                                    : dr.GetDecimal(dr.GetOrdinal("JiPiaoZhiChu")),
                            YongJin =
                                dr.IsDBNull(dr.GetOrdinal("YongJin")) ? 0M : dr.GetDecimal(dr.GetOrdinal("YongJin")),
                            MaoLi = dr.IsDBNull(dr.GetOrdinal("Profit")) ? 0M : dr.GetDecimal(dr.GetOrdinal("Profit")),
                            YiShou =
                                dr.IsDBNull(dr.GetOrdinal("ReceivedMoney"))
                                    ? 0M
                                    : dr.GetDecimal(dr.GetOrdinal("ReceivedMoney"))
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("LDate"))) model.LeaveDate = dr.GetDateTime(dr.GetOrdinal("LDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TicketInfo")))
                    {
                        model.TicketFloat = this.GetPlanTicketFloat(dr.GetString(dr.GetOrdinal("TicketInfo")));
                    }

                    list.Add(model);
                }

                dr.NextResult();
                if (dr.Read())
                {
                    heJi.Accompanys = dr.IsDBNull(dr.GetOrdinal("SumAccompanys")) ? 0 : dr.GetInt32(dr.GetOrdinal("SumAccompanys"));
                    heJi.Adults = dr.IsDBNull(dr.GetOrdinal("SumAdults")) ? 0 : dr.GetInt32(dr.GetOrdinal("SumAdults"));
                    heJi.Childs = dr.IsDBNull(dr.GetOrdinal("SumChilds")) ? 0 : dr.GetInt32(dr.GetOrdinal("SumChilds"));
                    heJi.ShouRu = dr.IsDBNull(dr.GetOrdinal("ZongShouRu")) ? 0M : dr.GetDecimal(dr.GetOrdinal("ZongShouRu"));
                    heJi.DiJieZhiChu = dr.IsDBNull(dr.GetOrdinal("SumDiJieZhiChu")) ? 0M : dr.GetDecimal(dr.GetOrdinal("SumDiJieZhiChu"));
                    heJi.JiPiaoZhiChu = dr.IsDBNull(dr.GetOrdinal("SumJiPiaoZhiChu")) ? 0M : dr.GetDecimal(dr.GetOrdinal("SumJiPiaoZhiChu"));
                    heJi.YongJin = dr.IsDBNull(dr.GetOrdinal("ZongYongJin")) ? 0M : dr.GetDecimal(dr.GetOrdinal("ZongYongJin"));
                    heJi.MaoLi = dr.IsDBNull(dr.GetOrdinal("ZongMaoLi")) ? 0M : dr.GetDecimal(dr.GetOrdinal("ZongMaoLi"));
                    heJi.YiShou = dr.IsDBNull(dr.GetOrdinal("SumReceivedMoney"))
                                      ? 0M
                                      : dr.GetDecimal(dr.GetOrdinal("SumReceivedMoney"));
                }

                #endregion
            }

            return list;
        }

        /// <summary>
        /// 获取组团社统计列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">组团社统计查询实体</param>
        /// <param name="heJi">组团社统计合计实体(应用传null或者实例化一个就可以了，不用具体赋值)</param>
        /// <returns></returns>
        public IList<MCustomerTongJi> GetCustomerTongJi(int companyId, int pageSize, int pageIndex, ref int recordCount
            , MSearchCustomerTongJi search, MCustomerTongJiHeJi heJi)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            if (heJi == null) heJi = new MCustomerTongJiHeJi();
            var tableName = new StringBuilder();
            var fields = new StringBuilder();
            var strWhere = new StringBuilder();
            var dateWhere = new StringBuilder();
            string orderByStr = " CustomerIssueTime desc ";
            string sumStr = " Sum(JiaoYiRenShu) as SumJiaoYiRenShu,Sum(JiaoYiCiShu) as SumJiaoYiCiShu, Sum(JiaoYiJinE) as SumJiaoYiJinE,Sum(BaiFangCiShu) as SumBaiFangCiShu,Sum(BaiFangZhiChu) as SumBaiFangZhiChu ";

            #region SqlWhere处理

            if (search != null)
            {
                if (search.ProvinceId != null && search.ProvinceId.Length > 0)
                {
                    if (search.ProvinceId.Length == 1)
                    {
                        strWhere.AppendFormat(" and CustomerProviceId = {0} ", search.ProvinceId[0]);
                    }
                    else
                    {
                        strWhere.AppendFormat(" and CustomerProviceId in ({0}) ", this.GetIdsByArr(search.ProvinceId));
                    }
                }
                if (search.CityId != null && search.CityId.Length > 0)
                {
                    if (search.CityId.Length == 1)
                    {
                        strWhere.AppendFormat(" and CustomerCityId = {0} ", search.CityId[0]);
                    }
                    else
                    {
                        strWhere.AppendFormat(" and CustomerCityId in ({0}) ", this.GetIdsByArr(search.CityId));
                    }
                }
                if (search.StartTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',IssueTime) >= 0 ", search.StartTime.Value.ToShortDateString());
                    dateWhere.AppendFormat(
                        " and datediff(dd,'{0}',ccf.VisitTime) >= 0 ", search.StartTime.Value.ToShortDateString());
                }
                if (search.EndTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',IssueTime) <= 0 ", search.EndTime.Value.ToShortDateString());
                    dateWhere.AppendFormat(
                        " and datediff(dd,'{0}',ccf.VisitTime) <= 0 ", search.EndTime.Value.ToShortDateString());
                }

                #region 排序处理

                switch (search.OrderByIndex)
                {
                    case 0:
                        orderByStr = " CustomerIssueTime asc ";
                        break;
                    case 2:
                        orderByStr = " JiaoYiRenShu asc ";
                        break;
                    case 3:
                        orderByStr = " JiaoYiRenShu desc ";
                        break;
                    case 4:
                        orderByStr = " JiaoYiCiShu asc ";
                        break;
                    case 5:
                        orderByStr = " JiaoYiCiShu desc ";
                        break;
                    case 6:
                        orderByStr = " JiaoYiJinE asc ";
                        break;
                    case 7:
                        orderByStr = " JiaoYiJinE desc ";
                        break;
                    case 8:
                        orderByStr = " BaiFangCiShu asc ";
                        break;
                    case 9:
                        orderByStr = " BaiFangCiShu desc ";
                        break;
                    case 10:
                        orderByStr = " BaiFangCiShu asc ";
                        break;
                    case 11:
                        orderByStr = " BaiFangZhiChu desc ";
                        break;
                    default:
                        orderByStr = " CustomerIssueTime desc ";
                        break;
                }

                #endregion
            }

            #endregion

            #region tableName处理

            tableName.Append(" SELECT ");
            tableName.Append(" CustomerId, CustomerName,CustomerProvinceName,CustomerCityName,ContactName,ContactTel,ContactMobile,CustomerIssueTime ");
            tableName.Append(" ,(ISNULL(SUM(Adults),0) + ISNULL(SUM(Childs),0)) AS JiaoYiRenShu ");
            tableName.Append(" ,COUNT(TourId) AS JiaoYiCiShu,SUM(SumPrice) AS JiaoYiJinE ");
            tableName.AppendFormat(
                " ,(SELECT COUNT(ccf.Id) AS BaiFangCiShu FROM tbl_CustomerCarefor AS ccf WHERE ccf.CustomerId = view_TourAndCustomer.CustomerId {0} ) AS BaiFangCiShu ",
                dateWhere.ToString());
            tableName.AppendFormat(
                " ,(SELECT SUM(ccf.PayMoney) AS BaiFangZhiChu FROM tbl_CustomerCarefor AS ccf WHERE ccf.CustomerId = view_TourAndCustomer.CustomerId {0} ) AS BaiFangZhiChu ",
                dateWhere.ToString());
            tableName.Append(" FROM view_TourAndCustomer ");
            tableName.AppendFormat(" where CompanyId = {0} ", companyId);
            tableName.Append(strWhere.ToString());
            tableName.Append(" GROUP BY CustomerId, CustomerName,CustomerProvinceName,CustomerCityName,ContactName,ContactTel,ContactMobile,CustomerIssueTime ");

            #endregion

            fields.Append(
                " CustomerId, CustomerName,CustomerProvinceName,CustomerCityName,ContactName,ContactTel,ContactMobile,CustomerIssueTime ");
            fields.Append(",JiaoYiRenShu,JiaoYiCiShu,JiaoYiJinE,BaiFangCiShu,BaiFangZhiChu ");

            var list = new List<MCustomerTongJi>();
            using (IDataReader dr = DbHelper.ExecuteReader2(_db, pageSize, pageIndex, ref recordCount, tableName.ToString()
                , fields.ToString(), string.Empty, orderByStr, sumStr))
            {
                #region 实体赋值

                while (dr.Read())
                {
                    list.Add(
                        new MCustomerTongJi
                            {
                                CustomerId =
                                    dr.IsDBNull(dr.GetOrdinal("CustomerId"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CustomerId")),
                                ProvinceName =
                                    dr.IsDBNull(dr.GetOrdinal("CustomerProvinceName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CustomerProvinceName")),
                                CityName =
                                    dr.IsDBNull(dr.GetOrdinal("CustomerCityName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CustomerCityName")),
                                CustomerName =
                                    dr.IsDBNull(dr.GetOrdinal("CustomerName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("CustomerName")),
                                ContactName =
                                    dr.IsDBNull(dr.GetOrdinal("ContactName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("ContactName")),
                                ContactTel =
                                    dr.IsDBNull(dr.GetOrdinal("ContactTel"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("ContactTel")),
                                ContactMobile =
                                    dr.IsDBNull(dr.GetOrdinal("ContactMobile"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("ContactMobile")),
                                JiaoYiRenShu =
                                    dr.IsDBNull(dr.GetOrdinal("JiaoYiRenShu"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("JiaoYiRenShu")),
                                JiaoYiCiShu =
                                    dr.IsDBNull(dr.GetOrdinal("JiaoYiCiShu"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("JiaoYiCiShu")),
                                JiaoYiJinE =
                                    dr.IsDBNull(dr.GetOrdinal("JiaoYiJinE"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("JiaoYiJinE")),
                                BaiFangCiShu =
                                    dr.IsDBNull(dr.GetOrdinal("BaiFangCiShu"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("BaiFangCiShu")),
                                BaiFangJinE =
                                    dr.IsDBNull(dr.GetOrdinal("BaiFangZhiChu"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("BaiFangZhiChu"))
                            });
                }

                dr.NextResult();
                if (dr.Read())
                {
                    if (!dr.IsDBNull(dr.GetOrdinal("SumJiaoYiRenShu"))) heJi.JiaoYiRenShu = dr.GetInt32(dr.GetOrdinal("SumJiaoYiRenShu"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SumJiaoYiCiShu"))) heJi.JiaoYiCiShu = dr.GetInt32(dr.GetOrdinal("SumJiaoYiCiShu"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SumJiaoYiJinE"))) heJi.JiaoYiJinE = dr.GetDecimal(dr.GetOrdinal("SumJiaoYiJinE"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SumBaiFangCiShu"))) heJi.BaiFangCiShu = dr.GetInt32(dr.GetOrdinal("SumBaiFangCiShu"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SumBaiFangZhiChu"))) heJi.BaiFangJinE = dr.GetDecimal(dr.GetOrdinal("SumBaiFangZhiChu"));
                }

                #endregion
            }

            return list;
        }

        /// <summary>
        /// 获取销售地区统计列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="search">销售地区统计查询实体</param>
        /// <param name="heJi">销售地区统计合计实体(应用传null或者实例化一个就可以了，不用具体赋值)</param>
        /// <returns></returns>
        public IList<MSaleAreaTongJi> GetSaleAreaTongJi(int companyId, int pageSize, int pageIndex, ref int recordCount
            , MSearchSaleAreaTongJi search, MSaleAreaTongJiHeJi heJi)
        {
            if (companyId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            if (heJi == null) heJi = new MSaleAreaTongJiHeJi();

            var tableName = new StringBuilder();
            string fields = " SaleAreadId,SaleAreaName,ZongShouRu,DiJieZhiChu,JiPiaoZhiChu,YongJin,MaoLi ";
            var strWhere = new StringBuilder();
            string orderByStr = " SaleAreadId desc ";
            string sumStr =
                " Sum(ZongShouRu) as SumZongShouRu,Sum(DiJieZhiChu) as SumDiJieZhiChu,Sum(JiPiaoZhiChu) as SumJiPiaoZhiChu, Sum(YongJin) as SumYongJin,Sum(MaoLi) as SumMaoLi ";

            #region tableName处理

            tableName.Append(" SELECT ");
            tableName.Append(" SaleAreadId,SaleAreaName ");
            tableName.Append(" ,SUM(SumPrice) AS ZongShouRu,Sum(DiJieZhiChu) as DiJieZhiChu,Sum(JiPiaoZhiChu) as JiPiaoZhiChu,SUM(YongJin) AS YongJin,SUM(Profit) AS MaoLi ");
            tableName.Append(" FROM view_TourAndCustomer ");
            tableName.AppendFormat(" where CompanyId = {0} ", companyId);

            #region where处理

            if (search != null)
            {
                if (search.StartTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',IssueTime) >= 0 ", search.StartTime.Value.ToShortDateString());
                }
                if (search.EndTime.HasValue)
                {
                    strWhere.AppendFormat(
                        " and datediff(dd,'{0}',IssueTime) <= 0 ", search.EndTime.Value.ToShortDateString());
                }
            }

            #endregion

            tableName.Append(strWhere.ToString());
            tableName.Append(" GROUP BY SaleAreadId,SaleAreaName ");

            #endregion

            var list = new List<MSaleAreaTongJi>();

            using (IDataReader dr = DbHelper.ExecuteReader2(_db, pageSize, pageIndex, ref recordCount, tableName.ToString()
                , fields, string.Empty, orderByStr, sumStr))
            {
                #region 实体赋值

                while (dr.Read())
                {
                    list.Add(
                        new MSaleAreaTongJi
                            {
                                SaleAreaId =
                                    dr.IsDBNull(dr.GetOrdinal("SaleAreadId"))
                                        ? 0
                                        : dr.GetInt32(dr.GetOrdinal("SaleAreadId")),
                                SaleAreaName =
                                    dr.IsDBNull(dr.GetOrdinal("SaleAreaName"))
                                        ? string.Empty
                                        : dr.GetString(dr.GetOrdinal("SaleAreaName")),
                                ZongShouRu =
                                    dr.IsDBNull(dr.GetOrdinal("ZongShouRu"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("ZongShouRu")),
                                DiJieZhiChu =
                                    dr.IsDBNull(dr.GetOrdinal("DiJieZhiChu"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("DiJieZhiChu")),
                                JiPiaoZhiChu =
                                    dr.IsDBNull(dr.GetOrdinal("JiPiaoZhiChu"))
                                        ? 0M
                                        : dr.GetDecimal(dr.GetOrdinal("JiPiaoZhiChu")),
                                YongJin =
                                    dr.IsDBNull(dr.GetOrdinal("YongJin")) ? 0M : dr.GetDecimal(dr.GetOrdinal("YongJin")),
                                MaoLi = dr.IsDBNull(dr.GetOrdinal("MaoLi")) ? 0M : dr.GetDecimal(dr.GetOrdinal("MaoLi"))
                            });
                }

                dr.NextResult();
                if (dr.Read())
                {
                    heJi.ShouRu = dr.IsDBNull(dr.GetOrdinal("SumZongShouRu")) ? 0M : dr.GetDecimal(dr.GetOrdinal("SumZongShouRu"));
                    heJi.DiJieZhiChu = dr.IsDBNull(dr.GetOrdinal("SumDiJieZhiChu")) ? 0M : dr.GetDecimal(dr.GetOrdinal("SumDiJieZhiChu"));
                    heJi.JiPiaoZhiChu = dr.IsDBNull(dr.GetOrdinal("SumJiPiaoZhiChu")) ? 0M : dr.GetDecimal(dr.GetOrdinal("SumJiPiaoZhiChu"));
                    heJi.YongJin = dr.IsDBNull(dr.GetOrdinal("SumYongJin")) ? 0M : dr.GetDecimal(dr.GetOrdinal("SumYongJin"));
                    heJi.MaoLi = dr.IsDBNull(dr.GetOrdinal("SumMaoLi")) ? 0M : dr.GetDecimal(dr.GetOrdinal("SumMaoLi"));
                }

                #endregion
            }

            return list;
        }

        /// <summary>
        /// 根据sqlxml获取机票安排浮动信息
        /// </summary>
        /// <param name="xml">sqlxml</param>
        /// <returns></returns>
        private IList<Model.PlanStructure.MPlanTicketFloat> GetPlanTicketFloat(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;

            var xRoot = XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            if (xRows == null || !xRows.Any()) return null;

            return (from t in xRows
                    where t != null
                    select
                        new Model.PlanStructure.MPlanTicketFloat
                            {
                                Interval = Utils.GetXAttributeValue(t, "Interval"),
                                TicketType =
                                    (Model.EnumType.PlanStructure.TicketType)
                                    Utils.GetInt(Utils.GetXAttributeValue(t, "TicketType")),
                                TrafficNumber = Utils.GetXAttributeValue(t, "TrafficNumber"),
                                TrafficTime = Utils.GetDateTimeNullable(Utils.GetXAttributeValue(t, "TrafficTime"))
                            }).
                ToList();
        }
    }
}
