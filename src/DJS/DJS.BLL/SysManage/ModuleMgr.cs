using DJS.Common;
using DJS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public class ModuleMgr
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>
        private static IDAL.IModuleMgr iModuleMgr = null;

        #endregion

        #region 构造函数

        static ModuleMgr()
        {
            iModuleMgr = DJS.DAL.DataAccessFactory.DataAccessFactory.CreateIModuleMgr();
        }

        #endregion

        #region 获取所有数据集合（包括已删除数据） +static List<Model.ModuleEntity> GetModels()
        /// <summary>
        /// 获取所有数据集合（包括已删除数据）
        /// </summary>
        /// <returns></returns>
        public static List<Model.ModuleEntity> GetAllList()
        {
            return iModuleMgr.GetAllList();
        }
        /// <summary>
        /// 获取所有数据集合（包括已删除数据）
        /// </summary>
        /// <returns></returns>
        public static List<Model.ModuleEntity> GetModels(Predicate<Model.ModuleEntity> pre)
        {
            List<Model.ModuleEntity> models = GetAllList();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(pre);
            }
            return models;
        }
        #endregion

        #region 获取所有未删除数据集合
        /// <summary>
        /// 获取所有未删除数据集合
        /// </summary>
        /// <returns></returns>
        public static List<ModuleEntity> GetList()
        {
            List<Model.ModuleEntity> models = GetAllList();
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
        public static List<ModuleEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<Model.ModuleEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
            }
            return iModuleMgr.GetList(pagination, expression);
        }
        #endregion

        public static ModuleEntity GetForm(string keyValue)
        {
            return iModuleMgr.GetForm(keyValue);
        }
        public static bool SubmitForm(ModuleEntity moduleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                moduleEntity.Modify(keyValue);
                return UpdateForm(moduleEntity);
            }
            else
            {
                moduleEntity.Create();
                return AddForm(moduleEntity);
            }
        }
        public static bool AddForm(ModuleEntity moduleEntity)
        {
            return iModuleMgr.AddForm(moduleEntity);
        }
        public static bool UpdateForm(ModuleEntity moduleEntity)
        {
            return iModuleMgr.UpdateForm(moduleEntity);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteForm(string keyValue)
        {
            return iModuleMgr.DeleteForm(keyValue);
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteByID(string keyValue)
        {
            bool ret = false;
            Model.ModuleEntity model = GetForm(keyValue);
            if (model != null)
            {
                model.Remove();
                ret = UpdateForm(model);
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
