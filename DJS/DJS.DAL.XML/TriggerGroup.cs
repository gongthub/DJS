using DJS.Common;
using DJS.Common.Util;
using DJS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.DAL.XML
{
    public class TriggerGroup : XmlDB, ITriggerGroup
    {

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private const string GROUPPATH = @"/DB/TRIGGERGROUPS/TRIGGERGROUP";

        private const string GROUPSPATH = @"/DB/TRIGGERGROUPS";

        private const string ELENMENTNAME = "TRIGGERGROUP";

        public TriggerGroup()
            : base(XMLDBCONFIGPATH, GROUPPATH, GROUPSPATH, ELENMENTNAME)
        {
        }

        #region 获取数据集合 +List<Model.TriggerGroup> GetModels()
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public List<Model.TriggerGroup> GetModels()
        {
            List<Model.TriggerGroup> models = GetList();
            return models;
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

        #region 添加 +bool Add(Model.TriggerGroup model)
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddForm(Model.TriggerGroup model)
        {
            bool bState = false;
            try
            {
                Add(model);
                bState = true;
            }
            catch (Exception e)
            {
                bState = false;
                throw e;
            }
            return bState;
        }
        #endregion

        public List<Model.TriggerGroup> GetAllList()
        {
            return GetModels<Model.TriggerGroup>();
        }

        public List<Model.TriggerGroup> GetList()
        {
            List<Model.TriggerGroup> models = GetAllList();
            models = models.FindAll(m => m.DeleteMark != true);
            return models;
        }

        public List<Model.TriggerGroup> GetList(Pagination pagination)
        {
            List<Model.TriggerGroup> models = GetAllList();
            models = models.FindAll(m => m.DeleteMark != true);
            if (models == null)
            {
                models = new List<Model.TriggerGroup>();
            }
            models = models.Skip<Model.TriggerGroup>(pagination.rows * (pagination.page - 1)).Take<Model.TriggerGroup>(pagination.rows).ToList();
            return models;
        }

        public List<Model.TriggerGroup> GetList(Pagination pagination, string keyword)
        {
            List<Model.TriggerGroup> models = GetAllList();

            models = models.FindAll(m => m.DeleteMark != true);
            if (keyword != null)
            {
                models = models.FindAll(m => m.Name.Contains(keyword));
            }
            if (models == null)
            {
                models = new List<Model.TriggerGroup>();
            }
            models = models.Skip<Model.TriggerGroup>(pagination.rows * (pagination.page - 1)).Take<Model.TriggerGroup>(pagination.rows).ToList();
            return models;
        }

        public Model.TriggerGroup GetForm(string keyValue)
        {
            return GetModel<Model.TriggerGroup>(keyValue);
        }

        public bool DeleteForm(string keyValue)
        {
            bool bState = false;
            try
            {
                Remove(ConfigHelp.SYSKEYNAME, keyValue);
                bState = true;
            }
            catch (Exception e)
            {
                bState = false;
                throw e;
            }
            return bState;
        }

        public bool UpdateForm(Model.TriggerGroup moduleEntity)
        {
            bool bState = false;
            try
            {
                Update(moduleEntity, ConfigHelp.SYSKEYNAME, moduleEntity.ID);
                bState = true;
            }
            catch (Exception e)
            {
                bState = false;
                throw e;
            }
            return bState;
        }
    }
}
