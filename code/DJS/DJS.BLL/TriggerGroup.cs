using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public class TriggerGroup
    {
        #region 属性
        /// <summary>
        /// 触发器组接口
        /// </summary>
        private static IDAL.ITriggerGroup iTriggerGroup = null;

        #endregion

        #region 构造函数

        static TriggerGroup()
        {
            iTriggerGroup = DJS.DAL.DataAccessFactory.DataAccessFactory.CreateITriggerGroup();
        }

        #endregion

        #region 获取触发器组数据 +List<Model.TriggerGroup> GetModels()
        /// <summary>
        /// 获取触发器组数据
        /// </summary>
        /// <returns></returns>
        public static List<Model.TriggerGroup> GetModels()
        {
            return iTriggerGroup.GetModels();
        }

        /// <summary>
        /// 获取触发器组数据
        /// </summary>
        /// <returns></returns>
        public static List<Model.TriggerGroup> GetModels(Predicate<Model.TriggerGroup> pre)
        {
            List<Model.TriggerGroup> models = GetModels();
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
            return iTriggerGroup.IsExist(name);
        }
        #endregion

        #region 根据id删除数据 +bool DelById(Guid Id)
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelById(Guid Id)
        {
            return iTriggerGroup.DelById(Id);
        }
        #endregion
    }
}
