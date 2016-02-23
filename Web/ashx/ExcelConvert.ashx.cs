using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Text;
using EyouSoft.Common;
using Newtonsoft.Json;

namespace Web.ashx
{
    using System.Data;
    using System.Data.OleDb;

    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ExcelConvert : IHttpHandler
    {
        /// <summary>
        /// excel连接串 路径用string.Formt(ExcelConn,filePath)替换
        /// </summary>
        private const string ExcelConn =
            "Provider=Microsoft.Ace.OleDb.12.0;" + "data source={0};Extended Properties='Excel 12.0; HDR=YES; IMEX=1'";

        public void ProcessRequest(HttpContext context)
        {
            //if (!CheckLogin(context)) return;

            HandlerFile(context);
        }

        private bool CheckLogin(HttpContext context)
        {
            //需判断 当前会话数据 是否有效
            EyouSoft.Model.SSOStructure.MUserInfo userInfo = null;
            bool isLogin = EyouSoft.Security.Membership.UserProvider.IsLogin(out userInfo);
            if (isLogin == false)
            {
                context.Response.Clear();
                context.Response.StatusCode = 200;

                context.Response.Write("{\"error\":\"请重新登录\"}");
                context.Response.End();
            }

            return isLogin;
        }

        private void HandlerFile(HttpContext context)
        {
            /*-------------------------------------*/
            // Get the file data.
            HttpPostedFile image_upload = context.Request.Files["Filedata"];

            if (image_upload == null || image_upload.ContentLength <= 0)//是否有文件
            {
                context.Response.Clear();
                context.Response.StatusCode = 200;

                context.Response.Write("{\"error\":\"上传的文件为空\"}");
                context.Response.End();
            }
            else if (image_upload.ContentLength > 2097152)//文件大小是否超过了2MB
            {
                context.Response.Clear();
                context.Response.StatusCode = 200;

                context.Response.Write("{\"error\":\"上传的文件超过了指定的大小\"}");
                context.Response.End();
            }
            //判断文件类型 是否是Excel
            string fileExtension = System.IO.Path.GetExtension(image_upload.FileName).ToLower();
            string mime = Utils.GetMimeTypeByFileExtension(fileExtension);
            if (((fileExtension == ".xls" || fileExtension == ".xlsx")
                && (mime == "application/vnd.ms-excel")) || fileExtension == ".txt")//是Excel或txt
            {
                //设置文件名
                var rnd = new Random();
                string saveFileName = DateTime.Now.ToFileTime() + rnd.Next(1000, 99999) + fileExtension;
                string resultArr = null;
                //保存文件
                string dPath = HttpContext.Current.Server.MapPath("/UploadFiles/temp/");
                if (!Directory.Exists(dPath))//如果目录不存在就创建目录
                {
                    Directory.CreateDirectory(dPath);
                }
                string fPath = dPath + saveFileName;//保存路径
                image_upload.SaveAs(fPath);//保存文件
                if (fileExtension == ".txt")//如果是txt文件
                {
                    resultArr = GetTxtTableData(fPath);
                }
                else
                {   //excel文件
                    IList<string> tblNames = this.GetExcelTableName(fPath);
                    if (tblNames != null && tblNames.Count > 0)
                    {
                        resultArr = this.GetExcelTableData(fPath, tblNames[0]);
                    }
                }

                File.Delete(fPath);

                context.Response.Write(resultArr ?? "{\"error\":\"文件中找不到有效内容\"}");
            }
            else//不是Excel文件
            {
                context.Response.Clear();
                context.Response.StatusCode = 200;

                context.Response.Write("{\"error\":\"上传的文件不是有效的Excel文件或Txt文件\"}");
                context.Response.End();
            }
        }

        #region 获取EXCEL表里的数据

        /// <summary>  
        /// 获取EXCEL的表 表名字列   
        /// </summary>  
        /// <param name="fPath">Excel文件</param>  
        /// <returns>数据表</returns>  
        private IList<string> GetExcelTableName(string fPath)
        {
            if (string.IsNullOrEmpty(fPath)) return null;

            IList<string> tblNames = new List<string>();

            try
            {
                if (File.Exists(fPath))
                {
                    var excelConn = new OleDbConnection(string.Format(ExcelConn, fPath));
                    excelConn.Open();
                    DataTable tb = excelConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    excelConn.Close();
                    excelConn.Dispose();

                    if (tb != null)
                    {
                        foreach (DataRow dr in tb.Rows)
                        {
                            tblNames.Add(dr[2].ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                return tblNames;
            }

            return tblNames;
        }

        /// <summary>
        /// 获取EXCEL表里的数据
        /// </summary>
        /// <param name="fPath"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetExcelTableData(string fPath, string tableName)
        {
            if (string.IsNullOrEmpty(fPath) || string.IsNullOrEmpty(tableName)) return string.Empty;

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            var conn = new OleDbConnection(string.Format(ExcelConn, fPath));
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [" + tableName + "]";
            myCommand = new OleDbDataAdapter(strExcel, string.Format(ExcelConn, fPath));
            ds = new DataSet();
            myCommand.Fill(ds, "table1");
            myCommand.Dispose();
            conn.Close();
            conn.Dispose();

            if (ds.Tables.Count == 0 || ds.Tables[0] == null)
            {
                return string.Empty;
            }

            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                {
                    jsonWriter.WriteStartArray();

                    jsonWriter.WriteStartArray();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        jsonWriter.WriteValue(dc.ColumnName);
                    }
                    jsonWriter.WriteEndArray();

                    foreach (DataRow dr in dt.Rows)
                    {
                        jsonWriter.WriteStartArray();
                        int i = 0;
                        for (; i < dr.ItemArray.Length; i++)
                        {
                            jsonWriter.WriteValue(dr[i].ToString());
                        }
                        jsonWriter.WriteEndArray();
                    }


                    jsonWriter.WriteEnd();
                }
            }

            dt.Dispose();

            return sb.ToString();
        }
        #endregion

        #region 获取txt文件里的数据

        /// <summary>
        /// 获取Txt文件里的数据
        /// </summary>
        /// <param name="fPath"></param>
        /// <returns></returns>
        private string GetTxtTableData(string fPath)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            using (var reader = new StreamReader(fPath, Encoding.Default))
            {
                if (reader.Peek() > 0)
                {
                    using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                    {
                        jsonWriter.WriteStartArray();

                        jsonWriter.WriteStartArray();
                        jsonWriter.WriteValue(reader.ReadLine());

                        jsonWriter.WriteEndArray();

                        while (reader.Peek() > 0)
                        {
                            jsonWriter.WriteStartArray();
                            jsonWriter.WriteValue(reader.ReadLine());
                            jsonWriter.WriteEndArray();

                        }
                        jsonWriter.WriteEnd();
                    }
                }
            }

            return sb.ToString();
        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
