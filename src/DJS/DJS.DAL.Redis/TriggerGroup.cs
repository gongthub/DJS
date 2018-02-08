using DJS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.DAL.Redis
{
    public class TriggerGroup:ITriggerGroup
    {

        private static readonly string TRIGGERGROUP_KEY = Common.RedisConfigHelp.redisConfigHelp.GetRedisKeyByName("TriggerGroup_K");

        #region 获取触发器集合 +List<Model.TriggerGroup> GetModels()
        /// <summary>
        /// 获取触发器集合
        /// </summary> 
        /// <returns></returns>
        public List<Model.TriggerGroup> GetModels()
        {
            return Common.RedisHelp.redisHelp.Get<List<Model.TriggerGroup>>(TRIGGERGROUP_KEY);
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
            List<Model.TriggerGroup> models = GetModels();
            if (models != null && models.Count > 0)
            {
                Model.TriggerGroup model = models.Find(m => m.Name == name);
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

            List<Model.TriggerGroup> models = GetModels();
            if (models != null && models.Count > 0)
            {
                Model.TriggerGroup model = models.Find(m => m.ID == Id);
                if (model != null)
                {
                    ret = models.Remove(model);
                    Common.RedisHelp.redisHelp.Set<List<Model.TriggerGroup>>(TRIGGERGROUP_KEY, models);
                }
            }

            return ret;
        }
        #endregion
         
        #region 添加 +bool Add(Model.TriggerGroup model)
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public bool AddForm(Model.TriggerGroup model)
        {
            List<Model.TriggerGroup> models = new List<Model.TriggerGroup>();
            models = Common.RedisHelp.redisHelp.Get<List<Model.TriggerGroup>>(TRIGGERGROUP_KEY);
            if (models == null)
            {
                models = new List<Model.TriggerGroup>();
            }
            models.Add(model);
            bool ret = Common.RedisHelp.redisHelp.Set<List<Model.TriggerGroup>>(TRIGGERGROUP_KEY, models);
            return ret;
        }
        #endregion

        public List<Model.TriggerGroup> GetAllList()
        {
            throw new NotImplementedException();
        }

        public List<Model.TriggerGroup> GetList()
        {
            throw new NotImplementedException();
        }

        public List<Model.TriggerGroup> GetList(Common.Pagination pagination)
        {
            throw new NotImplementedException();
        }


        public Model.TriggerGroup GetForm(string keyValue)
        {
            throw new NotImplementedException();
        }

        public bool UpdateForm(Model.TriggerGroup moduleEntity)
        {
            throw new NotImplementedException();
        }


        public List<Model.TriggerGroup> GetList(Common.Pagination pagination, System.Linq.Expressions.Expression<Func<Model.TriggerGroup, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
