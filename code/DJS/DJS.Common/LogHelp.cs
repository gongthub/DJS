using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common
{
    public class LogHelp
    {
        /// <summary>
        /// 获取日志文件路径
        /// </summary>
        private static string LOGURL = ConfigHelp.LogUrlPath;

        /// <summary>
        /// 获取日志文件名称规则
        /// </summary>
        private static string LOGNAME = ConfigHelp.LogNamePath;

        /// <summary>
        /// 获取日志存储类型
        /// </summary>
        private static string LogFileType = ConfigHelp.LogFileTypePath;

        private static string LOGMGR_KEY = Common.RedisConfigHelp.redisConfigHelp.GetRedisKeyByName("LogMgr_K");

        /// <summary>
        /// 消息队列
        /// </summary>
        private static Queue MsgQueue;

        /// <summary>
        /// 日志文件格式
        /// </summary>
        private static string LOGTYPE = ".txt";

        #region 单例模式创建对象
        //单例模式创建对象
        private static LogHelp _logHelp = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        LogHelp()
        {
        }

        public static LogHelp logHelp
        {
            get
            {
                // Double-Checked Locking
                if (null == _logHelp)
                {
                    lock (SynObject)
                    {
                        if (null == _logHelp)
                        {
                            MsgQueue = new Queue();
                            _logHelp = new LogHelp();
                        }
                    }
                }
                return _logHelp;
            }
        }
        #endregion

        #region 写入日志 +void WriteLog(string messages,int type)
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="messages"></param> 
        public void WriteLog(string messages, int type)
        {
            lock (MsgQueue.SyncRoot)
            {
                MsgQueue.Enqueue(messages);
                messages = System.Convert.ToString(MsgQueue.Dequeue());
                if (LogFileType == Model.Enums.LogFileType.File.ToString())
                {
                    WriteLogFile(messages, type);
                }

                if (LogFileType == Model.Enums.LogFileType.Redis.ToString())
                {
                    WriteLogRedis(messages, type);
                }
            }
        }
        #endregion

        #region 写入日志 +void WriteLog(string messages,Model.Enums.LogType type)
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="messages"></param> 
        public void WriteLog(string messages, Model.Enums.LogType type)
        {
            lock (MsgQueue.SyncRoot)
            {
                MsgQueue.Enqueue(messages);
                messages = System.Convert.ToString(MsgQueue.Dequeue());
                if (LogFileType == Model.Enums.LogFileType.File.ToString())
                {
                    WriteLogFile(messages, type);
                }
                if (LogFileType == Model.Enums.LogFileType.Redis.ToString())
                {
                    WriteLogRedis(messages, type);
                }
            }
        }
        #endregion

        #region 获取日志 +string GetLogs(int num)
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <returns></returns>
        public string GetLogs(int num)
        {
            lock (MsgQueue.SyncRoot)
            {
                string strs = "";

                if (LogFileType == Model.Enums.LogFileType.File.ToString())
                {
                    strs = GetLogsFile(num);
                }
                if (LogFileType == Model.Enums.LogFileType.Redis.ToString())
                {
                    strs = GetLogsRedis(num);
                }
                return strs;
            }
        }
        #endregion

        #region 写入日志到File -void WriteLogFile(string int type)
        /// <summary>
        /// 写入日志到File
        /// </summary>
        /// <param name="messages">描述</param>  
        private void WriteLogFile(string messages, int type)
        {

            if (!DJS.Common.FileHelp.DirectoryIsExists(LOGURL))
            {
                DJS.Common.FileHelp.CreateDirectory(LOGURL);
            }
            string strs = "";
            strs += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - ";
            strs += EnumHelp.enumHelp.GetDescription((Model.Enums.LogType)type) + " - ";
            strs += messages;
            DJS.Common.FileHelp.WirteStr(LOGURL + @"/" + DateTime.Now.ToString(LOGNAME) + LOGTYPE, strs);

        }
        #endregion

        #region 写入日志到File -void WriteLogFile(string Model.Enums.LogType type)
        /// <summary>
        /// 写入日志到File
        /// </summary>
        /// <param name="messages">描述</param>  
        private void WriteLogFile(string messages, Model.Enums.LogType type)
        {
            if (!DJS.Common.FileHelp.DirectoryIsExists(LOGURL))
            {
                DJS.Common.FileHelp.CreateDirectory(LOGURL);
            }
            string strs = "";
            strs += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - ";
            strs += EnumHelp.enumHelp.GetDescription(type) + " - ";
            strs += messages;
            DJS.Common.FileHelp.WirteStr(LOGURL + @"/" + DateTime.Now.ToString(LOGNAME) + LOGTYPE, strs);
        }
        #endregion

        #region 写入日志到Redis -void WriteLogRedis(string int type)
        /// <summary>
        /// 写入日志到Redis
        /// </summary>
        /// <param name="messages">描述</param>  
        private void WriteLogRedis(string messages, int type)
        {

            List<Model.LogModel> models = new List<Model.LogModel>();
            models = Common.RedisHelp.redisHelp.Get<List<Model.LogModel>>(LOGMGR_KEY);
            if (models == null)
            {
                models = new List<Model.LogModel>();
            }
            string typename = EnumHelp.enumHelp.GetDescription((Model.Enums.LogType)type);
            Model.LogModel model = new Model.LogModel();
            model.ID = Guid.NewGuid();
            model.Type = (int)type;
            model.TypeName = typename;
            model.Time = DateTime.Now;
            model.Desc = messages;
            models.Add(model);
            Common.RedisHelp.redisHelp.Set<List<Model.LogModel>>(LOGMGR_KEY, models);

        }
        #endregion

        #region 写入日志到Redis -void WriteLogRedis(string Model.Enums.LogType type)
        /// <summary>
        /// 写入日志到Redis
        /// </summary>
        /// <param name="messages">描述</param>  
        private void WriteLogRedis(string messages, Model.Enums.LogType type)
        {

            List<Model.LogModel> models = new List<Model.LogModel>();
            models = Common.RedisHelp.redisHelp.Get<List<Model.LogModel>>(LOGMGR_KEY);
            if (models == null)
            {
                models = new List<Model.LogModel>();
            }
            string typename = EnumHelp.enumHelp.GetDescription(type);
            Model.LogModel model = new Model.LogModel();
            model.ID = Guid.NewGuid();
            model.Type = (int)type;
            model.TypeName = typename;
            model.Time = DateTime.Now;
            model.Desc = messages;
            models.Add(model);
            Common.RedisHelp.redisHelp.Set<List<Model.LogModel>>(LOGMGR_KEY, models);

        }
        #endregion

        #region 获取文件日志 -string GetLogsFile(int num)
        /// <summary>
        /// 获取文件日志
        /// </summary>
        /// <returns></returns>
        private string GetLogsFile(int num)
        {
            string strs = "";
            string paths = LOGURL + @"\";
            ArrayList files = Common.FileHelp.GetFileslist(paths);
            if (files != null && files.Count > 0)
            {
                files.Sort();
                string file = files[files.Count - 1].ToString();
                paths += @"\" + file;
                strs = Common.FileHelp.ReadTxtFileNumE(paths, num);
            }
            return strs.ToString();
        }
        #endregion

        #region 获取Redis日志 -string GetLogsRedis(int num)
        /// <summary>
        /// 获取Redis日志
        /// </summary>
        /// <returns></returns>
        private string GetLogsRedis(int num)
        {
            StringBuilder strs = new StringBuilder();
            List<Model.LogModel> models = new List<Model.LogModel>();
            models = Common.RedisHelp.redisHelp.Get<List<Model.LogModel>>(LOGMGR_KEY);

            string str = null;
            if (models != null && models.Count > 0)
            {
                models = models.OrderByDescending(m => m.Time).ToList();
                if (models.Count > num)
                {
                    models = models.Take(num).ToList();
                }
                foreach (Model.LogModel model in models)
                {
                    str = model.Time.ToString("yyyy-MM-dd HH:mm:ss") + " - " + model.TypeName + " - " + model.Desc + "\r\n";
                    strs.Append(str);
                }
            }
            return strs.ToString();
        }
        #endregion
    }
}
