using Autofac;
using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Scheduler.Models;
using DJS.Core.CPlatform.Server.Utilities;
using DJS.Core.CPlatform.Transport;
using DJS.Core.CPlatform.Transport.Codec;
using System;
using System.Threading.Tasks;

namespace DJS.Core.Scheduler.Implement
{
    public class TriggerProvider
    {
        public async Task Run(JobModel job)
        {
            Console.WriteLine(job.Name + " 开始 执行时间：" + DateTime.Now + " 下次开始时间：" + job.NextTime);
            RemoteInvokeMessage message = new RemoteInvokeMessage();
            message.ServiceId = job.Id.ToString();
            message.InvokeType = RemoteInvokeType.TriggerJob;
            var client = CreateClient();
            RemoteInvokeResultMessage result = await client.SendAsync(message);
            CallBack(result);
        }

        public void CallBack(RemoteInvokeResultMessage message)
        {
            try
            {
                Console.WriteLine(message.Result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static ITransportClient CreateClient()
        {
            var iContainer = AppConfig.ICONTATINER;
            var iCodeFactory = iContainer.Resolve<ITransportMessageCodecFactory>();
            var iFactory = iContainer.Resolve<ITransportClientFactory>(new TypedParameter(iCodeFactory.GetType(), iCodeFactory));
            return iFactory.CreateClient(AppConfig.SERVERINFO.CreateEndPoint());
        }
    }
}
