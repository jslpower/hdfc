using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL
{
    /// <summary>
    /// 业务逻辑基类
    /// </summary>
    public class BLLBase
    {
        /// <summary>
        /// 将整形Id数组转换为半角逗号分割的字符串
        /// </summary>
        /// <param name="ids">整形Id数组</param>
        /// <returns>半角逗号分割的字符串</returns>
        protected string GetIdsByArr(params int[] ids)
        {

            if (ids == null || ids.Length < 1) return string.Empty;

            var s = new StringBuilder();

            foreach (var item in ids)
            {
                s.Append(",");
                s.Append(item);
            }

            return s.ToString().Substring(1);
        }

        /// <summary>
        /// 将char(36)Id转为可以直接in的格式('***','***','***')
        /// </summary>
        /// <param name="ids">char(36)Id数组</param>
        /// <returns>半角逗号分割的带单引号的字符串</returns>
        protected string GetIdsByArr(params string[] ids)
        {
            if (ids == null || ids.Length < 1) return string.Empty;

            var s = new StringBuilder();
            foreach (var item in ids)
            {
                s.Append(",");
                s.AppendFormat("'{0}'", item);
            }

            return s.ToString().Substring(1);
        }

        /// <summary>
        /// 分页参数验证
        /// </summary>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">面索引</param>
        /// <returns></returns>
        protected bool ValidPaging(int pageSize, int pageIndex)
        {
            if (pageSize <= 0) return false;
            if (pageIndex < 1) return false;

            return true;
        }
    }
}
