using Autofac;
using DJS.Core.CPlatform.Execute.Models;
using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Serialization;
using DJS.Core.CPlatform.Server;
using DJS.Core.CPlatform.Server.Utilities;
using DJS.Core.CPlatform.Transport;
using System;
using System.Threading.Tasks;

namespace DJS.Core.Execute.Implement
{
    public class ExecuteServiceExecutor : IServiceExecutor
    {
        public async Task ExecuteAsync(IMessageSender sender, IServerHostProvider serverHostProvider, TransportMessage message)
        {
            await ExecuteAsync(sender, null, message);
        }

        public async Task ExecuteAsync(IMessageSender sender, TransportMessage message)
        {
            Console.WriteLine("接收到消息。");

            await Task.Run(() =>
            {
                if (!message.IsInvokeResultMessage())
                    return;
                RemoteInvokeResultMessage remoteInvokeResultMessage;
                try
                {
                    remoteInvokeResultMessage = message.GetContent<RemoteInvokeResultMessage>();
                    switch (remoteInvokeResultMessage.remoteInvokeResultType)
                    {
                        case RemoteInvokeResultType.PublishExecuteJobs:
                            var iContainer = AppConfig.ICONTATINER;
                            var iCodeFactory = iContainer.Resolve<ISerializer<string>>();
                            ExecuteJobModel model = iCodeFactory.Deserialize<string, ExecuteJobModel>(remoteInvokeResultMessage.Result.ToString());
                            AppConfig.SetJob(model);
                            Console.WriteLine("发布执行任务：" + model.Name);
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("将接收到的消息反序列化成 TransportMessage<RemoteInvokeResultMessage> 时发送了错误。" + exception.Message);
                    return;
                }
            });

            Console.WriteLine("执行本地逻辑完成。");

        }

    }
}
