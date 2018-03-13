using DJS.Core.CPlatform.Server;
using System;
using System.Collections.Generic;
using System.Text;
using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Transport;
using System.Threading.Tasks;

namespace DJS.Core.Scheduler.Implement
{
    public class SchedulerServiceExecutor : IServiceExecutor
    {
        public async Task ExecuteAsync(IMessageSender sender, IServerHostProvider serverHostProvider, TransportMessage message)
        {
            Console.WriteLine("接收到消息。");

            if (!message.IsInvokeResultMessage())
                return;

            RemoteInvokeResultMessage remoteInvokeResultMessage;
            try
            {
                remoteInvokeResultMessage = message.GetContent<RemoteInvokeResultMessage>();
                Console.WriteLine("客户端接口服务端消息：" + remoteInvokeResultMessage.Result);
            }
            catch (Exception exception)
            {
                Console.WriteLine("将接收到的消息反序列化成 TransportMessage<RemoteInvokeResultMessage> 时发送了错误。");
                return;
            }

            Console.WriteLine("准备执行本地逻辑。");

        }

        public async Task ExecuteAsync(IMessageSender sender, TransportMessage message)
        {

            Console.WriteLine("接收到消息。");

            if (!message.IsInvokeResultMessage())
                return;

            RemoteInvokeResultMessage remoteInvokeResultMessage;
            try
            {
                remoteInvokeResultMessage = message.GetContent<RemoteInvokeResultMessage>();
                Console.WriteLine("客户端接口服务端消息："+ remoteInvokeResultMessage.Result);
            }
            catch (Exception exception)
            {
                Console.WriteLine("将接收到的消息反序列化成 TransportMessage<RemoteInvokeResultMessage> 时发送了错误。");
                return;
            }

            Console.WriteLine("准备执行本地逻辑。");
             
            
        }
    }
}
