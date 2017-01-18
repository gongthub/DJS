using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks; 

namespace DJS.BLL
{
    public class TriggersListen
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        public TriggersListen()
        {
            //初始化时间线程
            _timer = new Timer(new TimerCallback(TriggersCountListen), null, 0, _checkInterval);
 
        }
         

        /// <summary>
        /// 触发器数量
        /// </summary>
        public static int triggersCounts = 0;

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
        public event EventHandler<EventArgs> OnChange_ListenTriggers;

        /// <summary>
        /// 定义jobs条数改变事件订阅方法
        /// </summary>
        /// <param name="hander"></param>
        public void AddTriggersChangeHandler(EventHandler<EventArgs> hander)
        {
            //判断事件是否为null
            if (OnChange_ListenTriggers == null)
            {

                OnChange_ListenTriggers = new EventHandler<EventArgs>(hander);
            }
            else
            {
                OnChange_ListenTriggers += hander;
            }
        }


        /// <summary>
        /// 删除Triggers条数改变事件订阅方法
        /// </summary>
        /// <param name="hander"></param>
        public void DelTriggersChangeHandler(EventHandler<EventArgs> hander)
        {
            //判断事件是否为null
            if (OnChange_ListenTriggers == null)
            {

                OnChange_ListenTriggers = new EventHandler<EventArgs>(hander);
            }
            else
            { 
                OnChange_ListenTriggers -= hander;  
            }
        }


        /// <summary>
        /// 定义监测Triggers数量是否发生改变
        /// </summary>
        /// <param name="sender"></param>
        private void TriggersCountListen(object sender)
        {
            int counts = BLL.Triggers.GetTriggersForQuartz().Count;
            if (counts != triggersCounts)
            {
                if (OnChange_ListenTriggers != null)
                {
                    EventArgs arg = new EventArgs();
                    OnChange_ListenTriggers(this, arg);
                }
            }
        }
    }
}
