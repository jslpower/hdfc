//2011-09-23 汪奇志
using System;
using EyouSoft.Model.SSOStructure;

namespace EyouSoft.Security.Membership
{
    /// <summary>
    /// webmaster登录数据访问类接口
    /// </summary>
    internal interface IWebmasterLogin
    {
        /// <summary>
        /// webmaster登录，根据用户名、用户密码获取用户信息
        /// </summary>
        /// <param name="username">登录账号</param>
        /// <param name="pwd">登录密码</param>
        /// <returns></returns>
        MWebmasterInfo Login(string username, Model.CompanyStructure.PassWord pwd);
    }
}
