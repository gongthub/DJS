using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common
{
    /// <summary>
    /// Quartz 帮助类
    /// </summary>
    public class QuartzHelp
    {
        /// <summary>
        /// 获取程序集文件所在文件夹名称
        /// </summary>
        private static string IQUARTZ = ConfigHelp.IQuartzPath;

        static ISchedulerFactory _sf = new StdSchedulerFactory();
        static IScheduler _sched = _sf.GetScheduler();
        static object lockObj = new object();

        #region 单例模式创建对象
        //单例模式创建对象
        private static QuartzHelp _quartzHelp = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        QuartzHelp()
        {
        }

        public static QuartzHelp quartzHelp
        {
            get
            {
                // Double-Checked Locking
                if (null == _quartzHelp)
                {
                    lock (SynObject)
                    {
                        if (null == _quartzHelp)
                        {
                            _quartzHelp = new QuartzHelp();
                        }
                    }
                }
                return _quartzHelp;
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// InStandbyMode
        /// </summary>
        public bool InStandbyMode
        {
            get { return _sched.InStandbyMode; }
        }
        /// <summary>
        /// IsShutdown
        /// </summary>
        public bool IsShutdown
        {
            get { return _sched.IsShutdown; }
        }
        /// <summary>
        /// IsStarted
        /// </summary>
        public bool IsStarted
        {
            get { return _sched.IsStarted; }
        }
        #endregion

        #region 获取状态
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        public string GetState()
        {
            string str = "";

            if (IsStarted)
            {
                str = "正常运行";
            }
            else
                if (IsShutdown)
                {
                    str = "关闭";
                }
                else
                    if (InStandbyMode)
                    {
                        str = "待机模式";
                    }

            return str;
        } 
        #endregion

        #region 开始
        /// <summary>
        /// 开始
        /// </summary>
        public void Start<T>()
        {
            //作业一
            DateTime time = Convert.ToDateTime("2014/06/13 14:18:10");
            // DateTime time2 = Convert.ToDateTime("2014/06/15 11:09:20");
            DateTime time2 = Convert.ToDateTime(DateTime.Now.AddSeconds(5));
            DateTime time3 = Convert.ToDateTime("2014/06/13 14:18:30");
            //job = JobBuilder.Create<WriteLogJob>().WithIdentity("job1", "group1").Build();
            //ITrigger trigger = TriggerBuilder.Create().WithIdentity("name1","group1").StartAt(time).Build();
            //_sched.ScheduleJob(job, trigger);
            Type types = typeof(T);
            IJobDetail job2 = JobBuilder.Create(types).WithIdentity("job2", "group1").Build();
            //作业二
            //IJobDetail job2 = JobBuilder.Create<T>().WithIdentity("job2", "group1").Build();
            ITrigger triggers = TriggerBuilder.Create().WithIdentity("name2", "group1").StartAt(time2).Build();
            _sched.ScheduleJob(job2, triggers);

            ////一个触发器对应多个JOb
            //IJobDetail job3 = job.
            //ITrigger trigger3 = TriggerBuilder.Create().WithIdentity("name3", "group1").StartAt(time3).Build();
            //_sched.ScheduleJob(job3, trigger3);


            //JobKey jobkey = new JobKey("myjob", "mygroup");
            //IJobDetail job4 = JobBuilder.Create<WriteLogJob>().WithIdentity(jobkey).Build();
            //ITrigger trigger = TriggerBuilder.Create().StartNow().Build(); 
            //比较复杂的应用 
            //IOperableTrigger trigger = new CronTriggerImpl("trigName", "group1", "0/2 * * * * ?"); 
            //简单方式 
            // SimpleTriggerImpl trigger = new SimpleTriggerImpl("simpleTrig", "simpleGroup", 10, DateTime.Now.AddSeconds(5) - DateTime.Now);
            //_sched.ScheduleJob(job4, trigger);  

            _sched.Start();//开始
        }

        #region 开始所有任务 +void Start()
        /// <summary>
        /// 开始所有任务
        /// </summary>
        public void Start()
        {
            _sched.Start();//开始
        }


        #endregion

        #endregion

        #region 停止任务

        #region 停止所有任务 +void Shutdown()
        /// <summary>
        /// 停止所有任务
        /// </summary>
        public void Shutdown()
        {
            _sched.Shutdown(true);
        }
        #endregion

        #region 待机所有任务 +void Standby()
        /// <summary>
        /// 待机所有任务
        /// </summary>
        public void Standby()
        {
            _sched.Standby();
        }
        #endregion

        #region 暂停所有任务 +void PauseAll()
        /// <summary>
        /// 暂停所有任务
        /// </summary>
        public void PauseAll()
        {
            _sched.PauseAll();
        }
        #endregion

        #region 暂停指定任务 +void PauseJob(JobKey jobKey)
        /// <summary>
        /// 暂停指定任务
        /// </summary>
        public void PauseJob(JobKey jobKey)
        {
            _sched.PauseJob(jobKey);
        }
        #endregion

        #region 暂停指定任务集合 +void PauseJobs(GroupMatcher<JobKey> matcher)
        /// <summary>
        /// 暂停指定任务集合
        /// </summary>
        public void PauseJobs(GroupMatcher<JobKey> matcher)
        {
            _sched.PauseJobs(matcher);
        }
        #endregion

        #endregion

        #region 停止触发器

        #region 暂停指定触发器 +void PauseTrigger(TriggerKey triggerKey)
        /// <summary>
        /// 暂停指定触发器
        /// </summary>
        public void PauseTrigger(TriggerKey triggerKey)
        {
            _sched.PauseTrigger(triggerKey);
        }
        #endregion

        #region 暂停指定触发器集合 +void PauseTriggers(GroupMatcher<TriggerKey> matcher)
        /// <summary>
        /// 暂停指定触发器集合
        /// </summary>
        public void PauseTriggers(GroupMatcher<TriggerKey> matcher)
        {
            _sched.PauseTriggers(matcher);
        }
        #endregion

        #endregion

        #region 继续任务

        #region 继续所有任务 +void ResumeAll()
        /// <summary>
        /// 继续所有任务
        /// </summary>
        public void ResumeAll()
        {
            _sched.ResumeAll();
        }
        #endregion

        #region 继续指定任务 +void ResumeJob(JobKey jobKey)
        /// <summary>
        /// 继续指定任务
        /// </summary>
        public void ResumeJob(JobKey jobKey)
        {
            _sched.ResumeJob(jobKey);
        }
        #endregion

        #region 继续指定任务集合 +void ResumeJobs(GroupMatcher<JobKey> matcher)
        /// <summary>
        /// 继续指定任务集合
        /// </summary>
        public void ResumeJobs(GroupMatcher<JobKey> matcher)
        {
            _sched.ResumeJobs(matcher);
        }
        #endregion

        #endregion

        #region 继续触发器

        #region 继续指定触发器 +void ResumeTrigger(TriggerKey triggerKey)
        /// <summary>
        /// 继续指定触发器
        /// </summary>
        public void ResumeTrigger(TriggerKey triggerKey)
        {
            _sched.ResumeTrigger(triggerKey);
        }
        #endregion

        #region 继续指定触发器集合 +void ResumeTriggers(GroupMatcher<TriggerKey> matcher)
        /// <summary>
        /// 继续指定触发器集合
        /// </summary>
        public void ResumeTriggers(GroupMatcher<TriggerKey> matcher)
        {
            _sched.ResumeTriggers(matcher);
        }
        #endregion

        #endregion

        #region 删除

        #region 删除指定任务 +bool DeleteJob(JobKey jobkey)
        /// <summary>
        /// 删除指定任务
        /// </summary> 
        /// <param name="jobkey"></param> 
        public bool DeleteJob(JobKey jobkey)
        {
            return _sched.DeleteJob(jobkey);
        }
        #endregion

        #region 删除指定任务集合 +bool DeleteJobs(IList<JobKey> jobKeys)
        /// <summary>
        /// 删除指定任务集合
        /// </summary> 
        /// <param name="jobkey"></param> 
        public bool DeleteJobs(IList<JobKey> jobKeys)
        {
            return _sched.DeleteJobs(jobKeys);
        }
        #endregion

        #endregion

        #region 添加任务 +void AddJob(IJobDetail jobDetail, bool replace)
        /// <summary>
        /// 添加任务
        /// </summary> 
        /// <param name="jobkey"></param>
        /// <param name="replace"></param>
        public void AddJob(IJobDetail jobDetail, bool replace)
        {
            //_sched.ListenerManager.AddJobListener(myJobListener, KeyMatcher<JobKey>.KeyEquals(new JobKey("myJobName", "myJobGroup")));

            _sched.AddJob(jobDetail, replace);
        }
        #endregion

        #region 添加一个job +void AddJob(Type type, string jobName, string jobGroup)
        /// <summary>
        /// 添加一个job
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobName"></param>
        /// <param name="jobGroup"></param>
        public void AddJob(Type type, string jobName, string jobGroup)
        {
            JobKey jobkey = new JobKey(jobName, jobGroup);
            IJobDetail job = JobBuilder.Create(type).WithIdentity(jobkey).Build();
            ITrigger trigger = TriggerBuilder.Create().StartNow().Build();
            //比较复杂的应用 
            //IOperableTrigger trigger = new CronTriggerImpl("trigName", "group1", "0/2 * * * * ?"); 
            //简单方式 
            // SimpleTriggerImpl trigger = new SimpleTriggerImpl("simpleTrig", "simpleGroup", 10, DateTime.Now.AddSeconds(5) - DateTime.Now);
            _sched.ScheduleJob(job, trigger);
            //_sched.Start();//开始
        }
        /// <summary>
        /// 添加一个job
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobName"></param>
        /// <param name="jobGroup"></param>
        public MessageHelp AddJob(Type type, DateTime time, string jobName, string jobGroup, string triggerName, string triggerGroup)
        {
            MessageHelp mess = new MessageHelp();
            if (!ExistJob(new JobKey(jobName, jobGroup)))
            {

                if (!ExistTrigger(new TriggerKey(triggerName, triggerGroup)))
                {
                    IJobDetail job = JobBuilder.Create(type).WithIdentity(jobName, jobGroup).Build();
                    //3.创建并配置一个触发器
                    //ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInSeconds(3).WithRepeatCount(int.MaxValue)).Build();
                    ITrigger trigger = TriggerBuilder.Create().WithIdentity(triggerName, triggerGroup).StartAt(time).Build();
                    //4.加入作业调度池中
                    _sched.ScheduleJob(job, trigger);

                    mess.State = true;
                }
                else
                {
                    mess.State = false;
                    mess.Message = "触发器名称已经存在，请重新输入！";
                }
            }
            else
            {
                mess.State = false;
                mess.Message = "任务名称已经存在，请重新输入！";
            }
            return mess;

        }

        /// <summary>
        /// 添加一个job
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobName"></param>
        /// <param name="jobGroup"></param>
        public MessageHelp AddJob(Type type, string crons, string jobName, string jobGroup, string triggerName, string triggerGroup)
        {
            MessageHelp mess = new MessageHelp();
            if (!ExistJob(new JobKey(jobName, jobGroup)))
            {
                if (!ExistTrigger(new TriggerKey(triggerName, triggerGroup)))
                {
                    IJobDetail job = JobBuilder.Create(type).WithIdentity(jobName, jobGroup).Build();

                    //比较复杂的应用 
                    IOperableTrigger trigger = new CronTriggerImpl(triggerName, triggerGroup, crons);
                    // 加入作业调度池中
                    _sched.ScheduleJob(job, trigger);
                    mess.Message = "添加成功！";
                    mess.State = true;
                }
                else
                {
                    mess.State = false;
                    mess.Message = "触发器名称已经存在，请重新输入！";
                }
            }
            else
            {
                mess.State = false;
                mess.Message = "任务名称已经存在，请重新输入！";
            }
            return mess;

        }
        #endregion

        #region 暂停指定任务计划 +bool StopScheduleJob(string jobGroup, string jobName)
        /// <summary>
        /// 暂停指定任务计划
        /// </summary>
        /// <returns></returns>
        public bool StopScheduleJob(string jobGroup, string jobName)
        {
            try
            {
                _sched.PauseJob(new JobKey(jobName, jobGroup));
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 开启指定的任务计划 +bool RunScheduleJob(string jobGroup, string jobName)
        /// <summary>
        /// 开启指定的任务计划
        /// </summary>
        /// <returns></returns>
        public bool RunScheduleJob(string jobGroup, string jobName)
        {
            try
            {
                _sched.ResumeJob(new JobKey(jobName, jobGroup));
                return true;
            }
            catch 
            {
                return false;
            }
        }
        #endregion

        #region 获取所有任务组 +IList<string> GetJobGroupNames()
        /// <summary>
        /// 获取所有任务组
        /// </summary>
        /// <returns></returns>
        public IList<string> GetJobGroupNames()
        {
            return _sched.GetJobGroupNames();
        }
        #endregion

        #region 获取正在执行的任务 +IList<IJobExecutionContext> GetCurrentlyExecutingJobs()
        /// <summary>
        /// 获取正在执行的任务
        /// </summary>
        /// <returns></returns>
        public IList<IJobExecutionContext> GetCurrentlyExecutingJobs()
        {
            IList<IJobExecutionContext> list = _sched.GetCurrentlyExecutingJobs();
            return list;
        }
        #endregion

        #region 根据任务组组获取任务 +ISet<JobKey> GetJobKeys(string groupName)
        /// <summary>
        /// 根据任务组组获取任务
        /// </summary>
        /// <returns></returns>
        public ISet<JobKey> GetJobKeys(string groupName)
        {
            GroupMatcher<JobKey> matcher = GroupMatcher<JobKey>.GroupEquals(groupName);
            ISet<JobKey> jobKeys = (ISet<JobKey>)_sched.GetJobKeys(matcher);
            return jobKeys;
        }
        #endregion

        #region 获取一个job任务 +IJobDetail GetJobDetail(JobKey jobKey)
        /// <summary>
        /// 获取一个job任务
        /// </summary>
        /// <returns></returns>
        public IJobDetail GetJobDetail(JobKey jobKey)
        {
            IJobDetail job = _sched.GetJobDetail(jobKey);
            return job;
        }
        #endregion

        #region 获取所有触发器组名 +IList<string> GetTriggerGroupNames()
        /// <summary>
        /// 获取所有触发器组名
        /// </summary>
        /// <returns></returns>
        public IList<string> GetTriggerGroupNames()
        {
            return _sched.GetTriggerGroupNames();
        }
        #endregion

        #region 根据触发器组获取触发器 +ISet<TriggerKey> GetTriggerKeys(string groupName)
        /// <summary>
        /// 根据触发器组获取触发器
        /// </summary>
        /// <returns></returns>
        public ISet<TriggerKey> GetTriggerKeys(string groupName)
        {
            GroupMatcher<TriggerKey> matcher = GroupMatcher<TriggerKey>.GroupEquals(groupName);
            ISet<TriggerKey> triggerKeys = (ISet<TriggerKey>)_sched.GetTriggerKeys(matcher);
            return triggerKeys;
        }
        #endregion

        #region 获取一个触发器 +ITrigger GetTrigger(TriggerKey triggerKey)
        /// <summary>
        /// 获取一个触发器
        /// </summary>
        /// <param name="triggerKey">触发器</param>
        /// <returns></returns>
        public ITrigger GetTrigger(TriggerKey triggerKey)
        {
            ITrigger iTrigger = _sched.GetTrigger(triggerKey);
            return iTrigger;
        }
        #endregion

        #region 获取根据jobkey获取触发器 +IList<ITrigger> GetTriggersOfJob(JobKey jobKey)
        /// <summary>
        /// 获取根据jobkey获取触发器
        /// </summary>
        /// <param name="jobKey">任务</param>
        /// <returns></returns>
        public IList<ITrigger> GetTriggersOfJob(JobKey jobKey)
        {
            IList<ITrigger> iTriggers = _sched.GetTriggersOfJob(jobKey);
            return iTriggers;
        }
        #endregion

        #region 判断任务是否存在 +bool ExistJob(JobKey jobKey)
        /// <summary>
        /// 判断任务是否存在
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns>true:存在 false：不存在</returns>
        public bool ExistJob(JobKey jobKey)
        {

            return _sched.CheckExists(jobKey);

        }
        #endregion

        #region 判断触发器是否存在 +bool ExistTrigger(TriggerKey triggerKey)
        /// <summary>
        /// 判断触发器是否存在
        /// </summary>
        /// <param name="triggerKey">触发器</param>
        /// <returns>true:存在 false：不存在</returns>
        public bool ExistTrigger(TriggerKey triggerKey)
        {
            return _sched.CheckExists(triggerKey);
        }
        #endregion

        #region 获取所有实现IJob接口类名 +List<string> GetIClassName(string name,string nameSpace)
        /// <summary>
        /// 获取所有实现IJob接口类名
        /// </summary>
        /// <returns></returns>
        public ArrayList GetIClassName(string name,string nameSpace)
        {
            ArrayList strs = new ArrayList();
            List<Type> types = Common.AssemblyHelp.GetDllTypeNames(name,nameSpace);

            if (types != null && types.Count > 0)
            {
                foreach (Type type in types)
                {
                    if (type.GetInterface(IQUARTZ) != null)
                    {
                        strs.Add(type.Name);
                    }
                }
            }
            return strs;

        }
        #endregion

        #region 触发一个任务 +void TriggerJob(JobKey jobKey)
        /// <summary>
        /// 触发一个任务
        /// </summary>
        /// <param name="jobKey">任务</param>
        /// <returns></returns>
        public bool TriggerJob(JobKey jobKey)
        {
            bool ret = true;
            try
            {
                _sched.TriggerJob(jobKey);
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        #endregion

        #region 根据触发器获取触发器状态 +TriggerState GetTriggerState(TriggerKey triggerKey)
        /// <summary>
        /// 根据触发器获取触发器状态
        /// </summary>
        /// <param name="triggerKey">触发器</param>
        /// <returns></returns>
        public TriggerState GetTriggerState(TriggerKey triggerKey)
        {
            return _sched.GetTriggerState(triggerKey);
        }
        #endregion

    }
}
