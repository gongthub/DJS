using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace DJS.Core.Common
{
    public class WebHelper
    {
        #region HtmlEncode(对html字符串进行编码)
        /// <summary>
        /// 对html字符串进行编码
        /// </summary>
        /// <param name="html">html字符串</param>
        public static string HtmlEncode(string html)
        {
            return HttpUtility.HtmlEncode(html);
        }
        /// <summary>
        /// 对html字符串进行解码
        /// </summary>
        /// <param name="html">html字符串</param>
        public static string HtmlDecode(string html)
        {
            return HttpUtility.HtmlDecode(html);
        }

        #endregion

        #region UrlEncode(对Url进行编码)

        /// <summary>
        /// 对Url进行编码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="isUpper">编码字符是否转成大写,范例,"http://"转成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, bool isUpper = false)
        {
            return UrlEncode(url, Encoding.UTF8, isUpper);
        }

        /// <summary>
        /// 对Url进行编码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="isUpper">编码字符是否转成大写,范例,"http://"转成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, Encoding encoding, bool isUpper = false)
        {
            var result = HttpUtility.UrlEncode(url, encoding);
            if (!isUpper)
                return result;
            return GetUpperEncode(result);
        }

        /// <summary>
        /// 获取大写编码字符串
        /// </summary>
        private static string GetUpperEncode(string encode)
        {
            var result = new StringBuilder();
            int index = int.MinValue;
            for (int i = 0; i < encode.Length; i++)
            {
                string character = encode[i].ToString();
                if (character == "%")
                    index = i;
                if (i - index == 1 || i - index == 2)
                    character = character.ToUpper();
                result.Append(character);
            }
            return result.ToString();
        }

        #endregion

        #region UrlDecode(对Url进行解码)

        /// <summary>
        /// 对Url进行解码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码
        /// </summary>
        /// <param name="url">url</param>
        public static string UrlDecode(string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// 对Url进行解码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字符编码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码</param>
        public static string UrlDecode(string url, Encoding encoding)
        {
            return HttpUtility.UrlDecode(url, encoding);
        }

        #endregion

        #region 获取URL参数 +static List<string> GetUrls(string urlRaw)
        /// <summary>
        /// 获取URL参数
        /// </summary>
        /// <param name="urlRaw"></param>
        /// <returns></returns>
        public static List<string> GetUrls(string urlRaw)
        {
            List<string> strs = new List<string>();
            String[] urlTstrs = urlRaw.Split('/');
            if (urlTstrs != null && urlTstrs.Length > 0)
            {
                foreach (String item in urlTstrs)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        strs.Add(item);
                    }
                }
            }
            return strs;
        }

        #endregion
                
        #region HttpWebRequest(请求网络资源)

        /// <summary>
        /// 请求网络资源,返回响应的文本
        /// </summary>
        /// <param name="url">网络资源地址</param>
        public static string HttpWebRequest(string url)
        {
            return HttpWebRequest(url, string.Empty, Encoding.GetEncoding("utf-8"));
        }

        /// <summary>
        /// 请求网络资源,返回响应的文本
        /// </summary>
        /// <param name="url">网络资源Url地址</param>
        /// <param name="parameters">提交的参数,格式：参数1=参数值1&amp;参数2=参数值2</param>
        public static string HttpWebRequest(string url, string parameters)
        {
            return HttpWebRequest(url, parameters, Encoding.GetEncoding("utf-8"), true);
        }

        /// <summary>
        /// 请求网络资源,返回响应的文本
        /// </summary>
        /// <param name="url">网络资源地址</param>
        /// <param name="parameters">提交的参数,格式：参数1=参数值1&amp;参数2=参数值2</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="isPost">是否Post提交</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="cookie">Cookie容器</param>
        /// <param name="timeout">超时时间</param>
        public static string HttpWebRequest(string url, string parameters, Encoding encoding, bool isPost = false,
             string contentType = "application/x-www-form-urlencoded", CookieContainer cookie = null, int timeout = 120000)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeout;
            request.CookieContainer = cookie;
            if (isPost)
            {
                byte[] postData = encoding.GetBytes(parameters);
                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = postData.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                }
            }
            var response = (HttpWebResponse)request.GetResponse();
            string result;
            using (Stream stream = response.GetResponseStream())
            {
                if (stream == null)
                    return string.Empty;
                using (var reader = new StreamReader(stream, encoding))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        #endregion
        
        #region 格式化文本（防止SQL注入）
        /// <summary>
        /// 格式化文本（防止SQL注入）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Formatstr(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex10 = new System.Text.RegularExpressions.Regex(@"select", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex11 = new System.Text.RegularExpressions.Regex(@"update", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex12 = new System.Text.RegularExpressions.Regex(@"delete", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex10.Replace(html, "s_elect");
            html = regex11.Replace(html, "u_pudate");
            html = regex12.Replace(html, "d_elete");
            html = html.Replace("'", "’");
            html = html.Replace("&nbsp;", " ");
            return html;
        }
        #endregion
    }
}
