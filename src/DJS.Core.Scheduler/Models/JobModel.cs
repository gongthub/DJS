using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.Scheduler.Models
{
    public class JobModel
    {
        public string Name { get; set; }
        public string Cron { get; set; }
        public DateTime? NextTime { get; set; }
    }
}
