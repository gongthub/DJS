using DJS.SDK;
using HydroppowerService.Utils;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroppowerService
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
                HydroppowerService.Service.HydroppowercsService server = new Service.HydroppowercsService();
                server.GetData();

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
                //iConfigMgr.SetConfig("WebApiUrl", "http://192.168.25.20:8012");
                iConfigMgr.SetConfig("WebApiUrl", "http://localhost:17990/");
                iConfigMgr.SetConfig("HyUserName", "Joy");
                iConfigMgr.SetConfig("IDBRepository", "Data Source=192.168.25.20;User Id=SmartHome;Password=Vlinker123;Database=SmartHome;");
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

                iLog.WriteLog(name + "接口调用开始！", 0);

                HydroppowerService.Service.HydroppowercsService server = new Service.HydroppowercsService();

                server.GetData();


                iLog.WriteLog(name + "接口调用结束！", 0);
                DateTime timee = DateTime.Now;
                TimeSpan travelTime = timee - times;
                iLog.WriteLog(name + "接口调用耗时:" + travelTime, 0);

            }
            catch  
            {
                //iLog.WriteLog(name + "接口调用失败！" + ex.Message, 1);
                throw;
            }
        }
    }
}
