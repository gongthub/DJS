using DJS.SDK;
using PMPProjectSummary.Service;
using PMPProjectSummary.Utils;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPProjectSummary
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

        public void Execute(IJobExecutionContext context)
        {
            SetConfig(context.JobDetail.Key.Name);

            DateTime times = DateTime.Now;

            iLog.WriteLog(context.JobDetail.Key.Name + "接口调用开始！", 0);
            
            ProjectSummarySer.SendDayTrialEmail();

            iLog.WriteLog(context.JobDetail.Key.Name + "接口调用结束！", 0);
            DateTime timee = DateTime.Now;
            TimeSpan travelTime = timee - times;
            iLog.WriteLog(context.JobDetail.Key.Name + "接口调用耗时:" + travelTime, 0);
        }

        public void SetConfig(string jobName)
        {
            string configName = iLog.GetConfigNameByJobName(jobName);
            if (!string.IsNullOrEmpty(configName))
            {
                iConfigMgr = DJS.SDK.DataAccess.CreateIConfigMgr(configName);

                iConfigMgr.SetConfig("ServiceCode", "PMP001");
                iConfigMgr.SetConfig("SendMailSender", "helpdesk@vlinker.com.cn");
                iConfigMgr.SetConfig("SendMailPassword", "Vlinker1");
                iConfigMgr.SetConfig("EmailSMTP", "smtp.exmail.qq.com");  

                iConfigMgr.SetConfig("EmailTitle", "PMP项目汇总"); //邮件的标题

                iConfigMgr.SetConfig("IDBRepository", "Data Source=192.168.20.15;User Id=PMP;Password=Vlinker123;Database=PMP;");

                iConfigMgr.SetConfig("IDBAMSRepository", "Data Source=192.168.20.15;User Id=ams;Password=amsVlinker123;Database=AMS;");
                //iConfigMgr.SetConfig("IDBAMSRepository", "Data Source=192.168.25.30;User Id= amssit;Password=amssitVlinker123;Database=AMS_SIT;");
            }
            else
            {
                iLog.WriteLog("获取任务配置名称错误！" + jobName, 1, true);
            }
            ConstUtility consts = new ConstUtility();
        }


        public void ExecuteT(string name)
        {

            try
            {

                SetConfig(name);

                DateTime times = DateTime.Now;

                iLog.WriteLog(name + "接口调用开始！", 0);

                ProjectSummarySer.SendDayTrialEmail();

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
