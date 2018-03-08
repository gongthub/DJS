using Microsoft.Extensions.DependencyInjection;
using DJS.Core.CPlatform;
using DJS.Core.CPlatform.Runtime.Server;
using DJS.Core.CPlatform.Runtime.Server.Implementation;
using DJS.Core.CPlatform.Transport;
using Autofac;
using Microsoft.Extensions.Logging;
using DJS.Core.CPlatform.Transport.Codec;

namespace DJS.Core.DotNetty
{
   public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// 使用DotNetty进行传输。
        /// </summary>
        /// <param name="builder">服务构建者。</param>
        /// <returns>服务构建者。</returns>
        public static IServiceBuilder UseDotNettyTransport(this IServiceBuilder builder)
        {
            var services = builder.Services;
            services.RegisterType(typeof(DotNettyTransportClientFactory)).As(typeof(ITransportClientFactory)).SingleInstance();
            services.Register(provider => {
              return  new DotNettyServerMessageListener(
                    provider.Resolve<ITransportMessageCodecFactory>());
            }).SingleInstance();
            services.Register(provider =>
            {
                var messageListener = provider.Resolve<DotNettyServerMessageListener>();
                //var serviceExecutor = provider.Resolve<IServiceExecutor>();
                return new DefaultServiceHost(async endPoint =>
                {
                    await messageListener.StartAsync(endPoint);
                    return messageListener;
                });
                }).As<IServiceHost>(); 

            return builder;
        }
    }
}