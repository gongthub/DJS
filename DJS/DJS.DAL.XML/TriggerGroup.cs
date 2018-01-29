using DJS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.DAL.XML
{
    public class TriggerGroup : IDAL.ITriggerGroup
    {

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private const string GROUPPATH = @"/DB/TRIGGERGROUPS/TRIGGERGROUP";

        private const string GROUPSPATH = @"/DB/TRIGGERGROUPS";

        private const string ELENMENTNAME = "TRIGGERGROUP";

        #region 获取数据集合 +List<Model.TriggerGroup> GetModels()
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public List<Model.TriggerGroup> GetModels()
        {
            List<Model.TriggerGroup> models = new List<Model.TriggerGroup>();
            XmlNodeList list = XmlHelp.xmlHelp.GetNodes(XMLDBCONFIGPATH, GROUPPATH);
            if (list != null && list.Count > 0)
            {
                Model.TriggerGroup group = new Model.TriggerGroup();
                foreach (XmlNode node in list)
                {
                    group = new Model.TriggerGroup();
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

        #region 根据id删除数据 +bool DelById(Guid Id)
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DelById(Guid Id)
        {
            bool ret = false;
            ret = XmlHelp.xmlHelp.RemoveNode(XMLDBCONFIGPATH, GROUPPATH, ConfigHelp.SYSKEYNAME, Id);
            return ret;
        }
        #endregion

        #region 添加 +bool Add(Model.TriggerGroup model)
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(Model.TriggerGroup model)
        {
            bool ret = true;
            try
            {
                XmlHelp.xmlHelp.AppendNode<Model.TriggerGroup>(model, XMLDBCONFIGPATH, GROUPSPATH, ELENMENTNAME);
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        #endregion
    }
}
