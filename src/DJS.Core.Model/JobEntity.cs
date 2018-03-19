using System;

namespace DJS.Core.Model
{
    public class JobEntity : BaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        //任务类型 一次性或周期性
        public int JobType { get; set; }
        public string Cron { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime NextTime { get; set; }
        public DateTime? EndTime { get; set; }
        //执行服务器 指定一个或多个或所有、自动分配
        public string ExecuteServers { get; set; }
        //执行方式 多个服务器可同步执行或一次任务只能一台服务器执行
        public string ExecuteType { get; set; }

    }
}
