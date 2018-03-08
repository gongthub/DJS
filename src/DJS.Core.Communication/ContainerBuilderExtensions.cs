using Autofac;
using DJS.Core.Communication.Implement;
using DJS.Core.CPlatform;
using DJS.Core.CPlatform.Communication;
using DJS.Core.CPlatform.Runtime.Server;
using System;

namespace DJS.Core.Communication
{
    public static class ContainerBuilderExtensions
    {
        public static IServiceBuilder AddCommunicationServices(this IServiceBuilder builder)
        {
            var services = builder.Services;
            services.RegisterType(typeof(CommunicationProvider)).As(typeof(ICommunicationProvider)).SingleInstance();
            return builder;
        }

        public static ICommunicationBuilder AddServerInfo(this ICommunicationBuilder builder, string serverIp, int serverPort)
        {
            AppConfig.SERVERINFO.Ip = serverIp;
            AppConfig.SERVERINFO.Port = serverPort;
            return builder;
        }
    }
}
