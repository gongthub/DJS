using DJS.SDK;
using Quartz;
using RentWarningSMS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RentWarningSMS
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

                iLog.WriteLog(context.JobDetail.Key.Name + "租金到期短信提醒接口调用开始！", 0);
                RentWarningSMS.Service.RentWarningServer server = new Service.RentWarningServer();
                server.DoWork();

                iLog.WriteLog(context.JobDetail.Key.Name + "租金到期提短信醒接口调用结束！", 0);
                DateTime timee = DateTime.Now;
                TimeSpan travelTime = timee - times;
                iLog.WriteLog(context.JobDetail.Key.Name + "租金到期短信提醒接口调用耗时:" + travelTime, 0);

            }
            catch (Exception ex)
            {
                iLog.WriteLog(context.JobDetail.Key.Name + "租金到期短信提醒接口调用失败！" + ex.Message, 1, true);
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
                iConfigMgr.SetConfig("AmsType", "1");
                iConfigMgr.SetConfig("IsAddNotice", "1");
                iConfigMgr.SetConfig("IsSendSMS", "0");
                iConfigMgr.SetConfig("WarningDate", "5/3/1");
                iConfigMgr.SetConfig("WebApiUrl", "http://localhost:17990/");
                iConfigMgr.SetConfig("IDBRepository", "Data Source=192.168.25.20;User Id=SmallAmsSit;Password=1234567890;Database=SmallAms;"); 
            }
            else
            {
                iLog.WriteLog("获取任务配置名称错误！" + jobName, 1, true);
            }
            ConstUtility consts = new ConstUtility();
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

                iLog.WriteLog(name + "租金到期短信提醒接口调用开始！", 0);
                RentWarningSMS.Service.RentWarningServer server = new Service.RentWarningServer();
                server.DoWork();

                iLog.WriteLog(name + "租金到期短信提醒接口调用结束！", 0);
                DateTime timee = DateTime.Now;
                iLog.WriteLog(name + "租金到期短信提醒接口调用耗时:" + (timee - times).Milliseconds, 0);

            }
            catch
            {
                //iLog.WriteLog(name + "接口调用失败！" + ex.Message, 1);
                throw;
            }
        }
    }
}
