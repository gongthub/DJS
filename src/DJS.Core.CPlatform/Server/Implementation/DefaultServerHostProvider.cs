using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Server;
using DJS.Core.CPlatform.Transport;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DJS.Core.Server.Implement
{
    public class DefaultServerHostProvider : IServerHostProvider
    {
        private static List<IMessageSender> SUBCLIENTLST = new List<IMessageSender>();
        public List<IMessageSender> GetSubClient()
        {
            List<IMessageSender> models = new List<IMessageSender>();
            models = SUBCLIENTLST;
            return models;
        }

        public void SetSubClient(IMessageSender messageSender)
        {
            var b = SUBCLIENTLST.Contains(messageSender);
            if (!b)
            {
                if (SUBCLIENTLST != null)
                    SUBCLIENTLST.Add(messageSender);
            }
        }

        public void StartExecPublishJobs()
        {
            List<IMessageSender> models = GetSubClient();
            TransportMessage message = new TransportMessage();
            RemoteInvokeResultMessage resultMessage = new RemoteInvokeResultMessage();
            resultMessage.Result = "";
            if (models != null && models.Count > 0)
            {
                foreach (var item in models)
                {
                    resultMessage.Result = "111";
                    message = TransportMessage.CreateInvokeResultMessage("11212", resultMessage);
                    item.SendAndFlushAsync(message);
                }
            }
        }
    }
}
