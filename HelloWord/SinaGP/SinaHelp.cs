using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SinaGP
{
    public class SinaHelp : IJob
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>
        private static DJS.SDK.ILog iLog = null;
        private static DJS.SDK.IConfigMgr iConfigMgr = null;

        #endregion

        #region 构造函数

        static SinaHelp()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            iConfigMgr = DJS.SDK.DataAccess.CreateIConfigMgr();
            iConfigMgr.SetConfig("TestConfig","test");
        }

        #endregion
        /// <summary>
        /// 新浪接口url
        /// </summary>
        private static string URL = ConfigurationManager.AppSettings["SinaUrl"].ToString();

        /// <summary>
        /// 参数
        /// </summary>
        private static string POSTDATASTR = ConfigurationManager.AppSettings["SinaPram"].ToString();

        /// <summary>
        /// 数据保存路径
        /// </summary>
        private static string DATAPATH = System.IO.Path.GetFullPath(ConfigurationManager.AppSettings["DataPath"].ToString());
        /// <summary>
        /// 数据保存文件名称
        /// </summary>
        private static string DATANAME = ConfigurationManager.AppSettings["DataName"].ToString();

        CookieContainer cookie = new CookieContainer();
        private string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=GBK";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("GBK"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        #region 接口实现方法
        /// <summary>
        /// 接口实现方法
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                //System.Threading.Thread.Sleep(3000);
                string strs = HttpGet(URL, POSTDATASTR);
                Regex regex = new Regex("\"[^\"]*\"");
                strs = regex.Match(strs).Value.Replace("\"", "");  
                string namespaces = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;
                iLog.WirteDatas(namespaces, strs);
                iLog.WriteLog("新浪HB接口调用成功！", 0);
            }
            catch (Exception ex)
            {
                iLog.WriteLog("新浪HB接口调用失败！" + ex.Message, 1);
            }
        }
        #endregion

        public void Test()
        {
            string strs = HttpGet(URL, POSTDATASTR);
            Regex regex = new Regex("\"[^\"]*\"");
            string result = regex.Match(strs).Value.Replace("\"", "");

            //if (!DJS.Common.FileHelp.DirectoryIsExists(DATAPATH))
            //{
            //    DJS.Common.FileHelp.CreateDirectory(DATAPATH);
            //}
            //DJS.Common.FileHelp.WirteStr(DATAPATH + DATANAME, strs);
        }
    }
}
