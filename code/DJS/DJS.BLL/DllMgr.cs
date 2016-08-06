using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public static class DllMgr
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>
        private static IDAL.IDllMgr iDllMgr = null;

        #endregion

        #region 构造函数

        static DllMgr()
        {
            iDllMgr = DJS.DAL.DataAccessFactory.DataAccessFactory.CreateIDllMgr();
        }

        #endregion

        #region 获取数据集合 +static List<Model.DllMgr> GetModels()
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public static List<Model.DllMgr> GetModels()
        {
            return iDllMgr.GetModels();
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public static List<Model.DllMgr> GetModels(Predicate<Model.DllMgr> pre)
        {
            List<Model.DllMgr> models = GetModels();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(pre);
            }
            return models;
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
            return iDllMgr.IsExist(name);
        }
        #endregion
    }
}
