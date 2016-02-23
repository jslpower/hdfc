using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using EyouSoft.Toolkit.DAL;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace EyouSoft.Exception.Facade
{
    [ConfigurationElementTypeAttribute(typeof(CustomHandlerData))]
    public class ExceptionHandler:IExceptionHandler
    {

        #region IExceptionHandler 成员
        
        public ExceptionHandler(System.Collections.Specialized.NameValueCollection c)
        {
            
        }

        public System.Exception HandleException(System.Exception exception, Guid handlingInstanceId)
        {
            string requestUrl = EyouSoft.Toolkit.Utils.GetRequestUrl();

            if (requestUrl.ToLower().IndexOf("/favicon.ico") == -1)//favicon.ico文件不存在。不记录到系统异常日志
            {
                new ExceptionWriter().WriteException(EyouSoft.Toolkit.Utils.GetRemoteIP()
                    , requestUrl
                    , exception.Message
                    , exception.Source
                    , exception.StackTrace);
            }

            //System.Web.HttpContext.Current.Response.Write(exception.InnerException);
            //System.Web.HttpContext.Current.Response.End();
            //throw exception;            
            return exception;
        }  
              
        #endregion
    }

    /// <summary>
    /// 系统异常记录
    /// </summary>
    public class ExceptionWriter:EyouSoft.Toolkit.DAL.DALBase
    {
        const string SQL_EXCEPTION_ADD = "INSERT INTO tbl_SysExceptionLog([ErrorID],[RemoteIP],[RequestUrl],[ErrorMessage],[ErrorSource],[StackTrace],[BrowserType])VALUES(@ErrorID,@RemoteIP,@RequestUrl,@ErrorMessage,@ErrorSource,@StackTrace,@BrowserType)";

        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public ExceptionWriter()
        {
            this._db = base.SystemStore;
        }

        /// <summary>
        /// 写入异常
        /// </summary>
        /// <param name="remoteIp">错误发生IP</param>
        /// <param name="requestUrl">错误发生页面</param>
        /// <param name="message">错误信息</param>
        /// <param name="errorSource">错误程序集</param>
        /// <param name="stackTrace">错误发生位置</param>
        public void WriteException(string remoteIp, string requestUrl, string message, string errorSource, string stackTrace)
        {
            DbCommand dc = this.SystemStore.GetSqlStringCommand(SQL_EXCEPTION_ADD);
            this._db.AddInParameter(dc, "ErrorID", DbType.String, Guid.NewGuid().ToString());
            this._db.AddInParameter(dc, "RemoteIP", DbType.String, remoteIp);
            this._db.AddInParameter(dc, "RequestUrl", DbType.String, requestUrl);
            this._db.AddInParameter(dc, "ErrorMessage", DbType.String, message);
            this._db.AddInParameter(dc, "ErrorSource", DbType.String, errorSource);
            this._db.AddInParameter(dc, "StackTrace", DbType.String, stackTrace);
            _db.AddInParameter(dc, "BrowserType", DbType.String, new EyouSoft.Toolkit.BrowserInfo().ToJsonString());
            DbHelper.ExecuteSql(dc, this._db);
        }
    }
}

