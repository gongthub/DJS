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
        private static string DLLMGR_KEY = Common.RedisConfigHelp.redisConfigHelp.GetRedisKeyByName("DLLMgr_K");

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



        public bool Add(Model.DllMgr model)
        {
            throw new NotImplementedException();
        }

        public bool DelById(Guid Id)
        {
            throw new NotImplementedException();
        }


        public Model.DllMgr GetModelById(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
