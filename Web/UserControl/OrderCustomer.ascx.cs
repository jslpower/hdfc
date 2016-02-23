using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using EyouSoft.Common;
using System.ComponentModel;

namespace Web.UserControl
{
    /// <summary>
    /// 订单游客控件
    /// </summary>
    public partial class OrderCustomer : System.Web.UI.UserControl
    {
        /// <summary>
        /// 获取或者设置游客集合
        /// </summary>
        public IList<EyouSoft.Model.PlanStructure.MTraveller> CustomerList { get; set; }

        /// <summary>
        /// 游客是否可以编辑(修改、删除；默认 false 根据游客状态控制游客是否可以被修改和删除；赋值true后 不控制游客是否可编辑)
        /// </summary>
        [Bindable(true)]
        public bool IsEditOrderCustomer { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                CustomerList = GetCustomerList();
            }
            else
            {
                InitControl();
            }
        }

        #region 获取表单中的游客信息

        /// <summary>
        /// 获取表单中的游客信息
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.MTraveller> GetCustomerList()
        {
            string[] name = Utils.GetFormValues("txt_OrderCustomer_Name");
            string[] type = Utils.GetFormValues("ddl_OrderCustomer_CustomerType");
            string[] card = Utils.GetFormValues("ddl_OrderCustomer_CustomerCard");
            string[] cardNo = Utils.GetFormValues("txt_OrderCustomer_CardNo");
            string[] tel = Utils.GetFormValues("txt_OrderCustomer_Tel");
            string[] sex = Utils.GetFormValues("ddl_OrderCustomer_CustomerSex");
            string[] id = Utils.GetFormValues("hid_OrderCustomer_CustomerId");

            if (id == null || name == null || type == null || card == null || cardNo == null || tel == null || sex == null
                || id.Length != name.Length || name.Length != type.Length || type.Length != card.Length
                || card.Length != cardNo.Length || cardNo.Length != tel.Length || tel.Length != sex.Length)
            {
                return null;
            }

            var list = new List<EyouSoft.Model.PlanStructure.MTraveller>();
            for (int i = 0; i < name.Length; i++)
            {
                //没有填写名称，整行数据不保存
                if (string.IsNullOrEmpty(name[i])) continue;

                list.Add(new EyouSoft.Model.PlanStructure.MTraveller
                    {
                        TravellerId = id[i],
                        TravellerName = name[i],
                        TravellerType = (EyouSoft.Model.EnumType.TourStructure.TravellerType)Utils.GetInt(type[i]),
                        CardType = (EyouSoft.Model.EnumType.TourStructure.CardType)Utils.GetInt(card[i]),
                        CardNumber = cardNo[i],
                        Sex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Utils.GetInt(sex[i]),
                        Contact = tel[i],
                        Brithday = GetBirthday((EyouSoft.Model.EnumType.TourStructure.CardType)Utils.GetInt(card[i]), cardNo[i])

                    });
            }

            return list;
        }

        private DateTime? GetBirthday(EyouSoft.Model.EnumType.TourStructure.CardType type, string cardNo)
        {
            DateTime? birthday = null;
            if (type == EyouSoft.Model.EnumType.TourStructure.CardType.身份证)
            {
                if (cardNo.Length != 18 && cardNo.Length != 15)
                    return birthday;
                if (cardNo.Length == 18)
                {
                    birthday =
                        Utils.GetDateTimeNullable(
                            cardNo.Substring(6, 4) + "-" + cardNo.Substring(10, 2) + "-" + cardNo.Substring(12, 2));
                }
                if (cardNo.Length == 15)
                {
                    birthday =
                        Utils.GetDateTimeNullable(
                            "19" + cardNo.Substring(6, 2) + "-" + cardNo.Substring(8, 2) + "-" + cardNo.Substring(10, 2));
                }
            }
            return birthday;

        }

        #endregion

        /// <summary>
        /// 初始化游客信息
        /// </summary>
        private void InitControl()
        {
            if (CustomerList == null || !CustomerList.Any()) plnAdd.Visible = true;
            else
            {
                plnAdd.Visible = false;

                rptCustomer.DataSource = CustomerList;
                rptCustomer.DataBind();
            }
        }

        #region 前台方法

        /// <summary>
        /// 获取游客是否可编辑
        /// </summary>
        /// <param name="ticketStatus">游客机票状态</param>
        /// <returns></returns>
        protected bool GetCustomerIsEdit(int ticketStatus)
        {
            if (!IsEditOrderCustomer)
            {
                if (ticketStatus > 0)
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 生成游客类型下拉框
        /// </summary>
        /// <param name="id">要选中的项的值</param>
        /// <returns></returns>
        protected string GetCustomerType(object id)
        {

            var strHtml = new StringBuilder();
            string strValue = ((int)EyouSoft.Model.EnumType.TourStructure.TravellerType.成人).ToString();
            if (id != null && !string.IsNullOrEmpty(id.ToString())) strValue = id.ToString();
            strHtml.AppendFormat(
                " <select class=\"inputselect\" name=\"ddl_OrderCustomer_CustomerType\" id=\"ddl_{0}\" > ",
                new Random().Next(1000, 99999));
            var list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.TravellerType));
            foreach (var t in list)
            {
                if (t == null || string.IsNullOrEmpty(t.Value) || string.IsNullOrEmpty(t.Text)) continue;

                strHtml.AppendFormat(
                    " <option value=\"{0}\" {2}>{1}</option> ",
                    t.Value,
                    t.Text,
                    t.Value == strValue ? "selected=\"selected\"" : string.Empty);

            }
            strHtml.Append(" </select> ");

            return strHtml.ToString();
        }

        /// <summary>
        /// 生成游客证件类型下拉框
        /// </summary>
        /// <param name="id">要选中的项的值</param>
        /// <returns></returns>
        protected string GetCustomerCard(object id)
        {
            var strHtml = new StringBuilder();
            string strValue = ((int)EyouSoft.Model.EnumType.TourStructure.CardType.身份证).ToString();
            if (id != null && !string.IsNullOrEmpty(id.ToString())) strValue = id.ToString();
            strHtml.AppendFormat(
                " <select class=\"inputselect\" name=\"ddl_OrderCustomer_CustomerCard\" id=\"ddl_{0}\" > ",
                new Random().Next(1000, 99999));
            var list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.CardType));
            foreach (var t in list)
            {
                if (t == null || string.IsNullOrEmpty(t.Value) || string.IsNullOrEmpty(t.Text)) continue;

                strHtml.AppendFormat(
                    " <option value=\"{0}\" {2}>{1}</option> ",
                    t.Value,
                    t.Text,
                    t.Value == strValue ? "selected=\"selected\"" : string.Empty);
            }
            strHtml.Append(" </select> ");

            return strHtml.ToString();
        }

        /// <summary>
        /// 生成性别下拉框
        /// </summary>
        /// <param name="id">要选中的项的值</param>
        /// <returns></returns>
        protected string GetCustomerSex(object id)
        {
            var strHtml = new StringBuilder();
            string strValue = ((int)EyouSoft.Model.EnumType.CompanyStructure.Sex.男).ToString();
            if (id != null && !string.IsNullOrEmpty(id.ToString())) strValue = id.ToString();
            strHtml.AppendFormat(" <select class=\"inputselect\" name=\"ddl_OrderCustomer_CustomerSex\" id=\"ddl_{0}\" > ",
                new Random().Next(1000, 99999));
            var list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.CompanyStructure.Sex));
            foreach (var t in list)
            {
                if (t == null || string.IsNullOrEmpty(t.Value) || string.IsNullOrEmpty(t.Text)) continue;

                strHtml.AppendFormat(
                    " <option value=\"{0}\" {2}>{1}</option> ",
                    t.Value,
                    t.Text,
                    t.Value == strValue ? "selected=\"selected\"" : string.Empty);
            }
            strHtml.Append(" </select> ");

            return strHtml.ToString();
        }
        #endregion
    }
}