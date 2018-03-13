using DJS.Core.CPlatform.Transport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DJS.Core.CPlatform.Server
{
    public interface IServerHostProvider
    {
        void StartExecPublishJobs();

        List<IMessageSender> GetSubClient();
        void SetSubClient(IMessageSender messageSender);
    }
}
