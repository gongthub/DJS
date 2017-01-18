﻿using DJS.SDK;
using EmailToDayReportSumExcel.Utils;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailToDayReportSumExcel
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

                Service.EmailToDayReportSumExcel obj = new Service.EmailToDayReportSumExcel();

                obj.RepoertExcel();

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
                TemplatePath = iConfigMgr.GetFileSrc(jobName);
                SqlPath = iConfigMgr.GetFileSrc(jobName);

                iConfigMgr.SetConfig("UserStoreName", "UserStore.txt");
                iConfigMgr.SetConfig("Suffix", "xlsx");
                iConfigMgr.SetConfig("EmailReportName", "小V");
                iConfigMgr.SetConfig("SavesFilePath", @"D:\ExcelReport\");
                iConfigMgr.SetConfig("ZipFilePath", @"D:\ExcelReportZip\");

                iConfigMgr.SetConfig("Temp_DayReprotSum", "Temp_DayReprotSum.xlsx");
                iConfigMgr.SetConfig("EmailTitleDayReprotSum", "门店汇总日报");
                iConfigMgr.SetConfig("EmailDescDayReprotSum", "门店汇总日报");

                iConfigMgr.SetConfig("SendEmail", "testadmin@vlinker.com.cn");
                iConfigMgr.SetConfig("SendPwd", "Vlinker123");
                iConfigMgr.SetConfig("ConnType", "SQLServer");
                iConfigMgr.SetConfig("ServiceCode", "S005");
                iConfigMgr.SetConfig("ConnStr", "Data Source=.;User Id=sa;Password=1234567890;Database=AMS;");
            }
            else
            {
                iLog.WriteLog("获取任务配置名称错误！" + jobName, 1, true);
            }
            ConstUtility cons = new ConstUtility();
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
                ConstUtility cons = new ConstUtility();
                DateTime times = DateTime.Now;

                iLog.WriteLog(name + "接口调用开始！", 0);


                Service.EmailToDayReportSumExcel obj = new Service.EmailToDayReportSumExcel();

                obj.RepoertExcel();

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
