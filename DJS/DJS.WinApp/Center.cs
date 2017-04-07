using DJS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DJS.WinApp
{
    public partial class Center : Form
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string xmlDBConfigPath = ConfigHelp.XmlDBConfigPath;

        //private static string groupsPath = @"/DB/GROUPS/GROUP";

        /// <summary>
        /// 是否重启
        /// </summary>
        private static int ISRESTART = 0;

        /// <summary>
        ///  时间控件
        /// </summary> 
        private static Timer TIMER = new Timer();
        /// <summary>
        /// 时间控件执行间隔时间
        /// </summary>
        private static int INTERVAL = 1000;

        /// <summary>
        /// 主窗体
        /// </summary>
        private static string MAINFROM = "DJS.WinApp.Main";

        public Center()
        {
            InitializeComponent();
        }

        #region 窗体事件

        #region 窗体加载事件 -void Center_Load(object sender, EventArgs e)
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Center_Load(object sender, EventArgs e)
        {
            Common.LogHelp.logHelp.WriteLog("进入系统！", Model.Enums.LogType.Normal);
            //初始打开时就加载窗体
            string formClass = MAINFROM;
            GenerateForm(formClass, panMain);

            //设置时间控件
            TIMER.Interval = INTERVAL;
            TIMER.Tick += timer_Tick;
            TIMER.Start();

            //设置开机自启动
            try
            {
                string appurl = Common.ConfigHelp.APPURLPath;
                string isauto = Common.ConfigHelp.APPAUTOPath;
                string filenames = Common.FileHelp.GetFullPath(appurl);
                bool auto = false;
                bool.TryParse(isauto, out auto);
                bool isaouto = Common.SecurityHelp.securityHelp.AppIsAutoRun(filenames);
                if (isaouto != auto)
                {
                    Common.SecurityHelp.securityHelp.SetAutoRun(filenames, auto);
                    Common.LogHelp.logHelp.WriteLog("设置开机启动成功！", 0);

                }
                //开启自启动任务
                BLL.Jobs.StartAutoJobs();
            }
            catch (Exception ex)
            {
                Common.LogHelp.logHelp.WriteLog("设置开机启动失败！" + ex.Message, 1);
            }

            //XmlNodeList list = XmlHelp.xmlHelp.GetNodes(xmlDBConfigPath, groupsPath);
            //Model.JobGroup model=new DJS.Model.JobGroup();
            //Common.XmlHelp.xmlHelp.SetNodeToModel(model, list[0]);

        }
        #endregion

        #region 窗体正在关闭事件 -void Center_FormClosing(object sender, FormClosingEventArgs e)
        /// <summary>
        /// 窗体正在关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Center_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ISRESTART == 0)
            {
                ISRESTART = 0;
                DialogResult result;
                result = MessageBox.Show("确定退出吗？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    Common.LogHelp.logHelp.WriteLog("系统退出！", Model.Enums.LogType.Normal);
                    System.Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion

        #endregion

        #region 系统设置按钮 -void tsbtnSetting_Click(object sender, EventArgs e)
        /// <summary>
        /// 系统设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnSetting_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 时间控件触发事件 -void timer_Tick(object sender, EventArgs e)
        /// <summary>
        /// 时间控件触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            tsslblTime.Text = "系统时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            tsslblTime.Text += "   Quartz状态：" + Common.QuartzHelp.quartzHelp.GetState();
        }
        #endregion

        #region 菜单按钮

        #region 主页按钮点击事件 -void tsbtnMain_Click(object sender, EventArgs e)
        /// <summary>
        /// 主页按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnMain_Click(object sender, EventArgs e)
        {
            //初始打开时就加载窗体
            string formClass = "DJS.WinApp.Main";
            GenerateForm(formClass, panMain);
        }
        #endregion

        #region 任务列表点击事件 -void tsbtnJobShow_Click(object sender, EventArgs e)
        /// <summary>
        /// 任务列表点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnJobShow_Click(object sender, EventArgs e)
        {
            //初始打开时就加载窗体
            string formClass = "DJS.WinApp.JobList";
            GenerateForm(formClass, panMain);

        }
        #endregion

        #region 触发器列表按钮点击事件 -void tsbtnTriggerList_Click(object sender, EventArgs e)
        /// <summary>
        /// 触发器列表按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnTriggerList_Click(object sender, EventArgs e)
        {
            //初始打开时就加载窗体
            string formClass = "DJS.WinApp.TriggerList";
            GenerateForm(formClass, panMain);
        }
        #endregion

        #region 添加任务事件 -void tsmtAddJob_Click(object sender, EventArgs e)
        /// <summary>
        /// 添加任务事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmtAddJob_Click(object sender, EventArgs e)
        {
        }
        #endregion

        #region 任务组配置按钮点击事件 -void tsmtJobGroupMgr_Click(object sender, EventArgs e)
        /// <summary>
        /// 任务组配置按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmtJobGroupMgr_Click(object sender, EventArgs e)
        {
            //初始打开时就加载窗体
            string formClass = "DJS.WinApp.JobGroupMgr";
            GenerateForm(formClass, panMain);
        }
        #endregion

        #region 触发器组按钮点击事件 -void tsmtTriggerGroupMgr_Click(object sender, EventArgs e)
        /// <summary>
        /// 触发器组按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmtTriggerGroupMgr_Click(object sender, EventArgs e)
        {
            //初始打开时就加载窗体
            string formClass = "DJS.WinApp.TriggerGroupMgr";
            GenerateForm(formClass, panMain);
        }
        #endregion

        #region 数据查看按钮点击事件 -void tsbDataShow_Click(object sender, EventArgs e)
        /// <summary>
        /// 数据查看按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDataShow_Click(object sender, EventArgs e)
        {
            //初始打开时就加载窗体
            string formClass = "DJS.WinApp.DataList";
            GenerateForm(formClass, panMain);
        }
        #endregion

        #region 外部程序配置 -void tsmtDllMgr_Click(object sender, EventArgs e)
        /// <summary>
        /// 外部程序配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmtDllMgr_Click(object sender, EventArgs e)
        {
            //初始打开时就加载窗体
            string formClass = "DJS.WinApp.DLLMgr";
            GenerateForm(formClass, panMain);
        }
        #endregion

        #region DLL任务添加按钮点击事件 -void tsmtDllJob_Click(object sender, EventArgs e)
        /// <summary> 
        /// DLL任务添加按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmtDllJob_Click(object sender, EventArgs e)
        {
            AddJob addjob = new AddJob();
            addjob.Show();
        }
        #endregion

        #region 重新启动 -void tsmtRestart_Click(object sender, EventArgs e)
        /// <summary>
        /// 重新启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmtRestart_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("确定重新启动吗？", "重新启动", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                ISRESTART = 1;
                Application.Exit();
                Application.Restart();
            }
        }
        #endregion

        #endregion

        #region 生成窗体 +void GenerateForm(string form, object sender)
        /// <summary>
        /// 生成窗体
        /// </summary>
        /// <param name="form"></param>
        /// <param name="sender"></param> 
        public void GenerateForm(string form, object sender)
        {
            panMain.Controls.Clear();
            // 反射生成窗体
            Form fm = (Form)Assembly.GetExecutingAssembly().CreateInstance(form);

            //设置窗体没有边框  
            fm.FormBorderStyle = FormBorderStyle.None;
            fm.TopLevel = false;
            fm.Parent = (Panel)sender;
            fm.ControlBox = false;
            fm.Dock = DockStyle.Fill;
            fm.Show();

        }
        #endregion

        #region 窗体大小改变事件 -void Center_SizeChanged(object sender, EventArgs e)
        /// <summary>
        /// 窗体大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Center_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)  //判断是否最小化
            {
                this.ShowInTaskbar = false;  //不显示在系统任务栏
                niImg.Visible = true;  //托盘图标可见
            }
        }
        #endregion

        #region icon 双击事件 -void niImg_DoubleClick(object sender, EventArgs e)
        /// <summary>
        /// icon 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void niImg_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Maximized;  //还原窗体
                this.ShowInTaskbar = true;
                niImg.Visible = false;  //托盘图标隐藏
            }
        }
        #endregion


    }

}
