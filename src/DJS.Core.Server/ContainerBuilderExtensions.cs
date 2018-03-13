using Autofac;
using DJS.Core.Server.Implement;
using DJS.Core.CPlatform;
using DJS.Core.CPlatform.Server;
using DJS.Core.CPlatform.Server.Implementation;

namespace DJS.Core.Server
{
    public static class ContainerBuilderExtensions
    {
        public static IServiceBuilder AddServerServices(this IServiceBuilder builder)
        {
            var services = builder.Services;
            services.RegisterType(typeof(DefaultServerHostProvider)).As(typeof(IServerHostProvider)).SingleInstance();
            services.RegisterType(typeof(ServerProvider)).As(typeof(IServerProvider)).SingleInstance();
            services.RegisterType(typeof(DefaultServiceExecutor)).As(typeof(IServiceExecutor)).SingleInstance();
            return builder;
        }

        public static IServerBuilder AddServerInfo(this IServerBuilder builder, string serverIp, int serverPort)
        {
            AppConfig.SERVERINFO.Ip = serverIp;
            AppConfig.SERVERINFO.Port = serverPort;
            return builder;
        }
    }
}
