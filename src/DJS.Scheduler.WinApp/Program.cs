using DJS.Core.CPlatform;
using DJS.Core.DotNetty;
using DJS.Core.Scheduler;
using DJS.Core.Scheduler.Utilities;
using System;

namespace DJS.Scheduler.WinApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var builder = new ConfigurationBuilder()
            //.SetBasePath(AppContext.BaseDirectory)
            //.AddSchedulerFile("Configs/schedulerSettings.json", optional: false);

            //ISchedulerProvider _schedulerBuilder;
            //_schedulerBuilder = SchedulerContainer.GetInstances<ISchedulerProvider>("Polling");
            //_schedulerBuilder
            //    .BulidOnStartRunJob(OnStartRunJob)
            //    .BulidOnCompleteRunJob(OnCompleteRunJob)
            //    .Start();

            var host = new SchedulerBuilder()
                .RegisterServices(builder =>
                {
                    builder.AddServices(option =>
                    {
                        option.UseDotNettyTransport()
                        .UsePolling();
                    });
                })
                .AddServerInfo("127.0.0.1", 11120)
               .Build();
            //host.BulidOnStartRunJob(OnStartRunJob);
            //host.BulidOnCompleteRunJob(OnCompleteRunJob);

            host.Start();
            Console.ReadKey();
        }

        private static void OnStartRunJob(object sender, SchedulerEventArgs e)
        {
            Console.WriteLine(e.job.Name + " 开始 执行时间：" + DateTime.Now + " 下次开始时间：" + e.job.NextTime);
        }
        private static void OnCompleteRunJob(object sender, SchedulerEventArgs e)
        {
            Console.WriteLine(e.job.Name + " 完成 执行时间：" + DateTime.Now + " 下次开始时间：" + e.job.NextTime);
        }
    }
}
