using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SMSStructure
{
    /// <summary>
    /// 短信中心-短信常用短语列表及常用短语类型数据访问接口
    /// Author xuqh 2011-01-22
    /// </summary>
    public class CommonWords
    {
        private readonly EyouSoft.IDAL.SMSStructure.ICommonWords Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SMSStructure.ICommonWords>();
        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        /// <summary>
        /// 添加常用短语
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddCommonWords(EyouSoft.Model.SMSStructure.CommonWords model)
        {
            if (model == null)
            {
                return false;
            }

            bool result = false;
            result = Dal.AddCommonWords(model);
            handleLogsBll.Add(AddLogs("新增常用短语", result));

            return result;
        }

        /// <summary>
        /// 添加常用短语类型,返回0插入失败,>0时为插入的类型编号
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public int AddCommonWordsClass(EyouSoft.Model.SMSStructure.CommonWordClass model)
        {
            int result = 0;
            result = Dal.AddCommonWordsClass(model);
            handleLogsBll.Add(AddLogs("新增常用短语类型", result == 0 ? false:true));
            return result;
        }

        /// <summary>
        /// 更新常用短语
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCommonWords(EyouSoft.Model.SMSStructure.CommonWords model)
        {
            bool result = false;
            result = Dal.UpdateCommonWords(model);
            handleLogsBll.Add(AddLogs("修改了常用短语", result));
            return result;
        }

        /// <summary>
        /// 删除一条常用短语
        /// </summary>
        /// <param name="Id">常用短语编号</param>
        /// <returns></returns>
        public bool DeleteCommonWords(string[] Ids)
        {
            bool result = false;
            result = Dal.DeleteCommonWords(Ids);
            handleLogsBll.Add(AddLogs("删除了常用短语", result));
            return result;
        }

        /// <summary>
        /// 删除一条常用短语类型
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteCommonWordsClass(int Id)
        {
            bool result = false;
            result = Dal.DeleteCommonWordsClass(Id);
            handleLogsBll.Add(AddLogs("删除了常用短语类型", result));
            return result;
        }

        /// <summary>
        /// 获取一个常用短语实体
        /// </summary>
        /// <param name="Id">常用短语编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SMSStructure.CommonWords GetCommonWords(string Id)
        {
            return Dal.GetCommonWords(Id);
        }

        /// <summary>
        /// 根据公司ID获取所有常用短语类型实体
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
       public IList<EyouSoft.Model.SMSStructure.CommonWordClass> GetCommonWordsClass(int CompanyID)
        {
            return Dal.GetCommonWordsClass(CompanyID);
        }

        /// <summary>
        /// 分页获取常用短语列表
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">开始页码</param>
        /// <param name="RecordCount">总数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="keywords">关键字为空不做查询条件</param>
        /// <param name="ClassId">常用短语类型ID</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SMSStructure.CommonWords> GetList(int pageSize, int pageIndex, ref int RecordCount, int companyId, string keywords, int ClassId)
        {
            return Dal.GetList(pageSize, pageIndex, ref RecordCount, companyId, keywords, ClassId);
        }

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="areaModel">日志操作实体</param>
        /// <param name="actionName">操作名称</param>
        /// <param name="flag">操作状态</param>
        /// <returns></returns>
        private Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, bool flag)
        {
            var model = new Model.CompanyStructure.SysHandleLogs
                {
                    ModuleId = Model.EnumType.PrivsStructure.Privs2.短信中心_短信中心,
                    EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                    EventMessage =
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在"
                        + Model.EnumType.PrivsStructure.Privs2.短信中心_短信中心 + (flag ? actionName : actionName + "失败") + "!",
                    EventTitle =
                        (flag ? actionName + "在" : actionName + "失败在") + Model.EnumType.PrivsStructure.Privs2.短信中心_短信中心
                };

            return model;
        }
    }
}
