using Autofac;
using DJS.Core.Common;
using DJS.Core.CPlatform.Server.Utilities;
using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Scheduler.Models;
using DJS.Core.CPlatform.Serialization;
using DJS.Core.CPlatform.Transport;
using DJS.Core.CPlatform.Transport.Codec;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DJS.Core.CPlatform.Server;

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
            try
            {
                while (true)
                {
                    RemoteInvokeMessage message = new RemoteInvokeMessage();
                    message.ServiceId ="11111";
                    message.InvokeType = RemoteInvokeType.SubScriptionScheduler;
                    var client = CreateClient();
                    RemoteInvokeResultMessage result = await client.SendAsync(message);
                    //client.Close();
                    //var clientT = CreateClient();
                    //result = await clientT.SendAsync(message);
                    CallBack(result);
                }
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
            var iServiceExecutor = iContainer.Resolve<IServiceExecutor>();
            TypedParameter[] paras = new TypedParameter[2];
            paras[0] = new TypedParameter(iCodeFactory.GetType(), iCodeFactory);
            paras[1] = new TypedParameter(iServiceExecutor.GetType(), iServiceExecutor);
            var iFactory = iContainer.Resolve<ITransportClientFactory>(paras);
            return iFactory.CreateClient(AppConfig.SERVERINFO.CreateEndPoint());
        }
    }
}
