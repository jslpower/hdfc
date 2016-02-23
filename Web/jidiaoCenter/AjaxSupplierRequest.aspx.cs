using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Page;
using System.Text;
using System.Collections;
using EyouSoft.Model.SourceStructure;

namespace Web.jidiaoCenter
{
    public partial class AjaxSupplierRequest : BackPage
    {
        protected int pageSize = 24;
        protected int pageIndex = 0;
        protected int recordCount = 0;
        protected int listCount = 0;
        protected string NodataMsg = string.Empty;
        protected int listcount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string dotype = Utils.GetQueryStringValue("dotype").ToLower();
            string type = Utils.GetQueryStringValue("type");
            if (dotype == "save")
            {
                Response.Clear();
                Response.Write(PageSave(type));
                Response.End();
            }
            string isadd = Utils.GetQueryStringValue("isadd");
            if (isadd == "0")
            {
                this.formUnit.Visible = false;
            }
            if (!IsPostBack)
            {
                //名称
                string Name = Utils.GetQueryStringValue("name");
                if (Name.Trim() != "")
                {
                    ListDataInit(Name);
                }
                else
                {
                    this.formUnit.Visible = false;
                }
            }

        }
        #region 初始化列表
        /// <summary>
        /// 列表数据初始化
        /// </summary>
        /// <param name="searchModel"></param>
        private void ListDataInit(string name)
        {

            //供应商选用类别
            string type = Utils.GetQueryStringValue("type");

            switch (type.ToLower())
            {
                case "guide"://导游
                    if (name != "defaultname")
                    {
                        RptDateList.Visible = false;
                        EyouSoft.BLL.SourceStructure.BGuideSupplier bllguide = new EyouSoft.BLL.SourceStructure.BGuideSupplier();
                        IList<EyouSoft.Model.SourceStructure.MGuideSupplier> list = bllguide.GetList(SiteUserInfo.CompanyId, name);
                        if (list != null && list.Count > 0)
                        {
                            this.listcount = list.Count;
                            RptguideList.DataSource = list;
                            RptguideList.DataBind();
                            this.formUnit.Visible = false;
                        }
                        else
                        {
                            lbemptymsg.Text = "<tr class='old'><td colspan='4' align='center'>没有相关数据</td></tr>";
                            this.Tabform.Visible = false;
                            BindStar();
                        }
                    }
                    else
                    {
                        this.formUnit.Visible = false;
                    }

                    break;
                case "ground"://地接社
                    this.RptguideList.Visible = false;
                    bool isdefaultname = false;
                    if (name == "defaultname")
                    {
                        name = "";
                        isdefaultname = true;
                    }
                    EyouSoft.BLL.SourceStructure.BSupplier bllground = new EyouSoft.BLL.SourceStructure.BSupplier();
                    IList<EyouSoft.Model.SourceStructure.MSupplier> locallist = bllground.GetGroundList(this.SiteUserInfo.CompanyId, name);
                    if (locallist != null && locallist.Count > 0)
                    {
                        this.listcount = locallist.Count;
                        if (isdefaultname && listcount > 20)
                        {

                        }
                        else
                        {
                            RptDateList.DataSource = locallist;
                            RptDateList.DataBind();
                        }
                        this.formUnit.Visible = false;
                    }
                    else
                    {
                        lbemptymsg.Text = "<tr class='old'><td colspan='4' align='center'>没有相关数据</td></tr>";
                        this.TabGuide.Visible = false;
                    }
                    break;
                case "ticket":
                    bool isdefault = false;
                    if (name == "defaultname")
                    {
                        name = "";
                        isdefault = true;
                    }
                    EyouSoft.BLL.SourceStructure.BSupplier bllticket = new EyouSoft.BLL.SourceStructure.BSupplier();
                    IList<EyouSoft.Model.SourceStructure.MSupplier> listticket = bllticket.GetList(SiteUserInfo.CompanyId, name, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务);
                    if (listticket != null && listticket.Count > 0)
                    {
                        listcount = listticket.Count;
                        if (isdefault && listcount > 20)
                        {

                        }
                        else
                        {
                            RptDateList.DataSource = listticket;
                            this.RptDateList.DataBind();
                        }
                        this.formUnit.Visible = false;
                    }
                    else
                    {
                        lbemptymsg.Text = "<tr class='old'><td colspan='4' align='center'>没有相关数据</td></tr>";
                        this.TabGuide.Visible = false;
                    }
                    break;
            }

        }
        #endregion

        private string PageSave(string type)
        {
            int result = 0;
            string msg = string.Empty;
            switch (type)
            {
                case "地接":
                case "票务":
                    EyouSoft.BLL.SourceStructure.BSupplier bllground = new EyouSoft.BLL.SourceStructure.BSupplier();
                    MSupplier model = new MSupplier();
                    model.CityId = Utils.GetInt(Utils.GetFormValue(this.ddlCity.UniqueID));
                    model.ProvinceId = Utils.GetInt(Utils.GetFormValue(this.ddlProvice.UniqueID));
                    model.CompanyId = SiteUserInfo.CompanyId;
                    model.IssueTime = DateTime.Now;
                    model.SupplierType = ((type == "地接") ? EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接 : EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务);
                    model.UnitName = Utils.GetFormValue(this.txtunitname.UniqueID);
                    model.ContactList = getcontactlist();
                    model.OperatorId = SiteUserInfo.UserId;

                    result = bllground.Add(model);
                    if (result == 1)
                    {
                        msg = UtilsCommons.AjaxReturnJson("1", "添加成功", model);
                    }
                    else
                    {
                        msg = UtilsCommons.AjaxReturnJson("0", "添加失败");
                    }
                    break;
                case "导游":
                    EyouSoft.BLL.SourceStructure.BGuideSupplier bllguide = new EyouSoft.BLL.SourceStructure.BGuideSupplier();
                    MGuideSupplier modelguide = new MGuideSupplier();
                    modelguide.GysName = Utils.GetFormValue(this.txtgysname.UniqueID);
                    modelguide.CityId = Utils.GetInt(Utils.GetFormValue(this.ddlCity_guide.UniqueID));
                    modelguide.ProvinceId = Utils.GetInt(Utils.GetFormValue(this.ddlProvice_guide.UniqueID));
                    modelguide.OperatorId = SiteUserInfo.UserId;
                    modelguide.CompanyId = SiteUserInfo.CompanyId;
                    modelguide.GuideName = Utils.GetFormValue(this.txtguidename.UniqueID);
                    modelguide.TourTime = "";
                    modelguide.Phone = Utils.GetFormValue(this.txtcontacttel_guide.UniqueID);
                    modelguide.GuideStar = (EyouSoft.Model.EnumType.SourceStructure.GuideStar)Utils.GetInt(Utils.GetFormValue(ddlStar.UniqueID));
                    result = bllguide.Add(modelguide);
                    if (result == 1)
                    {
                        msg = UtilsCommons.AjaxReturnJson("1", "添加成功", modelguide);
                    }
                    else
                    {
                        msg = UtilsCommons.AjaxReturnJson("0", "添加失败");
                    }
                    break;
                default:
                    break;
            }
            return msg;
        }

        private void BindStar()
        {
            //绑定导游星级
            Array values = Enum.GetValues(typeof(EyouSoft.Model.EnumType.SourceStructure.GuideStar));
            foreach (var item in values)
            {
                int value = (int)Enum.Parse(typeof(EyouSoft.Model.EnumType.SourceStructure.GuideStar), item.ToString());
                string text = Enum.GetName(typeof(EyouSoft.Model.EnumType.SourceStructure.GuideStar), item);
                ListItem ddlItem = new ListItem();
                ddlItem.Text = text;
                ddlItem.Value = value.ToString();
                this.ddlStar.Items.Add(ddlItem);
            }
            this.ddlStar.Items.Insert(0, new ListItem("--请选择--", ""));
        }

        /// <summary>
        /// 获取联系人信息
        /// </summary>
        /// <returns></returns>
        private IList<MSupplierContact> getcontactlist()
        {
            string contactname = Utils.GetFormValue(this.txtcontactname.UniqueID);
            string contacttel = Utils.GetFormValue(this.txtcontacttel.UniqueID);
            IList<MSupplierContact> list = new List<MSupplierContact>();
            if (contactname != "" || contacttel != "")
            {
                list.Add(new MSupplierContact { ContactName = contactname, ContactMobile = contacttel });
            }
            return list;
        }


        /// <summary>
        /// 获取联系人信息
        /// </summary>
        /// <param name="linkManlist"></param>
        /// <returns></returns>
        protected string GetContactInfo(object linkManlist, string type)
        {
            StringBuilder stb = new System.Text.StringBuilder();
            IList<MSupplierContact> list = (IList<MSupplierContact>)linkManlist;
            switch (type)
            {
                case "name":
                    if (list != null && list.Count > 0)
                    {
                        stb.Append(list[0].ContactName);
                    }
                    break;
                case "tel":
                    if (list != null && list.Count > 0)
                    {
                        stb.Append(string.IsNullOrEmpty(list[0].ContactTel) ? list[0].ContactMobile : list[0].ContactTel);
                    }
                    break;
                case "fax":
                    if (list != null && list.Count > 0)
                    {
                        stb.Append(list[0].ContactFax);
                    }
                    break;
            }
            return stb.ToString();
        }
    }
}
