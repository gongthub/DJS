using DJS.Core.Common;
using DJS.Core.Scheduler;
using DJS.Core.Scheduler.Configurations;
using DJS.Core.Scheduler.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DJS.Scheduler.WinApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddSchedulerFile("Configs/schedulerSettings.json", optional: false);

            ISchedulerProvider _schedulerBuilder;
            _schedulerBuilder = SchedulerContainer.GetInstances<ISchedulerProvider>("Polling");
            _schedulerBuilder
                .BulidOnStartRunJob(OnStartRunJob)
                .BulidOnCompleteRunJob(OnCompleteRunJob)
                .Start();
            Console.ReadKey();
        }

        private static void OnStartRunJob(object sender, SchedulerEventArgs e)
        {
            Console.WriteLine(e.job.Name + " 开始");
        }
        private static void OnCompleteRunJob(object sender, SchedulerEventArgs e)
        {
            Console.WriteLine(e.job.Name + " 完成");
        }
    }
}
