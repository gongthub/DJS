using DJS.Common;
using DJS.Common.Util;
using DJS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.DAL.XML
{
    public class JobGroup  : XmlDB ,IJobGroup
    {

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private const string GROUPPATH = @"/DB/JOBGROUPS/JOBGROUP";

        private const string GROUPSPATH = @"/DB/JOBGROUPS";

        private const string ELENMENTNAME = "JOBGROUP";

        public JobGroup()
            : base(XMLDBCONFIGPATH, GROUPPATH, GROUPSPATH, ELENMENTNAME)
        {
        }

        #region 获取数据集合 +List<Model.JobGroup> GetModels()
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public List<Model.JobGroup> GetModels()
        {
            return GetList();
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
            List<Model.JobGroup> models = GetModels();
            if (models != null && models.Count > 0)
            {
                Model.JobGroup model = models.Find(m => m.Name == name);
                if (model != null)
                {
                    ret = true;
                }
            }
            return ret;
        }
        #endregion

        public List<Model.JobGroup> GetAllList()
        {
            return GetAllModels<Model.JobGroup>();
        }

        public List<Model.JobGroup> GetList()
        {
            return GetModels<Model.JobGroup>();
        }

        public List<Model.JobGroup> GetList(Pagination pagination)
        {
            return GetModels<Model.JobGroup>(pagination);
        }

        public List<Model.JobGroup> GetList(Pagination pagination, Expression<Func<Model.JobGroup, bool>> predicate)
        {
            return GetModels<Model.JobGroup>(pagination, predicate);
        }

        public Model.JobGroup GetForm(string keyValue)
        {
            return GetModel<Model.JobGroup>(keyValue);
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

        public bool AddForm(Model.JobGroup modelEntity)
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

        public bool UpdateForm(Model.JobGroup modelEntity)
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
