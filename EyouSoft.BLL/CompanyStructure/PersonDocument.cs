using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 个人中心-文档管理业务层
    /// </summary>
    /// 鲁功源  2011-01-20
    public class PersonDocument : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.CompanyStructure.IPersonDocument idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.IPersonDocument>();
        /// <summary>
        /// 系统操作日志逻辑
        /// </summary>
        private readonly BLL.CompanyStructure.SysHandleLogs HandleLogsBll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();

        #region PersonDocument 成员
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">文档管理实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.PersonDocument model)
        {
            if (model == null)
                return false;
            bool Result= idal.Add(model);
            if (Result)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.PrivsStructure.Privs2.公司文件_文档管理,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.PrivsStructure.Privs2.公司文件_文档管理.ToString() + "新增了文档！编号为：" + model.DocumentId,
                           EventTitle = "新增" + Model.EnumType.PrivsStructure.Privs2.公司文件_文档管理.ToString() + "数据"
                       });
            }
            return Result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">文档管理实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.PersonDocument model)
        {
            if (model == null)
                return false;
            bool Result=idal.Update(model);
            if (Result)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.PrivsStructure.Privs2.公司文件_文档管理,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.PrivsStructure.Privs2.公司文件_文档管理.ToString() + "修改了文档！编号为：" + model.DocumentId,
                           EventTitle = "修改" + Model.EnumType.PrivsStructure.Privs2.公司文件_文档管理.ToString() + "数据"
                       });
            }
            return Result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params string[] Ids)
        {
            if (Ids == null || Ids.Length == 0)
                return false;
            bool Result= idal.Delete(Ids);
            if (Result)
            {
                string strIDs=string.Empty;
                foreach(var item in Ids)
                {
                    strIDs+=item+",";
                }
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.PrivsStructure.Privs2.公司文件_文档管理,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.PrivsStructure.Privs2.公司文件_文档管理.ToString() + "删除了文档！编号为：" + strIDs.TrimEnd(','),
                           EventTitle = "删除" + Model.EnumType.PrivsStructure.Privs2.公司文件_文档管理.ToString() + "数据"
                       });
            }
            return Result;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns>文档管理实体</returns>
        public EyouSoft.Model.CompanyStructure.PersonDocument GetModel(int Id)
        {
            if (Id <= 0)
                return null;
            return idal.GetModel(Id);
        }
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号 =0返回所有</param>
        /// <param name="OperatorId">上传人编号 =0返回所有</param>
        /// <returns>文档管理列表</returns>
        public IList<EyouSoft.Model.CompanyStructure.PersonDocument> GetList(int pageSize, int pageIndex, ref int RecordCount, int CompanyId, int OperatorId)
        {
            return idal.GetList(pageSize, pageIndex, ref RecordCount, CompanyId, OperatorId);
        }

        #endregion
    }
}
