using Autofac;
using DJS.Core.Common;
using DJS.Core.CPlatform.Communication.Utilities;
using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Scheduler.Models;
using DJS.Core.CPlatform.Serialization;
using DJS.Core.CPlatform.Transport;
using DJS.Core.CPlatform.Transport.Codec;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DJS.Core.Scheduler.Implement
{
    public class JobProvider
    {

        private static List<JobModel> JOBMODELS = new List<JobModel>();

        public static List<JobModel> GetJobs()
        {
            return JOBMODELS;
        }
        public static List<JobModel> AsyncJobs(List<JobModel> jobs)
        {
            List<JobModel> models = new List<JobModel>();
            if (jobs != null && jobs.Count > 0)
            {
                models = jobs;
            }
            else
            {
                models.Add(new JobModel() { Name = "任务1", Cron = "0/5 * * * * ?" });
                models.Add(new JobModel() { Name = "任务2", Cron = "0/3 * * * * ?" });

                if (models != null && models.Count > 0)
                {
                    models.ForEach(delegate (JobModel model)
                    {
                        DateTime tTime = DateTime.Now;
                        var exp = new CronExpression(model.Cron);
                        DateTime? ntimet = exp.GetNextValidTimeAfter(tTime);
                        model.NextTime = ntimet;
                    });
                }
            }
            JOBMODELS = models;
            return models;
        }

        public async Task AsyncRemote()
        {
            while (true)
            {
                RemoteInvokeMessage message = new RemoteInvokeMessage();
                message.ServiceId = Guid.NewGuid().ToString();
                message.InvokeType = RemoteInvokeType.GetJobLists;
                var client = CreateClient();
                RemoteInvokeResultMessage result = await client.SendAsync(message);
                CallBack(result);
            }
            //RemoteInvokeResultMessage result =await client.SendAsync(message);
        }

        public void CallBack(RemoteInvokeResultMessage message)
        {
            try
            {
                List<JobModel> models = new List<JobModel>();
                var iContainer = AppConfig.ICONTATINER;
                var iCodeFactory = iContainer.Resolve<ISerializer<string>>();
                models = iCodeFactory.Deserialize<string, List<JobModel>>(message.Result.ToString());
                //List<JobModel> models = new List<JobModel>();
                if (models != null && models.Count > 0)
                {
                    models.ForEach(delegate (JobModel model)
                    {
                        DateTime tTime = DateTime.Now;
                        var exp = new CronExpression(model.Cron);
                        DateTime? ntimet = exp.GetNextValidTimeAfter(tTime);
                        model.NextTime = ntimet;
                    });
                }
                JOBMODELS = models;
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
