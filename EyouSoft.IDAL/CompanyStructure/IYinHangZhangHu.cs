//公司银行账户相关信息数据访问类接口 汪奇志 2012-11-19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Model.CompanyStructure;
using EyouSoft.Model.EnumType.CompanyStructure;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 公司银行账户相关信息数据访问类接口
    /// </summary>
    public interface IYinHangZhangHu
    {
        /// <summary>
        /// 写入公司银行账信息，返回1成功，其它失败
        /// </summary>
        /// <param name="info">实体</param>
        /// <returns></returns>
        int Insert(CompanyAccount info);
        /// <summary>
        /// 修改公司银行账信息，返回1成功，其它失败
        /// </summary>
        /// <param name="info">实体</param>
        /// <returns></returns>
        int Update(CompanyAccount info);
        /// <summary>
        /// 删除公司银行账信息，返回1成功，其它失败
        /// </summary>
        /// <param name="zhangHuId">银行账户编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        int Delete(string zhangHuId, int companyId);
        /*/// <summary>
        /// 获取公司银行账户状态
        /// </summary>
        /// <param name="zhangHuId">银行账户编号</param>
        /// <returns></returns>
        AccountState GetStatus(string zhangHuId);*/
        /// <summary>
        /// 获取公司所有银行账户信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        IList<CompanyAccount> GetZhangHus(int companyId);
        /// <summary>
        /// 设置公司银行账户状态，返回1成功，其它失败
        /// </summary>
        /// <param name="zhangHuId">银行账户编号</param>
        /// <param name="status">状态</param>
        /// <param name="info">相关信息</param>
        /// <returns></returns>
        int SetStatus(string zhangHuId, AccountState status, MOperatorInfo info);
        /// <summary>
        /// 是否存在相同的银行账号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="zhanghuId">银行账户编号</param>
        /// <param name="zhangHao">银行账号</param>
        /// <returns></returns>
        bool IsExistsZhangHao(int companyId, string zhanghuId, string zhangHao);
    }
}
