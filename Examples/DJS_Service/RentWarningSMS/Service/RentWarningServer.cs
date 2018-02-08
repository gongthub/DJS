using RentWarningSMS.Model;
using RentWarningSMS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using Vlinker.Common.ApiInvoker;

namespace RentWarningSMS.Service
{
    public class RentWarningServer
    {
        private string apiBaseAddress = ConstUtility.WebApiUrl;

        public static DJS.SDK.ILog iLog = null;

        private IDBRepository dbContext = new IDBRepository();

        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static RentWarningServer()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }

        #endregion

        /// <summary>
        /// 处理方法
        /// </summary>
        public void DoWork()
        {
            string times = DateTime.Now.ToString("HH:mm");
            iLog.WriteLog("租金到期短信预警提醒开始", 0);
            try
            {
                int storeType = 0;
                if (Int32.TryParse(ConstUtility.AmsType, out storeType))
                {
                    DateTime StartDate = DateTime.Now;
                    DateTime EndDate = DateTime.Now;
                    List<int> days = GetDays();
                    List<RentExpireView> overdueList = dbContext.RentExpireViews.Where(m => m.ID > 0).OrderBy(t => t.StoreID).ThenBy(t => t.ExpireDay).ToList();
                    foreach (RentExpireView rentex in overdueList)
                    {
                        if (days.Contains(rentex.ExpireDay) || rentex.ExpireDay < 0)
                        {
                            string strs = SendSMSWarning(storeType, rentex.CustomerCode, rentex.Name, rentex.Phone, rentex.FullName, rentex.ExpireDay.ToString(), ConstUtility.IsAddNotice, ConstUtility.IsSendSMS);
                            Result result = Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(strs);
                            if (result.success)
                            {
                                iLog.WriteLog("短信发送成功！门店：" + rentex.StoreName + "；房间：" + rentex.FullName + "；合同号：" + rentex.ContractNo + "；租户：" + rentex.Name + "；手机号：" + rentex.Phone + "；剩余天数：" + rentex.ExpireDay.ToString(), 0);
                            }
                            else
                            {
                                iLog.WriteLog("短信发送失败！门店：" + rentex.StoreName + "；房间：" + rentex.FullName + "；合同号：" + rentex.ContractNo + "；租户：" + rentex.Name + "；手机号：" + rentex.Phone + "；剩余天数：" + rentex.ExpireDay.ToString(), 0);
                            }
                        }
                    }
                }
                else
                {
                    iLog.WriteLog("租金到期短信预警提醒任务失败，AmsType只能为0或者1", 0);
                }
            }
            catch
            {
                throw;
            }

            iLog.WriteLog("租金到期短信预警提醒结束", 0);
        }

        /// <summary>
        /// 处理到期提醒天数
        /// </summary>
        /// <returns></returns>
        private List<int> GetDays()
        {
            List<int> days = new List<int>();

            string strs = ConstUtility.WarningDate;

            string[] arr = strs.Split('/').ToArray();

            for (int i = 0; i < arr.Length; i++)
            {
                int dayT = 0;
                if (Int32.TryParse(arr[i], out dayT))
                {
                    days.Add(dayT);
                }
            }

            return days;
        }


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="StoreType">门店类型</param>
        /// <param name="UserName">姓名</param>
        /// <param name="Phone">手机号</param>
        /// <param name="meterType">表类型</param>
        /// <param name="day">天数</param>
        /// <returns></returns>
        public string SendSMSWarning(int StoreType, string CustomerCode, string UserName, string Phone, string RoomName, string day, string IsAddNotice, string IsSendSMS)
        {
            string api = string.Format(@"/SMS/SendRentWarning");

            string strJson = @"{'StoreType':" + StoreType + ",'CustomerCode':'" + CustomerCode + "','UserName':'" + UserName + "','Phone':'" + Phone + "','RoomName':'" + RoomName + "','days':'" + day + "','IsAddNotice':'" + IsAddNotice + "','IsSendSMS':'" + IsSendSMS + "'}";

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
