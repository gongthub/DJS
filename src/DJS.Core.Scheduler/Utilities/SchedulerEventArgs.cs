using DJS.Core.Scheduler.Models;
using System;

namespace DJS.Core.Scheduler.Utilities
{
    public class SchedulerEventArgs : EventArgs
    {
        public JobModel job { get; set; }
    }
}
