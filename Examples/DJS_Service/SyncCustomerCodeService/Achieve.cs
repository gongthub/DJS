using DJS.SDK;
using Quartz;
using SyncCustomerCodeService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncCustomerCodeService
{
    public class Achieve : IJob, IConfigClient
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 
        public static DJS.SDK.IConfigMgr iConfigMgr = null;
        public static DJS.SDK.ILog iLog = null;

        #endregion

        #region 构造函数

        static Achieve()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }

        #endregion

        /// <summary>
        /// 任务执行
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                SetConfig(context.JobDetail.Key.Name);
                DateTime times = DateTime.Now;

                iLog.WriteLog(context.JobDetail.Key.Name + "接口调用开始！", 0);

                string IDBRepositorySRC = iConfigMgr.GetConfig("IDBRepository");
                service.SyncCustomerCodeService.UpdateCustomerCodeAll(IDBRepositorySRC);

                iLog.WriteLog(context.JobDetail.Key.Name + "接口调用结束！", 0);
                DateTime timee = DateTime.Now;
                TimeSpan travelTime = timee - times;
                iLog.WriteLog(context.JobDetail.Key.Name + "接口调用耗时:" + travelTime, 0);

            }
            catch (Exception ex)
            {
                iLog.WriteLog(context.JobDetail.Key.Name + "接口调用失败！" + ex.Message, 1, true);
            }

        }

        /// <summary>
        /// 设置配置信息
        /// </summary>
        /// <param name="jobName"></param>
        public void SetConfig(string jobName)
        {
            string configName = iLog.GetConfigNameByJobName(jobName);
            if (!string.IsNullOrEmpty(configName))
            {
                iConfigMgr = DJS.SDK.DataAccess.CreateIConfigMgr(configName);

                iConfigMgr.SetConfig("IDBRepository", "Data Source=.;User Id=sa;Password=1234567890;Database=AMS;");
                iConfigMgr.SetConfig("OfficialWebServiceURL", "http://192.168.25.12:8020/vldservice.asmx");

                //CRM相关
                iConfigMgr.SetConfig("CRMURL", "http://sitwebapi.vlinker.com.cn:8888/");
                iConfigMgr.SetConfig("ApiUserId", "10001");
                iConfigMgr.SetConfig("Signature", "7c6d977fa475444086f47687f1b8c2ea");
                //获取用户code相应超时时间 单位：毫秒(如果为0则不启用webservice)
                iConfigMgr.SetConfig("GetCustomerCodeTimeOut", "3000");

                //0调用官网接口，1调用CRM接口
                iConfigMgr.SetConfig("CallFrom", "1");
            }
            else
            {
                iLog.WriteLog("获取任务配置名称错误！" + jobName, 1, true);
            }
            ConstUtility conts = new ConstUtility();
        }

        /// <summary>
        /// 任务执行
        /// </summary>
        /// <param name="context"></param>
        public void ExecuteT(string name)
        {
            try
            {
                SetConfig(name);
                DateTime times = DateTime.Now;

                iLog.WriteLog(name + "接口调用开始！", 0);

                string IDBRepositorySRC = iConfigMgr.GetConfig("IDBRepository");
                service.SyncCustomerCodeService.UpdateCustomerCodeAll(IDBRepositorySRC);

                iLog.WriteLog(name + "接口调用结束！", 0);
                DateTime timee = DateTime.Now;
                iLog.WriteLog(name + "接口调用耗时:" + (timee - times).Milliseconds, 0);

            }
            catch (Exception ex)
            {
                iLog.WriteLog(name + "接口调用失败！" + ex.Message, 1);
            }

        }
    }
}
