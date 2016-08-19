using DJS.IDAL;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public class Jobs  
    {
        #region 属性
        /// <summary>
        /// 任务接口
        /// </summary>
        private static IDAL.IJobs iJobs = null;

        #endregion

        #region 构造函数

        static Jobs()
        {
            iJobs = DJS.DAL.DataAccessFactory.DataAccessFactory.CreateIJobs();
        }

        #endregion

        #region  获取所有Quartz中 Jobs +static List<Model.Jobs> GetJobsForQuartz()
        /// <summary>
        /// 获取所有Quartz中 Jobs
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetJobsForQuartz()
        {
            List<Model.Jobs> models = new List<Model.Jobs>();

            IList<string> groups = Common.QuartzHelp.quartzHelp.GetJobGroupNames();
            if (groups != null && groups.Count > 0)
            {
                foreach (string group in groups)
                {
                    ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(group);
                    if (jobkeys != null && jobkeys.Count > 0)
                    {
                        foreach (JobKey jobkey in jobkeys)
                        {
                            IJobDetail JobDetail = Common.QuartzHelp.quartzHelp.GetJobDetail(jobkey);
                            ITrigger iTrigger = Common.QuartzHelp.quartzHelp.GetTriggersOfJob(jobkey).FirstOrDefault();
                            
                            if (JobDetail != null)
                            {
                                Model.Jobs model = new Model.Jobs();
                                model.Name = jobkey.Name;
                                model.GroupName = jobkey.Group;
                                if (iTrigger != null)
                                {
                                    model.TriggerGroup = iTrigger.Key.Group;
                                    model.TriggerName = iTrigger.Key.Name;
                                }
                                models.Add(model);
                            }
                        }
                    }
                }
            }

            return models;
        }

        /// <summary>
        /// 获取所有Quartz中 Jobs
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetJobsForQuartz(Predicate<Model.Jobs> m)
        {
            List<Model.Jobs> models = GetJobsForQuartz();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(m);
            }
            return models;
        }
        #endregion

        #region  获取所有Quartz中 Jobs数量 +static int GetJobsForQuartzCount()
        /// <summary>
        /// 获取所有Quartz中 Jobs数量
        /// </summary>
        /// <returns></returns>
        public static int GetJobsForQuartzCount()
        {
            int counts = 0;
            List<Model.Jobs> models = GetJobsForQuartz();
            if (models != null && models.Count > 0)
            {
                counts = models.Count;
            }
            return counts;
        }
        #endregion

        #region 获取存储中的jobs + List<Model.Jobs> GetJobs()
        /// <summary>
        /// 获取存储中的jobs
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetJobs()
        {
            throw new NotImplementedException();
        } 
        #endregion

        #region 根据任务组和任务名称触发任务 +static bool TriggerJob(string groupName,string name)
        /// <summary>
        /// 根据任务组和任务名称触发任务
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool TriggerJob(string groupName,string name)
        {
            bool ret = true;

            ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(groupName);

            JobKey key = jobkeys.First(m=>m.Name==name&&m.Group==groupName);
            if (key != null)
            {
                ret = Common.QuartzHelp.quartzHelp.TriggerJob(key);
            }

            return ret;
        } 
        #endregion
    }
}
