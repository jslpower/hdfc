using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UserControl
{
    using System.ComponentModel;

    public partial class ExcelFileUploadControl : System.Web.UI.UserControl
    {
        public string ClientHideID
        {
            get { return this.ClientID + "hidFileName"; }
        }

        private int _companyID;
        /// <summary>
        /// 当前登录公司编号
        /// </summary>
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        private string setimgheight;
        /// <summary>
        /// 上传控件图片大小(高度)参数必须匹配images/swfuplod目录下图片的高度
        /// </summary>
        public string SetImgheight
        {
            get { return setimgheight ?? "34"; }
            set { setimgheight = value; }
        }

        private string setimgwidth;
        /// <summary>
        /// 上传控件图片大小(宽度)参数必须匹配images/swfuplod目录下图片的宽度
        /// </summary>
        public string Setimgwidth
        {
            get { return setimgwidth ?? "178"; }
            set { setimgwidth = value; }
        }

        /// <summary>
        /// 是否可以上传txt文件
        /// </summary>
        public bool ContainsTxt
        {
            get;
            set;
        }

        private string _fileTypes = "*.xls;*.xlsx";
        /// <summary>
        /// 设置可上传文件格式 默认: *.xls;*.xlsx
        /// </summary>
        public string FileTypes
        {
            get
            {
                if (ContainsTxt) return _fileTypes + ";*.txt";

                return _fileTypes;
            }
            set { _fileTypes = value; }
        }

        protected string _uploadSuccessJavaScriptFunCallBack = "";
        /// <summary>
        /// 设置文件上传成功后，回调的JavaScript函数名称, 
        /// The CallBackFun exec Like This : FunCalllBack(arr),The Property Initial like this: FunCalllBack
        /// </summary>
        [Bindable(true)]
        public string UploadSuccessJavaScriptFunCallBack
        {
            set
            {
                _uploadSuccessJavaScriptFunCallBack = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.CompanyID == 0)
            {
                throw new Exception("UploadControl控件的CompanyID未赋值");
            }

            if(!IsPostBack)
            {
                litDown.Text = "<a href='/PrintTemplate/importtemplate.xls' target='_blank'>游客信息模板下载</a>";
            }
        }
    }
}