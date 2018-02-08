using DJS.Common;
using DJS.Common.Util;
using DJS.IDAL;
using DJS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.DAL.XML
{
    public class ModuleMgr : XmlDB, IModuleMgr
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private const string GROUPPATH = @"/DB/SYSMODULEMGRS/SYSMODULEMGR";

        private const string GROUPSPATH = @"/DB/SYSMODULEMGRS";

        private const string ELENMENTNAME = "SYSMODULEMGR";

        public ModuleMgr()
            : base(XMLDBCONFIGPATH, GROUPPATH, GROUPSPATH, ELENMENTNAME)
        {
        }

        #region 获取数据
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public List<Model.ModuleEntity> GetAllList()
        {
            return GetAllModels<ModuleEntity>();
        }


        public List<Model.ModuleEntity> GetList()
        {
            return GetModels<Model.ModuleEntity>();
        }

        public List<Model.ModuleEntity> GetList(Pagination pagination)
        {
            return GetModels<Model.ModuleEntity>(pagination);
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<Model.ModuleEntity> GetList(Pagination pagination, Expression<Func<Model.ModuleEntity, bool>> predicate)
        {
            return GetModels<Model.ModuleEntity>(pagination, predicate);
        }

        public Model.ModuleEntity GetForm(string keyValue)
        {
            return GetModel<ModuleEntity>(keyValue);
        }

        #endregion
        /// <summary>
        /// 数据物理删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
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

        public bool AddForm(Model.ModuleEntity moduleEntity)
        {
            bool bState = false;
            try
            {
                Add(moduleEntity);
                bState = true;
            }
            catch (Exception e)
            {
                bState = false;
                throw e;
            }
            return bState;
        }

        public bool UpdateForm(Model.ModuleEntity moduleEntity)
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
