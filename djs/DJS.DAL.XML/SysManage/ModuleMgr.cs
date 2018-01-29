using DJS.Common;
using DJS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.DAL.XML
{
    public class ModuleMgr : IModuleMgr
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private const string GROUPPATH = @"/DB/SYSMODULEMGRS/SYSMODULEMGR";

        private const string GROUPSPATH = @"/DB/SYSMODULEMGRS";

        private const string ELENMENTNAME = "SYSMODULEMGR";

        #region 获取数据
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public List<Model.ModuleEntity> GetModels()
        {
            List<Model.ModuleEntity> models = new List<Model.ModuleEntity>();
            XmlNodeList list = XmlHelp.xmlHelp.GetNodes(XMLDBCONFIGPATH, GROUPPATH);
            if (list != null && list.Count > 0)
            {
                Model.ModuleEntity group = new Model.ModuleEntity();
                foreach (XmlNode node in list)
                {
                    group = new Model.ModuleEntity();
                    group = XmlHelp.xmlHelp.SetNodeToModel(group, node);
                    models.Add(group);
                }
            }
            return models;
        }


        public List<Model.ModuleEntity> GetList()
        {
            List<Model.ModuleEntity> models = GetModels();
            models = models.FindAll(m => m.DeleteMark != true);
            return models;
        }

        public List<Model.ModuleEntity> GetList(Pagination pagination)
        {
            List<Model.ModuleEntity> models = GetModels();
            models = models.FindAll(m => m.DeleteMark != true);
            if (models == null)
            {
                models = new List<Model.ModuleEntity>();
            }
            models = models.Skip<Model.ModuleEntity>(pagination.rows * (pagination.page - 1)).Take<Model.ModuleEntity>(pagination.rows).ToList();
            return models;
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<Model.ModuleEntity> GetList(Pagination pagination, string keyword)
        {
            List<Model.ModuleEntity> models = GetModels();

            models = models.FindAll(m => m.DeleteMark != true);
            if (keyword != null)
            {
                models = models.FindAll(m => m.FullName.Contains(keyword));
            }
            if (models == null)
            {
                models = new List<Model.ModuleEntity>();
            }
            models = models.Skip<Model.ModuleEntity>(pagination.rows * (pagination.page - 1)).Take<Model.ModuleEntity>(pagination.rows).ToList();
            return models;
        }

        public Model.ModuleEntity GetForm(string keyValue)
        {
            Model.ModuleEntity model = new Model.ModuleEntity();
            List<Model.ModuleEntity> models = GetList();
            if (models != null && models.Count > 0)
            {
                model = models.Find(m => m.ID == keyValue);

            }
            return model;
        }

        #endregion
        /// <summary>
        /// 数据物理删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool DeleteForm(string keyValue)
        {
            bool ret = false;
            ret = XmlHelp.xmlHelp.RemoveNode(XMLDBCONFIGPATH, GROUPPATH, ConfigHelp.SYSKEYNAME, keyValue);
            return ret;
        }

        public bool AddForm(Model.ModuleEntity moduleEntity)
        {
            bool ret = true;
            try
            {
                XmlHelp.xmlHelp.AppendNode<Model.ModuleEntity>(moduleEntity, XMLDBCONFIGPATH, GROUPSPATH, ELENMENTNAME);
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

        public bool UpdateForm(Model.ModuleEntity moduleEntity)
        {
            bool ret = true;
            try
            {
                XmlHelp.xmlHelp.UpdateNode<Model.ModuleEntity>(moduleEntity, XMLDBCONFIGPATH, GROUPPATH, ConfigHelp.SYSKEYNAME, moduleEntity.ID);
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

    }
}
