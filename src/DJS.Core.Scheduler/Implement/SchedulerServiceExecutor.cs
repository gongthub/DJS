using DJS.Core.CPlatform.Server;
using System;
using System.Collections.Generic;
using System.Text;
using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Transport;
using System.Threading.Tasks;
using DJS.Core.CPlatform.Server.Utilities;
using DJS.Core.CPlatform.Scheduler.Models;
using Autofac;
using DJS.Core.CPlatform.Serialization;

namespace DJS.Core.Scheduler.Implement
{
    public class SchedulerServiceExecutor : IServiceExecutor
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
                    if (remoteInvokeResultMessage.remoteInvokeResultType == RemoteInvokeResultType.PublishSchedulerJobs)
                    {
                        List<JobModel> models = GethedulerJobs(remoteInvokeResultMessage.Result.ToString());
                        JobProvider.SetJobs(models);
                    }
                    //Console.WriteLine("客户端接口服务端消息：" + remoteInvokeResultMessage.Result);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("将接收到的消息反序列化成 TransportMessage<RemoteInvokeResultMessage> 时发送了错误。" + exception.Message);
                    return;
                }
            });

            Console.WriteLine("执行本地逻辑完成。");

        }

        private List<JobModel> GethedulerJobs(string msg)
        {
            List<JobModel> models = new List<JobModel>();
            var iContainer = AppConfig.ICONTATINER;
            var iCodeFactory = iContainer.Resolve<ISerializer<string>>();
            models = iCodeFactory.Deserialize<string, List<JobModel>>(msg);
            return models;
        }
    }
}
