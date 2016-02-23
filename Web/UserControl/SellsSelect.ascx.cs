using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.ComponentModel;

namespace Web.UserControl
{
    public partial class SellsSelect : System.Web.UI.UserControl
    {
        /// <summary>
        /// 页面：DOM
        /// </summary>
        /// 创建人：刘飞
        /// 创建时间：2012-12-20
        /// 说明：选择销售员

        /// <summary>
        /// 销售员ID
        /// </summary>
        private string _sellsID;
        [Bindable(true)]
        public string SellsID
        {
            get { return _sellsID; }
            set { _sellsID = value; }
        }
        /// <summary>
        /// 隐藏域客户端ID和Name
        /// </summary>
        public string SellsIDClient
        {
            get { return SetPriv + "_hideSellID"; }
        }
        /// <summary>
        /// 显示文本框客户端ID和Name
        /// </summary>
        public string SellsNameClient
        {
            get { return SetPriv + "_txtSellName"; }
        }
        /// <summary>
        /// 销售员姓名
        /// </summary>
        private string _sellsName;
        [Bindable(true)]
        public string SellsName
        {
            get { return _sellsName; }
            set { _sellsName = value; }
        }
        /// <summary>
        /// 设置标题
        /// </summary>
        private string _setTitle = "销售员";
        [Bindable(true)]
        public string SetTitle
        {
            get { return _setTitle; }
            set { _setTitle = value; }
        }

        private string _setPriv = string.Empty;
        /// <summary>
        /// 指定控件Name前缀，默认为控件ClientID
        /// </summary>
        public string SetPriv
        {
            get { return string.IsNullOrEmpty(_setPriv) ? this.ClientID : _setPriv; }
            set { _setPriv = value; }
        }


        /// <summary>
        /// 先赋值的控件的boxyID
        /// </summary>
        private string _parentIframeID;
        [Bindable(true)]
        public string ParentIframeID
        {
            get { return _parentIframeID ?? Utils.GetQueryStringValue("iframeId"); }
            set { _parentIframeID = value; }
        }
        /// <summary>
        /// 单选设置 true，多选设置为 fase ;默认单选 false
        /// </summary>
        private bool _sMode = false;
        [Bindable(true)]
        public bool SMode
        {
            get { return _sMode; }
            set { _sMode = value; }
        }

        /// <summary>
        /// 设置控件只读,默认为可以修改
        /// </summary>
        private bool _readOnly = false;
        [Bindable(true)]
        public bool ReadOnly
        {
            get { return _sMode ? true : _readOnly; }
            set { _readOnly = value; }
        }

        private string _callBackFun;
        /// <summary>
        /// 设置回调  sellsCallBack
        /// </summary>
        public string CallBackFun
        {
            get
            {
                if (string.IsNullOrEmpty(_callBackFun)) return ClientID + "._callBack";

                return _callBackFun;
            }
            set { _callBackFun = value; }
        }

        /// <summary>
        /// 是否强制选中和失去焦点选择
        /// </summary>
        private bool _selectFirst = true;
        [Bindable(true)]
        public bool SelectFrist
        {
            get { return _selectFirst; }
            set { _selectFirst = value; }
        }

        private bool _isShowSelect = true;
        /// <summary>
        /// 是否显示选用按钮
        /// </summary>
        [Bindable(true)]
        public bool IsShowSelect
        {
            get { return _isShowSelect; }
            set { _isShowSelect = value; }
        }

        private string _clientDeptID;
        public string ClientDeptID
        {
            get { return _clientDeptID; }
            set { _clientDeptID = value; }
        }


        private string _clientDeptName;
        public string ClientDeptName
        {
            get { return _clientDeptName; }
            set { _clientDeptName = value; }
        }

        /// <summary>
        /// 设置是否为必填
        /// </summary>
        private bool _isNotValid = true;
        [Bindable(true)]
        public bool IsNotValid
        {
            get { return _isNotValid; }
            set { _isNotValid = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


    }
}