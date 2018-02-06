using DJS.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public class LogListen
    {
        /// <summary>
        /// 获取日志文件路径
        /// </summary>
        private static string LOGURL = ConfigHelp.LogUrlPath;
        /// <summary>
        /// 获取Nlog日志文件路径
        /// </summary>
        private static string NLOGURL = ConfigHelp.NLogUrlPath;
        /// <summary>
        /// 获取日志存储类型
        /// </summary>
        private static string LogFileType = ConfigHelp.LogFileTypePath;

        private static string LOGMGR_KEY = Common.RedisConfigHelp.redisConfigHelp.GetRedisKeyByName("LogMgr_K");

        private static int ERROERNUM = 0;

        /// <summary>
        /// 上一个文件MD5值
        /// </summary>
        private static string LastMD5 = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogListen()
        {
            //初始化时间线程
            _timer = new Timer(new TimerCallback(LogsCountListen), null, 0, _checkInterval);

        }
        /// <summary>
        /// Redis日志最后时间
        /// </summary>
        public static DateTime NewTime = DateTime.MinValue;

        /// <summary>
        /// 定义检测时间间隔
        /// </summary>
        private static readonly int _checkInterval = 3000;

        /// <summary>
        /// 检测信息监听的时间对象
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// 此委托类型的事件
        /// </summary>
        public event EventHandler<EventArgs> OnChange_ListenLogs;

        /// <summary>
        /// 定义Logs改变事件订阅方法
        /// </summary>
        /// <param name="hander"></param>
        public void AddLogsChangeHandler(EventHandler<EventArgs> hander)
        {
            //判断事件是否为null
            if (OnChange_ListenLogs == null)
            {

                OnChange_ListenLogs = new EventHandler<EventArgs>(hander);
            }
            else
            {
                OnChange_ListenLogs += hander;
            }
        }


        /// <summary>
        /// 删除Logs条数改变事件订阅方法
        /// </summary>
        /// <param name="hander"></param>
        public void DelLogsChangeHandler(EventHandler<EventArgs> hander)
        {
            //判断事件是否为null
            if (OnChange_ListenLogs == null)
            {

                OnChange_ListenLogs = new EventHandler<EventArgs>(hander);
            }
            else
            {
                OnChange_ListenLogs -= hander;
            }
        }


        /// <summary>
        /// 定义监测Logs是否发生改变
        /// </summary>
        /// <param name="sender"></param>
        private void LogsCountListen(object sender)
        {
            lock (this)
            {
                if (LogFileType == Enums.LogFileType.File.ToString())
                {
                    string paths = LOGURL + @"\";
                    ArrayList files = Common.FileHelp.GetFileslist(paths);
                    if (files != null && files.Count > 0)
                    {
                        files.Sort();
                        string file = files[files.Count - 1].ToString();
                        paths += @"\" + file;

                        if (paths != "" && Common.FileHelp.FileExists(paths) && !Common.FileHelp.IsFileInUse(paths))
                        {
                            string mdnow = Common.SecurityHelp.securityHelp.GetMD5HashFromFile(paths);
                            if (mdnow != LastMD5)
                            {
                                if (OnChange_ListenLogs != null)
                                {
                                    LastMD5 = mdnow;
                                    EventArgs arg = new EventArgs();
                                    OnChange_ListenLogs(this, arg);
                                }
                            }
                        }

                    }
                }
                if (LogFileType == Enums.LogFileType.Redis.ToString())
                {
                    List<LogModel> models = new List<LogModel>();
                    models = Common.RedisHelp.redisHelp.Get<List<LogModel>>(LOGMGR_KEY);
                    if (models != null)
                    {
                        LogModel model = models.OrderByDescending(m => m.Time).FirstOrDefault();
                        if (model != null)
                        {
                            if (NewTime == DateTime.MinValue)
                            {
                                NewTime = model.Time;
                            }
                            if (model.Time > NewTime)
                            {
                                EventArgs arg = new EventArgs();
                                OnChange_ListenLogs(this, arg);
                            }
                        }
                    }
                }
                if (LogFileType == Enums.LogFileType.NLog.ToString())
                {
                    string paths = NLOGURL + @"\";
                    ArrayList files = Common.FileHelp.GetFileslist(paths);
                    if (files != null && files.Count > 0)
                    {
                        files.Sort();
                        string file = files[files.Count - 1].ToString();
                        paths += @"\" + file;

                        if (paths != "" && Common.FileHelp.FileExists(paths) && !Common.FileHelp.IsFileInUse(paths))
                        {
                            string mdnow = Common.SecurityHelp.securityHelp.GetMD5HashFromFile(paths);
                            if (mdnow != LastMD5)
                            {
                                if (OnChange_ListenLogs != null)
                                {
                                    LastMD5 = mdnow;
                                    EventArgs arg = new EventArgs();
                                    OnChange_ListenLogs(this, arg);
                                }
                            }
                            else
                            {
                                if (ERROERNUM > 10)
                                {
                                    LastMD5 = "";
                                    ERROERNUM = 0;
                                }
                                ERROERNUM++;
                            }
                        }

                    }
                }
            }
        }

    }
}
