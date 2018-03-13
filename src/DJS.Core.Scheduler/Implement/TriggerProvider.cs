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
            RemoteInvokeMessage message = new RemoteInvokeMessage();
            message.ServiceId = Guid.NewGuid().ToString();
            message.InvokeType = RemoteInvokeType.TriggerJob;
            var client = CreateClient();
            RemoteInvokeResultMessage result = await client.SendAsync(message);
            CallBack(result);
        }

        public void CallBack(RemoteInvokeResultMessage message)
        {
            try
            {
                //List<JobModel> models = new List<JobModel>();
                //var iContainer = AppConfig.ICONTATINER;
                //var iCodeFactory = iContainer.Resolve<ISerializer<string>>();
                //models = iCodeFactory.Deserialize<string, List<JobModel>>(message.Result.ToString());
                ////List<JobModel> models = new List<JobModel>();
                //if (models != null && models.Count > 0)
                //{
                //    models.ForEach(delegate (JobModel model)
                //    {
                //        DateTime tTime = DateTime.Now;
                //        var exp = new CronExpression(model.Cron);
                //        DateTime? ntimet = exp.GetNextValidTimeAfter(tTime);
                //        model.NextTime = ntimet;
                //    });
                //}
                //JOBMODELS = models;
                //Console.WriteLine(message.Result);
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
