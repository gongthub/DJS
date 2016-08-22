using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.Common
{
    /// <summary>
    /// Xml的操作公共类
    /// </summary>    
    public class XmlHelp
    {
        #region 单例模式创建对象
        //单例模式创建对象
        private static XmlHelp _xmlHelp = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        XmlHelp()
        {
        }

        public static XmlHelp xmlHelp
        {
            get
            {
                // Double-Checked Locking
                if (null == _xmlHelp)
                {
                    lock (SynObject)
                    {
                        if (null == _xmlHelp)
                        {
                            _xmlHelp = new XmlHelp();
                        }
                    }
                }
                return _xmlHelp;
            }
        }
        #endregion

        #region 字段定义
        /// <summary>
        /// XML文件的物理路径
        /// </summary>
        private string _filePath = string.Empty;
        /// <summary>
        /// Xml文档
        /// </summary>
        private XmlDocument _xml;
        /// <summary>
        /// XML的根节点
        /// </summary>
        private XmlElement _element;
        #endregion

        #region 构造方法
        /// <summary>
        /// 实例化XmlHelper对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        public XmlHelp(string xmlFilePath)
        {
            //获取XML文件的绝对路径
            _filePath = FileHelp.GetFullPath(xmlFilePath);
        }
        #endregion

        #region 创建XML的根节点
        /// <summary>
        /// 创建XML的根节点
        /// </summary>
        private void CreateXMLElement()
        {

            //创建一个XML对象
            _xml = new XmlDocument();

            if (FileHelp.FileExists(_filePath))
            {
                //加载XML文件
                _xml.Load(this._filePath);
            }

            //为XML的根节点赋值
            _element = _xml.DocumentElement;
        }
        #endregion

        #region 获取指定XPath表达式的节点对象
        /// <summary>
        /// 获取指定XPath表达式的节点对象
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public XmlNode GetNode(string xPath)
        {
            //创建XML的根节点
            CreateXMLElement();

            //返回XPath节点
            return _element.SelectSingleNode(xPath);
        }
        #endregion

        #region 获取指定XPath表达式节点的值
        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public string GetValue(string xPath)
        {
            //创建XML的根节点
            CreateXMLElement();

            //返回XPath节点的值
            return _element.SelectSingleNode(xPath).InnerText;
        }
        #endregion

        #region 获取指定XPath表达式节点的属性值
        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public string GetAttributeValue(string xPath, string attributeName)
        {
            //创建XML的根节点
            CreateXMLElement();

            //返回XPath节点的属性值
            return _element.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }
        #endregion

        #region 新增节点
        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点插入到当前Xml文件中。
        /// </summary>        
        /// <param name="xmlNode">要插入的Xml节点</param>
        public void AppendNode(XmlNode xmlNode)
        {
            //创建XML的根节点
            CreateXMLElement();

            //导入节点
            XmlNode node = _xml.ImportNode(xmlNode, true);

            //将节点插入到根节点下
            _element.AppendChild(node);
        }

        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将DataSet中的第一条记录插入Xml文件中。
        /// </summary>        
        /// <param name="ds">DataSet的实例，该DataSet中应该只有一条记录</param>
        public void AppendNode(DataSet ds)
        {
            //创建XmlDataDocument对象
            XmlDataDocument xmlDataDocument = new XmlDataDocument(ds);

            //导入节点
            XmlNode node = xmlDataDocument.DocumentElement.FirstChild;

            //将节点插入到根节点下
            AppendNode(node);
        }
        #endregion

        #region 删除节点
        /// <summary>
        /// 删除指定XPath表达式的节点
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public void RemoveNode(string xPath)
        {
            //创建XML的根节点
            CreateXMLElement();

            //获取要删除的节点
            XmlNode node = _xml.SelectSingleNode(xPath);

            //删除节点
            _element.RemoveChild(node);
        }
        #endregion //删除节点

        #region 保存XML文件
        /// <summary>
        /// 保存XML文件
        /// </summary>        
        public void Save()
        {
            //创建XML的根节点
            CreateXMLElement();

            //保存XML文件
            _xml.Save(this._filePath);
        }
        #endregion //保存XML文件

        #region 方法

        #region 创建根节点对象 -XmlElement CreateRootElement(string xmlFilePath)
        /// <summary>
        /// 创建根节点对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>        
        private XmlElement CreateRootElement(string xmlFilePath)
        {
            //定义变量，表示XML文件的绝对路径
            string filePath = "";

            //获取XML文件的绝对路径
            filePath = FileHelp.GetFullPath(xmlFilePath);

            //创建XmlDocument对象
            XmlDocument xmlDocument = new XmlDocument();
            //加载XML文件
            xmlDocument.Load(filePath);

            //返回根节点
            return xmlDocument.DocumentElement;
        }

        /// <summary>
        /// 创建根节点对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>        
        private XmlElement CreateRootElement(string xmlFilePath, out XmlDocument xmlDoc)
        {
            //定义变量，表示XML文件的绝对路径
            string filePath = "";

            //获取XML文件的绝对路径
            filePath = FileHelp.GetFullPath(xmlFilePath);

            //创建XmlDocument对象
            XmlDocument xmlDocument = new XmlDocument();
            //加载XML文件
            xmlDocument.Load(filePath);

            xmlDoc = xmlDocument;
            //返回根节点
            return xmlDocument.DocumentElement;
        }
        #endregion

        #region 获取指定XPath表达式节点的值 +string GetValue(string xmlFilePath, string xPath)
        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public string GetValue(string xmlFilePath, string xPath)
        {
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            //返回XPath节点的值
            return rootElement.SelectSingleNode(xPath).InnerText;
        }
        #endregion

        #region 获取指定XPath表达式节点的属性值 +string GetAttributeValue(string xmlFilePath, string xPath, string attributeName)
        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public string GetAttributeValue(string xmlFilePath, string xPath, string attributeName)
        {
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            //返回XPath节点的属性值
            return rootElement.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }
        #endregion

        #region 获取指定XPath表达式的节点对象集合 +XmlNodeList GetNodes(string filePath, string xPath)
        /// <summary>
        /// 获取指定XPath表达式的节点对象集合
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public XmlNodeList GetNodes(string filePath, string xPath)
        {
            //创建XML的根节点
            //创建根对象
            XmlElement rootElement = CreateRootElement(filePath);

            //返回XPath节点
            return rootElement.SelectNodes(xPath);
        }
        /// <summary>
        /// 获取指定XPath表达式的节点对象集合
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public XmlNodeList GetNodes(string filePath, string xPath, out XmlDocument xmlDoc)
        { 
            //创建XML的根节点
            //创建根对象
            XmlElement rootElement = CreateRootElement(filePath, out xmlDoc);

            //返回XPath节点
            return rootElement.SelectNodes(xPath);
        }
        #endregion

        #region 指定路径添加节点
        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点插入到当前Xml文件中。
        /// </summary>        
        /// <param name="xmlNode">要插入的Xml节点</param>
        public void AppendNode(string filePath, string xPath, XmlNode node)
        {
            //创建XML的根节点
            //创建根对象
            XmlElement rootElement = CreateRootElement(filePath);
            XmlNode xmlnode = rootElement.SelectSingleNode(xPath);
            //导入节点
            xmlnode.AppendChild(node);

        }

        #endregion

        #region 指定路径添加节点
        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点插入到当前Xml文件中。
        /// </summary>        
        /// <param name="xmlNode">要插入的Xml节点</param>
        public void AppendNode<T>(string xmlFilePath, string xPath, string elenmentName, T t)
        { 
            //创建XmlDocument对象
            XmlDocument xmlDocument = new XmlDocument();
            //创建XML的根节点
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath, out xmlDocument); 
            XmlNode node = xmlDocument.CreateElement(elenmentName);
            node = SetModelToNode(t, node); 
            XmlNode xmlnode = rootElement.SelectSingleNode(xPath);
            if (xmlnode == null)
            {
                string name = elenmentName + "S";
                xmlnode = xmlDocument.CreateElement(name);
                //导入节点
                rootElement.AppendChild(xmlnode);
            }
            //导入节点
            xmlnode.AppendChild(node);
            xmlDocument.Save(xmlFilePath);
        }

        #endregion
         
        #region 根据指定属性名称和属性值删除指定节点 +bool RemoveNode(string filePath,string xPath,string attName,object val)
        /// <summary>
        /// 根据指定属性名称和属性值删除指定节点
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="xPath">xml路径</param>
        /// <param name="attName">属性名称</param>
        /// <param name="val">属性值</param>
        public bool RemoveNode(string filePath, string xPath, string attName, object val)
        {
            bool ret = true;
            try
            {
                //创建XmlDocument对象
                XmlDocument xmlDocument = new XmlDocument(); 
                 
                //获取要删除的节点
                XmlNodeList nodes = GetNodes(filePath, xPath, out xmlDocument);
                if (nodes != null && nodes.Count > 0)
                {
                    foreach (XmlNode node in nodes)
                    {
                        if (node.Attributes[attName] != null && node.Attributes[attName].Name == attName && node.Attributes[attName].Value.ToString() == val.ToString())
                        {
                            XmlElement xe = (XmlElement)node.ParentNode;   
                            //删除节点
                            xe.RemoveChild(node);
                        } 
                    }
                }
                xmlDocument.Save(filePath);
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }
        #endregion

        #region xmlNode转实体 +T SetNodeToModel<T>(T t, XmlNode node)
        /// <summary>
        /// xmlNode转实体 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public T SetNodeToModel<T>(T t, XmlNode node)
        {
            XmlAttributeCollection atts = node.Attributes;
            foreach (XmlAttribute att in atts)
            {
                if (t.GetType().GetProperty(att.Name) != null)
                {
                    Type type = t.GetType().GetProperty(att.Name).PropertyType;
                    if (type.Name == "Type")
                    {
                        string[] strs = att.Value.Split('.');
                        if (strs.Length >= 2)
                        {
                            type = Common.AssemblyHelp.assembly.GetDllType(strs[0], strs[1]);
                        }
                        t.GetType().GetProperty(att.Name).SetValue(t, type);  
                    }
                    else
                    {
                        object obj = SetType(type, att.Value);
                        t.GetType().GetProperty(att.Name).SetValue(t, obj);  
                    }
                }
            }
            return t;
        }
        #endregion

        #region 数据类型转换 +object SetType(Type type, string val)
        /// <summary>
        /// 数据类型转换
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public object SetType(Type type, string val)
        {
            object obj = new object();
            if (type.Equals(typeof(String)))
            {
                obj = val.ToString();
            }
            else if (type.Equals(typeof(int)))
            {
                int temp = 0;
                Int32.TryParse(val, out temp);
                obj = temp;
            }
            else if (type.Equals(typeof(decimal)))
            {
                Decimal temp = 0;
                Decimal.TryParse(val, out temp);
                obj = temp;
            }
            else if (type.Equals(typeof(System.DateTime)))
            {
                DateTime temp = DateTime.MinValue;
                DateTime.TryParse(val, out temp);
                obj = temp;
            }
            else if (type.Equals(typeof(double)))
            {
                double temp = 0;
                double.TryParse(val, out temp);
                obj = temp;
            }
            else if (type.Equals(typeof(bool)))
            {
                bool temp = true;
                bool.TryParse(val, out temp);
                obj = temp;
            }
            else if (type.Equals(typeof(Guid)))
            {
                Guid temp = Guid.Empty;
                Guid.TryParse(val, out temp);
                obj = temp;
            }

            return obj;
        }
        #endregion

        #region 实体转xmlNode + XmlNode SetModelToNode<T>(T t, XmlNode node)
        /// <summary>
        /// 实体转xmlNode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public XmlNode SetModelToNode<T>(T t, XmlNode node)
        {
            PropertyInfo[] infos = t.GetType().GetProperties();
            if (infos != null && infos.Count() > 0)
            {
                XmlDocument doc = node.OwnerDocument;
                foreach (PropertyInfo info in infos)
                {
                    XmlAttribute attr = null;
                    attr = doc.CreateAttribute(info.Name);
                    if (info.GetValue(t) != null)
                    {
                        attr.Value = info.GetValue(t).ToString();
                    }
                    else
                    {
                        attr.Value = ""; 
                    }
                    node.Attributes.SetNamedItem(attr);
                }
            }
            return node;
        }
        #endregion

        #endregion

    }
}
