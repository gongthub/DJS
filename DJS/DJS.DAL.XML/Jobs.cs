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
    public class Jobs : XmlDB, IJobs
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private const string GROUPPATH = @"/DB/JOBS/JOB";

        private const string GROUPSPATH = @"/DB/JOBS";

        private const string ELENMENTNAME = "JOB";

        public Jobs()
            : base(XMLDBCONFIGPATH, GROUPPATH, GROUPSPATH, ELENMENTNAME)
        {
        }

        #region  获取所有jobs +List<Model.Jobs> GetModels()
        /// <summary>
        /// 获取所有jobs
        /// </summary>
        /// <returns></returns>
        public List<Model.Jobs> GetModels()
        {
            return GetList();
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
            List<Model.Jobs> models = GetModels();
            if (models != null && models.Count > 0)
            {
                Model.Jobs model = models.Find(m => m.Name == name);
                if (model != null)
                {
                    ret = true;
                }
            }
            return ret;
        }
        #endregion

        public List<Model.Jobs> GetAllList()
        {
            return GetModels<Model.Jobs>();
        }

        public List<Model.Jobs> GetList()
        {
            List<Model.Jobs> models = GetAllList();
            models = models.FindAll(m => m.DeleteMark != true);
            return models;
        }

        public List<Model.Jobs> GetList(Pagination pagination)
        {
            List<Model.Jobs> models = GetAllList();
            models = models.FindAll(m => m.DeleteMark != true);
            if (models == null)
            {
                models = new List<Model.Jobs>();
            }
            models = models.Skip<Model.Jobs>(pagination.rows * (pagination.page - 1)).Take<Model.Jobs>(pagination.rows).ToList();
            return models;
        }

        public List<Model.Jobs> GetList(Pagination pagination, string keyword)
        {
            List<Model.Jobs> models = GetAllList();

            models = models.FindAll(m => m.DeleteMark != true);
            if (keyword != null)
            {
                models = models.FindAll(m => (m.Name.Contains(keyword)));
            }
            if (models == null)
            {
                models = new List<Model.Jobs>();
            }
            models = models.Skip<Model.Jobs>(pagination.rows * (pagination.page - 1)).Take<Model.Jobs>(pagination.rows).ToList();
            return models;
        }

        public Model.Jobs GetForm(string keyValue)
        {
            return GetModel<Model.Jobs>(keyValue);
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

        public bool AddForm(Model.Jobs modelEntity)
        {
            bool bState = false;
            try
            {
                Add(modelEntity);
                bState = true;
            }
            catch (Exception e)
            {
                bState = false;
                throw e;
            }
            return bState;
        }

        public bool UpdateForm(Model.Jobs modelEntity)
        {
            bool bState = false;
            try
            {
                Update(modelEntity, ConfigHelp.SYSKEYNAME, modelEntity.ID);
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
