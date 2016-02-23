<%@ Page Title="" Language="C#" MasterPageFile="~/Webmaster/mpage.Master" AutoEventWireup="true" CodeBehind="setting.aspx.cs" Inherits="Web.Webmaster.setting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripts" runat="server">

    <script type="text/javascript">
        //初始化系统配置
        function InitSetting() {
            if (setting == null) return;
            $("#txtTourCodeRule").val(setting.TourNoSetting);
            $("#txtShowBeforeMonth").val(setting.ShowBeforeMonth);
            $("#txtShowAfterMonth").val(setting.ShowAfterMonth);
            $("#txtSaveTime").val(setting.SaveTime);
            $("#txtCountryArea").val(setting.CountryArea);
            $("#txtProvinceArea").val(setting.ProvinceArea);
            $("#txtExitArea").val(setting.ExitArea);
            $("#txtIntegralProportion").val(setting.IntegralProportion);

            $("#ckbSkipGuide").attr("checked", setting.SkipGuide ? "checked" : "");
            $("#ckbSkipSale").attr("checked", setting.SkipSale ? "checked" : "");
            $("#ckbSkipFinalJudgment").attr("checked", setting.SkipFinalJudgment ? "checked" : "");

            $("#txtContractRemind").val(setting.ContractRemind);
            $("#txtSContractRemind").val(setting.SContractRemind);
            $("#txtComPanyContractRemind").val(setting.ComPanyContractRemind);

            $("#ckbFinancialExpensesReview").attr("checked", setting.FinancialExpensesReview ? "checked" : "");
            $("#ckbFinancialIncomeReview").attr("checked", setting.FinancialIncomeReview ? "checked" : "");
            $("#ckbArrearsRangeControl").attr("checked", setting.ArrearsRangeControl ? "checked" : "");

            $("#txtHotelControlRemind").val(setting.HotelControlRemind);
            $("#txtCarControlRemind").val(setting.CarControlRemind);
            $("#txtShipControlRemind").val(setting.ShipControlRemind);

            $("#chkIsEnableKis").attr("checked", setting.IsEnableKis ? "checked" : "");
            $("#chkIsEnableDuanXian").attr("checked", setting.IsEnableDuanXian ? "checked" : "");

            $("#txtMaxUserNumber").val(setting.MaxUserNumber);
            $("input[name='radUserLoginLimitType']").each(function() {
                if (this.value == setting.UserLoginLimitType) this.checked = true;
            });
        }

        //获取打印单据配置的HTML printType:打印单据类型
        function getPrintDocumentHTML(printType) {
            var s = [];            
            var printUri = '';

            //系统未配置时读取默认的配置
            var printSetting = printDefaultSetting;
            //系统有配置时取系统的配置
            if (setting != null && setting.PrintDocument != null && setting.PrintDocument.length > 0) {
                printSetting = setting.PrintDocument;
            }
            //打印单据配置里查找printType指定的单据类型已经设置的值 
            for (var i = 0; i < printSetting.length; i++) {
                if (printSetting[i].PrintTemplateType == printType) {
                    printUri = printSetting[i].PrintTemplate; break;
                }
            }

            s.push('<span class="unrequired">*</span>打印单据配置：');
            s.push('<select disabled="disabled" name="txt_print_type_' + printType + '">');
            for (var i = 0; i < printDocumentTypes.length; i++) {
                s.push('<option value="' + printDocumentTypes[i].Value + '" ' + (printDocumentTypes[i].Value == printType ? 'selected="selected"' : '') + '>' + printDocumentTypes[i].Text + '</option>');
            }
            s.push('</select>');

            s.push('&nbsp;&nbsp;页面路径：');
            s.push('<input class="input_text" type="text" value="' + printUri + '" style="width: 280px" maxlength="72" name="txt_print_url_' + printType + '">');            
            
            return s.join('');            
        }

        //初始化打印单据配置
        function InitPrintSetting() {
            if (printDocumentTypes.length == 0) return;

            var s = [];
            for (var i = 0; i < printDocumentTypes.length; i++) {
                s.push('<tr>');
                s.push('<td>');
                s.push(getPrintDocumentHTML(printDocumentTypes[i].Value));                
                s.push('</td>');
                s.push('</tr>');
            }

            $("#trPrintDocumentsAfter").before(s.join(""));
        }

        function WebForm_OnSubmit_Validate() {
            return true;
        }

        $(document).ready(function() {
            InitSetting();
            InitPrintSetting();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
    设置系统配置-<asp:Literal runat="server" ID="ltrSysName"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">
    <table cellpadding="2" cellspacing="1" style="font-size: 12px; width: 100%;">
        <tr>
            <td>
                <span class="required">*</span>团号生成规则：<input type="text" id="txtTourCodeRule" name="txtTourCodeRule" class="input_text" maxlength="72" value="35" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>列表显示控制--前X个月数据：
                <input type="text" id="txtShowBeforeMonth" name="txtShowBeforeMonth" maxlength="3" value="12" class="input_text" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>列表显示控制--后X个月数据：
                <input type="text" id="txtShowAfterMonth" name="txtShowAfterMonth" maxlength="3" value="12" class="input_text" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>订单最长留位时间（单位分钟）：
                <input type="text" id="txtSaveTime" name="txtSaveTime" maxlength="10" value="1440" class="input_text" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>国内线--提前X天自动停止收客（单位天）：
                <input type="text" id="txtCountryArea" name="txtCountryArea" maxlength="3" value="3" class="input_text" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>省内线--提前X天自动停止收客（单位天）：
                <input type="text" id="txtProvinceArea" name="txtProvinceArea" maxlength="3" value="3" class="input_text" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>出境线--提前X天自动停止收客（单位天）：
                <input type="text" id="txtExitArea" name="txtExitArea" maxlength="3" value="3" class="input_text" />                
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>个人会员积分比例（单位%）：
                <input type="text" id="txtIntegralProportion" name="txtIntegralProportion" maxlength="10" value="1" class="input_text" />
                
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>是否跳过导游报账：
                <label>
                    <input type="checkbox" id="ckbSkipGuide" name="ckbSkipGuide" value="1" />跳过</label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>是否跳过销售报账：
                <label>
                    <input type="checkbox" id="ckbSkipSale" name="ckbSkipSale" value="1" />跳过</label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>是否跳过计调终审：
                <label>
                    <input type="checkbox" id="ckbSkipFinalJudgment" name="ckbSkipFinalJudgment" value="1" />跳过</label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>劳动合同到期提前X天提醒：
                <input type="text" id="txtContractRemind" name="txtContractRemind" maxlength="3" value="15" class="input_text" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>供应商合同到期提前X天提醒：
                <input type="text" id="txtSContractRemind" name="txtSContractRemind" maxlength="3" value="15" class="input_text" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>公司合同到期提前X天提醒：
                <input type="text" id="txtComPanyContractRemind" name="txtComPanyContractRemind" maxlength="3" value="15" class="input_text" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>财务支出登记是否需要审核：
                <label>
                    <input type="checkbox" id="ckbFinancialExpensesReview" name="ckbFinancialExpensesReview" checked="checked" value="1" />是(财务支出登记需要审核)</label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>财务收入登记是否需要审核：
                <label>
                    <input type="checkbox" id="ckbFinancialIncomeReview" name="ckbFinancialIncomeReview" checked="checked" value="1" />是(财务收入登记需要审核)</label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>是否开启欠款额度控制：
                <label>
                    <input type="checkbox" id="ckbArrearsRangeControl" name="ckbArrearsRangeControl" value="1" checked="checked" />开启</label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>是否开启KIS整合：
                <label>
                    <input type="checkbox" id="chkIsEnableKis" name="chkIsEnableKis" value="1"/>开启(开启后收款审核、付款支付、借款支付、核算结束后才会出现财务入账)</label>
            </td>
        </tr>        
        <tr>   
             <td>
                <span class="unrequired">*</span>是否开启短线：
                <label>
                    <input type="checkbox" id="chkIsEnableDuanXian" name="chkIsEnableDuanXian" value="1"/>开启</label>
            </td>
        </tr>        
        <tr>
            <td>
                <span class="required">*</span>洒店预控到期在最后保留日前N天提醒：
                <input type="text" id="txtHotelControlRemind" name="txtHotelControlRemind" maxlength="3" value="15" class="input_text" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>车辆预控到期在最后保留日前N天提醒：
                <input type="text" id="txtCarControlRemind" name="txtCarControlRemind" maxlength="3" value="15" class="input_text" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>游船预控到期在最后保留日前N天提醒：
                <input type="text" id="txtShipControlRemind" name="txtShipControlRemind" maxlength="3" value="15" class="input_text" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>最大用户数(设置0不做限制)：
                <input type="text" id="txtMaxUserNumber" name="txtMaxUserNumber" maxlength="3" value="0" class="input_text" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>用户登录限制：
                <label><input type="radio" name="radUserLoginLimitType" value="0" />不限制</label>
                <label><input type="radio" name="radUserLoginLimitType" value="1" />最早登录有效</label>
            </td>
        </tr>
        
        <tr id="trPrintDocumentsAfter">
            <td align="right">
                <asp:Button runat="server" ID="btnSave" Text="保存" OnClick="btnSave_Click" OnClientClick="return WebForm_OnSubmit_Validate()" />
                <input type="button" value="返回" onclick="javascript:window.location.href = 'systems.aspx';" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageRemark" runat="server">
</asp:Content>
