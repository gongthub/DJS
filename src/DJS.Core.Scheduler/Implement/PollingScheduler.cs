using DJS.Core.Common;
using DJS.Core.CPlatform.Scheduler;
using DJS.Core.CPlatform.Scheduler.Models;
using DJS.Core.Scheduler.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DJS.Core.Scheduler.Implement
{
    public class PollingScheduler : ISchedulerProvider
    {
        private const int MINTHREADNUM = 1;
        private const int MAXTHREADNUM = 3;
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
            Task.Run(() =>
            {
                Thread.Sleep(5000);
                JobProvider jobProvider = new JobProvider();
                return jobProvider.AsyncRemote();
            });
            //ThreadPool.SetMinThreads(MINTHREADNUM, MINTHREADNUM);
            //ThreadPool.SetMaxThreads(MAXTHREADNUM, MAXTHREADNUM);
            //for (int i = 1; i <= MAXTHREADNUM; i++)
            //{
            //    bool state = ThreadPool.QueueUserWorkItem(new WaitCallback(Run), i.ToString());
            //}
            Run(1);
        }

        private void Run(object obj)
        {
            while (true)
            {
                //Debug.WriteLine(" 开始 执行时间：" + DateTime.Now + " 线程：" + obj);
                JOBS = JobProvider.GetJobs();
                if (JOBS != null && JOBS.Count > 0)
                {
                    DateTime nTime = DateTime.Now;
                    List<JobModel> models = JOBS.FindAll(m => m.NextTime.Value.ToString().Equals(nTime.ToString()));
                    if (models != null && models.Count > 0)
                    {
                        Console.WriteLine(" 开始 执行时间：" + DateTime.Now + " 总任务数：" + models.Count);
                        int num = 0;
                        models.ForEach(delegate (JobModel model)
                        {
                            model.IsTriggering = true;
                            model.ExecTime = nTime;
                            var expt = new CronExpression(model.Cron);
                            DateTime? ntimet = expt.GetNextValidTimeAfter(nTime);
                            if (ntimet != model.NextTime)
                            {
                                model.NextTime = ntimet;
                                SchedulerEventArgs arg = new SchedulerEventArgs();
                                arg.job = model;
                                //OnStartRunJob(this, arg);

                                Console.WriteLine(model.Name + " 开始 执行时间：" + DateTime.Now + " 下次开始时间：" + model.NextTime + " 线程：" + obj);
                                //TriggerProvider triggerProvider = new TriggerProvider();
                                //triggerProvider.Run(model);
                                Thread thread = new Thread(() =>
                                 {
                                     new TriggerProvider().Run(model);
                                     //OnCompleteRunJob(this, arg);
                                     Console.WriteLine(model.Name + " 完成 执行时间：" + DateTime.Now + " 下次开始时间：" + model.NextTime + " 线程：" + obj);
                                 });
                                thread.Start();
                                //OnCompleteRunJob(this, arg);
                                //Console.WriteLine(model.Name + " 完成 执行时间：" + DateTime.Now + " 下次开始时间：" + model.NextTime + " 线程：" + obj);
                            }
                            model.IsTriggering = false;
                            num++;
                        });
                        Console.WriteLine(" 完成 执行时间：" + DateTime.Now + " 总任务数：" + num);
                    }
                    RemoveExpiredJobs();
                    //Debug.WriteLine(" 结束 执行时间：" + DateTime.Now + " 线程：" + obj);
                }
            }
        }

        /// <summary>
        /// 移除过期任务
        /// </summary>
        private void RemoveExpiredJobs()
        {
            if (JOBS != null && JOBS.Count > 0)
            {
                JOBS = JOBS.FindAll(m => m.EndTime != null);
                if (JOBS != null && JOBS.Count > 0)
                {
                    DateTime nTime = DateTime.Now;
                    JOBS.RemoveAll(m => m.EndTime.Value < nTime);
                }
            }
        }

    }
}
