﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common
{
    public class ConfigHelp
    {
        #region 单例模式创建对象
        //单例模式创建对象
        private static ConfigHelp _configHelp = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        ConfigHelp()
        {
        }

        public static ConfigHelp configHelp
        {
            get
            {
                // Double-Checked Locking
                if (null == _configHelp)
                {
                    lock (SynObject)
                    {
                        if (null == _configHelp)
                        {
                            _configHelp = new ConfigHelp();
                        }
                    }
                }
                return _configHelp;
            }
        }
        #endregion

        /// <summary>
        /// Redis配置文件路径
        /// </summary>
        public static string RedisConfigPath
        {
            get
            {
                return ConfigurationManager.AppSettings["RedisConfig"].ToString();
            }
        }
         
        /// <summary>
        /// xml存储文件路径
        /// </summary>
        public static string XmlDBConfigPath
        {
            get
            {
                return ConfigurationManager.AppSettings["XmlDBConfig"].ToString();
            }
        }
         
        /// <summary>
        /// 获取程序集文件所在文件夹名称
        /// </summary>
        public static string AssemblySrcPath
        {
            get
            {
                return ConfigurationManager.AppSettings["AssemblySrc"].ToString();
            }
        }
         
        /// <summary>
        /// 数据保存路径
        /// </summary>
        public static string DataPathPath
        {
            get
            {
                return ConfigurationManager.AppSettings["DataPath"].ToString();
            }
        }
         
        /// <summary>
        /// 数据保存文件名称
        /// </summary>
        public static string DataNamePath
        {
            get
            {
                return ConfigurationManager.AppSettings["DataName"].ToString();
            }
        }

        /// <summary>
        /// 获取日志文件路径
        /// </summary>
        public static string LogUrlPath
        {
            get
            {
                return ConfigurationManager.AppSettings["LogUrl"].ToString();
            }
        }
         
        /// <summary>
        /// 获取日志文件名称规则
        /// </summary>
        public static string LogNamePath
        {
            get
            {
                return ConfigurationManager.AppSettings["LogName"].ToString();
            }
        }
         
        /// <summary>
        /// 获取日志文件类型
        /// </summary>
        public static string LogFileTypePath
        {
            get
            {
                return ConfigurationManager.AppSettings["LogFileType"].ToString();
            }
        }

        /// <summary>
        /// 获取程序集文件所在文件夹名称
        /// </summary>
        public static string IQuartzPath
        {
            get
            {
                return ConfigurationManager.AppSettings["IQuartz"].ToString();
            }
        }
         
        /// <summary>
        /// redis服务器地址
        /// </summary>
        public static string redisServerPath
        {
            get
            {
                return ConfigurationManager.AppSettings["redisServer"].ToString();
            }
        }
         
        /// <summary>
        /// redis服务器端口
        /// </summary>
        public static string redisServerPortPath
        {
            get
            {
                return ConfigurationManager.AppSettings["redisServerPort"].ToString();
            }
        }
         
        /// <summary>
        /// Cron 路径
        /// </summary>
        public static string CronUrlPath
        {
            get
            {
                return ConfigurationManager.AppSettings["CronUrl"].ToString();
            }
        } 
         
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static string DALTYPEPath
        {
            get
            {
                return ConfigurationManager.AppSettings["DALTYPE"].ToString();
            }
        }  
    }
}
