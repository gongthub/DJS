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
    public class Jobs : XmlDB, IJobs
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        /// <summary>
        /// SDK配置文件路径
        /// </summary>
        private static readonly string SDKCONFIGPATH = ConfigHelp.SDKCONFIGPath;
        /// <summary>
        /// 节点
        /// </summary>
        private const string CONFIGSPATH = @"/CONFIGS";

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
            List<Model.Jobs> models = GetAllModels<Model.Jobs>();
            if (models != null && models.Count > 0)
            {
                foreach (var item in models)
                {
                    item.TimeShow = string.Empty;
                    if (item.Time != DateTime.MinValue)
                    {
                        item.TimeShow = item.Time.ToString("yyyy-MM-dd HH;mm:ss");
                    }
                }
            }
            return models;
        }

        public List<Model.Jobs> GetList()
        {
            List<Model.Jobs> models = GetModels<Model.Jobs>();
            if (models != null && models.Count > 0)
            {
                foreach (var item in models)
                {
                    item.TimeShow = string.Empty;
                    if(item.Time!=DateTime.MinValue)
                    {
                        item.TimeShow = item.Time.ToString("yyyy-MM-dd HH;mm:ss");
                    }
                }
            }
            return models;
        }

        public List<Model.Jobs> GetList(Pagination pagination)
        {
            List<Model.Jobs> models = GetModels<Model.Jobs>(pagination);
            if (models != null && models.Count > 0)
            {
                foreach (var item in models)
                {
                    item.TimeShow = string.Empty;
                    if (item.Time != DateTime.MinValue)
                    {
                        item.TimeShow = item.Time.ToString("yyyy-MM-dd HH;mm:ss");
                    }
                }
            }
            return models;
        }

        public List<Model.Jobs> GetList(Pagination pagination, Expression<Func<Model.Jobs, bool>> predicate)
        {
            List<Model.Jobs> models = GetModels<Model.Jobs>(pagination, predicate);
            if (models != null && models.Count > 0)
            {
                foreach (var item in models)
                {
                    item.TimeShow = string.Empty;
                    if (item.Time != DateTime.MinValue)
                    {
                        item.TimeShow = item.Time.ToString("yyyy-MM-dd HH;mm:ss");
                    }
                }
            }
            return models;
        }

        public Model.Jobs GetForm(string keyValue)
        {
            return GetModel<Model.Jobs>(keyValue);
        }

        public Model.Jobs GetFormByName(string keyValue)
        {
            return GetModel<Model.Jobs>("Name", keyValue);
        }
        public List<SelectStrLists> GetConfigs(string keyValue)
        {
            string cponfignames = string.Empty;
            List<SelectStrLists> dicLst = new List<SelectStrLists>();
            Model.Jobs modelJpb = GetForm(keyValue);
            if (modelJpb != null && !string.IsNullOrEmpty(modelJpb.ID))
            {
                cponfignames = CONFIGSPATH + @"/" + modelJpb.ConfigName + @"/" + "CONFIG";
                XmlNodeList list = XmlHelp.xmlHelp.GetNodes(SDKCONFIGPATH, cponfignames);
                foreach (XmlNode node in list)
                {
                    if (node.Attributes["Name"] != null && node.Attributes["Value"] != null)
                    {
                        SelectStrLists sel = new SelectStrLists();
                        string name = node.Attributes["Name"].Value;
                        string val = node.Attributes["Value"].Value;
                        sel.Name = name;
                        sel.Value = val;
                        dicLst.Add(sel);
                    }

                }
            }
            return dicLst;
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
                if (modelEntity.Time != DateTime.MinValue)
                {
                    modelEntity.TimeShow = modelEntity.Time.ToString("yyyy-MM-dd HH;mm:ss");
                }
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
                if (modelEntity.Time != DateTime.MinValue)
                {
                    modelEntity.TimeShow = modelEntity.Time.ToString("yyyy-MM-dd HH;mm:ss");
                }
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


        public bool SaveConfigs(string keyValue, List<SelectStrLists> selConfigs)
        {
            bool bState = false;
            try
            {
                Model.Jobs model = GetForm(keyValue);
                if (model != null)
                {
                    string cponfignames = CONFIGSPATH + @"/" + model.ConfigName + @"/" + "CONFIG";
                    foreach (SelectStrLists selConfig in selConfigs)
                    {
                        XmlHelp.xmlHelp.SetValue(SDKCONFIGPATH, cponfignames, "Name", selConfig.Name, "Value", selConfig.Value);
                    }
                    bState = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bState;
        }
        public bool RemoveConfigs(string keyValue)
        {
            bool bState = false;
            try
            {
                Model.Jobs model = GetForm(keyValue);
                if (model != null)
                {
                    string cponfignames = CONFIGSPATH + @"/" + model.ConfigName;
                    XmlHelp.xmlHelp.RemoveNode(SDKCONFIGPATH, cponfignames);
                    bState = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bState;
        }

    }
}
