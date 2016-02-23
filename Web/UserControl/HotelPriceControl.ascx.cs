using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Model.SourceStructure;

namespace Web.UserControl
{
    public partial class HotelPriceControl : System.Web.UI.UserControl
    {
        private IList<MHotelRomePrice> _setHotelRoomList;
        /// <summary>
        /// 初始化房型信息
        /// </summary>
        public IList<MHotelRomePrice> SetHotelRoomList
        {
            get { return _setHotelRoomList; }
            set { _setHotelRoomList = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetDataList();
            }
        }

        /// <summary>
        /// 页面初始化时绑定数据
        /// </summary>
        private void SetDataList()
        {
            if (this.SetHotelRoomList != null && this.SetHotelRoomList.Count > 0)
            {
                this.rpList.DataSource = this.SetHotelRoomList;
                this.rpList.DataBind();
                this.phDefault.Visible = false;
            }
            else
            {
                this.phDefault.Visible = true;
            }
        }
        /// <summary>
        /// 获取房型信息
        /// </summary>
        /// <returns></returns>
        public IList<MHotelRomePrice> GetHotelRoomList()
        {
            string[] name = Utils.GetFormValues("Name");
            string[] SellingPrice = Utils.GetFormValues("SellingPrice");
            string[] AccountingPrice = Utils.GetFormValues("AccountingPrice");
            string[] isdinner = Utils.GetFormValues("sltdinner");
            if (name == null || SellingPrice == null || AccountingPrice == null || isdinner == null || name.Length != SellingPrice.Length || SellingPrice.Length != AccountingPrice.Length || AccountingPrice.Length != isdinner.Length)
            {
                return null;
            }
            IList<MHotelRomePrice> list = new List<MHotelRomePrice>();
            MHotelRomePrice model = new MHotelRomePrice();
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] != "")
                {
                    model.Name = name[i].ToString();
                    model.SellingPrice = Utils.GetDecimal(SellingPrice[i].ToString());
                    model.IsBreakfast = Utils.GetInt(isdinner[i]) == 1 ? true : false;
                    model.AccountingPrice = Utils.GetDecimal(AccountingPrice[i].ToString());
                    list.Add(model);
                }
            }
            return list;
        }

    }
}