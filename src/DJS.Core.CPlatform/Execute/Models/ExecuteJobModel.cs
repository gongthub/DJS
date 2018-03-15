using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.CPlatform.Execute.Models
{
    public class ExecuteJobModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int JobType { get; set; }
        public string Cron { get; set; }
        public string Desc { get; set; }
        public DateTime ExecTime { get; set; }
        public DateTime? NextTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsTriggering { get; set; }
        public string AssPath { get; set; }
        public string FileName { get; set; }
    }
}
