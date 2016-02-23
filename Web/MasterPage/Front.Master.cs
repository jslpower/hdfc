using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Model.EnumType.PrivsStructure;
using EyouSoft.Security.Membership;
using EyouSoft.Common;

namespace Web.MasterPage
{
    public partial class Front : System.Web.UI.MasterPage
    {
        #region attributes
        /// <summary>
        /// 页面标题
        /// </summary>
        public string ITitle = string.Empty;
        /// <summary>
        /// 公司名称
        /// </summary>
        protected string CompanyName = string.Empty;
        /// <summary>
        /// logo文件路径
        /// </summary>
        protected string LogoFilePath = "/images/pngclear.gif";
        /// <summary>
        /// 当前登录用户姓名
        /// </summary>
        protected string UserXingMing = string.Empty;

        /// <summary>
        /// 获取弹窗提醒间隔时间，单位毫秒 600000
        /// </summary>
        protected int TanChuangTiXingInterval = 600000;

        /// <summary>
        /// 是否有提醒的权限
        /// </summary>
        protected int IsTranRemind = 0;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            EyouSoft.Model.SysStructure.SystemDomain sysDomain = EyouSoft.Security.Membership.UserProvider.GetDomain();

            if (sysDomain == null || sysDomain.CompanyId < 1 || sysDomain.SysId < 1)
            {
                Response.Clear();
                Response.Write("请求异常：错误的域名配置。");
                Response.End();
            }

            CompanyName = sysDomain.CompanyName;

            var setting = EyouSoft.Security.Membership.UserProvider.GetComSetting(sysDomain.CompanyId);

            if (setting != null && !string.IsNullOrEmpty(setting.CompanyLogo)) LogoFilePath = setting.CompanyLogo;

            var uinfo = EyouSoft.Security.Membership.UserProvider.GetUserInfo();

            if (uinfo != null)
            {
                IsTranRemind = UserProvider.IsPrivs3(uinfo.Privs, (int)Privs3.客户管理_生日中心_生日弹窗提醒) ? 1 : 0;

                UserXingMing = uinfo.Name;
                if (uinfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.专线用户)
                {
                    ImgNewNotice.Visible = new EyouSoft.BLL.CompanyStructure.News().IsNews(
                        uinfo.CompanyId, uinfo.DeptId, uinfo.UserId);
                    BindGongGao(uinfo.CompanyId, uinfo.UserId);
                }
                else//供应商用户隐藏公告栏
                {
                    gonggaobox.Visible = false;
                }
            }

            if (string.IsNullOrEmpty(ITitle)) ITitle = Page.Title;

            InitMenuPrivs(uinfo);
            InitHighlight();

        }

        private void BindGongGao(int companyId, int userId)
        {
            var newsBll = new EyouSoft.BLL.CompanyStructure.News();
            int recordcount = 0;
            IList<EyouSoft.Model.CompanyStructure.NoticeNews> list = newsBll.GetAcceptNews(10, 1, ref recordcount, userId, companyId);
            if (list != null && list.Count > 0)
            {
                this.RptList.DataSource = list.Take(10).ToList();
                this.RptList.DataBind();
            }
        }

        #region private members
        /// <summary>
        /// init left menu
        /// </summary>
        /// <param name="info">user info</param>
        void InitMenuPrivs(EyouSoft.Model.SSOStructure.MUserInfo info)
        {
            if (info == null)
            {
                div_1.Visible = div_2.Visible = div_3.Visible = div_4.Visible = div_5.Visible = div_6.Visible = div_7.Visible = div_8.Visible = div9.Visible = false;
                return;
            }

            bool b1 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.计调中心_确认件登记_栏目);
            bool b2 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.计调中心_地接安排_栏目);
            bool b3 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.计调中心_票务安排_栏目);
            bool b4 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.计调中心_回访提醒_栏目);
            bool b5 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.计调中心_团队质量反馈_栏目);
            bool b6 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.计调中心_线路管理_栏目);
            bool b38 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.计调中心_团队报价资料库_栏目);
            div_1.Visible = b1 || b2 || b3 || b4 || b5 || b6 || b38;
            li_1.Visible = b1;
            li_2.Visible = b2;
            li_3.Visible = b3;
            li_4.Visible = b4;
           // li_5.Visible = b5;
            li_6.Visible = b6;
            li_38.Visible = b38;

            bool b7 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.供应商管理_地接_栏目);
            bool b8 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.供应商管理_票务_栏目);
            bool b9 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.供应商管理_酒店_栏目);
            bool b10 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.供应商管理_餐馆_栏目);
            bool b11 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.供应商管理_景点_栏目);
            bool b12 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.供应商管理_导游_栏目);
            div_2.Visible = b7 || b8 || b9 || b10 || b11 || b12;
            li_7.Visible = b7;
            li_8.Visible = b8;
            li_9.Visible = b9;
            li_10.Visible = b10;
            li_11.Visible = b11;
            li_12.Visible = b12;

            bool b13 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.财务管理_应收管理_栏目);
            bool b14 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.财务管理_应付管理_栏目);
            bool b15 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.财务管理_其他收入_栏目);
            bool b16 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.财务管理_其他支出_栏目);
            bool b17 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.财务管理_出纳登账_栏目);
            bool b18 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.财务管理_银行余额_栏目);
            div_3.Visible = b13 || b14 || b15 || b16 || b17 || b18;
            li_13.Visible = b13;
            li_14.Visible = b14;
            li_15.Visible = b15;
            li_16.Visible = b17;
            li_17.Visible = b17;
            li_18.Visible = b18;

            bool b19 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.统计中心_团散统计_栏目);
            bool b20 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.统计中心_组团社统计_栏目);
            bool b21 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.统计中心_销售地区统计_栏目);
            div_4.Visible = b19 || b20 || b21;
            li_19.Visible = b19;
            li_20.Visible = b20;
            li_21.Visible = b21;

            bool b22 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.客户管理_客户关怀_栏目);
            bool b23 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.客户管理_客户资料_栏目);
            bool b24 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.客户管理_生日中心_栏目);

            div_5.Visible = b22 || b23 || b24;
            li_22.Visible = b22;
            li_23.Visible = b23;
            li_24.Visible = b24;



            bool b25 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.客户询价_客户日常询价_栏目);
            bool b26 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.客户询价_外联每天足迹_栏目);



            div_6.Visible = b25 || b26;
            li_25.Visible = b25;
            li_26.Visible = b26;


            //专线用户公告通知不进行权限验证
            bool b27 = true;
            if (info.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.地接用户
                || info.UserType == EyouSoft.Model.EnumType.CompanyStructure.UserType.票务用户)
            {
                b27 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.公司文件_文档管理_栏目);
            }
            bool b28 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.公司文件_文档管理_栏目);
            div9.Visible = b27 || b28;
            li_27.Visible = b27;
            li_28.Visible = b28;


            bool b29 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.系统设置_基础设置_栏目);
            bool b30 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.系统设置_组织机构_栏目);
            bool b31 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.系统设置_角色管理_栏目);
            bool b32 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.系统设置_公司信息_栏目);
            bool b33 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.系统设置_系统日志_栏目);
            div_7.Visible = b29 || b30 || b31 || b32 || b33;
            li_29.Visible = b29;
            li_30.Visible = b30;
            li_31.Visible = b31;
            li_32.Visible = b32;
            li_33.Visible = b33;



            bool b34 = UserProvider.IsPrivs3(info.Privs, (int)Privs3.短信中心_短信中心_栏目);
            div_8.Visible = b34;
            li_34.Visible = li_35.Visible = li_36.Visible = li_37.Visible = b34;
        }

        /// <summary>
        /// init hightlight
        /// </summary>
        void InitHighlight()
        {
            string s = Request.Url.AbsolutePath.ToLower();
            string showStyle = "display:'';";
            string highlightClass = "listIn";
            string h2ShowClass = "firstNav";

            if (s.Equals("/jidiaoCenter/TeamList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_1.Attributes["class"] = h2ShowClass;
                ul_1.Attributes["style"] = showStyle;
                a_1.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/jidiaoCenter/DijieList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_1.Attributes["class"] = h2ShowClass;
                ul_1.Attributes["style"] = showStyle;
                a_2.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/jidiaoCenter/TicketList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_1.Attributes["class"] = h2ShowClass;
                ul_1.Attributes["style"] = showStyle;
                a_3.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/jidiaoCenter/VisitList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_1.Attributes["class"] = h2ShowClass;
                ul_1.Attributes["style"] = showStyle;
                a_4.Attributes["class"] = highlightClass;
            }
            //else if (s.Equals("/jidiaoCenter/FankuiList.aspx", StringComparison.OrdinalIgnoreCase))
            //{
            //    h2_1.Attributes["class"] = h2ShowClass;
            //    ul_1.Attributes["style"] = showStyle;
            //    a_5.Attributes["class"] = highlightClass;
            //}
            else if (s.Equals("/jidiaoCenter/RouteList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_1.Attributes["class"] = h2ShowClass;
                ul_1.Attributes["style"] = showStyle;
                a_6.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/jidiaoCenter/TourData.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_1.Attributes["class"] = h2ShowClass;
                ul_1.Attributes["style"] = showStyle;
                a_38.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/CustomerManage/CustomerList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_5.Attributes["class"] = h2ShowClass;
                ul_5.Attributes["style"] = showStyle;
                a_23.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/CustomerManage/CustomerGuanhuai.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_5.Attributes["class"] = h2ShowClass;
                ul_5.Attributes["style"] = showStyle;
                a_22.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/CustomerManage/YuangongList.aspx", StringComparison.OrdinalIgnoreCase) || s.Equals("/CustomerManage/DaoYouList.aspx", StringComparison.OrdinalIgnoreCase) || s.Equals("/CustomerManage/ZutuansheList.aspx", StringComparison.OrdinalIgnoreCase) || s.Equals("/CustomerManage/DijiesheList.aspx", StringComparison.OrdinalIgnoreCase) || s.Equals("/CustomerManage/JingdianList.aspx", StringComparison.OrdinalIgnoreCase) || s.Equals("/CustomerManage/YouKeList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_5.Attributes["class"] = h2ShowClass;
                ul_5.Attributes["style"] = showStyle;
                a_24.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/ResourceManage/TicketList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_2.Attributes["class"] = h2ShowClass;
                ul_2.Attributes["style"] = showStyle;
                a_8.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/ResourceManage/TicketAdd.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_2.Attributes["class"] = h2ShowClass;
                ul_2.Attributes["style"] = showStyle;
                a_8.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/ResourceManage/GroundList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_2.Attributes["class"] = h2ShowClass;
                ul_2.Attributes["style"] = showStyle;
                a_7.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/ResourceManage/HotelList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_2.Attributes["class"] = h2ShowClass;
                ul_2.Attributes["style"] = showStyle;
                a_9.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/ResourceManage/ScenicList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_2.Attributes["class"] = h2ShowClass;
                ul_2.Attributes["style"] = showStyle;
                a_11.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/ResourceManage/DinnerList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_2.Attributes["class"] = h2ShowClass;
                ul_2.Attributes["style"] = showStyle;
                a_10.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/ResourceManage/GuideList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_2.Attributes["class"] = h2ShowClass;
                ul_2.Attributes["style"] = showStyle;
                a_12.Attributes["class"] = highlightClass;
            }

            //财务管理
            else if (s.Equals("/Fin/YingShou.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_3.Attributes["class"] = h2ShowClass;
                ul_3.Attributes["style"] = showStyle;
                a_13.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/Fin/DiJieYingFu.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_3.Attributes["class"] = h2ShowClass;
                ul_3.Attributes["style"] = showStyle;
                a_14.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/Fin/PiaoWuYingFu.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_3.Attributes["class"] = h2ShowClass;
                ul_3.Attributes["style"] = showStyle;
                a_14.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/Fin/OtherIncome.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_3.Attributes["class"] = h2ShowClass;
                ul_3.Attributes["style"] = showStyle;
                a_15.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/Fin/OtherPay.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_3.Attributes["class"] = h2ShowClass;
                ul_3.Attributes["style"] = showStyle;
                a_16.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/Fin/ChuNaDengZhang.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_3.Attributes["class"] = h2ShowClass;
                ul_3.Attributes["style"] = showStyle;
                a_17.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/Fin/Bank.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_3.Attributes["class"] = h2ShowClass;
                ul_3.Attributes["style"] = showStyle;
                a_18.Attributes["class"] = highlightClass;
            }


            //统计中心
            else if (s.Equals("/TongJi/TuanAndSan.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_4.Attributes["class"] = h2ShowClass;
                ul_4.Attributes["style"] = showStyle;
                a_19.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/TongJi/Customer.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_4.Attributes["class"] = h2ShowClass;
                ul_4.Attributes["style"] = showStyle;
                a_20.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/TongJi/SaleArea.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_4.Attributes["class"] = h2ShowClass;
                ul_4.Attributes["style"] = showStyle;
                a_21.Attributes["class"] = highlightClass;
            }

            //客户询价
            else if (s.Equals("/CustomerManage/CustomerQuote.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_6.Attributes["class"] = h2ShowClass;
                ul_6.Attributes["style"] = showStyle;
                a_25.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/CustomerManage/Outreach.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_6.Attributes["class"] = h2ShowClass;
                ul_6.Attributes["style"] = showStyle;
                a_26.Attributes["class"] = highlightClass;
            }

            //公司文件
            else if (s.Equals("/CompanyFiles/MsgManageList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_9.Attributes["class"] = h2ShowClass;
                ul_9.Attributes["style"] = showStyle;
                a_27.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/CompanyFiles/MsgAdd.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_9.Attributes["class"] = h2ShowClass;
                ul_9.Attributes["style"] = showStyle;
                a_27.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/CompanyFiles/NoticeDetail.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_9.Attributes["class"] = h2ShowClass;
                ul_9.Attributes["style"] = showStyle;
                a_27.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/CompanyFiles/FileList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_9.Attributes["class"] = h2ShowClass;
                ul_9.Attributes["style"] = showStyle;
                a_28.Attributes["class"] = highlightClass;
            }


            //系统设置
            else if (s.Equals("/SystemSet/Rating.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_7.Attributes["class"] = h2ShowClass;
                ul_7.Attributes["style"] = showStyle;
                a_29.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SystemSet/CityManage.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_7.Attributes["class"] = h2ShowClass;
                ul_7.Attributes["style"] = showStyle;
                a_29.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SystemSet/LineManage.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_7.Attributes["class"] = h2ShowClass;
                ul_7.Attributes["style"] = showStyle;
                a_29.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SystemSet/YinHangZhangHu.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_7.Attributes["class"] = h2ShowClass;
                ul_7.Attributes["style"] = showStyle;
                a_29.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SystemSet/SaleArea.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_7.Attributes["class"] = h2ShowClass;
                ul_7.Attributes["style"] = showStyle;
                a_29.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SystemSet/CarFlight.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_7.Attributes["class"] = h2ShowClass;
                ul_7.Attributes["style"] = showStyle;
                a_29.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SystemSet/DepartManage.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_7.Attributes["class"] = h2ShowClass;
                ul_7.Attributes["style"] = showStyle;
                a_30.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SystemSet/UserList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_7.Attributes["class"] = h2ShowClass;
                ul_7.Attributes["style"] = showStyle;
                a_30.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SystemSet/RoleList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_7.Attributes["class"] = h2ShowClass;
                ul_7.Attributes["style"] = showStyle;
                a_31.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SystemSet/CompanyInfo.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_7.Attributes["class"] = h2ShowClass;
                ul_7.Attributes["style"] = showStyle;
                a_32.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SystemSet/LoginLog.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_7.Attributes["class"] = h2ShowClass;
                ul_7.Attributes["style"] = showStyle;
                a_33.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SystemSet/OperationLog.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_7.Attributes["class"] = h2ShowClass;
                ul_7.Attributes["style"] = showStyle;
                a_33.Attributes["class"] = highlightClass;
            }
            //短信中心
            else if (s.Equals("/SMS/SendSms.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_8.Attributes["class"] = h2ShowClass;
                ul_8.Attributes["style"] = showStyle;
                a_34.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SMS/SendHistory.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_8.Attributes["class"] = h2ShowClass;
                ul_8.Attributes["style"] = showStyle;
                a_35.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SMS/CommonSms.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_8.Attributes["class"] = h2ShowClass;
                ul_8.Attributes["style"] = showStyle;
                a_36.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SMS/AccountInfo.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_8.Attributes["class"] = h2ShowClass;
                ul_8.Attributes["style"] = showStyle;
                a_37.Attributes["class"] = highlightClass;
            }
            else if (s.Equals("/SMS/CustomerCare.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2_8.Attributes["class"] = h2ShowClass;
                ul_8.Attributes["style"] = showStyle;
                a_39.Attributes["class"] = highlightClass;
            }
            else
            {
                throw new SystemException("请到~\\MasterPage\\Front.Master页InitHighlight()设置高亮显示的位置，3Q。");
            }

        }
        #endregion
    }
}
