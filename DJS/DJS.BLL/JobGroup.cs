using DJS.Common;
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
                List<Model.JobGroup> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.JobGroup model = models.Where(m => m.Name == name && m.ID != keyvalue).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            else
            {
                List<Model.JobGroup> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.JobGroup model = models.Where(m => m.Name == name).FirstOrDefault();
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
            return iJobGroup.IsExist(name);
        }
        #endregion

        #region 根据id删除数据 +static bool DelById(Guid Id)
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelById(string Ids)
        {
            return iJobGroup.DeleteForm(Ids);
        } 
        #endregion

        #region 获取所有数据集合（包括已删除数据） +static List<Model.JobGroup> GetAllList()
        /// <summary>
        /// 获取所有数据集合（包括已删除数据）
        /// </summary>
        /// <returns></returns>
        public static List<Model.JobGroup> GetAllList()
        {
            return iJobGroup.GetAllList();
        }
        #endregion

        #region 获取所有未删除数据集合
        /// <summary>
        /// 获取所有未删除数据集合
        /// </summary>
        /// <returns></returns>
        public static List<Model.JobGroup> GetList()
        {
            List<Model.JobGroup> models = GetAllList();
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
        public static List<Model.JobGroup> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<Model.JobGroup>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Name.Contains(keyword));
            }
            return iJobGroup.GetList(pagination, expression);
        }
        #endregion

        public static Model.JobGroup GetForm(string keyValue)
        {
            return iJobGroup.GetForm(keyValue);
        }

        public static bool SubmitForm(Model.JobGroup modelEntity, string keyValue)
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
        public static bool AddForm(Model.JobGroup modelEntity)
        {
            return iJobGroup.AddForm(modelEntity);
        }
        public static bool UpdateForm(Model.JobGroup modelEntity)
        {
            return iJobGroup.UpdateForm(modelEntity);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteForm(string keyValue)
        {
            return iJobGroup.DeleteForm(keyValue);
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteByID(string keyValue)
        {
            bool ret = false;
            Model.JobGroup model = GetForm(keyValue);
            if (model != null)
            {
                model.Remove();
                ret = UpdateForm(model);
            }
            return ret;
        }
    }
}
