using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.SDK
{
    public abstract class DJSClientBase : IJob
    {
        public static DJS.SDK.ILog iLog = null;
        static DJSClientBase()
        {
            iLog = DataAccess.CreateILog();
        }
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                iLog.WriteLog(context.JobDetail.Key.Name + "接口调用开始--SYSTEM！", 0);
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start(); //  开始监视代码

                ExceteJob(context.JobDetail.Key.Name);

                stopwatch.Stop(); //  停止监视
                TimeSpan timeSpan = stopwatch.Elapsed; //  获取总时间
                iLog.WriteLog(context.JobDetail.Key.Name + "接口调用结束--SYSTEM！", 0);

                iLog.WriteLog(context.JobDetail.Key.Name + "接口调用执行时间--SYSTEM！任务消耗时间：" + timeSpan, 0);
            }
            catch (Exception e)
            {
                iLog.WriteLog(context.JobDetail.Key.Name + "接口调用失败--SYSTEM！" + e.Message, 0);
                throw e;
            }
        }
        public abstract void ExceteJob(string jobName);

    }
}