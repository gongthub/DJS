using DJS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.DAL.Redis
{
    public class DllMgr : IDllMgr
    {
        private static readonly string DLLMGR_KEY = Common.RedisConfigHelp.redisConfigHelp.GetRedisKeyByName("DLLMgr_K");

        #region 获取数据集合 +List<Model.DllMgr> GetModels()
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public List<Model.DllMgr> GetModels()
        {
            return Common.RedisHelp.redisHelp.Get<List<Model.DllMgr>>(DLLMGR_KEY);
        } 
        #endregion

        #region 根据名称判断是否存在 +bool IsExist(string name)
        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsExist(string name)
        {
            bool ret = false;
            List<Model.DllMgr> models = GetModels();
            if (models != null && models.Count > 0)
            {
                Model.DllMgr model = models.Find(m => m.Name == name);
                if (model != null)
                {
                    ret = true;
                }
            }
            return ret;
        } 
        #endregion


        public Model.DllMgr GetForm(string Id)
        {
            throw new NotImplementedException();
        }

        public List<Model.DllMgr> GetAllList()
        {
            throw new NotImplementedException();
        }

        public List<Model.DllMgr> GetList()
        {
            throw new NotImplementedException();
        }

        public List<Model.DllMgr> GetList(Common.Pagination pagination)
        {
            throw new NotImplementedException();
        }

        public bool DeleteForm(string keyValue)
        {
            throw new NotImplementedException();
        }

        public bool AddForm(Model.DllMgr moduleEntity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateForm(Model.DllMgr moduleEntity)
        {
            throw new NotImplementedException();
        }


        public List<Model.DllMgr> GetList(Common.Pagination pagination, System.Linq.Expressions.Expression<Func<Model.DllMgr, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
