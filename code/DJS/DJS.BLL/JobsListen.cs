using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public class JobsListen
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public JobsListen()
        {
            //初始化时间线程
            _timer = new Timer(new TimerCallback(JobsCountListen), null, 0, _checkInterval);
 
        }
         

        /// <summary>
        /// 任务数量
        /// </summary>
        public static int jobsCounts = 0;

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
        public event EventHandler<EventArgs> OnChange_ListenJobs;

        /// <summary>
        /// 定义jobs条数改变事件订阅方法
        /// </summary>
        /// <param name="hander"></param>
        public void AddJobsChangeHandler(EventHandler<EventArgs> hander)
        {
            //判断事件是否为null
            if (OnChange_ListenJobs == null)
            {

                OnChange_ListenJobs = new EventHandler<EventArgs>(hander);
            }
            else
            {
                OnChange_ListenJobs += hander;
            }
        }


        /// <summary>
        /// 删除jobs条数改变事件订阅方法
        /// </summary>
        /// <param name="hander"></param>
        public void DelJobsChangeHandler(EventHandler<EventArgs> hander)
        {
            //判断事件是否为null
            if (OnChange_ListenJobs == null)
            {

                OnChange_ListenJobs = new EventHandler<EventArgs>(hander);
            }
            else
            { 
                OnChange_ListenJobs -= hander;  
            }
        }


        /// <summary>
        /// 定义监测jobs数量是否发生改变
        /// </summary>
        /// <param name="sender"></param>
        private void JobsCountListen(object sender)
        {
            int counts = BLL.Jobs.GetJobsForQuartzCount();
            if (counts != jobsCounts)
            {
                if (OnChange_ListenJobs != null)
                {
                    EventArgs arg = new EventArgs();
                    OnChange_ListenJobs(this, arg);
                }
            }
        }

    }
}
