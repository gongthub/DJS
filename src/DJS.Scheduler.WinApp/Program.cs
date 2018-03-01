using DJS.Core.Common;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DJS.Scheduler.WinApp
{
    public class JobModel
    {
        public string Name { get; set; }
        public string Cron { get; set; }
        public DateTime? NextTime { get; set; }
    }
    class Program
    {
        private static List<JobModel> JOBS = new List<JobModel>();
        static void Main(string[] args)
        {
            JOBS.Add(new JobModel() { Name = "任务1", Cron = "0/5 * * * * ?", NextTime = null });
            JOBS.Add(new JobModel() { Name = "任务2", Cron = "0/3 * * * * ?", NextTime = null });
            JOBS.Add(new JobModel() { Name = "任务3", Cron = "0/2 * * * * ?", NextTime = null });

            if (JOBS != null && JOBS.Count > 0)
            {
                JOBS.ForEach(delegate (JobModel model)
                {
                    DateTime tTime = DateTime.Now;
                    var exp = new CronExpression(model.Cron);
                    DateTime? ntimet = exp.GetNextValidTimeAfter(tTime);
                    model.NextTime = ntimet;
                });
            }
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);
            for (int i = 1; i <= 10; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(testFun), i.ToString());
            }
            //testFun(1);
            Console.ReadKey();
        }
        public static void testFun(object obj)
        {
            while (true)
            {
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
                                Console.WriteLine(DateTime.Now.ToString() + " " + model.Name + " " + ntimet);
                            }
                        });
                    }
                }
            }
        }
    }
}
