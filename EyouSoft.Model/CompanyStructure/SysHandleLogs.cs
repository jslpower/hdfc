using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    #region 系统操作日志实体
    /// <summary>
    /// 系统操作日志实体
    /// </summary>
    [Serializable]
    public class SysHandleLogs
    {
        #region Model
        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 操作员
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 部门
        /// </summary>
        public int DepatId
        {
            get;
            set;
        }
        /// <summary>
        /// 公司id
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }
        
        /// <summary>
        /// 操作模块
        /// </summary>
        public EnumType.PrivsStructure.Privs2 ModuleId
        {
            get;
            set;
        }
        /// <summary>
        /// 日志类型号
        /// </summary>
        public int EventCode
        {
            get;
            set;
        }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string EventMessage
        {
            get;
            set;
        }
        /// <summary>
        /// 日志标题
        /// </summary>
        public string EventTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 记录日志时间
        /// </summary>
        public DateTime EventTime
        {
            get;
            set;
        }
        /// <summary>
        /// 日志发生IP
        /// </summary>
        public string EventIp
        {
            get;
            set;
        }
        #endregion Model

        #region 附加属性
        /// <summary>
        /// 操作员名称
        /// </summary>
        public string OperatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName
        {
            get;
            set;
        }
        #endregion

    }
    #endregion

    #region 系统操作日志查询实体
    /// <summary>
    /// 系统操作日志查询实体
    /// </summary>
    public class QueryHandleLog
    {
        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepartId
        {
            get;
            set;
        }
        /// <summary>
        /// 操作员编号
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 操作员名称
        /// </summary>
        public string OperatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 操作开始时间
        /// </summary>
        public DateTime? HandStartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 操作结束时间
        /// </summary>
        public DateTime? HandEndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }
    }
    #endregion
}
