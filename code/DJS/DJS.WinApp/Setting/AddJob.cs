using DJS.Common;
using Quartz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJS.WinApp
{
    public partial class AddJob : Form
    {
        /// <summary>
        /// 获取程序集文件所在文件夹名称
        /// </summary>
        private static string PATH = ConfigHelp.AssemblySrcPath;

        public AddJob()
        {
            InitializeComponent();
        }

        #region 事件

        #region 窗体加载事件 -void AddJob_Load(object sender, EventArgs e)
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddJob_Load(object sender, EventArgs e)
        {
            //绑定任务组下拉框
            BindCombox();
            //dtpTime.Text = DateTime.Now.AddSeconds(15).ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        #region 显示列表 -void btnListShow_Click(object sender, EventArgs e)
        /// <summary>
        /// 显示列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListShow_Click(object sender, EventArgs e)
        {
            JobList joblist = new JobList();
            joblist.ShowDialog();
        }
        #endregion

        #region 添加按钮点击事件 -void btnAdd_Click(object sender, EventArgs e)
        /// <summary>
        /// 添加按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string times = dtpTime.Text;
            string crons = txtCron.Text;
            string jobNames = txtJobName.Text;
            string jobGroups = cbJobGroup.Text;
            string triggerNames = txtTriggerName.Text;
            string triggerGroups = cbTriggerGroup.Text;
            string nameSpages = cbNameSpace.Text;
            string classNames = cbClassName.Text;

            string types = cbType.Text;

            Type type = Common.AssemblyHelp.assembly.GetDllType(nameSpages, classNames);
            if (types == "周期性")
            {
                MessageHelp mes = Common.QuartzHelp.quartzHelp.AddJob(type, crons, jobNames, jobGroups, triggerNames, triggerGroups);

                MessageBox.Show(mes.Message); 
            }
            else
                if (types == "一次性")
                {
                    DateTime time = DateTime.MinValue;
                    if (DateTime.TryParse(times, out time))
                    {
                        MessageHelp mes = Common.QuartzHelp.quartzHelp.AddJob(type, time, jobNames, jobGroups, triggerNames, triggerGroups);

                        MessageBox.Show(mes.Message);
                    }
                }

        }

        #endregion

        #region 开始 结束 -void btnOperation_Click(object sender, EventArgs e)
        /// <summary>
        /// 开始 结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOperation_Click(object sender, EventArgs e)
        {
            string operations = btnOperation.Text;
            if (operations == "开始")
            {
                Common.QuartzHelp.quartzHelp.Start();
                btnOperation.Text = "已启动";
                btnOperation.Enabled = false;
            }
        }
        #endregion

        #region 策略双击事件 -void txtCron_DoubleClick(object sender, EventArgs e)
        /// <summary>
        /// 策略双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCron_DoubleClick(object sender, EventArgs e)
        {
            CronSet cronset = new CronSet();
            cronset.Show(this);
        }
        #endregion

        #region 类型切换时间 -void cbType_SelectedIndexChanged(object sender, EventArgs e)
        /// <summary>
        /// 类型切换时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string types = cbType.Text;
            if (types == "周期性")
            {
                dtpTime.Visible = false;
                txtCron.Visible = true;
                lblTime.Text = "策略";
            }
            else
                if (types == "一次性")
                {
                    dtpTime.Visible = true;
                    txtCron.Visible = false;
                    lblTime.Text = "时间";
                }
        }
        #endregion

        #region 暂停继续按钮点击事件 -void btnPause_Click(object sender, EventArgs e)
        /// <summary>
        /// 暂停继续按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPause_Click(object sender, EventArgs e)
        {
            string pauses = btnPause.Text;
            if (pauses == "暂停")
            {
                Common.QuartzHelp.quartzHelp.PauseAll();
                btnPause.Text = "继续";
            }
            else
                if (pauses == "继续")
                {
                    Common.QuartzHelp.quartzHelp.ResumeAll();
                    btnPause.Text = "暂停";
                }
        }
        #endregion

        #endregion

        #region 绑定下拉选项

        #region 绑定下拉列表框 -void BindCombox()
        /// <summary>
        /// 绑定下拉列表框
        /// </summary>
        private void BindCombox()
        {
            BindType();
            BindJobGroup();
            BindTriggerGroup();
            BindNameSpace();
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

        #region 绑定触发器组下拉框 -void BindTriggerGroup()
        /// <summary>
        /// 绑定触发器组下拉框
        /// </summary>
        private void BindTriggerGroup()
        {
            List<Model.TriggerGroup> models = BLL.TriggerGroup.GetModels();
            cbTriggerGroup.DataSource = models;
            cbTriggerGroup.ValueMember = "No";
            cbTriggerGroup.DisplayMember = "Name";
        }
        #endregion

        #region 绑定命名空间下拉框 -void BindNameSpace()
        /// <summary>
        /// 绑定命名空间下拉框
        /// </summary>
        private void BindNameSpace()
        {
            ArrayList arry = Common.FileHelp.GetDirectoryList(PATH);
            cbNameSpace.DataSource = arry;
            cbNameSpace.DisplayMember = "Name";
        }
        #endregion

        #region 绑定操作类下拉框 -void BindClassName()
        /// <summary>
        /// 绑定操作类下拉框
        /// </summary>
        private void BindClassName(string nameSpace)
        {
            ArrayList arry = Common.QuartzHelp.quartzHelp.GetIClassName(nameSpace);
            cbClassName.DataSource = arry;
            cbClassName.DisplayMember = "Name";
        }
        #endregion

        #region 绑定类型下拉框 -void BindType()
        /// <summary>
        /// 绑定类型下拉框
        /// </summary>
        private void BindType()
        {
            ArrayList arry = Common.EnumHelp.enumHelp.ToArrayList(typeof(Model.Enums.TimeType)); 
            cbType.DataSource = arry;
            cbType.DisplayMember = "Name";
        }
        #endregion
         
        #region 命名空间选择事件 -void cbNameSpace_SelectedIndexChanged(object sender, EventArgs e)
        /// <summary>
        /// 命名空间选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbNameSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nameSpaces = cbNameSpace.Text;
            BindClassName(nameSpaces);
        }
        #endregion
          
        #endregion

    }
}
