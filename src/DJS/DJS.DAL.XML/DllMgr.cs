using DJS.Common;
using DJS.Common.Util;
using DJS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.DAL.XML
{
    public class DllMgr : XmlDB, IDllMgr
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private const string GROUPPATH = @"/DB/DDLMGRS/DDLMGR";

        private const string GROUPSPATH = @"/DB/DDLMGRS";

        private const string ELENMENTNAME = "DDLMGR";

        public DllMgr()
            : base(XMLDBCONFIGPATH, GROUPPATH, GROUPSPATH, ELENMENTNAME)
        {
        }

        #region 获取数据集合 +List<Model.JobGroup> GetModels()
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public List<Model.DllMgr> GetModels()
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
            List<Model.DllMgr> models = GetModels();
            if (models != null && models.Count > 0)
            {
                Model.DllMgr model = models.Find(m => m.Name == name);
                if (model != null)
                {
                    ret = true;
                }
            }
            return ret;
        }
        #endregion

        public List<Model.DllMgr> GetAllList()
        {
            return GetAllModels<Model.DllMgr>();
        }

        public List<Model.DllMgr> GetList()
        {
            return GetModels<Model.DllMgr>();
        }

        public List<Model.DllMgr> GetList(Pagination pagination)
        {
            return GetModels<Model.DllMgr>(pagination);
        }

        public List<Model.DllMgr> GetList(Pagination pagination, Expression<Func<Model.DllMgr, bool>> predicate)
        {
            return GetModels<Model.DllMgr>(pagination, predicate);
        }
        public Model.DllMgr GetForm(string keyValue)
        {
            return GetModel<Model.DllMgr>(keyValue);
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

        public bool AddForm(Model.DllMgr modelEntity)
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

        public bool UpdateForm(Model.DllMgr modelEntity)
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
