using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.Common.Util
{
    public class XmlDB//<T>  where T : class
    {
        private static readonly object LOCKOBJ = new object();
        private string xPath = string.Empty;
        private string groupPath = string.Empty;
        private string groupsPath = string.Empty;
        private string elenmentName = string.Empty;


        //创建XmlDocument对象
        private XmlDocument xmlDocument = new XmlDocument();

        public XmlDB(string xPath, string groupPath, string groupsPath, string elenmentName)
        {
            this.xPath = xPath;
            this.groupPath = groupPath;
            this.groupsPath = groupsPath;
            this.elenmentName = elenmentName;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Add<T>(T t)
        {
            lock (LOCKOBJ)
            {
                //创建XML的根节点
                //创建根对象
                XmlElement rootElement = XmlHelp.xmlHelp.CreateRootElement(xPath, out xmlDocument);
                XmlNode node = xmlDocument.CreateElement(elenmentName);
                node = XmlHelp.xmlHelp.SetModelToNode(t, node);
                XmlNode xmlnode = null;
                XmlElement rootElementP = null;
                string[] strs = groupsPath.Split('/');
                if (strs != null && strs.Length > 0)
                {
                    string strt = "";
                    foreach (string str in strs)
                    {
                        strt = strt + "/" + str;
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (rootElement != null)
                            {

                                xmlnode = xmlDocument.SelectSingleNode(strt);
                                if (xmlnode != null)
                                {
                                    if (rootElementP != null)
                                        xmlnode.AppendChild(rootElementP);
                                }
                                else
                                {
                                    rootElementP = xmlDocument.CreateElement(str);
                                    if (xmlnode == null)
                                    {
                                        xmlnode = rootElement;
                                    }
                                    xmlnode = xmlnode.AppendChild(rootElementP);
                                }
                            }
                        }
                    }
                }
                //导入节点
                xmlnode.AppendChild(node);
                Save();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Update<T>(T t, string keyName, string keyValue)
        {
            lock (LOCKOBJ)
            {
                //创建XML的根节点
                //创建根对象
                XmlElement rootElement = XmlHelp.xmlHelp.CreateRootElement(xPath, out xmlDocument);
                XmlNode node = XmlHelp.xmlHelp.GetXmlNodeByAttKeyValue(xPath, groupPath, keyName, keyValue, out xmlDocument);
                if (node != null)
                {
                    XmlDocument doc = node.OwnerDocument;
                    PropertyInfo[] infos = t.GetType().GetProperties();
                    if (infos != null && infos.Count() > 0)
                    {
                        XmlAttributeCollection atts = node.Attributes;
                        foreach (PropertyInfo info in infos)
                        {
                            XmlAttribute attr = XmlHelp.xmlHelp.GetAttByName(atts, info.Name);
                            if (attr != null)
                            {
                                if (!ConfigHelp.UpdateNotChange.Contains(info.Name))
                                {
                                    if (info.GetValue(t) != null)
                                    {
                                        attr.Value = info.GetValue(t).ToString();
                                    }
                                    else
                                    {
                                        attr.Value = "";
                                    }
                                }
                            }
                            else
                            {
                                attr = doc.CreateAttribute(info.Name);
                                if (info.GetValue(t) != null)
                                {
                                    attr.Value = info.GetValue(t).ToString();
                                }
                                else
                                {
                                    attr.Value = "";
                                }
                            }
                            node.Attributes.SetNamedItem(attr);

                        }
                    }
                    Save();
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Remove(string keyName, string keyValue)
        {
            lock (LOCKOBJ)
            {

                //获取要删除的节点
                XmlNodeList nodes = XmlHelp.xmlHelp.GetNodes(xPath, groupPath, out xmlDocument);
                if (nodes != null && nodes.Count > 0)
                {
                    foreach (XmlNode node in nodes)
                    {
                        if (node.Attributes[keyName] != null && node.Attributes[keyName].Name == keyName && node.Attributes[keyName].Value.ToString() == keyValue.ToString())
                        {
                            XmlElement xe = (XmlElement)node.ParentNode;
                            //删除节点
                            xe.RemoveChild(node);
                        }
                    }
                }
                Save();
            }
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetModels<T>()
        {
            lock (LOCKOBJ)
            {
                List<T> models = new List<T>();
                XmlNodeList list = XmlHelp.xmlHelp.GetNodes(xPath, groupPath);
                if (list != null && list.Count > 0)
                {
                    foreach (XmlNode node in list)
                    {
                        T t = System.Activator.CreateInstance<T>();
                        t = XmlHelp.xmlHelp.SetNodeToModel(t, node);
                        models.Add(t);
                    }
                }
                return models;
            }
        }


        /// <summary>
        /// 获取指定数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public T GetModel<T>(string keyValue)
        {
            lock (LOCKOBJ)
            {
                T t = System.Activator.CreateInstance<T>();
                List<T> models = new List<T>();
                XmlNodeList list = XmlHelp.xmlHelp.GetNodes(xPath, groupPath);
                if (list != null && list.Count > 0)
                {
                    foreach (XmlNode node in list)
                    {
                        XmlAttributeCollection attrs = node.Attributes;
                        if (attrs != null && attrs.Count > 0)
                        {
                            XmlAttribute attr = XmlHelp.xmlHelp.GetAttByName(attrs, ConfigHelp.SYSKEYNAME);
                            if (attr != null && attr.Value == keyValue)
                            {
                                t = XmlHelp.xmlHelp.SetNodeToModel(t, node);
                                break;
                            }
                        }
                    }
                }
                return t;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            if (!FileHelp.FileExists(xPath))
            {
                xPath = FileHelp.GetFullPath(xPath);
            }
            xmlDocument.Save(xPath);
        }
    }
}
