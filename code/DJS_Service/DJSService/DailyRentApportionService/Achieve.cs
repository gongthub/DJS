using DailyRentApportionService.Services;
using DailyRentApportionService.Utils;
using DJS.SDK;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyRentApportionService
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

                RentDailyApportionService
                    RentDailyApportionService = new RentDailyApportionService();
                RentDailyApportionService.ApportionService();

                iLog.WriteLog(context.JobDetail.Key.Name + "接口调用结束！", 0);
                DateTime timee = DateTime.Now;
                iLog.WriteLog(context.JobDetail.Key.Name + "接口调用耗时:" + (timee - times).Milliseconds, 0);

            }
            catch (Exception ex)
            {
                iLog.WriteLog(context.JobDetail.Key.Name + "接口调用失败！" + ex.Message, 1);
            }
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

                RentDailyApportionService
                    RentDailyApportionService = new RentDailyApportionService();
                RentDailyApportionService.ApportionService();

                iLog.WriteLog(name + "接口调用结束！", 0);
                DateTime timee = DateTime.Now;
                iLog.WriteLog(name + "接口调用耗时:" + (timee - times).Milliseconds, 0);

            }
            catch (Exception ex)
            {
                iLog.WriteLog(name + "接口调用失败！" + ex.Message, 1);
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

                string status = iConfigMgr.GetConfig("STATUS");

                if (string.IsNullOrEmpty(status) || status == "true")
                {
                    iConfigMgr.SetConfig("STATUS", "true");
                    iConfigMgr.SetConfig("ExecuteStartDate", "2015-01-01");
                    iConfigMgr.SetConfig("ReCalculationDays", "30");
                    iConfigMgr.SetConfig("EFDbContext", "Data Source=.;User Id=sa;Password=1234567890;Database=AMS;");

                }
            }
            else
            {
                iLog.WriteLog("获取任务配置名称错误！" + jobName, 1);
            }
        }
    }
}
