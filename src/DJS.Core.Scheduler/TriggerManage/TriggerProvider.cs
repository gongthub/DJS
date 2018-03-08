using DJS.Core.Scheduler.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DJS.Core.Scheduler.TriggerManage
{
    public class TriggerProvider
    {
        public void Run(JobModel job)
        {
            Thread.Sleep(3000);
            //Debug.WriteLine(job.Name + " 执行 执行时间：" + DateTime.Now + " 下次开始时间：" + job.NextTime);
        }
    }
}
