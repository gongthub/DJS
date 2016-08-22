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
            dgvJobs.CellClick += new DataGridViewCellEventHandler(dgvlinkDo_Click);
            dgvJobs.CellClick += new DataGridViewCellEventHandler(dgvlinkReAdd_Click);
            dgvJobs.CellClick += new DataGridViewCellEventHandler(dgvlinkDoDel_Click);
            BindCombox();
            BindList();

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
            List<Model.Jobs> models = BLL.Jobs.GetJobs();
            if (models != null && models.Count > 0)
            {
                dgvJobs.DataSource = models;
                BLL.JobsListen.jobsCounts = models.Count;
            }
            else
            {
                dgvJobs.DataSource = null;
                BLL.JobsListen.jobsCounts = 0;
            }
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
            BindList(); 
        }
        #endregion

        #region 绑定列表方法
        /// <summary>
        /// 绑定列表方法
        /// </summary>
        private void BindList()
        {
            string name = txtJobName.Text;
            string group = cbJobGroup.Text;
            if (dgvJobs.Columns.Contains("dgvlinkDo"))
            {
                dgvJobs.Columns.Remove("dgvlinkDo");
            }
            if (dgvJobs.Columns.Contains("dgvlinkReAdd"))
            {
                dgvJobs.Columns.Remove("dgvlinkReAdd");
            }
            if (dgvJobs.Columns.Contains("dgvlinkDoDel"))
            {
                dgvJobs.Columns.Remove("dgvlinkDoDel");
            }
            List<Model.Jobs> models = BLL.Jobs.GetJobs(m => m.Name.Contains(name) && m.GroupName.Contains(group));

            dgvJobs.DataSource = models;

            DataGridViewLinkColumn dgvlinkDo = new DataGridViewLinkColumn();
            {
                dgvlinkDo.Name = "dgvlinkDo";
                dgvlinkDo.HeaderText = "操作";
                dgvlinkDo.Text = "执行";
                dgvlinkDo.LinkColor = Color.Red;
                dgvlinkDo.ActiveLinkColor = Color.Red;
                dgvlinkDo.UseColumnTextForLinkValue = true;
                dgvlinkDo.AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.AllCells;
                //dgvbtnDel.FlatStyle = FlatStyle.Standard;
                dgvlinkDo.CellTemplate.Style.BackColor = Color.Honeydew;
            }
            dgvJobs.Columns.Add(dgvlinkDo);
            //dgvJobs.Refresh();

            DataGridViewLinkColumn dgvlinkReAdd = new DataGridViewLinkColumn();
            {
                dgvlinkReAdd.Name = "dgvlinkReAdd";
                dgvlinkReAdd.HeaderText = "重新添加";
                dgvlinkReAdd.Text = "重新添加";
                dgvlinkReAdd.LinkColor = Color.Red;
                dgvlinkReAdd.ActiveLinkColor = Color.Red;
                dgvlinkReAdd.UseColumnTextForLinkValue = true;
                dgvlinkReAdd.AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.AllCells;
                //dgvbtnDel.FlatStyle = FlatStyle.Standard;
                dgvlinkReAdd.CellTemplate.Style.BackColor = Color.Honeydew;
            }
            dgvJobs.Columns.Add(dgvlinkReAdd);


            DataGridViewLinkColumn dgvlinkDoDel = new DataGridViewLinkColumn();
            {
                dgvlinkDoDel.Name = "dgvlinkDoDel";
                dgvlinkDoDel.HeaderText = "删除";
                dgvlinkDoDel.Text = "删除";
                dgvlinkDoDel.LinkColor = Color.Red;
                dgvlinkDoDel.ActiveLinkColor = Color.Red;
                dgvlinkDoDel.UseColumnTextForLinkValue = true;
                dgvlinkDoDel.AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.AllCells;
                //dgvbtnDel.FlatStyle = FlatStyle.Standard;
                dgvlinkDoDel.CellTemplate.Style.BackColor = Color.Honeydew;
            }
            dgvJobs.Columns.Add(dgvlinkDoDel);

            dgvJobs.Refresh();
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

        #region 数据列表绑定完成事件 -void dgvJobs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        /// <summary>
        /// 数据列表绑定完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJobs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvJobs.Columns["ID"].Visible = false;//隐藏某列：
            dgvJobs.Columns["State"].Visible = false;//隐藏某列：
            dgvJobs.Columns["Type"].Visible = false;//隐藏某列：
            dgvJobs.Columns["AssType"].Visible = false;//隐藏某列：
            dgvJobs.Columns["Name"].HeaderText = "名称";
            dgvJobs.Columns["GroupName"].HeaderText = "任务组";
            dgvJobs.Columns["TriggerName"].HeaderText = "触发器";
            dgvJobs.Columns["TriggerGroup"].HeaderText = "触发器组";
            //dgvJobs.Columns["State"].HeaderText = "状态";
            dgvJobs.Columns["StateName"].HeaderText = "状态";
            dgvJobs.Columns["TypeName"].HeaderText = "类型";
            dgvJobs.Columns["Time"].HeaderText = "执行时间";
        }
        #endregion

        #region 立即执行按钮点击事件 -void dgvlinkDo_Click(object sender, DataGridViewCellEventArgs e)
        /// <summary>
        /// 立即执行按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvlinkDo_Click(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvJobs.Columns[e.ColumnIndex] != null && dgvJobs.Columns[e.ColumnIndex].HeaderText == "操作" && e.RowIndex >= 0)
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确定要立即执行吗?", "立即执行", messButton);
                if (dr == DialogResult.OK)//如果点击“确定”按钮
                {

                    if (dgvJobs.Rows.Count > e.RowIndex && dgvJobs.Rows[e.RowIndex] != null)
                    {
                        if (dgvJobs.Rows[e.RowIndex].Cells["GroupName"] != null && dgvJobs.Rows[e.RowIndex].Cells["Name"] != null)
                        {
                            string groupNames = dgvJobs.Rows[e.RowIndex].Cells["GroupName"].Value.ToString();
                            string names = dgvJobs.Rows[e.RowIndex].Cells["Name"].Value.ToString();

                            if (BLL.Jobs.TriggerJob(groupNames, names))
                            {
                                BindList();
                                MessageBox.Show("执行成功");
                            }
                            else
                            {
                                MessageBox.Show("执行失败");
                            }
                        }
                    }
                }
            }

        }
        #endregion

        #region 重新添加按钮点击事件 -void dgvlinkReAdd_Click(object sender, DataGridViewCellEventArgs e)
        /// <summary>
        /// 重新添加按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvlinkReAdd_Click(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvJobs.Columns[e.ColumnIndex] != null && dgvJobs.Columns[e.ColumnIndex].HeaderText == "重新添加" && e.RowIndex >= 0)
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确定要重新添加吗?", "重新添加", messButton);
                if (dr == DialogResult.OK)//如果点击“确定”按钮
                {

                    if (dgvJobs.Rows.Count > e.RowIndex && dgvJobs.Rows[e.RowIndex] != null)
                    {
                        if (dgvJobs.Rows[e.RowIndex].Cells["ID"] != null)
                        {
                            string Ids = dgvJobs.Rows[e.RowIndex].Cells["ID"].Value.ToString();

                            Guid Id = Guid.Empty;
                            if (Guid.TryParse(Ids, out Id))
                            {
                                Model.Jobs model = BLL.Jobs.GetModelById(Id);
                                if (BLL.Jobs.ReAddJobs(model))
                                {
                                    BindList();
                                    MessageBox.Show("重新添加成功");
                                }
                                else
                                {
                                    MessageBox.Show("重新添加失败");
                                }
                            }
                            else
                            {
                                MessageBox.Show("重新添加失败");
                            }
                        }
                    }
                }
            }

        }
        #endregion

        #region 删除按钮点击事件 -void dgvlinkDoDel_Click(object sender, DataGridViewCellEventArgs e)
        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvlinkDoDel_Click(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvJobs.Columns[e.ColumnIndex] != null && dgvJobs.Columns[e.ColumnIndex].HeaderText == "删除" && e.RowIndex >= 0)
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确定要删除吗?", "删除", messButton);
                if (dr == DialogResult.OK)//如果点击“确定”按钮
                {

                    if (dgvJobs.Rows.Count > e.RowIndex && dgvJobs.Rows[e.RowIndex] != null)
                    {
                        if (dgvJobs.Rows[e.RowIndex].Cells["ID"] != null)
                        {
                            string Ids = dgvJobs.Rows[e.RowIndex].Cells["ID"].Value.ToString();

                            Guid Id = Guid.Empty;
                            if (Guid.TryParse(Ids, out Id))
                            {
                                if (BLL.Jobs.DelByIdAndQuartz(Id))
                                {
                                    BindList();
                                    MessageBox.Show("删除成功");
                                }
                                else
                                {
                                    MessageBox.Show("删除失败");
                                }
                            }
                            else
                            {
                                MessageBox.Show("删除失败");
                            }
                        }
                    }
                }
            }

        }
        #endregion
    }
}
