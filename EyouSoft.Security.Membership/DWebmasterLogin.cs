//2011-09-23 汪奇志
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Model.SSOStructure;

namespace EyouSoft.Security.Membership
{
    /// <summary>
    /// webmaster登录数据访问类
    /// </summary>
    internal class DWebmasterLogin:DALBase,IWebmasterLogin
    {
        #region static constants
        //static constants
        const string SQL_SELECT_Login = "SELECT [Id],[Username] FROM [tbl_Webmaster] WHERE [Username]=@UN AND [MD5Password]=@MD5PWD";
        #endregion

        #region constructor
        /// <summary>
        /// database
        /// </summary>
        Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public DWebmasterLogin()
        {
            _db = SystemStore;
        }
        #endregion      

        #region IWebmasterLogin 成员
        /// <summary>
        /// webmaster登录，根据用户名、用户密码获取用户信息
        /// </summary>
        /// <param name="username">登录账号</param>
        /// <param name="pwd">登录密码</param>
        /// <returns></returns>
        public MWebmasterInfo Login(string username, Model.CompanyStructure.PassWord pwd)
        {
            MWebmasterInfo info = null;
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_Login);
            _db.AddInParameter(cmd, "UN", DbType.String, username);
            _db.AddInParameter(cmd, "MD5PWD", DbType.String, pwd.MD5Password);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    info = new MWebmasterInfo
                    {
                        UserId = rdr.GetInt32(0),
                        Username = rdr.GetString(1)
                    };
                }
            }

            return info;
        }
        #endregion
    }
}
