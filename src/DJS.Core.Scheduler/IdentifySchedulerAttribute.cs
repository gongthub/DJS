using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.Scheduler
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class IdentifySchedulerAttribute : Attribute
    {
        public IdentifySchedulerAttribute(SchedulerTargetType name)
        {
            this.Name = name;
        }

        public SchedulerTargetType Name { get; set; }
    }
}
