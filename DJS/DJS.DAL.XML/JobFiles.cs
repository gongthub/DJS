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
    public class JobFiles : IJobFiles
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private const string GROUPPATH = @"/DB/JOBFILES/JOBFILE";

        private const string GROUPSPATH = @"/DB/JOBFILES";

        private const string ELENMENTNAME = "JOBFILE";

        #region  获取所有jobs +List<Model.JobFiles> GetModels()
        /// <summary>
        /// 获取所有jobs
        /// </summary>
        /// <returns></returns>
        public List<Model.JobFiles> GetModels()
        {
            List<Model.JobFiles> models = new List<Model.JobFiles>();
            XmlNodeList list = XmlHelp.xmlHelp.GetNodes(XMLDBCONFIGPATH, GROUPPATH);
            if (list != null && list.Count > 0)
            {
                Model.JobFiles group = new Model.JobFiles();
                foreach (XmlNode node in list)
                {
                    group = new Model.JobFiles();
                    group = XmlHelp.xmlHelp.SetNodeToModel(group, node);
                    models.Add(group);
                }
            }
            return models;
        }
        #endregion

        #region 根据id获取job +Model.JobFiles GetModelById(string Id)
        /// <summary>
        /// 根据id获取job
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Model.JobFiles GetModelById(string Id)
        {
            Model.JobFiles model = new Model.JobFiles();
            List<Model.JobFiles> models = GetModels();
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
            List<Model.JobFiles> models = GetModels();
            if (models != null && models.Count > 0)
            {
                Model.JobFiles model = models.Find(m => m.Name == name);
                if (model != null)
                {
                    ret = true;
                }
            }
            return ret;
        }
        #endregion

        #region 添加 +bool Add(Model.JobFiles model)
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(Model.JobFiles model)
        {
            bool ret = true;
            try
            {
                XmlHelp.xmlHelp.AppendNode<Model.JobFiles>(model, XMLDBCONFIGPATH, GROUPSPATH, ELENMENTNAME);
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        #endregion

        #region 根据id删除数据 +bool DelById(string Id)
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DelById(string Id)
        {
            bool ret = false;
            ret = XmlHelp.xmlHelp.RemoveNode(XMLDBCONFIGPATH, GROUPPATH, ConfigHelp.SYSKEYNAME, Id);
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
    }
}
