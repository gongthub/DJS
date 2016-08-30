using DJS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.SDK.Service
{
    public class ConfigMgr : IConfigMgr
    {
        public ConfigMgr(string namespaces)
        {
            CONFIGPATH = "";
            CONFIGSPATH = @"/CONFIGS";
            CONFIGPATH = CONFIGSPATH + @"/" + namespaces + @"/" + ELENMENTNAME;
            CONFIGSPATH = CONFIGSPATH + @"/" + namespaces;
        }
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string SDKCONFIGPATH = ConfigHelp.SDKCONFIGPath;
        /// <summary>
        /// 文件附件路径
        /// </summary>
        private static string JobFileSrcPath = ConfigHelp.JobFileSrcPath;

        /// <summary>
        /// 节点
        /// </summary>
        private static string CONFIGPATH = "";
        private static string CONFIGSPATH = @"/CONFIGS";

        private static string ELENMENTNAME = "CONFIG";

        #region 设置Config值 +bool SetConfig(string name, string value)
        /// <summary>
        /// 设置Config值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param> 
        /// <returns></returns>
        public bool SetConfig(string name, string value)
        {
            bool ret = true;
            try
            {
                if (!IsExist(name))
                {

                    Model.ConfigMgr model = new Model.ConfigMgr();
                    model.Name = name;
                    model.Value = value;
                    Common.XmlHelp.xmlHelp.AppendNode<Model.ConfigMgr>(SDKCONFIGPATH, CONFIGSPATH, ELENMENTNAME, model);
                }
                else
                {
                    Model.ConfigMgr model = new Model.ConfigMgr();
                    model.Name = name;
                    model.Value = value;
                    XmlHelp.xmlHelp.SetValue(SDKCONFIGPATH, CONFIGSPATH + @"/" + ELENMENTNAME, "Name", model.Name, "Value", model.Value);
                }
            }
            catch (Exception ex)
            {
                Common.LogHelp.logHelp.WriteLog(ex.Message, Model.Enums.LogType.Error);
                ret = false;
            }
            return ret;
        }
        #endregion

        #region 根据名称查找值 +string GetConfig(string name)
        /// <summary>
        /// 根据名称查找值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public string GetConfig(string name)
        {
            string val = "";
            XmlNodeList nodes = XmlHelp.xmlHelp.GetNodes(SDKCONFIGPATH, CONFIGPATH);
            if (nodes != null && nodes.Count > 0)
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["Name"] != null)
                    {
                        string vals = node.Attributes["Name"].Value;
                        if (vals == name)
                        {
                            if (node.Attributes["Value"] != null)
                            {
                                val = node.Attributes["Value"].Value;
                            }
                        }
                    }
                }
            }
            return val;
        }
        #endregion

        #region 根据名称判断是否存在 +bool IsExist(string name)
        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public bool IsExist(string name)
        {
            bool ret = false;
            XmlNodeList nodes = XmlHelp.xmlHelp.GetNodes(SDKCONFIGPATH, CONFIGPATH);
            if (nodes != null && nodes.Count > 0)
            {
                foreach (XmlNode node in nodes)
                {
                    string vals = node.Attributes["Name"].Value;
                    if (vals == name)
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }
        #endregion

        #region  设置Config值并返回提示信息 +bool SetConfig(string name, string value, out string msg)
        /// <summary>
        /// 设置Config值并返回提示信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetConfig(string name, string value, out string msg)
        {
            bool ret = true;
            if (IsExist(name))
            {
                ret = false;
                msg = "该名称已经存在，请重新输入";
            }
            else
            {
                ret = SetConfig(name, value);
                if (ret)
                {
                    msg = "设置成功";
                }
                else
                {
                    msg = "设置失败";
                }
            }
            return ret;
        }
        #endregion

        #region 根据任务名称和文件名称获取路径
        /// <summary>
        /// 根据任务名称和文件名称获取路径
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <returns></returns> 
        public string GetFileSrc(string name, string fileName)
        {
            string src = "";
            Model.JobFiles model = BLL.JobFiles.GetModels(m => m.JobName == name && m.Name == fileName).FirstOrDefault();
            if (model != null)
            {
                src = model.Src;
            }
            return src;
        }
        #endregion

        #region 根据任务名称获取文件路径
        /// <summary>
        /// 根据任务名称获取文件路径
        /// </summary>
        /// <param name="name"></param> 
        /// <returns></returns> 
        public string GetFileSrc(string name)
        {
            string src = "";
            src = FileHelp.GetFullPath(JobFileSrcPath) + @"\" + name + @"\";
            return src;
        }
        #endregion
    }
}
