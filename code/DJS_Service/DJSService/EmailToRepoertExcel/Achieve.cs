using DJS.SDK;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailToRepoertExcel
{
    public class Achieve : IJob, IConfigClient
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 
        public static DJS.SDK.IConfigMgr iConfigMgr = null;
        public static DJS.SDK.ILog iLog = null;
        public static string TemplatePath = "";
        public static string SqlPath = "";

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


                Service.EmailToRepoertExcel obj = new Service.EmailToRepoertExcel();
                Service.EmailToRevparExcel REVPAROBJ = new Service.EmailToRevparExcel();
                obj.RepoertExcel();
                REVPAROBJ.RepoertExcel();

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
        /// 设置配置信息
        /// </summary>
        /// <param name="jobName"></param>
        public void SetConfig(string jobName)
        {
            string configName = iLog.GetConfigNameByJobName(jobName);
            if (!string.IsNullOrEmpty(configName))
            {
                iConfigMgr = DJS.SDK.DataAccess.CreateIConfigMgr(configName);
                TemplatePath = iConfigMgr.GetFileSrc(jobName);
                SqlPath = iConfigMgr.GetFileSrc(jobName);

                string status = iConfigMgr.GetConfig("STATUS");

                if (string.IsNullOrEmpty(status) || status == "true")
                {
                    iConfigMgr.SetConfig("STATUS", "true");
                    iConfigMgr.SetConfig("StoreFileName", "Store.txt");
                    iConfigMgr.SetConfig("DayRepoertFileName", "DayRepoertExcel.txt");
                    iConfigMgr.SetConfig("UserStoreName", "UserStore.txt");
                    iConfigMgr.SetConfig("SearchAll", "SearchAll.txt");
                    iConfigMgr.SetConfig("SearchStore", "SearchStore.txt");
                    iConfigMgr.SetConfig("Temp_Revpar", "Temp_Revpar.xlsx");
                    iConfigMgr.SetConfig("Temp_DaylReportSum", "Temp_DaylReportSum.xlsx");
                    iConfigMgr.SetConfig("Suffix", "xlsx");
                    iConfigMgr.SetConfig("EmailTitle", "日报表");
                    iConfigMgr.SetConfig("EmailDesc", "日报表");
                    iConfigMgr.SetConfig("SavesFilePath", @"D:\ExcelReport\");
                    iConfigMgr.SetConfig("ZipFilePath", @"D:\ExcelReportZip\");

                    iConfigMgr.SetConfig("SendEmail", "testadmin@vlinker.com.cn");
                    iConfigMgr.SetConfig("SendPwd", "Vlinker123");
                    iConfigMgr.SetConfig("ConnType", "SQLServer");
                    iConfigMgr.SetConfig("ConnStr", "Data Source=.;User Id=sa;Password=1234567890;Database=SmallAmsTemp;");

                }
            }
            else
            {
                iLog.WriteLog("获取任务配置名称错误！" + jobName, 1);
            }
        }
    }
}
