using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Webmaster
{
    /// <summary>
    /// 设置系统配置信息
    /// </summary>
    public partial class setting : WebmasterPageBase
    {
        /*string CompanyId = string.Empty;
        string SysId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyId = Utils.GetQueryStringValue("cid");
            SysId = Utils.GetQueryStringValue("sysid");

            if (!IsPostBack)
            {
                InitSysInfo();
                InitSetting();
                InitPrintDocumentTypes();
                InitPrintDefaultSetting();                
            }
        }

        #region private members
        /// <summary>
        /// init sys info
        /// </summary>
        void InitSysInfo()
        {
            var sysInfo = new EyouSoft.BLL.SysStructure.BSys().GetSysInfo(SysId);

            if (sysInfo == null)
            {
                RegisterAlertAndRedirectScript("请求异常。", "systems.aspx");
            }

            ltrSysName.Text = sysInfo.SysName;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitSetting()
        {
            string s = "var setting={0};";
            var setting = new EyouSoft.BLL.ComStructure.BComSetting().GetModel(CompanyId);

            if (setting != null)
            {
                s = string.Format(s, Newtonsoft.Json.JsonConvert.SerializeObject(setting));
            }
            else
            {
                s = string.Format(s, "null");
            }

            RegisterScript(s);
        }

        /// <summary>
        /// 初始化默认的打印单据配置
        /// </summary>
        void InitPrintDefaultSetting()
        {
            IList<EyouSoft.Model.ComStructure.PrintDocument> items = new List<EyouSoft.Model.ComStructure.PrintDocument>();
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.出境游客名单,
                PrintTemplate = "/printpage/xz/youkexinxihuizongdan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.导游任务单,
                PrintTemplate = "/printpage/xz/daoyourenwudan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.地接确认单,
                PrintTemplate = "/printpage/xz/dijieshetongzhidan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.订餐确认单,
                PrintTemplate = "/printpage/xz/dingcantongzhidan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.订单信息汇总表,
                PrintTemplate = "/printpage/xz/dingdanxinxihuizongbiao.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.购物确认单,
                PrintTemplate = "/printpage/xz/gouwutongzhidan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.国内游轮确认单,
                PrintTemplate = "/printpage/xz/guoneiyouluntongzhidan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.核算单,
                PrintTemplate = "/printpage/xz/hesuandan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.火车确认单,
                PrintTemplate = "/printpage/xz/huochepiaodingdan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.机票确认单,
                PrintTemplate = "/printpage/xz/jipiaodingdan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.结算单,
                PrintTemplate = "/printpage/xz/tuanduijiesuandan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.景点确认单,
                PrintTemplate = "/printpage/xz/jingdiantongzhidan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.酒店确认单,
                PrintTemplate = "/printpage/xz/jiudiandingfangdan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.其它安排确认单,
                PrintTemplate = "/printpage/xz/qitaanpaiquerendan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.汽车确认单,
                PrintTemplate = "/printpage/xz/qichepiaodingdan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.散拼行程单,
                PrintTemplate = "/printpage/xz/sanpin.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.涉外游轮确认单,
                PrintTemplate = "/printpage/xz/shewaiyouluntongzhidan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.团队行程单,
                PrintTemplate = "/printpage/xz/zutuan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.用车确认单,
                PrintTemplate = "/printpage/xz/baochekeyunjihuadan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.游客名单,
                PrintTemplate = "/printpage/xz/youkexinxihuizongdan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.车辆预控确认单,
                PrintTemplate = "/printpage/xz/cheliangyukongquerendan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.酒店预控确认单,
                PrintTemplate = "/printpage/xz/jiudianyukongquerendan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.游轮预控确认单,
                PrintTemplate = "/printpage/xz/youchuanyukongquerendan.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.分销商平台订单信息汇总单,
                PrintTemplate = "/printpage/xz/fxs/dingdanxinxi.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.分销商平台散拼线路行程单,
                PrintTemplate = "/printpage/xz/fxs/sanpin.aspx"
            });
            items.Add(new EyouSoft.Model.ComStructure.PrintDocument()
            {
                PrintTemplateType = EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.分销商平台游客信息打印单,
                PrintTemplate = "/printpage/xz/fxs/youkexinxi.aspx"
            });

            string s = "var printDefaultSetting={0};";

            s = string.Format(s, Newtonsoft.Json.JsonConvert.SerializeObject(items));

            RegisterScript(s);
        }

        /// <summary>
        /// 初始化打印单据
        /// </summary>
        void InitPrintDocumentTypes()
        {
            var items = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.ComStructure.PrintTemplateType), new string[] { "0" });

            string s = "var printDocumentTypes={0};";

            if (items != null && items.Count > 0)
            {
                s = string.Format(s, Newtonsoft.Json.JsonConvert.SerializeObject(items));
            }
            else
            {
                s = string.Format(s, "[]");
            }

            RegisterScript(s);
        }

        /// <summary>
        /// 获取打印配置信息集合
        /// </summary>
        /// <returns></returns>
        IList<EyouSoft.Model.ComStructure.PrintDocument> GetPrintSettings()
        {
            var printTypes = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.ComStructure.PrintTemplateType), new string[] { "0" });

            if (printTypes == null || printTypes.Count == 0) return null;

            IList<EyouSoft.Model.ComStructure.PrintDocument> items =new List<EyouSoft.Model.ComStructure.PrintDocument>();

            foreach (var printType in printTypes)
            {
                var item = new EyouSoft.Model.ComStructure.PrintDocument();
                item.PrintTemplateType = Utils.GetEnumValue<EyouSoft.Model.EnumType.ComStructure.PrintTemplateType>(printType.Value, EyouSoft.Model.EnumType.ComStructure.PrintTemplateType.None);
                item.PrintTemplate = Utils.GetFormValue("txt_print_url_" + printType.Value);

                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 获取表单值
        /// </summary>
        EyouSoft.Model.ComStructure.MComSetting GetFormValue()
        {
            var setting = new EyouSoft.BLL.ComStructure.BComSetting().GetModel(CompanyId);
            setting = setting ?? new EyouSoft.Model.ComStructure.MComSetting();

            setting.CompanyId = CompanyId;

            setting.TourNoSetting = Utils.GetFormValue("txtTourCodeRule");
            setting.ShowBeforeMonth = Utils.GetInt(Utils.GetFormValue("txtShowBeforeMonth"), 12);
            setting.ShowAfterMonth = Utils.GetInt(Utils.GetFormValue("txtShowAfterMonth"), 12);
            setting.SaveTime = Utils.GetInt(Utils.GetFormValue("txtSaveTime"), 1440);
            setting.CountryArea = Utils.GetInt(Utils.GetFormValue("txtCountryArea"), 3);
            setting.ProvinceArea = Utils.GetInt(Utils.GetFormValue("txtProvinceArea"), 3);
            setting.ExitArea = Utils.GetInt(Utils.GetFormValue("txtExitArea"), 3);
            setting.IntegralProportion = Utils.GetInt(Utils.GetFormValue("txtIntegralProportion"), 1);
            setting.SkipGuide = Utils.GetFormValue("ckbSkipGuide") == "1";
            setting.SkipSale = Utils.GetFormValue("ckbSkipSale") == "1";
            setting.SkipFinalJudgment = Utils.GetFormValue("ckbSkipFinalJudgment") == "1";
            setting.ContractRemind = Utils.GetInt(Utils.GetFormValue("txtContractRemind"), 15);
            setting.SContractRemind = Utils.GetInt(Utils.GetFormValue("txtSContractRemind"), 15);
            setting.ComPanyContractRemind = Utils.GetInt(Utils.GetFormValue("txtComPanyContractRemind"), 15);
            setting.FinancialExpensesReview = Utils.GetFormValue("ckbFinancialExpensesReview") == "1";
            setting.FinancialIncomeReview = Utils.GetFormValue("ckbFinancialIncomeReview") == "1";
            setting.ArrearsRangeControl = Utils.GetFormValue("ckbArrearsRangeControl") == "1";
            setting.HotelControlRemind = Utils.GetInt(Utils.GetFormValue("txtHotelControlRemind"), 15);
            setting.CarControlRemind = Utils.GetInt(Utils.GetFormValue("txtCarControlRemind"), 15);
            setting.ShipControlRemind = Utils.GetInt(Utils.GetFormValue("txtShipControlRemind"), 15);
            setting.IsEnableKis = Utils.GetFormValue("chkIsEnableKis") == "1";
            setting.MaxUserNumber = Utils.GetInt(Utils.GetFormValue("txtMaxUserNumber"));
            setting.UserLoginLimitType = Utils.GetEnumValue<EyouSoft.Model.EnumType.ComStructure.UserLoginLimitType>(Utils.GetFormValue("radUserLoginLimitType"), EyouSoft.Model.EnumType.ComStructure.UserLoginLimitType.None);
            setting.IsEnableDuanXian = Utils.GetFormValue("chkIsEnableDuanXian") == "1";

            setting.PrintDocument = GetPrintSettings();

            return setting;
        }

        
        #endregion

        #region protected members
        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var setting = GetFormValue();

            if (new EyouSoft.BLL.ComStructure.BComSetting().SetSysSetting(setting))
            {
                RegisterAlertAndReloadScript("系统配置设置成功");
            }
            else
            {
                RegisterAlertAndReloadScript("系统配置设置失败");
            }
        }
        #endregion */
    }
}
