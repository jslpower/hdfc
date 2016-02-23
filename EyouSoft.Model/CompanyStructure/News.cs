using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    #region 公司公告信息实体
    /// <summary>
    /// 公司公告信息
    /// </summary>
    [Serializable]
    public class News
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public News()
        { }
        #endregion

        #region Model
        /// <summary>
        /// 主键编号
        /// </summary>
        public int ID
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
        /// 标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 添加人编号
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 添加人姓名
        /// </summary>
        public string OperatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime
        {
            get;
            set;
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get;
            set;
        }
        /// <summary>
        /// 点击数
        /// </summary>
        public int Clicks
        {
            get;
            set;
        }
        /// <summary>
        /// 附件
        /// </summary>
        public string UploadFiles
        {
            get;
            set;
        }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete
        {
            get;
            set;
        }
        #endregion Model

        #region 附加属性
        /// <summary>
        /// 发布对象集合
        /// </summary>
        public IList<NewsAccept> AcceptList
        {
            get;
            set;
        }
        #endregion

    }
    #endregion

    #region 公告信息发布对象实体
    /// <summary>
    /// 公告信息发布对象实体
    /// </summary>
    [Serializable]
    public class NewsAccept
    {
        public NewsAccept()
        { }

        #region Model
        /// <summary>
        /// 公告Id
        /// </summary>
        public int NewId
        {
            get;
            set;
        }
        /// <summary>
        /// 发布到的对象类型Id
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.AcceptType AcceptType
        {
            get;
            set;
        }
        /// <summary>
        /// 对象Id（部门Id，组团社Id，没有为0）
        /// </summary>
        public int AcceptId
        {
            get;
            set;
        }
        #endregion Model
    }
    #endregion

    #region 自己能看到的公告实体

    /// <summary>
    /// 自己能看到的公告实体
    /// </summary>
    public class NoticeNews
    {
        /// <summary>
        /// 消息编号
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ClickNum
        {
            get;
            set;
        }

        /// <summary>
        /// 发布人
        /// </summary>
        public string OperateName
        {
            get;
            set;
        }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? IssueTime
        {
            get;
            set;
        }
    }

    #endregion

}
