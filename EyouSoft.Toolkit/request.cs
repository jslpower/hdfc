//汪奇志 2012-12-06
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace EyouSoft.Toolkit
{
    using System.Drawing;
    using System.Net.Mime;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;

    /// <summary>
    /// http request
    /// </summary>
    public class request
    {
        Bitmap m_Bitmap;
        string m_Url, m_Cookies = "";//"TYPE_LTU=admin; ASP.NET_SessionId=poxvt1iumeglbl55yqsqal55; SYS_YIBAI_CID=1; SYS_YIBAI_SID=1; SYS_YIBAI_UID=1; SYS_YIBAI_UN=admin; SYS_YIBAI_SESSIONID=8d6668b5-a60f-4153-86f9-d30e295b4c56; SYS_YIBAI_VC=9714"; 
        int m_BrowserWidth, m_BrowserHeight, m_ThumbnailWidth, m_ThumbnailHeight,m_CompanyId;
        HttpCookieCollection m_Cookie=new HttpCookieCollection();

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref int pcchCookieData, int dwFlags, object lpReserved);

        public request(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight, int CompanyId, HttpCookieCollection Cookie) 
        {
            m_Url = Url; 
            m_BrowserHeight = BrowserHeight; 
            m_BrowserWidth = BrowserWidth;
            m_ThumbnailWidth = ThumbnailWidth;
            m_ThumbnailHeight = ThumbnailHeight;
            m_CompanyId = CompanyId;
            m_Cookie = Cookie;
            m_Cookies = GetCookieString(m_Cookie);
        }
        public static Bitmap GetWebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight, int CompanyId, HttpCookieCollection Cookie)
        {
            request thumbnailGenerator = new request(Url, BrowserWidth, BrowserHeight, ThumbnailWidth, ThumbnailHeight, CompanyId,Cookie);
            return thumbnailGenerator.GenerateWebSiteThumbnailImage(); 
        } 
        public Bitmap GenerateWebSiteThumbnailImage() 
        {
            Thread m_thread = new Thread(new ThreadStart(_GenerateWebSiteThumbnailImage));
            m_thread.SetApartmentState(ApartmentState.STA);
            m_thread.Start();
            m_thread.Join(); 
            //_GenerateWebSiteThumbnailImage();
            return m_Bitmap; 
        }
        private static string GetCookieString(string url)
        {
            // Determine the size of the cookie     
            int datasize = 256;
            StringBuilder cookieData = new StringBuilder(datasize);

            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, null))
            {
                if (datasize < 0)
                    return null;

                // Allocate stringbuilder large enough to hold the cookie     
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, null))
                    return null;
            }
            return cookieData.ToString();
        }
        private static string GetCookieString(HttpCookieCollection c)
        {
            var s = string.Empty;
            if (c != null && c.Count > 0)
            {
                for (var i = 0; i < c.Count; i++)
                {
                    if (c[i].Name == "ASP.NET_SessionId") continue;
                    s += c[i].Name + "=" + c[i].Value + ";";
                }
            }
            return s.TrimEnd(';');
        }  
        private void _GenerateWebSiteThumbnailImage() 
        { 
            WebBrowser m_WebBrowser = new WebBrowser(); 
            m_WebBrowser.ScrollBarsEnabled = false;
            //var arr = GetCookieString(m_Url).Split(';');
            //var arr = m_Cookie.Split(';');
            if (m_Cookie!=null&&m_Cookie.Count>0)
            {
                for (var i = 0; i < m_Cookie.Count; i++)
                {
                    InternetSetCookie(this.m_Url, m_Cookie[i].Name, m_Cookie[i].Value);
                }
            }
            m_WebBrowser.Navigate(m_Url);
            m_WebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
            while (m_WebBrowser.ReadyState != WebBrowserReadyState.Complete)                   
                Application.DoEvents(); 
            m_WebBrowser.Dispose();
        } 
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) 
        {
            WebBrowser m_WebBrowser = (WebBrowser)sender;
            m_WebBrowser.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight); 
            m_WebBrowser.ScrollBarsEnabled = false;
            m_Bitmap = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height); 
            m_WebBrowser.BringToFront(); 
            m_WebBrowser.DrawToBitmap(m_Bitmap, m_WebBrowser.Bounds); 
            m_Bitmap = (Bitmap)m_Bitmap.GetThumbnailImage(m_ThumbnailWidth, m_ThumbnailHeight, null, IntPtr.Zero); 
        }

        public string SavePageAsImg()
        {
            try
            {
                var dir = "/UploadFiles/" + this.m_CompanyId + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/History/";
                var path = Utils.GetMapPath(dir);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = DateTime.Now.ToString("yyyyMMddHHmmssfffff") + new Random().Next(1, 99).ToString() + ".html";
                //Bitmap m_Bitmap = GenerateWebSiteThumbnailImage();
                //MemoryStream ms = new MemoryStream();
                //m_Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);//JPG、GIF、PNG等均可       
                ////byte[] buff = ms.ToArray();       
                ////Response.BinaryWrite(buff);
                //System.Drawing.Image postImage = System.Drawing.Image.FromStream(ms);
                //postImage.Save(path + file, System.Drawing.Imaging.ImageFormat.Png);

                //创建文件信息对象
                var finfo = new FileInfo(path + file);

                //以打开或者写入的形式创建文件流
                using (var fs = finfo.OpenWrite())
                {
                    //根据上面创建的文件流创建写数据流
                    var sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("GB2312"));

                    //把新的内容写到创建的HTML页面中
                    sw.WriteLine((this.create(m_Url, string.Empty, Method.GET, string.Empty, ref m_Cookies, false)));
                    sw.Flush();
                    sw.Close();
                }

                return dir + file;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 过滤html
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string FiltrationHTML(string html)
        {
            //删除脚本            
            html = Regex.Replace(html, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);         
            //删除HTML            
            //html = Regex.Replace(html, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);         
            //html = Regex.Replace(html, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);         
            //html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);         
            //html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);           
            //html = Regex.Replace(html, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);         
            //html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);         
            //html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);         
            //html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);         
            //html = Regex.Replace(html, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);         
            //html = Regex.Replace(html, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);         
            //html = Regex.Replace(html, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);         
            //html = Regex.Replace(html, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);         
            //html = Regex.Replace(html, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);         
            //html = Regex.Replace(html, @"&#(/d+);", "", RegexOptions.IgnoreCase);           
            //html.Replace("<", "");         
            //html.Replace(">", "");         
            //html.Replace("/r/n", "");         
            //html = HttpContext.Current.Server.HtmlEncode(html).Trim();
            return html; 
        }

        /// <summary>
        /// create request
        /// </summary>
        /// <param name="requestUriString">request uri string</param>
        /// <param name="referer">referer</param>
        /// <param name="method">method</param>
        /// <param name="data">request query</param>
        /// <param name="cookies">request cookies</param>
        /// <param name="keepAlive">is keepalive</param>
        /// <returns></returns>
        public string create(
            string requestUriString, string referer, Method method, string data, ref string cookies, bool keepAlive)
        {
            //requestUriString = "http://www.baidu.com";
            StringBuilder responseText = new StringBuilder();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
            request.Timeout = 300000;
            request.Method = method.ToString();
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.KeepAlive = keepAlive;
            request.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:8.0) Gecko/20100101 Firefox/8.0";
            request.Accept = "*/*";
            request.Referer = referer;
            request.Headers.Set("Cookie", cookies);

            Encoding encode = System.Text.Encoding.UTF8;

            if (method == Method.POST && !string.IsNullOrEmpty(data))
            {
                byte[] bytes = encode.GetBytes(data);
                request.ContentLength = bytes.Length;

                Stream oStreamOut = request.GetRequestStream();
                oStreamOut.Write(bytes, 0, bytes.Length);
                oStreamOut.Close();
            }

            HttpWebResponse response = null;

            try
            {
                int i = 1;
                while (i > 0)
                {
                    response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK) break;
                    else response = null;
                    i--;
                }
            }
            catch
            {
                response = null;
            }

            if (response != null)
            {
                try
                {
                    string rcookies = response.Headers["Set-Cookie"];

                    if (!string.IsNullOrEmpty(rcookies))
                    {
                        StringBuilder sb = new StringBuilder();
                        string[] arr = rcookies.Split(';');
                        foreach (string s in arr)
                        {
                            if (string.IsNullOrEmpty(s) || s.ToLower().IndexOf("domain=") > -1
                                || s.ToLower().IndexOf("path=") > -1 || s.ToLower().IndexOf("expires=") > -1) continue;

                            sb.Append(s.Trim(','));
                            sb.Append(";");
                        }

                        cookies += sb.ToString();
                    }

                    Stream resStream = null;
                    resStream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(resStream, encode);

                    Char[] read = new Char[256];
                    int count = readStream.Read(read, 0, 256);
                    while (count > 0)
                    {
                        string s = new String(read, 0, count);
                        responseText.Append(s);
                        count = readStream.Read(read, 0, 256);
                    }

                    resStream.Close();
                }
                catch
                {
                }
            }

            return responseText.ToString();

        }

        /// <summary>
        /// http request method
        /// </summary>
        public enum Method
        {
            GET,

            POST
        }
    }
}
