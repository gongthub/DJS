using DJS.Common;
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
            return iTriggerGroup.GetList();
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
                List<Model.TriggerGroup> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.TriggerGroup model = models.Where(m => m.Name == name && m.ID != keyvalue).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            else
            {
                List<Model.TriggerGroup> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.TriggerGroup model = models.Where(m => m.Name == name).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            return bSatae;
        }
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

        #region 添加 +static bool Add(Model.TriggerGroup model)
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Add(Model.TriggerGroup model)
        {
            return iTriggerGroup.AddForm(model);
        }
        #endregion

        #region 获取所有数据集合（包括已删除数据） +static List<Model.TriggerGroup> GetModels()
        /// <summary>
        /// 获取所有数据集合（包括已删除数据）
        /// </summary>
        /// <returns></returns>
        public static List<Model.TriggerGroup> GetAllList()
        {
            return iTriggerGroup.GetAllList();
        }
        #endregion

        #region 获取所有未删除数据集合
        /// <summary>
        /// 获取所有未删除数据集合
        /// </summary>
        /// <returns></returns>
        public static List<Model.TriggerGroup> GetList()
        {
            List<Model.TriggerGroup> models = GetAllList();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(m => m.DeleteMark != true);
            }
            return models;
        }

        /// <summary>
        /// 获取所有未删除数据集合
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<Model.TriggerGroup> GetList(Pagination pagination, string keyword)
        {
            return iTriggerGroup.GetList(pagination, keyword);
        }
        #endregion

        public static Model.TriggerGroup GetForm(string keyValue)
        {
            return iTriggerGroup.GetForm(keyValue);
        }

        public static bool SubmitForm(Model.TriggerGroup modelEntity, string keyValue)
        {
            if (IsExistName(modelEntity.Name, keyValue))
            {
                throw new Exception("名称已存在！");
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
        public static bool AddForm(Model.TriggerGroup modelEntity)
        {
            return iTriggerGroup.AddForm(modelEntity);
        }
        public static bool UpdateForm(Model.TriggerGroup modelEntity)
        {
            return iTriggerGroup.UpdateForm(modelEntity);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteForm(string keyValue)
        {
            return iTriggerGroup.DeleteForm(keyValue);
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteByID(string keyValue)
        {
            bool ret = false;
            Model.TriggerGroup model = GetForm(keyValue);
            if (model != null)
            {
                model.Remove();
                ret = UpdateForm(model);
            }
            return ret;
        }
    }
}
