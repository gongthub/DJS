using Autofac;
using DJS.Core.CPlatform.Execute;
using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Server.Utilities;
using DJS.Core.CPlatform.Transport;
using DJS.Core.CPlatform.Transport.Codec;
using DJS.Core.Execute;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DJS.Core.Scheduler.Implement
{
    public class ExecuteProvider : IExecuteProvider
    {
        private static ITransportClient IEXECUTECLIENT;
        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                return Run();
            });
        }
        public async Task Run()
        {
            try
            {
                await MonitorExecuteClient();
                //while (true)
                //{
                //    RemoteInvokeMessage message = new RemoteInvokeMessage();
                //    message.ServiceId = Guid.NewGuid().ToString();
                //    message.InvokeType = RemoteInvokeType.SubScriptionExecute;
                //    var client = CreateClient();
                //    RemoteInvokeResultMessage result = await client.SendAsync(message);
                //    CallBack(result);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //RemoteInvokeResultMessage result =await client.SendAsync(message);
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


        public async Task CreateSubExecute()
        {
            try
            {
                RemoteInvokeMessage message = new RemoteInvokeMessage();
                message.ServiceId = "2222";
                message.InvokeType = RemoteInvokeType.SubScriptionExecute;
                if (IEXECUTECLIENT == null)
                    IEXECUTECLIENT = CreateClient();
                RemoteInvokeResultMessage result = await IEXECUTECLIENT.SendAsync(message);
                CallBack(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally {

                await MonitorExecuteClient();
            }
        }
        private async Task MonitorExecuteClient()
        {
            await Task.Run(() =>
             {
                 while (true)
                 {
                     try
                     {
                         Thread.Sleep(1000);
                         RemoteInvokeMessage message = new RemoteInvokeMessage();
                         message.InvokeType = RemoteInvokeType.Heartbeat;
                         IEXECUTECLIENT.SendAsync(message);
                     }
                     catch
                     {
                         return CreateSubExecute();
                     }
                 }
             });
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
