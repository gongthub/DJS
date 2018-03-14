using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Server;
using DJS.Core.CPlatform.Transport;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Autofac;
using System;
using DJS.Core.CPlatform.Scheduler.Models;
using DJS.Core.CPlatform.Server.Utilities;
using DJS.Core.Common;

namespace DJS.Core.Server.Implement
{
    public class DefaultServerHostProvider : IServerHostProvider
    {
        private static List<IMessageSender> SUBSCHEDULERCLIENTLST = new List<IMessageSender>();

        public void SetSubSchedulerClient(IMessageSender messageSender)
        {
            if (SUBSCHEDULERCLIENTLST != null)
            {
                var modelT = SUBSCHEDULERCLIENTLST.Find(m => m.ClientId == messageSender.ClientId);
                if (modelT == null || string.IsNullOrEmpty(modelT.ClientId))
                    SUBSCHEDULERCLIENTLST.Add(messageSender);
            }
        }
        public List<IMessageSender> GetSubSchedulerClients()
        {
            List<IMessageSender> models = new List<IMessageSender>();
            models = SUBSCHEDULERCLIENTLST;
            return models;
        }

        public void RemoveSubSchedulerClient(string clientId)
        {
            if (SUBSCHEDULERCLIENTLST != null)
            {
                var modelT = SUBSCHEDULERCLIENTLST.Find(m => m.ClientId == clientId);
                if (modelT == null || string.IsNullOrEmpty(modelT.ClientId))
                    SUBSCHEDULERCLIENTLST.Remove(modelT);
            }
        }

        public void PublishSchedulerJobs()
        {
            List<JobModel> schedulerJobs = GetSchedulerJobs();
            List<IMessageSender> models = GetSubSchedulerClients();
            TransportMessage message = new TransportMessage();
            RemoteInvokeResultMessage resultMessage = new RemoteInvokeResultMessage();
            resultMessage.remoteInvokeResultType = RemoteInvokeResultType.PublishSchedulerJobs;
            resultMessage.Result = schedulerJobs;
            if (models != null && models.Count > 0)
            {
                foreach (var item in models)
                {
                    //message = TransportMessage.CreateInvokeResultMessage(Guid.NewGuid().ToString(), resultMessage);
                    //item.SendAndFlushAsync(message);
                }
            }
        }

        public List<JobModel> GetSchedulerJobs()
        {
            List<JobModel> models = new List<JobModel>();
            Random ran = new Random();
            int num = ran.Next(2000, 10000);
            int numT = ran.Next(1, 10);
            models.Add(new JobModel() { Id = Guid.NewGuid().ToString(), Name = "任务-" + num, Cron = "0/5 * * * * ?" });
            models.Add(new JobModel() { Id = Guid.NewGuid().ToString(), Name = "任务-" + numT, Cron = "0/3 * * * * ?" });
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
            return models;
        }
    }
}
