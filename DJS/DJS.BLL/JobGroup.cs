using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public static class JobGroup
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>
        private static IDAL.IJobGroup iJobGroup = null;

        #endregion

        #region 构造函数

        static JobGroup()
        {
            iJobGroup = DJS.DAL.DataAccessFactory.DataAccessFactory.CreateIJobGroup();
        }

        #endregion

        #region 获取任务组数据 +static List<Model.JobGroup> GetModels()
        /// <summary>
        /// 获取任务组数据
        /// </summary>
        /// <returns></returns>
        public static List<Model.JobGroup> GetModels()
        {
            return iJobGroup.GetModels();
        }
        /// <summary>
        /// 获取任务组数据
        /// </summary>
        /// <returns></returns>
        public static List<Model.JobGroup> GetModels(Predicate<Model.JobGroup> pre)
        {
            List<Model.JobGroup> models = GetModels();
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
            return iJobGroup.IsExist(name);
        }
        #endregion

        #region 根据id删除数据 +static bool DelById(Guid Id)
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelById(Guid Id)
        { 
            return iJobGroup.DelById(Id);
        } 
        #endregion

        #region 添加 +static bool Add(Model.JobGroup model)
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Add(Model.JobGroup model)
        { 
            return iJobGroup.Add(model);
        }
        #endregion
    }
}
