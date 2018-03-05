using DJS.Core.Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.Scheduler.Utilities
{
    public class SchedulerEventArgs : EventArgs
    {
        public JobModel job { get; set; }
    }
}
