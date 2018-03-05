using DJS.Core.Common;
using DJS.Core.Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.Scheduler.JobManage
{
    public class JobProvider
    {
        public static List<JobModel> AsyncJobs(List<JobModel> jobs)
        {
            List<JobModel> models = new List<JobModel>();
            if (jobs != null && jobs.Count > 0)
            {
                models = jobs;
            }
            else
            {
                models.Add(new JobModel() { Name = "任务1", Cron = "0/5 * * * * ?", NextTime = null });
                models.Add(new JobModel() { Name = "任务2", Cron = "0/3 * * * * ?", NextTime = null });

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
            return models;
        }
    }
}
