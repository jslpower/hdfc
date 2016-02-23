using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    #region 文档管理实体
    /// <summary>
    /// 个人中心-文档管理实体
    /// </summary>
    [Serializable]
    public class PersonDocument
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PersonDocument()
        { }
        #endregion

        #region Model
        /// <summary>
        /// 文档编号
        /// </summary>
        public int DocumentId
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
        /// <summary>
        /// 文档名称
        /// </summary>
        public string DocumentName
        {
            get;
            set;
        }
        /// <summary>
        /// 文档路径
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }
        /// <summary>
        /// 上传人编号
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 上传人姓名
        /// </summary>
        public string OperatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime CreateTime
        {
            get;
            set;
        }
        public bool IsDelete
        {
            get;
            set;
        }
        #endregion Model
    }
    #endregion
}
