using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public class LogListen
    {

        private static string LOGMGR_KEY = Common.RedisConfigHelp.redisConfigHelp.GetRedisKeyByName("LogMgr_K");
        /// <summary>
        /// 构造函数
        /// </summary>
        public LogListen()
        {
            //初始化时间线程
            _timer = new Timer(new TimerCallback(LogsCountListen), null, 0, _checkInterval);
 
        } 
        /// <summary>
        /// 任务数量
        /// </summary>
        public static DateTime NewTime = DateTime.MinValue;

        /// <summary>
        /// 定义检测时间间隔
        /// </summary>
        private static readonly int _checkInterval = 2000;

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
            List<Model.LogModel> models = new List<Model.LogModel>();
            models = Common.RedisHelp.redisHelp.Get<List<Model.LogModel>>(LOGMGR_KEY);
            if (models != null)
            { 
                Model.LogModel model= models.OrderByDescending(m => m.Time).FirstOrDefault();
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

    }
}
