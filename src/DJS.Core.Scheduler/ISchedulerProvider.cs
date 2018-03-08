using Autofac;
using DJS.Core.Scheduler.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.Scheduler
{
    public interface ISchedulerProvider
    {
        ISchedulerProvider BulidOnStartRunJob(EventHandler<SchedulerEventArgs> onStartRunJob);
        ISchedulerProvider BulidOnCompleteRunJob(EventHandler<SchedulerEventArgs> onCompleteRunJob);
        void Start();
    }
}