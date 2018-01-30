using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.DAL.Redis
{
    public class JobGroup : IDAL.IJobGroup
    {

        private static readonly string JOBGROUP_KEY = Common.RedisConfigHelp.redisConfigHelp.GetRedisKeyByName("JobGroup_K");

        #region 获取任务组数据 +List<Model.JobGroup> GetModels()
        /// <summary>
        /// 获取任务组数据
        /// </summary>
        /// <returns></returns>
        public List<Model.JobGroup> GetModels()
        {
            return Common.RedisHelp.redisHelp.Get<List<Model.JobGroup>>(JOBGROUP_KEY);
        }
        #endregion

        #region 根据名称判断是否存在 +bool IsExist(string name)
        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>true:存在 false:不存在</returns>
        public bool IsExist(string name)
        {
            bool ret = false;
            List<Model.JobGroup> models = GetModels();
            if (models != null && models.Count > 0)
            {
                Model.JobGroup model = models.Find(m => m.Name == name);
                if (model != null)
                {
                    ret = true;
                }
            }
            return ret;
        }
        #endregion

        #region 根据id删除数据 +bool DelById(Guid Id)
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteForm(string Id)
        {
            bool ret = false;


            List<Model.JobGroup> models = GetModels();
            if (models != null && models.Count > 0)
            {
                Model.JobGroup model = models.Find(m => m.ID == Id);
                if (model != null)
                {
                    ret = models.Remove(model);
                    Common.RedisHelp.redisHelp.Set<List<Model.JobGroup>>(JOBGROUP_KEY, models);
                }
            }

            return ret;
        }
        #endregion

        #region 添加 +bool Add(Model.JobGroup model)
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public bool AddForm(Model.JobGroup model)
        { 
            List<Model.JobGroup> models = new List<Model.JobGroup>();
            models = Common.RedisHelp.redisHelp.Get<List<Model.JobGroup>>(JOBGROUP_KEY);
            if (models == null)
            {
                models = new List<Model.JobGroup>();
            }
            models.Add(model);
            bool ret = Common.RedisHelp.redisHelp.Set<List<Model.JobGroup>>(JOBGROUP_KEY, models);
            return ret;
        }
        #endregion

        public List<Model.JobGroup> GetAllList()
        {
            throw new NotImplementedException();
        }

        public List<Model.JobGroup> GetList()
        {
            throw new NotImplementedException();
        }

        public List<Model.JobGroup> GetList(Common.Pagination pagination)
        {
            throw new NotImplementedException();
        }

        public List<Model.JobGroup> GetList(Common.Pagination pagination, string keyword)
        {
            throw new NotImplementedException();
        }

        public Model.JobGroup GetForm(string keyValue)
        {
            throw new NotImplementedException();
        }

        public bool UpdateForm(Model.JobGroup moduleEntity)
        {
            throw new NotImplementedException();
        }
    }
}
