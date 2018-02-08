using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.Common
{
    public class RedisConfigHelp
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string RedisConfigPath = ConfigHelp.RedisConfigPath;

        /// <summary>
        /// key节点
        /// </summary>
        private static string KEYPATH = @"/KEYS/KEY";
        

        #region 单例模式创建对象
        //单例模式创建对象
        private static RedisConfigHelp _redisConfigHelp = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        RedisConfigHelp()
        {
        }

        public static RedisConfigHelp redisConfigHelp
        {
            get
            {
                // Double-Checked Locking
                if (null == _redisConfigHelp)
                {
                    lock (SynObject)
                    {
                        if (null == _redisConfigHelp)
                        {
                            _redisConfigHelp = new RedisConfigHelp();
                        }
                    }
                }
                return _redisConfigHelp;
            }
        }
        #endregion 

        #region 根据配置文件节点名获取key
        /// <summary>
        /// 根据配置文件节点名获取key
        /// </summary>
        /// <returns></returns>
        public string GetRedisKeyByName(string Name)
        {
            string val = "";
            if (FileHelp.FileExists(RedisConfigPath))
            {
                XmlNodeList nodes = XmlHelp.xmlHelp.GetNodes(RedisConfigPath, KEYPATH);
                if (nodes != null && nodes.Count > 0)
                {
                    foreach (XmlNode node in nodes)
                    {
                        string vals = node.Attributes["Name"].Value;
                        if (vals == Name)
                        {
                            val = node.Attributes["Value"].Value;
                        }
                    }
                }
            }
            return val;
        }
        #endregion
    }
}
