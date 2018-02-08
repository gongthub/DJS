using Common.Logging;
using DJS.Common;
using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJS.WinApp
{
    public partial class Main : Form
    {

        /// <summary>
        /// 获取日志显示条数
        /// </summary>
        private static int LogShowNum = ConfigHelp.LogShowNumPath;

        //注册log改变事件
        private BLL.LogListen logListen = new BLL.LogListen();

        /// <summary>
        /// 定义检测时间间隔
        /// </summary>
        private static readonly int _checkInterval = 1000;

        /// <summary>
        /// 检测信息监听的时间对象
        /// </summary>
        private System.Threading.Timer _timer;

        /// <summary>
        /// 构造函数
        /// </summary> 
        public Main()
        {
            InitializeComponent();
            //初始化时间线程
            _timer = new System.Threading.Timer(new TimerCallback(BindJobsNow), null, 0, _checkInterval);

        }

        #region 窗体启动事件 -void Main_Load(object sender, EventArgs e)
        /// <summary>
        /// 窗体启动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {
            logListen.AddLogsChangeHandler(OnChange_Logs);
            string logs = Common.LogHelp.logHelp.GetLogs(LogShowNum);
            txtLogsShow.Text = logs;
              
        } 
        #endregion

        #region Logs发生改变事件 -void OnChange_Logs(object sender, EventArgs e)
        /// <summary>
        /// Logs发生改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChange_Logs(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            string logs = Common.LogHelp.logHelp.GetLogs(LogShowNum);
            if (logs != null && logs.Trim() != "")
            {
                txtLogsShow.Text = logs;
                BLL.LogListen.NewTime = DateTime.Now;
            }
        }
        #endregion

        #region 绑定正在执行中任务 -void BindJobsNow(object sender)
        /// <summary>
        /// 绑定正在执行中任务
        /// </summary>
        private void BindJobsNow(object sender)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            StringBuilder strs = new StringBuilder();
            IList<IJobExecutionContext> models = Common.QuartzHelp.quartzHelp.GetCurrentlyExecutingJobs();
            string str = null;
            if (models != null && models.Count > 0)
            {
                foreach (IJobExecutionContext model in models)
                {
                    str = "本次任务时间：" + model.FireTimeUtc;
                    str += "下次任务时间：" + model.NextFireTimeUtc;
                    str += "任务名称：" + model.JobDetail.Key.Name;
                    str += "任务组名称：" + model.JobDetail.Key.Group;
                    str += "触发器名称：" + model.Trigger.Key.Name;
                    str += "触发器组名称：" + model.Trigger.Key.Group + "\r\n";
                    strs.Append(str);
                }
            }
            if (strs != null && strs.Length > 0)
            {
                txtJobsNow.Text = strs.ToString();
            }
            else
            {
                txtJobsNow.Text = "";
            }
        } 
        #endregion
         
         
    }

}
