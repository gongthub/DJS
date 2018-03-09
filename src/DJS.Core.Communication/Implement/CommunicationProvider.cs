using Autofac;
using DJS.Core.CPlatform.Communication;

namespace DJS.Core.Communication.Implement
{
    public class CommunicationProvider : ICommunicationProvider
    {
        public void Start()
        {
            var iContainer = AppConfig.ICONTATINER;
            //var iCodeFactory = iContainer.Resolve<ITransportMessageCodecFactory>();
            //var iFactory = iContainer.Resolve<IServiceHost>(new TypedParameter(iCodeFactory.GetType(), iCodeFactory));
            var iFactory = iContainer.Resolve<IServiceHost>();
            iFactory.StartAsync(AppConfig.SERVERINFO.CreateEndPoint());
            //return iFactory.CreateClient(AppConfig.SERVERINFO.CreateEndPoint());
        }
    }
}
