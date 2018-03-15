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
using DJS.Core.Common.File;

namespace DJS.Core.Server.Implement
{
    public class DefaultServerHostProvider : IServerHostProvider
    {
        private static List<IMessageSender> SUBSCHEDULERCLIENTLST = new List<IMessageSender>();
        private static List<IMessageSender> SUBEXECUTECLIENTLST = new List<IMessageSender>();

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
                    try
                    {
                        message = TransportMessage.CreateInvokeResultMessage(Guid.NewGuid().ToString(), resultMessage);
                        item.SendAndFlushAsync(message);
                    }
                    catch
                    {
                        RemoveSubSchedulerClient(item.ClientId);
                    }
                }
            }
        }

        public void SetSubExecuteClient(IMessageSender messageSender)
        {
            if (SUBEXECUTECLIENTLST != null)
            {
                var modelT = SUBEXECUTECLIENTLST.Find(m => m.ClientId == messageSender.ClientId);
                if (modelT == null || string.IsNullOrEmpty(modelT.ClientId))
                    SUBEXECUTECLIENTLST.Add(messageSender);
            }
        }
        public List<IMessageSender> GetSubExecuteClients()
        {
            List<IMessageSender> models = new List<IMessageSender>();
            models = SUBEXECUTECLIENTLST;
            return models;
        }

        public void RemoveSubExecuteClient(string clientId)
        {
            if (SUBEXECUTECLIENTLST != null)
            {
                var modelT = SUBEXECUTECLIENTLST.Find(m => m.ClientId == clientId);
                if (modelT == null || string.IsNullOrEmpty(modelT.ClientId))
                    SUBEXECUTECLIENTLST.Remove(modelT);
            }
        }

        public void PublishExecuteJobs(string jobId)
        {
            List<JobModel> schedulerJobs = GetSchedulerJobs();
            if (schedulerJobs != null && schedulerJobs.Count > 0)
            {
                JobModel jobModel = schedulerJobs.Find(m => m.Id == jobId);
                if (jobModel != null && !string.IsNullOrEmpty(jobModel.Id))
                {
                    List<IMessageSender> models = GetSubExecuteClients();
                    TransportMessage message = new TransportMessage();
                    RemoteInvokeResultMessage resultMessage = new RemoteInvokeResultMessage();
                    if (models != null && models.Count > 0)
                    {
                        foreach (var item in models)
                        {
                            try
                            {
                                resultMessage.Result = jobModel;
                                resultMessage.remoteInvokeResultType = RemoteInvokeResultType.PublishExecuteJobs;
                                message = TransportMessage.CreateInvokeResultMessage(Guid.NewGuid().ToString(), resultMessage);
                                item.SendAndFlushAsync(message);
                            }
                            catch
                            {
                                RemoveSubExecuteClient(item.ClientId);
                            }
                        }
                    }
                }
            }
        }

        private List<JobModel> GetSchedulerJobs()
        {
            List<JobModel> models = new List<JobModel>();
            Random ran = new Random();
            int num = ran.Next(2000, 10000);
            int numT = ran.Next(1, 10);
            models.Add(new JobModel() { Id = "1111111111", Name = "任务-1", Cron = "0/15 * * * * ?", AssPath = @"http://localhost:62538/Stores/InvoiceStatusService.dll" });
            //models.Add(new JobModel() { Id = "2222222222", Name = "任务-2", Cron = "0/3 * * * * ?" });
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
