using Autofac;
using DJS.Core.CPlatform;
using DJS.Core.CPlatform.Execute;
using DJS.Core.CPlatform.Server;
using DJS.Core.Execute.Implement;
using DJS.Core.Scheduler.Implement;
using System;

namespace DJS.Core.Execute
{
    public static class ContainerBuilderExtensions
    {
        public static IServiceBuilder AddExecuteServices(this IServiceBuilder builder)
        {
            var services = builder.Services;
            services.RegisterType(typeof(ExecuteServiceExecutor)).As(typeof(IServiceExecutor)).SingleInstance();
            services.RegisterType(typeof(ExecuteProvider)).As(typeof(IExecuteProvider)).SingleInstance();
            return builder;
        }
        public static IExecuteBuilder AddServerInfo(this IExecuteBuilder builder, string serverIp, int serverPort)
        {
            AppConfig.SERVERINFO.Ip = serverIp;
            AppConfig.SERVERINFO.Port = serverPort;
            return builder;
        }
    }
}
