using Autofac;
using DJS.Core.CPlatform;
using DJS.Core.CPlatform.Scheduler;
using DJS.Core.Scheduler.Implement;
using DJS.Core.Scheduler.Utilities;
using System;

namespace DJS.Core.Scheduler
{
    public static class ContainerBuilderExtensions
    {
        public static ISchedulerBuilder AddServerInfo(this ISchedulerBuilder builder, string serverIp, int serverPort)
        {
            AppConfig.SERVERINFO.Ip = serverIp;
            AppConfig.SERVERINFO.Port = serverPort;
            return builder;
        }
        public static IServiceBuilder UsePolling(this IServiceBuilder builder)
        {
            var services = builder.Services;
            services.RegisterType(typeof(PollingScheduler)).As(typeof(ISchedulerProvider)).SingleInstance();
            return builder;
        }

        public static ISchedulerProvider BulidOnStartRunJob(this ISchedulerProvider builder, EventHandler<SchedulerEventArgs> onStartRunJob)
        {
            builder.BulidOnStartRunJob(onStartRunJob);
            return builder;
        }
        public static ISchedulerProvider BulidOnCompleteRunJob(this ISchedulerProvider builder, EventHandler<SchedulerEventArgs> onCompleteRunJob)
        {
            builder.BulidOnCompleteRunJob(onCompleteRunJob);
            return builder;
        }
    }
}
