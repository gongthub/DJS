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
    public class DllMgr : IDllMgr
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private static string GROUPPATH = @"/DB/DDLMGRS/DDLMGR";

        private static string GROUPSPATH = @"/DB/DDLMGRS";

        private static string ELENMENTNAME = "DDLMGR";


        #region 获取数据集合 +List<Model.JobGroup> GetModels()
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public List<Model.DllMgr> GetModels()
        {
            List<Model.DllMgr> models = new List<Model.DllMgr>();
            XmlNodeList list = XmlHelp.xmlHelp.GetNodes(XMLDBCONFIGPATH, GROUPPATH);
            if (list != null && list.Count > 0)
            {
                Model.DllMgr group = new Model.DllMgr();
                foreach (XmlNode node in list)
                {
                    group = new Model.DllMgr();
                    group = XmlHelp.xmlHelp.SetNodeToModel(group, node);
                    models.Add(group);
                }
            }
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
            List<Model.DllMgr> models = GetModels();
            if (models != null && models.Count > 0)
            {
                Model.DllMgr model = models.Find(m => m.NameSpace == name);
                if (model != null)
                {
                    ret = true;
                }
            }
            return ret;
        }
        #endregion
         
        #region 根据id删除数据 +bool DelById(Guid Id)
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DelById(Guid Id)
        {
            bool ret = false;
            ret = XmlHelp.xmlHelp.RemoveNode(XMLDBCONFIGPATH, GROUPPATH, "ID", Id);
            return ret;
        }
        #endregion

        #region 添加 +bool Add(Model.DllMgr model)
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(Model.DllMgr model)
        {
            bool ret = true;
            try
            {
                XmlHelp.xmlHelp.AppendNode<Model.DllMgr>(XMLDBCONFIGPATH, GROUPSPATH, ELENMENTNAME, model);
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }
        #endregion
    }
}
