using Autofac;
using DJS.Core.CPlatform;
using DJS.Core.CPlatform.Server;
using DJS.Core.CPlatform.Server.Implementation;
using DJS.Core.CPlatform.Transport;
using DJS.Core.CPlatform.Transport.Codec;
using DJS.Core.CPlatform.Transport.Implementation;
using System.Threading.Tasks;

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
                var serverHostProvider = provider.Resolve<IServerHostProvider>();
                return  new DotNettyServerMessageListener(
                    provider.Resolve<ITransportMessageCodecFactory>(), serverHostProvider);
            }).SingleInstance();
            services.Register(provider =>
            {
                var messageListener = provider.Resolve<DotNettyServerMessageListener>();
                var serviceExecutor = provider.Resolve<IServiceExecutor>();
                var serverHostProvider = provider.Resolve<IServerHostProvider>();
                return new DefaultServiceHost(async endPoint =>
                {
                    await messageListener.StartAsync(endPoint);
                    return messageListener;
                }, serviceExecutor, serverHostProvider);
                }).As<IServiceHost>(); 

            return builder;
        }
    }
}