using HydroppowerService.Services;
using HydroppowerService.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using Vlinker.Common.ApiInvoker;

namespace HydroppowerService.Service
{
    public class HydroppowercsService
    {
        private string apiBaseAddress = ConstUtility.WebApiUrl;

        private string HyUserName = ConstUtility.HyUserName;


        public static DJS.SDK.ILog iLog = null;
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static HydroppowercsService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }

        #endregion


        public void GetData()
        {
            string times = DateTime.Now.ToString("HH:mm");
            iLog.WriteLog("超仪水电接口读取数据开始", 0);

            try
            {
                DateTime StartDate = DateTime.Now.AddDays(-1);
                DateTime EndDate = DateTime.Now;
                HydroppowercsService service = new HydroppowercsService();
                string result = FindReadInfoByDate(StartDate, EndDate);

                GetCompute();
            }
            catch
            {
                throw;
            }

            iLog.WriteLog("超仪水电接口读取数据结束", 0);
        }


        public void GetCompute()
        {
            string times = DateTime.Now.ToString("HH:mm");
            iLog.WriteLog("超仪读取数据计算开始", 0);
            IDBRepository db = new IDBRepository();

            try
            {
                string timeslast = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                string timesthis = DateTime.Now.ToString("yyyyMMdd");

                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext.CommandTimeout = 0;
                db.Database.ExecuteSqlCommand("exec PCompute '" + timeslast + "';");
                db.Database.ExecuteSqlCommand("exec PCompute '" + timesthis + "';");
            }
            catch
            {
                throw;
            }
            iLog.WriteLog("超仪读取数据计算结束", 0);
        }

        /// <summary>
        /// 根据时间段获取数据
        /// </summary>
        /// <param name="meterNo"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public string FindReadInfoByDate(DateTime startDate, DateTime endDate)
        {
            string api = string.Format(@"/Hydropower/FindReadInfoByDate");

            string strJson = @"{'SupplierCode':'" + HyUserName + "','StartTime':'" + startDate.ToString("yyyy-MM-dd") + "','EndTime':'" + endDate.ToString("yyyy-MM-dd") + "'}";

            var requestParam = new Dictionary<string, string>
                {
                    { "jsonData", strJson },
                };

            return WebApiInvokePost(api, requestParam);
        }


        /// <summary>
        /// 根据时间段获取数据
        /// </summary>
        /// <param name="meterNo"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public string FindReadInfoByDateTime(DateTime startDate, DateTime endDate)
        {
            string api = string.Format(@"/Hydropower/FindReadInfoByDateTime");

            string strJson = @"{'SupplierCode':'" + HyUserName + "','StartTime':'" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "','EndTime':'" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'}";

            var requestParam = new Dictionary<string, string>
                {
                    { "jsonData", strJson },
                };

            return WebApiInvokePost(api, requestParam);
        }

        private string WebApiInvokePost(string api, Dictionary<string, string> requestParam)
        {
            string result = "";

            try
            {
                try
                {
                    WebApiInvoker ApiInvoker = new WebApiInvoker(apiBaseAddress);
                    result = ApiInvoker.InvokePostRequestNoToken(api, requestParam).Result;
                }
                catch (HttpResponseException re)
                {
                    if (re.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        result = "{\"success\":false,\"desc\":\"无权限访问\"}";
                    }
                    else
                    {
                        result = "{\"success\":false,\"desc\":\"" + re.Response.Content.ReadAsStringAsync().Result + "\"}";
                    }
                }
            }
            catch (Exception e)
            {
                result = "{\"success\":false,\"desc\":\"" + e.Message + "\"}";
            }

            return result;
        }
    }
}