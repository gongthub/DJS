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
using System.Threading;

namespace DJS.Core.Scheduler.Implement
{
    public class JobProvider
    {

        private static List<JobModel> JOBMODELS = new List<JobModel>();

        private static ITransportClient ISUBSCHEDULERCLIENT;

        public static void SetJobs(List<JobModel> jobs)
        {
            JOBMODELS = jobs;
        }
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
                await MonitorSchedulerClient();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task CreateSubScheduler()
        {
            try
            {
                RemoteInvokeMessage message = new RemoteInvokeMessage();
                message.ServiceId = "1111";
                message.InvokeType = RemoteInvokeType.SubScriptionScheduler;
                if (ISUBSCHEDULERCLIENT == null)
                    ISUBSCHEDULERCLIENT = CreateClient();
                RemoteInvokeResultMessage result = await ISUBSCHEDULERCLIENT.SendAsync(message);
                CallBack(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {

                await MonitorSchedulerClient();
            }
        }
        private async Task MonitorSchedulerClient()
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
                         ISUBSCHEDULERCLIENT.SendAsync(message);
                     }
                     catch
                     {
                         return CreateSubScheduler();
                     }
                 }
             });
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
            var iServiceExecutor = iContainer.Resolve<IServiceExecutor>();
            TypedParameter[] paras = new TypedParameter[2];
            paras[0] = new TypedParameter(iCodeFactory.GetType(), iCodeFactory);
            paras[1] = new TypedParameter(iServiceExecutor.GetType(), iServiceExecutor);
            var iFactory = iContainer.Resolve<ITransportClientFactory>(paras);
            return iFactory.CreateClient(AppConfig.SERVERINFO.CreateEndPoint());
        }
    }
}
