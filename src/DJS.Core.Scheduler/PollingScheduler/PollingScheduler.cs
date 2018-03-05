using DJS.Core.Common;
using DJS.Core.Scheduler.JobManage;
using DJS.Core.Scheduler.Models;
using DJS.Core.Scheduler.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DJS.Core.Scheduler.PollingScheduler
{
    [IdentifyScheduler(name: SchedulerTargetType.Polling)]
    public class PollingScheduler : ISchedulerProvider
    {
        private const int MINTHREADNUM = 1;
        private const int MAXTHREADNUM = 5;
        //private ISchedulerProvider _iSchedulerProvider = null;
        private static List<JobModel> JOBS = new List<JobModel>();

        public event EventHandler<SchedulerEventArgs> OnStartRunJob;
        public event EventHandler<SchedulerEventArgs> OnCompleteRunJob;

        public ISchedulerProvider BulidOnStartRunJob(EventHandler<SchedulerEventArgs> onStartRunJob)
        { 
            if (OnStartRunJob == null)
            {

                OnStartRunJob = new EventHandler<SchedulerEventArgs>(onStartRunJob);
            }
            else
            {
                OnStartRunJob += onStartRunJob;
            }
            return this;
        }

        public ISchedulerProvider BulidOnCompleteRunJob(EventHandler<SchedulerEventArgs> onCompleteRunJob)
        {
            if (OnCompleteRunJob == null)
            {

                OnCompleteRunJob = new EventHandler<SchedulerEventArgs>(onCompleteRunJob);
            }
            else
            {
                OnCompleteRunJob += onCompleteRunJob;
            }
            return this;
        }

        public void Start()
        {
            ThreadPool.SetMinThreads(MINTHREADNUM, MINTHREADNUM);
            ThreadPool.SetMaxThreads(MAXTHREADNUM, MAXTHREADNUM);
            for (int i = 1; i <= MAXTHREADNUM; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(Run), i.ToString());
            }
        }

        private void Run(object obj)
        {
            while (true)
            {
                JOBS = JobProvider.AsyncJobs(JOBS);
                if (JOBS != null && JOBS.Count > 0)
                {
                    DateTime nTime = DateTime.Now;
                    List<JobModel> models = JOBS.FindAll(m => m.NextTime.Value.ToString().Equals(nTime.ToString()));
                    if (models != null && models.Count > 0)
                    {
                        models.ForEach(delegate (JobModel model)
                        {
                            var expt = new CronExpression(model.Cron);
                            DateTime? ntimet = expt.GetNextValidTimeAfter(nTime);
                            if (ntimet != model.NextTime)
                            {
                                model.NextTime = ntimet;
                                SchedulerEventArgs arg = new SchedulerEventArgs();
                                arg.job = model;
                                OnStartRunJob(this, arg);

                                Thread thread = new Thread(() =>
                                {
                                    new TriggerProvider().Run(model);
                                    OnCompleteRunJob(this, arg);
                                });
                                thread.Start();
                            }
                        });
                    }
                }
            }
        }

    }
}
