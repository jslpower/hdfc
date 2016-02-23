using System;
using System.Collections;
using System.Collections.Generic;


namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 实体类CustomerCallBack ,客户关怀信息
    /// autor:李焕超
    /// date:2011-1-17
    /// </summary>
    [Serializable]
    public class CustomerCareforInfo
    {
        public CustomerCareforInfo()
        { }
        #region Model
        private int _id;
        private int _companyid;
        private string _mobilecode;
        private bool _ismatchcustomerinfo;
        private bool _ismatchsupplierinfo;
        private bool _ismatchdepartmentinfo;
        private string _content;
        private DateTime? _time;
        private EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime _fixtype;
        private int _channelid;
        private bool _isenabled;
        private int _operatorid;
        private DateTime _issuetime;
        private bool _isseded;
        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 发送号码
        /// </summary>
        public string MobileCode
        {
            set { _mobilecode = value; }
            get { return _mobilecode; }
        }
        /// <summary>
        /// 是否匹配客户资料
        /// </summary>
        public bool IsMatchCustomerInfo
        {
            set { _ismatchcustomerinfo = value; }
            get { return _ismatchcustomerinfo; }
        }
        /// <summary>
        /// 是否匹配供应商资料
        /// </summary>
        public bool IsMatchSupplierInfo
        {
            set { _ismatchsupplierinfo = value; }
            get { return _ismatchsupplierinfo; }
        }
        /// <summary>
        /// 是否匹配部门人员
        /// </summary>
        public bool IsMatchDepartmentInfo
        {
            set { _ismatchdepartmentinfo = value; }
            get { return _ismatchdepartmentinfo; }
        }
        /// <summary>
        /// 发送内容（替换具体人员名称）
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// 固定发送时间（未来时间值）
        /// </summary>
        public DateTime? Time
        {
            set { _time = value; }
            get { return _time; }
        }
        /// <summary>
        /// 固定发送类型（生日、国庆等）
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.CustomerCareForSendSpecialTime FixType
        {
            set { _fixtype = value; }
            get { return _fixtype; }
        }
        /// <summary>
        /// 发送通道编号
        /// </summary>
        public int ChannelId
        {
            set { _channelid = value; }
            get { return _channelid; }
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled
        {
            set { _isenabled = value; }
            get { return _isenabled; }
        }
        /// <summary>
        /// 添加人
        /// </summary>
        public int OperatorId
        {
            set { _operatorid = value; }
            get { return _operatorid; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime
        {
            set { _issuetime = value; }
            get { return _issuetime; }
        }
        /// <summary>
        /// 当天是否已发送
        /// </summary>
        public bool IsSeded
        {
            set { _isseded = value; }
            get { return _isseded; }
        }
        #endregion Model

    }

    #region 客户关怀短信发送人员信息实体

    /// <summary>
    /// 客户关怀短信发送人员信息实体
    /// </summary>
    public class CustomerCareforSendInfo
    {
        /// <summary>
        /// 客户关怀信息Id
        /// </summary>
        public int CustomerCareforId { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 手填手机号码
        /// </summary>
        public string MobileCode { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 短信通道
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// 客户关怀添加人Id
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 姓名手机号码集合
        /// </summary>
        public IList<NameAndMobile> NameAndMobile { get; set; }
    }

    #endregion

    #region 姓名、手机号码实体

    /// <summary>
    /// 姓名、手机号码实体
    /// </summary>
    public class NameAndMobile
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string ContactMobile { get; set; }
    }

    #endregion
}

