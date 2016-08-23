using DJS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.DAL.XML
{
    public class Jobs : IDAL.IJobs
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private static string GROUPPATH = @"/DB/JOBS/JOB";

        private static string GROUPSPATH = @"/DB/JOBS";

        private static string ELENMENTNAME = "JOB";

        #region  获取所有jobs +List<Model.Jobs> GetJobs()
        /// <summary>
        /// 获取所有jobs
        /// </summary>
        /// <returns></returns>
        public List<Model.Jobs> GetModels()
        {
            List<Model.Jobs> models = new List<Model.Jobs>();
            XmlNodeList list = XmlHelp.xmlHelp.GetNodes(XMLDBCONFIGPATH, GROUPPATH);
            if (list != null && list.Count > 0)
            {
                Model.Jobs group = new Model.Jobs();
                foreach (XmlNode node in list)
                {
                    group = new Model.Jobs();
                    group = XmlHelp.xmlHelp.SetNodeToModel(group, node);
                    models.Add(group);
                }
            }
            return models;
        }
        #endregion

        #region 根据id获取job +Model.Jobs GetModelById(Guid Id)
        /// <summary>
        /// 根据id获取job
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Model.Jobs GetModelById(Guid Id)
        {
            Model.Jobs model = new Model.Jobs();
            List<Model.Jobs> models = GetModels();
            if (models != null && models.Count > 0)
            {
                model = models.Find(m => m.ID == Id);

            }
            return model;
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

        #region 添加 +bool Add(Model.Jobs model)
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(Model.Jobs model)
        {
            bool ret = true;
            try
            {
                XmlHelp.xmlHelp.AppendNode<Model.Jobs>(XMLDBCONFIGPATH, GROUPSPATH, ELENMENTNAME, model);
            }
            catch (Exception ex)
            {
                ret = false;
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

    }
}
