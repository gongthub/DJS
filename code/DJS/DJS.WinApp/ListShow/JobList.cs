using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJS.WinApp
{
    public partial class JobList : Form
    {
        //注册jobs数量改变事件
        private BLL.JobsListen jobsListen = new BLL.JobsListen();
        public JobList()
        {
            InitializeComponent();
        }

        #region 窗体加载事件 -void JobList_Load(object sender, EventArgs e)
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobList_Load(object sender, EventArgs e)
        {
            //IList<string> groups = Common.QuartzHelp.quartzHelp.GetJobGroupNames();

            //JobKey job = new JobKey("job1", "jobgroup1");

            //IJobDetail jobd = Common.QuartzHelp.quartzHelp.GetJobDetail(job);

            //TriggerKey triggerKey = new TriggerKey("trigger1", "triggergroup1");

            //ITrigger iTrigger = Common.QuartzHelp.quartzHelp.GetTrigger(triggerKey);

            //Common.QuartzHelp.quartzHelp.GetJobKeys("jobgroup1");

            //Common.QuartzHelp.quartzHelp.GetCurrentlyExecutingJobs();

            ControlSetting.controlSetting.DataGridViewSet(dgvJobs);
            BindCombox();
            BindList("", "");

        }
        #endregion

        #region jobs数量发生改变事件 -void OnChange_Jobs(object sender, EventArgs e)
        /// <summary>
        /// jobs数量发生改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChange_Jobs(object sender, EventArgs e)
        {

            Control.CheckForIllegalCrossThreadCalls = false;
            List<Model.Jobs> models = BLL.Jobs.GetJobsForQuartz();
            dgvJobs.DataSource = models;
            BLL.JobsListen.jobsCounts = models.Count;
        }
        #endregion

        #region 开始刷新按钮点击事件 -void btnRefresh_Click(object sender, EventArgs e)
        /// <summary>
        /// 开始刷新按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string strs = btnRefresh.Text;
            if (strs == "开始刷新")
            {
                jobsListen.AddJobsChangeHandler(OnChange_Jobs);
                btnRefresh.Text = "停止刷新";
            }
            else
                if (strs == "停止刷新")
                {
                    jobsListen.DelJobsChangeHandler(OnChange_Jobs);
                    btnRefresh.Text = "开始刷新";
                }
        }
        #endregion

        #region 查询按钮点击事件 -void btnQuery_Click(object sender, EventArgs e)
        /// <summary>
        /// 查询按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            string name = txtJobName.Text;
            string group = cbJobGroup.Text;
            BindList(name, group);
        }
        #endregion

        #region 绑定列表方法
        /// <summary>
        /// 绑定列表方法
        /// </summary>
        private void BindList(string name, string group)
        {
            List<Model.Jobs> models = BLL.Jobs.GetJobsForQuartz(m => m.Name.Contains(name) && m.GroupName.Contains(group));
            dgvJobs.DataSource = models;
        }
        #endregion
         
        #region 绑定下拉列表框 -void BindCombox()
        /// <summary>
        /// 绑定下拉列表框
        /// </summary>
        private void BindCombox()
        {
            BindJobGroup(); 
        }
        #endregion

        #region 绑定任务组下拉框 -void BindJobGroup()
        /// <summary>
        /// 绑定任务组下拉框
        /// </summary>
        private void BindJobGroup()
        {
            List<Model.JobGroup> models = BLL.JobGroup.GetModels();
            cbJobGroup.DataSource = models;
            cbJobGroup.ValueMember = "No";
            cbJobGroup.DisplayMember = "Name";
        }
        #endregion

    }
}
