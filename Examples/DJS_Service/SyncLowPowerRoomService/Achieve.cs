using DJS.SDK;
using Quartz;
using SyncLowPowerRoomService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLowPowerRoomService
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
                SyncLowPowerRoomService.Service.SyncLowPowerRoomService server = new Service.SyncLowPowerRoomService();
                server.GetLowPowerRooms();

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
                iConfigMgr.SetConfig("WebApiUrl", "http://localhost:17990/");
                iConfigMgr.SetConfig("PartnerId", "566a3bdf707264056d000001");
                iConfigMgr.SetConfig("Electric", "100");
                iConfigMgr.SetConfig("SystemType", "1");     //0大V  1小v
                iConfigMgr.SetConfig("ElectricWarn", "25");  //电量下降警戒值
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

                iLog.WriteLog(name + "接口调用开始！", 0);

                SyncLowPowerRoomService.Service.SyncLowPowerRoomService server = new Service.SyncLowPowerRoomService();
                server.GetLowPowerRooms();

                iLog.WriteLog(name + "接口调用结束！", 0);
                DateTime timee = DateTime.Now;
                iLog.WriteLog(name + "接口调用耗时:" + (timee - times).Milliseconds, 0);

            }
            catch 
            {
                //iLog.WriteLog(name + "接口调用失败！" + ex.Message, 1);
                throw;
            }
        }
    }
}
