using Autofac;
using DJS.Core.CPlatform.Server;
using System.Threading;

namespace DJS.Core.Server.Implement
{
    public class ServerProvider : IServerProvider
    {
        public void Start()
        {
            var iContainer = AppConfig.ICONTATINER;
            var iFactory = iContainer.Resolve<IServiceHost>();
            var iServerHostProvider = iContainer.Resolve<IServerHostProvider>();
            iFactory.StartAsync(AppConfig.SERVERINFO.CreateEndPoint());

            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10000);
                    iServerHostProvider.PublishSchedulerJobs();
                }
            });
            thread.Start();
        }
    }
}
