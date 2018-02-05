using DJS.Common;
using DJS.IDAL;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
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

                            TriggerState state = Common.QuartzHelp.quartzHelp.GetTriggerState(iTrigger.Key);

                            if (JobDetail != null)
                            {
                                Model.Jobs model = new Model.Jobs();
                                model.Name = jobkey.Name;
                                model.GroupName = jobkey.Group;
                                if (iTrigger != null)
                                {
                                    model.TriggerGroup = iTrigger.Key.Group;
                                    model.TriggerName = iTrigger.Key.Name;
                                    model.State = (int)state;
                                }
                                model.AddPoolTime = iTrigger.StartTimeUtc.LocalDateTime;
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
        /// <summary>
        /// 获取所有Quartz中 Jobs
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetJobsForQuartz(Pagination pagination, Expression<Func<Model.Jobs, bool>> predicate)
        {
            List<Model.Jobs> models = GetJobsForQuartz();
            IQueryable<Model.Jobs> tempData = models.AsQueryable().Where(predicate);
            bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sidx.Split(',');
            MethodCallExpression resultExp = null;
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(Model.Jobs), "t");
                var property = typeof(Model.Jobs).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(Model.Jobs), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<Model.Jobs>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<Model.Jobs>(pagination.rows * (pagination.page - 1)).Take<Model.Jobs>(pagination.rows).AsQueryable();
            return tempData.ToList();
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

        #region 根据任务组和任务名称触发任务 +static bool TriggerJob(string groupName,string name)
        /// <summary>
        /// 根据任务组和任务名称触发任务
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool TriggerJob(string groupName, string name)
        {
            bool ret = true;

            ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(groupName);

            if (jobkeys != null && jobkeys.Count > 0)
            {
                JobKey key = jobkeys.First(m => m.Name == name && m.Group == groupName);
                if (key != null)
                {
                    ret = Common.QuartzHelp.quartzHelp.TriggerJob(key);
                }
            }
            else
            {
                ret = false;
            }

            return ret;
        }
        #endregion

        #region 根据任务ID触发任务 +static bool TriggerJob(string jobID)
        /// <summary>
        /// 根据任务ID触发任务
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool TriggerJob(string jobID)
        {
            bool ret = false;

            Model.Jobs model = GetForm(jobID);
            if (model != null)
            {
                ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(model.GroupName);
                if (jobkeys != null && jobkeys.Count > 0)
                {
                    JobKey key = jobkeys.First(m => m.Name == model.Name && m.Group == model.GroupName);
                    if (key != null)
                    {
                        ret = Common.QuartzHelp.quartzHelp.TriggerJob(key);
                    }
                }
                else
                {
                    ret = false;
                }
            }

            return ret;
        }
        #endregion

        #region 根据任务组和任务名称暂停任务 +static bool PauseJob(string groupName,string name)
        /// <summary>
        /// 根据任务组和任务名称暂停任务
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool PauseJob(string groupName, string name)
        {
            bool ret = true;

            ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(groupName);

            if (jobkeys != null && jobkeys.Count > 0)
            {
                JobKey key = jobkeys.First(m => m.Name == name && m.Group == groupName);
                if (key != null)
                {
                    Common.QuartzHelp.quartzHelp.PauseJob(key);
                }
            }
            else
            {
                ret = false;
            }

            return ret;
        }
        #endregion

        #region 根据jobID暂停任务 +static bool PauseJob(string jobID)
        /// <summary>
        /// 根据jobID暂停任务
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool PauseJob(string jobID)
        {
            bool ret = false;
            Model.Jobs model = GetForm(jobID);
            if (model != null)
            {
                ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(model.GroupName);

                if (jobkeys != null && jobkeys.Count > 0)
                {
                    JobKey key = jobkeys.First(m => m.Name == model.Name && m.Group == model.GroupName);
                    if (key != null)
                    {
                        Common.QuartzHelp.quartzHelp.PauseJob(key);
                        ret = true;
                    }
                }
                else
                {
                    ret = false;
                }
            }

            return ret;
        }
        #endregion

        #region 根据任务组和任务名称继续任务 +static bool ResumeJob(string groupName,string name)
        /// <summary>
        /// 根据任务组和任务名称继续任务
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool ResumeJob(string groupName, string name)
        {
            bool ret = true;

            ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(groupName);

            if (jobkeys != null && jobkeys.Count > 0)
            {
                JobKey key = jobkeys.First(m => m.Name == name && m.Group == groupName);
                if (key != null)
                {
                    Common.QuartzHelp.quartzHelp.ResumeJob(key);
                }
            }
            else
            {
                ret = false;
            }

            return ret;
        }
        #endregion

        #region 根据jobID继续任务 +static bool ResumeJob(string jobID)
        /// <summary>
        /// 根据jobID继续任务
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool ResumeJob(string jobID)
        {
            bool ret = false;
            Model.Jobs model = GetForm(jobID);
            if (model != null)
            {
                ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(model.GroupName);

                if (jobkeys != null && jobkeys.Count > 0)
                {
                    JobKey key = jobkeys.First(m => m.Name == model.Name && m.Group == model.GroupName);
                    if (key != null)
                    {
                        Common.QuartzHelp.quartzHelp.ResumeJob(key);
                        ret = true;
                    }
                }
                else
                {
                    ret = false;
                }
            }
            return ret;
        }
        #endregion

        #region 根据id判断JobKey是否存在 +static bool IsExistJobkeyById(string Id)
        /// <summary>
        /// 根据id判断JobKey是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool IsExistJobkeyById(string Id)
        {
            bool retState = false;
            JobKey key = new JobKey("", "");
            Model.Jobs model = GetModelById(Id);
            if (model != null)
            {
                ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(model.GroupName);
                if (jobkeys != null && jobkeys.Count > 0)
                {
                    key = jobkeys.FirstOrDefault(m => m.Name == model.Name && m.Group == model.GroupName);
                    if (key != null)
                    {
                        retState = true;
                    }
                }
            }
            return retState;
        }
        #endregion

        #region 获取所有任务 +List<Model.Jobs> GetJobs()
        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetJobs()
        {
            List<Model.Jobs> models = new List<Model.Jobs>();
            List<Model.Jobs> quartzs = GetJobsForQuartz();
            models = GetModels();
            if (models != null && models.Count > 0)
            {
                models.ForEach(delegate(Model.Jobs job)
                {
                    job.State = (int)Enums.TriggerState.Complete;
                    job.StateName = Common.EnumHelp.enumHelp.GetDescription(Enums.TriggerState.Complete);

                    Enums.TimeType st = (Enums.TimeType)job.Type;
                    job.TypeName = Common.EnumHelp.enumHelp.GetDescription(st);
                });
                foreach (Model.Jobs model in models)
                {
                    Model.Jobs job = quartzs.Find(m => m.Name == model.Name);
                    if (job != null)
                    {
                        model.State = job.State;
                        Enums.TriggerState st = (Enums.TriggerState)job.State;
                        model.StateName = Common.EnumHelp.enumHelp.GetDescription(st);
                    }
                }
            }
            return models;
        }
        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetJobs(Predicate<Model.Jobs> m)
        {
            List<Model.Jobs> models = GetJobs();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(m);
            }
            return models;
        }
        #endregion

        #region 添加任务包括添加到Quartz +bool AddJobs(string jobID)
        /// <summary>
        /// 添加任务包括添加到Quartz
        /// </summary>
        /// <returns></returns>
        public static bool AddJobs(string jobID)
        {
            if (IsExistJobkeyById(jobID))
            {
                throw new Exception("该任务已在任务池，禁止重复添加！");
            }
            bool ret = true;
            try
            {
                Model.Jobs model = GetForm(jobID);
                if (model != null)
                {
                    if (model.Type == (int)Enums.TimeType.Periodicity)
                    {
                        //model.Crons = "0 0 0 ? * MON";
                        Common.QuartzHelp.quartzHelp.AddJob(model.AssType, model.Crons, model.Name, model.GroupName, model.TriggerName, model.TriggerGroup);
                    }
                    if (model.Type == (int)Enums.TimeType.Disposable)
                    {
                        Common.QuartzHelp.quartzHelp.AddJob(model.AssType, model.Time, model.Name, model.GroupName, model.TriggerName, model.TriggerGroup);
                    }
                }
                else
                {
                    ret = false;
                }

            }
            catch (Exception ex)
            {
                ret = false;
                Common.LogHelp.logHelp.WriteLog(ex.Message, Enums.LogType.Error);
            }
            return ret;
        }
        #endregion

        #region 添加任务包括添加到Quartz +bool AddJobs(Model.Jobs model)
        /// <summary>
        /// 添加任务包括添加到Quartz
        /// </summary>
        /// <returns></returns>
        public static bool AddJobs(Model.Jobs model)
        {
            bool ret = true;
            try
            {
                if (model.Type == (int)Enums.TimeType.Periodicity)
                {
                    //model.Crons = "0 0 0 ? * MON";
                    Common.QuartzHelp.quartzHelp.AddJob(model.AssType, model.Crons, model.Name, model.GroupName, model.TriggerName, model.TriggerGroup);
                }
                if (model.Type == (int)Enums.TimeType.Disposable)
                {
                    Common.QuartzHelp.quartzHelp.AddJob(model.AssType, model.Time, model.Name, model.GroupName, model.TriggerName, model.TriggerGroup);
                }
                ret = iJobs.AddForm(model);

            }
            catch (Exception ex)
            {
                ret = false;
                Common.LogHelp.logHelp.WriteLog(ex.Message, Enums.LogType.Error);
            }
            return ret;
        }
        #endregion

        #region 根据id获取JobKey -static JobKey GetJobkeyById(string Id)
        /// <summary>
        /// 根据id获取JobKey
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static JobKey GetJobkeyById(string Id)
        {
            JobKey key = new JobKey("", "");
            Model.Jobs model = GetModelById(Id);
            if (model != null)
            {
                ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(model.GroupName);
                if (jobkeys != null && jobkeys.Count > 0)
                {
                    key = jobkeys.First(m => m.Name == model.Name && m.Group == model.GroupName);
                }
            }
            return key;
        }
        #endregion

        #region 根据id删除Quartz中任务 +static bool DelByIdForQuartz(string Id)
        /// <summary>
        /// 根据id删除Quartz中任务
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelByIdForQuartz(string Id)
        {
            bool ret = false;
            try
            {
                JobKey key = GetJobkeyById(Id);
                if (key != null && key.Name != "")
                {
                    ret = Common.QuartzHelp.quartzHelp.DeleteJob(key);
                }
            }
            catch (Exception ex)
            {
                ret = false;
                Common.LogHelp.logHelp.WriteLog(ex.Message, Enums.LogType.Error);
            }
            return ret;
        }
        #endregion

        #region 根据id删除数据包括删除Quartz +static bool DelByIdAndQuartz(string Id)
        /// <summary>
        /// 根据id删除数据包括删除Quartz
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelByIdAndQuartz(string Id)
        {
            bool ret = true;
            try
            {
                JobKey key = GetJobkeyById(Id);
                if (key != null && key.Name != "")
                {
                    ret = Common.QuartzHelp.quartzHelp.DeleteJob(key);
                }
                ret = RemoveByID(Id);
            }
            catch (Exception ex)
            {
                ret = false;
                Common.LogHelp.logHelp.WriteLog(ex.Message, Enums.LogType.Error);
            }
            return ret;
        }
        #endregion

        #region 重新添加任务Quartz +bool ReAddJobs(Model.Jobs model)
        /// <summary>
        /// 重新添加任务Quartz
        /// </summary>
        /// <returns></returns>
        public static bool ReAddJobs(Model.Jobs model)
        {
            bool ret = true;
            try
            {
                if (model.Type == (int)Enums.TimeType.Periodicity)
                {
                    Common.QuartzHelp.quartzHelp.AddJob(model.AssType, model.Crons, model.Name, model.GroupName, model.TriggerName, model.TriggerGroup);
                }
                if (model.Type == (int)Enums.TimeType.Disposable)
                {
                    Common.QuartzHelp.quartzHelp.AddJob(model.AssType, model.Time, model.Name, model.GroupName, model.TriggerName, model.TriggerGroup);
                }

            }
            catch (Exception ex)
            {
                ret = false;
                Common.LogHelp.logHelp.WriteLog(ex.Message, Enums.LogType.Error);
            }
            return ret;
        }
        #endregion

        #region 获取任务数据 +static List<Model.Jobs> GetModels()
        /// <summary>
        /// 获取任务数据
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetModels()
        {
            return iJobs.GetModels();
        }
        /// <summary>
        /// 获取任务数据
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetModels(Predicate<Model.Jobs> pre)
        {
            List<Model.Jobs> models = GetModels();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(pre);
            }
            return models;
        }
        #endregion

        #region 根据ID获取任务数据 +static Model.Jobs GetModelById(string Id)
        /// <summary>
        /// 根据ID获取任务数据
        /// </summary>
        /// <returns></returns>
        public static Model.Jobs GetModelById(string Id)
        {
            Model.Jobs model = iJobs.GetForm(Id);
            return model;
        }
        #endregion

        #region 根据名称判断是否存在 +static bool IsExist(string name)
        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>true:存在 false:不存在</returns>
        public static bool IsExist(string name)
        {
            return iJobs.IsExist(name);
        }
        /// <summary>
        /// 验证名称是否已存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsExistName(string name, string keyvalue = "")
        {
            bool bSatae = false;
            if (!string.IsNullOrEmpty(keyvalue))
            {
                List<Model.Jobs> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.Jobs model = models.Where(m => m.Name == name && m.ID != keyvalue).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            else
            {
                List<Model.Jobs> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.Jobs model = models.Where(m => m.Name == name).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            return bSatae;
        }

        /// <summary>
        /// 验证触发器名称是否已存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsExistTriggerName(string name, string keyvalue = "")
        {
            bool bSatae = false;
            if (!string.IsNullOrEmpty(keyvalue))
            {
                List<Model.Jobs> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.Jobs model = models.Where(m => m.TriggerName == name && m.ID != keyvalue).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            else
            {
                List<Model.Jobs> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.Jobs model = models.Where(m => m.TriggerName == name).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            return bSatae;
        }
        /// <summary>
        /// 验证配置名称是否已存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsExistConfigName(string name, string keyvalue = "")
        {
            bool bSatae = false;
            if (!string.IsNullOrEmpty(keyvalue))
            {
                List<Model.Jobs> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.Jobs model = models.Where(m => m.ConfigName == name && m.ID != keyvalue).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            else
            {
                List<Model.Jobs> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.Jobs model = models.Where(m => m.ConfigName == name).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            return bSatae;
        }
        #endregion

        #region 添加 +static bool Add(Model.Jobs model)
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Add(Model.Jobs model)
        {
            return iJobs.AddForm(model);
        }
        #endregion

        #region 根据id设置配置信息 +static bool SetConfigById(string Id)
        /// <summary>
        /// 根据id设置配置信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool SetConfigById(string Id)
        {
            bool ret = true;
            try
            {
                Model.Jobs model = GetModelById(Id);
                if (model != null)
                {
                    Model.DllMgr ddlmgr = BLL.DllMgr.GetModelById(model.DLLID);
                    if (ddlmgr != null)
                    {
                        DJS.SDK.IConfigClient iconfig = (DJS.SDK.IConfigClient)Common.AssemblyHelp.assembly.GetDllTypeI(model.DLLName, model.AssType.Namespace, model.AssType.Name);
                        iconfig.SetConfig(model.Name);
                    }
                }
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        #endregion

        #region 启动自动启动任务 +static void StartAutoJobs()
        /// <summary>
        /// 启动自动启动任务
        /// </summary>
        public static void StartAutoJobs()
        {
            try
            {
                List<Model.Jobs> models = BLL.Jobs.GetJobs();
                if (models != null && models.Count > 0)
                {
                    models = models.FindAll(m => m.IsAuto == true);
                }
                if (models != null && models.Count > 0)
                {
                    foreach (Model.Jobs model in models)
                    {
                        if (model != null)
                        {
                            if (model.State == (int)TriggerState.Complete)
                            {
                                if (BLL.Jobs.ReAddJobs(model))
                                {
                                    Common.LogHelp.logHelp.WriteLog("启动自启动任务成功！任务名称：" + model.Name, 0);
                                }
                                else
                                {
                                    Common.LogHelp.logHelp.WriteLog("启动自启动任务失败！任务名称：" + model.Name, 0);
                                }
                            }
                        }
                    }
                    Common.QuartzHelp.quartzHelp.Start();
                }
            }
            catch (Exception ex)
            {
                Common.LogHelp.logHelp.WriteLog("启动自启动任务失败！" + ex.Message, 1);
            }
        }
        #endregion

        /// <summary>
        /// 处理任务状态
        /// </summary>
        /// <param name="models"></param>
        /// <param name="quartzs"></param>
        private static void InitJobs(List<Model.Jobs> quartzs)
        {
            if (quartzs != null && quartzs.Count > 0)
            {
                quartzs.ForEach(delegate(Model.Jobs job)
                {
                    Model.Jobs model = GetFormByName(job.Name);
                    if (model != null)
                    {
                        job.ID = model.ID;
                        job.Name = model.Name;
                        job.GroupName = model.GroupName;
                        job.TriggerName = model.TriggerName;
                        job.TriggerGroup = model.TriggerGroup;
                        job.Crons = model.Crons;
                        job.Time = model.Time;
                        job.Type = model.Type;
                        job.TypeName = model.TypeName;
                        job.AssType = model.AssType;
                        job.ClassName = model.ClassName;
                        job.DLLID = model.DLLID;
                        job.DLLName = model.DLLName;
                        job.ConfigName = model.ConfigName;
                        job.IsAuto = model.IsAuto;
                        job.DllVersion = model.DllVersion;
                        job.Description = model.Description;
                        job.CreatorTime = model.CreatorTime;
                    }
                    job.IsAddPool = true;

                    Enums.TimeType st = (Enums.TimeType)job.Type;
                    job.TypeName = Common.EnumHelp.enumHelp.GetDescription(st);

                    Enums.TriggerState sts = (Enums.TriggerState)job.State;
                    job.StateName = Common.EnumHelp.enumHelp.GetDescription(sts);
                });
            }
        }

        /// <summary>
        /// 处理任务状态
        /// </summary>
        /// <param name="models"></param>
        /// <param name="quartzs"></param>
        private static void InitJobs(List<Model.Jobs> models, List<Model.Jobs> quartzs)
        {
            if (models != null && models.Count > 0)
            {
                models.ForEach(delegate(Model.Jobs job)
                {
                    job.State = (int)Enums.TriggerState.None;
                    job.StateName = Common.EnumHelp.enumHelp.GetDescription(Enums.TriggerState.Complete);

                    Enums.TimeType st = (Enums.TimeType)job.Type;
                    job.TypeName = Common.EnumHelp.enumHelp.GetDescription(st);
                });
                foreach (Model.Jobs model in models)
                {
                    model.IsAddPool = false;
                    Model.Jobs job = quartzs.Find(m => m.Name == model.Name);
                    if (job != null)
                    {
                        model.IsAddPool = true;
                        model.State = job.State;
                        Enums.TriggerState st = (Enums.TriggerState)job.State;
                        model.StateName = Common.EnumHelp.enumHelp.GetDescription(st);
                    }
                }
            }
        }

        /// <summary>
        /// 获取任务配置信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static List<SelectStrLists> GetConfigs(string keyValue)
        {
            SetConfigById(keyValue);
            return iJobs.GetConfigs(keyValue);
        }
        /// <summary>
        /// 保存任务配置信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool SaveConfigs(string keyValue, List<SelectStrLists> selConfigs)
        {
            return iJobs.SaveConfigs(keyValue, selConfigs);
        }
        /// <summary>
        /// 获取任务文件信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static List<Model.JobFiles> GetJobFiles(string keyValue)
        {
            return BLL.JobFiles.GetModels(keyValue);
        }
        /// <summary>
        /// 获取任务文件信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static void SaveJobFiles(List<Model.JobFiles> jobFiles)
        {
            if (jobFiles != null && jobFiles.Count > 0)
            {
                foreach (var jobFile in jobFiles)
                {
                    BLL.JobFiles.Add(jobFile);
                }
            }
        }

        /// <summary>
        /// 获取任务文件信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static void DeleteFile(string keyValue)
        {
            BLL.JobFiles.RemoveByID(keyValue);
        }

        #region 获取所有数据集合（包括已删除数据） +static List<Model.Jobs> GetAllList()
        /// <summary>
        /// 获取所有数据集合（包括已删除数据）
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetAllList()
        {
            List<Model.Jobs> models = iJobs.GetAllList();
            List<Model.Jobs> quartzs = GetJobsForQuartz();
            InitJobs(models, quartzs);
            return models;
        }

        #endregion

        #region 获取所有未删除数据集合
        /// <summary>
        /// 获取所有未删除数据集合
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetList()
        {
            List<Model.Jobs> models = iJobs.GetList();
            List<Model.Jobs> quartzs = GetJobsForQuartz();
            InitJobs(models, quartzs);
            return models;
        }

        /// <summary>
        /// 获取所有未删除数据集合
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<Model.Jobs> GetList(Pagination pagination, string keyword)
        {
            List<Model.Jobs> models = new List<Model.Jobs>();
            var expression = ExtLinq.True<Model.Jobs>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Name.Contains(keyword));
            }
            models = iJobs.GetList(pagination, expression);
            List<Model.Jobs> quartzs = GetJobsForQuartz();
            InitJobs(models, quartzs);
            return models;
        }

        /// <summary>
        /// 获取所有任务池数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<Model.Jobs> GetJobPoolList(Pagination pagination, string keyword)
        {
            List<Model.Jobs> models = new List<Model.Jobs>();
            var expression = ExtLinq.True<Model.Jobs>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Name.Contains(keyword));
            }
            models = GetJobsForQuartz(pagination, expression);
            InitJobs(models);
            return models;
        }
        #endregion

        public static Model.Jobs GetForm(string keyValue)
        {
            return iJobs.GetForm(keyValue);
        }

        public static Model.Jobs GetFormByName(string keyValue)
        {
            return iJobs.GetFormByName(keyValue);
        }
        public static bool SubmitForm(Model.Jobs modelEntity, string keyValue)
        {
            if (IsExistJobkeyById(keyValue))
            {
                throw new Exception("该任务已在任务池，请先移除任务池！");
            }
            if (IsExistName(modelEntity.Name, keyValue))
            {
                throw new Exception("任务名称已存在！");
            }
            if (IsExistTriggerName(modelEntity.TriggerName, keyValue))
            {
                throw new Exception("触发器名称已存在！");
            }
            if (IsExistConfigName(modelEntity.ConfigName, keyValue))
            {
                throw new Exception("配置名称已存在！");
            }
            Model.DllMgr ddlmgr = BLL.DllMgr.GetForm(modelEntity.DLLID);
            if (ddlmgr != null)
            {
                modelEntity.DllVersion = ddlmgr.Version;
                Type type = Common.AssemblyHelp.assembly.GetDllType(ddlmgr.Name, ddlmgr.NameSpace, modelEntity.ClassName);
                modelEntity.AssType = type;
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                modelEntity.Modify(keyValue);
                return UpdateForm(modelEntity);
            }
            else
            {
                modelEntity.Create();
                return AddForm(modelEntity);
            }
        }
        public static bool AddForm(Model.Jobs modelEntity)
        {
            return iJobs.AddForm(modelEntity);
        }
        public static bool UpdateForm(Model.Jobs modelEntity)
        {
            return iJobs.UpdateForm(modelEntity);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteForm(string keyValue)
        {
            bool ret = false;
            Model.Jobs model = GetForm(keyValue);
            if (model != null)
            {
                ret = BLL.JobFiles.DelByJobId(keyValue);
                ret = iJobs.RemoveConfigs(keyValue);
                if (ret)
                {
                    ret = iJobs.DeleteForm(keyValue);
                }
            }
            return ret;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteByID(string keyValue)
        {
            bool ret = false;
            Model.Jobs model = GetForm(keyValue);
            if (model != null)
            {
                ret = BLL.JobFiles.RemoveByJobId(keyValue);
                ret = iJobs.RemoveConfigs(keyValue);
                if (ret)
                {
                    model.Remove();
                    ret = UpdateForm(model);
                }
            }
            return ret;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool RemoveByID(string keyValue)
        {
            if (IsExistJobkeyById(keyValue))
            {
                throw new Exception("该任务已在任务池，请先移除任务池！");
            }
            bool bState = true;
            if (ConfigHelp.SYSDELETEMODEL == 0)
            {
                bState = DeleteByID(keyValue);
            }
            else
                if (ConfigHelp.SYSDELETEMODEL == 1)
                {
                    bState = DeleteForm(keyValue);
                }
            return bState;
        }
    }
}
