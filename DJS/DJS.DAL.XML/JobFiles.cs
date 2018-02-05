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
    public class JobFiles : XmlDB, IJobFiles
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private const string GROUPPATH = @"/DB/JOBFILES/JOBFILE";

        private const string GROUPSPATH = @"/DB/JOBFILES";

        private const string ELENMENTNAME = "JOBFILE";

        public JobFiles()
            : base(XMLDBCONFIGPATH, GROUPPATH, GROUPSPATH, ELENMENTNAME)
        {
        }

        #region 根据名称判断是否存在 +bool IsExist(string name,string jobID)
        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsExist(string name, string jobID)
        {
            bool ret = false;
            List<Model.JobFiles> models = GetList();
            if (models != null && models.Count > 0)
            {
                Model.JobFiles model = models.Find(m => m.Name == name && m.JobID == jobID);
                if (model != null)
                {
                    ret = true;
                }
            }
            return ret;
        }
        #endregion

        #region 根据JobId删除数据 +bool DelByJobId(string JobId)
        /// <summary>
        /// 根据JobId删除数据
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        public bool DelByJobId(string JobId)
        {
            bool ret = false;
            ret = XmlHelp.xmlHelp.RemoveNode(XMLDBCONFIGPATH, GROUPPATH, "JobID", JobId);
            return ret;
        }
        #endregion

        #region 根据JobName删除数据 +bool DelByJobName(string JobName)
        /// <summary>
        /// 根据JobName删除数据
        /// </summary>
        /// <param name="JobName"></param>
        /// <returns></returns>
        public bool DelByJobName(string JobName)
        {
            bool ret = false;
            ret = XmlHelp.xmlHelp.RemoveNode(XMLDBCONFIGPATH, GROUPPATH, "JobName", JobName);
            return ret;
        }
        #endregion

        public List<Model.JobFiles> GetAllList()
        {
            return GetAllModels<Model.JobFiles>();
        }

        public List<Model.JobFiles> GetList()
        {
            return GetModels<Model.JobFiles>();
        }

        public List<Model.JobFiles> GetList(Pagination pagination)
        {
            return GetModels<Model.JobFiles>(pagination);
        }

        public List<Model.JobFiles> GetList(Pagination pagination, System.Linq.Expressions.Expression<Func<Model.JobFiles, bool>> predicate)
        {
            return GetModels<Model.JobFiles>(pagination, predicate);
        }

        public Model.JobFiles GetForm(string keyValue)
        {
            return GetModel<Model.JobFiles>(keyValue);
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

        public bool DeleteByName(string name)
        {
            bool bState = false;
            try
            {
                Remove("Name", name);
                bState = true;
            }
            catch (Exception e)
            {
                bState = false;
                throw e;
            }
            return bState;
        }

        public bool AddForm(Model.JobFiles modelEntity)
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

        public bool UpdateForm(Model.JobFiles modelEntity)
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

        public List<Model.JobFiles> GetModels(string jobId)
        {
            List<Model.JobFiles> models = GetList();
            models = models.Where(m => m.JobID == jobId).ToList();
            return models;
        }
    }
}
