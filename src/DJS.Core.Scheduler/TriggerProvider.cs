using DJS.Core.Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DJS.Core.Scheduler
{
    public class TriggerProvider
    {
        public void Run(JobModel job)
        {
            Debug.WriteLine(job.Name+" 执行");
        }
    }
}
