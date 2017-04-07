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
            string statusStr = Common.QuartzHelp.quartzHelp.GetState();
            if (statusStr == "正常运行")
            {
                btnStart.Text = "暂停Quartz";
            }
            ControlSetting.controlSetting.DataGridViewSet(dgvJobs);
            dgvJobs.CellClick += new DataGridViewCellEventHandler(dgvlinkDo_Click);
            dgvJobs.CellClick += new DataGridViewCellEventHandler(dgvlinkReAdd_Click);
            dgvJobs.CellClick += new DataGridViewCellEventHandler(dgvlinkDoDel_Click);
            dgvJobs.CellClick += new DataGridViewCellEventHandler(dgvlinkDoConfig_Click);
            dgvJobs.CellClick += new DataGridViewCellEventHandler(dgvlinkStart_Click);
            //dgvJobs.CellClick += new DataGridViewCellEventHandler(dgvlinkAutoStart_Click);
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
            BindList();
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

        #region 绑定列表方法 -void BindList()
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
            if (dgvJobs.Columns.Contains("dgvlinkDoConfig"))
            {
                dgvJobs.Columns.Remove("dgvlinkDoConfig");
            }
            if (dgvJobs.Columns.Contains("dgvlinkStart"))
            {
                dgvJobs.Columns.Remove("dgvlinkStart");
            }
            //if (dgvJobs.Columns.Contains("dgvlinkAutoStart"))
            //{
            //    dgvJobs.Columns.Remove("dgvlinkAutoStart");
            //}
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


            DataGridViewLinkColumn dgvlinkDoConfig = new DataGridViewLinkColumn();
            {
                dgvlinkDoConfig.Name = "dgvlinkDoConfig";
                dgvlinkDoConfig.HeaderText = "配置";
                dgvlinkDoConfig.Text = "配置";
                dgvlinkDoConfig.LinkColor = Color.Red;
                dgvlinkDoConfig.ActiveLinkColor = Color.Red;
                dgvlinkDoConfig.UseColumnTextForLinkValue = true;
                dgvlinkDoConfig.AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.AllCells;
                //dgvbtnDel.FlatStyle = FlatStyle.Standard;
                dgvlinkDoConfig.CellTemplate.Style.BackColor = Color.Honeydew;
            }
            dgvJobs.Columns.Add(dgvlinkDoConfig);

            DataGridViewLinkColumn dgvlinkStart = new DataGridViewLinkColumn();
            {
                dgvlinkStart.Name = "dgvlinkStart";
                dgvlinkStart.HeaderText = "启用/暂停";
                dgvlinkStart.Text = "启用/暂停";
                dgvlinkStart.LinkColor = Color.Red;
                dgvlinkStart.ActiveLinkColor = Color.Red;
                dgvlinkStart.UseColumnTextForLinkValue = true;
                dgvlinkStart.AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.AllCells;
                //dgvbtnDel.FlatStyle = FlatStyle.Standard;
                dgvlinkStart.CellTemplate.Style.BackColor = Color.Honeydew;
            }
            dgvJobs.Columns.Add(dgvlinkStart);

            //DataGridViewLinkColumn dgvlinkAutoStart = new DataGridViewLinkColumn();
            //{
            //    dgvlinkStart.Name = "dgvlinkAutoStart";
            //    dgvlinkStart.HeaderText = "是否自启动";
            //    dgvlinkStart.Text = "自动/手动";
            //    dgvlinkStart.LinkColor = Color.Red;
            //    dgvlinkStart.ActiveLinkColor = Color.Red;
            //    dgvlinkStart.UseColumnTextForLinkValue = true;
            //    dgvlinkStart.AutoSizeMode =
            //        DataGridViewAutoSizeColumnMode.AllCells;
            //    //dgvbtnDel.FlatStyle = FlatStyle.Standard;
            //    dgvlinkStart.CellTemplate.Style.BackColor = Color.Honeydew;
            //}
            //dgvJobs.Columns.Add(dgvlinkAutoStart);

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
            if (dgvJobs.Columns["ID"] != null)
                dgvJobs.Columns["ID"].Visible = false;//隐藏某列：
            if (dgvJobs.Columns["State"] != null)
                dgvJobs.Columns["State"].Visible = false;//隐藏某列：
            if (dgvJobs.Columns["Type"] != null)
                dgvJobs.Columns["Type"].Visible = false;//隐藏某列：
            if (dgvJobs.Columns["AssType"] != null)
                dgvJobs.Columns["AssType"].Visible = false;//隐藏某列：
            if (dgvJobs.Columns["DLLID"] != null)
                dgvJobs.Columns["DLLID"].Visible = false;//隐藏某列：
            if (dgvJobs.Columns["Time"] != null)
                dgvJobs.Columns["Time"].Visible = false;//隐藏某列：
            if (dgvJobs.Columns["Crons"] != null)
                dgvJobs.Columns["Crons"].HeaderText = "策略";
            if (dgvJobs.Columns["Name"] != null)
                dgvJobs.Columns["Name"].HeaderText = "名称";
            if (dgvJobs.Columns["GroupName"] != null)
                dgvJobs.Columns["GroupName"].HeaderText = "任务组";
            if (dgvJobs.Columns["TriggerName"] != null)
                dgvJobs.Columns["TriggerName"].HeaderText = "触发器";
            if (dgvJobs.Columns["TriggerGroup"] != null)
                dgvJobs.Columns["TriggerGroup"].HeaderText = "触发器组";
            //dgvJobs.Columns["State"].HeaderText = "状态";
            if (dgvJobs.Columns["StateName"] != null)
                dgvJobs.Columns["StateName"].HeaderText = "状态";
            if (dgvJobs.Columns["TypeName"] != null)
                dgvJobs.Columns["TypeName"].HeaderText = "类型"; ;
            if (dgvJobs.Columns["DLLName"] != null)
                dgvJobs.Columns["DLLName"].HeaderText = "DLL名称";
            if (dgvJobs.Columns["Time"] != null)
                dgvJobs.Columns["Time"].HeaderText = "执行时间";
            if (dgvJobs.Columns["ConfigName"] != null)
                dgvJobs.Columns["ConfigName"].HeaderText = "配置名称";
            if (dgvJobs.Columns["IsAuto"] != null)
                dgvJobs.Columns["IsAuto"].HeaderText = "是否自启动";
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
                        int state = 0;
                        if (dgvJobs.Rows[e.RowIndex].Cells["State"] != null && Int32.TryParse(dgvJobs.Rows[e.RowIndex].Cells["State"].Value.ToString(), out state))
                        {
                            if (state == (int)Model.Enums.TriggerState.Normal)
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
                            else
                            {
                                MessageBox.Show("任务状态只能为正常时才能执行！");
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
                                //任务已经存在时
                                if (BLL.Jobs.IsExistJobkeyById(Id))
                                {
                                    MessageBoxButtons messButtonKey = MessageBoxButtons.OKCancel;
                                    DialogResult drKey = MessageBox.Show("任务已经存在，是否继续重新添加?", "重新添加", messButtonKey);
                                    if (drKey == DialogResult.OK)//如果点击“确定”按钮
                                    {
                                        if (!BLL.Jobs.DelByIdForQuartz(Id))
                                        {
                                            MessageBox.Show("删除Quartz中任务失败！");
                                        }
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }

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

        #region 配置按钮点击事件 -void dgvlinkDoConfig_Click(object sender, DataGridViewCellEventArgs e)
        /// <summary>
        /// 配置按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvlinkDoConfig_Click(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvJobs.Columns[e.ColumnIndex] != null && dgvJobs.Columns[e.ColumnIndex].HeaderText == "配置" && e.RowIndex >= 0)
            {

                if (dgvJobs.Rows.Count > e.RowIndex && dgvJobs.Rows[e.RowIndex] != null)
                {
                    if (dgvJobs.Rows[e.RowIndex].Cells["ID"] != null)
                    {
                        string Ids = dgvJobs.Rows[e.RowIndex].Cells["ID"].Value.ToString();

                        Guid Id = Guid.Empty;
                        if (Guid.TryParse(Ids, out Id))
                        {
                            DJS.WinApp.Setting.JobConfig jobconfig = new Setting.JobConfig();
                            jobconfig.IDS = Id.ToString();
                            jobconfig.Show();
                        }
                        else
                        {
                            MessageBox.Show("配置失败");
                        }
                    }
                }
            }

        }
        #endregion

        #region 启用/暂停按钮点击事件 -void dgvlinkStart_Click(object sender, DataGridViewCellEventArgs e)
        /// <summary>
        /// 启用/暂停按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvlinkStart_Click(object sender, DataGridViewCellEventArgs e)
        {

            bool retState = false;
            if (e.ColumnIndex >= 0 && dgvJobs.Columns[e.ColumnIndex] != null && dgvJobs.Columns[e.ColumnIndex].HeaderText == "启用/暂停" && e.RowIndex >= 0)
            {
                int state = 0;
                if (dgvJobs.Rows.Count > e.RowIndex && dgvJobs.Rows[e.RowIndex] != null)
                {
                    if (dgvJobs.Rows[e.RowIndex].Cells["State"] != null && Int32.TryParse(dgvJobs.Rows[e.RowIndex].Cells["State"].Value.ToString(), out state))
                    {
                        if (dgvJobs.Rows[e.RowIndex].Cells["GroupName"] != null && dgvJobs.Rows[e.RowIndex].Cells["Name"] != null)
                        {
                            string groupNames = dgvJobs.Rows[e.RowIndex].Cells["GroupName"].Value.ToString();
                            string names = dgvJobs.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                            if (state == (int)Model.Enums.TriggerState.Normal)
                            {
                                DialogResult dr = MessageBox.Show("确定要暂停服务吗?", "暂停服务", messButton);
                                if (dr == DialogResult.OK)//如果点击“确定”按钮
                                {
                                    if (BLL.Jobs.PauseJob(groupNames, names))
                                    {
                                        BindList();
                                        retState = true;
                                    }
                                }
                            }
                            else
                                if (state == (int)Model.Enums.TriggerState.Paused)
                                {
                                    DialogResult dr = MessageBox.Show("启动时会执行一次任务，确定要启动服务吗?", "启动服务", messButton);
                                    if (dr == DialogResult.OK)//如果点击“确定”按钮
                                    {
                                        if (BLL.Jobs.ResumeJob(groupNames, names))
                                        {
                                            BindList();
                                            retState = true;
                                        }
                                    }
                                }
                        }
                    }
                }

                if (retState)
                {
                    MessageBox.Show("执行成功");
                }
                else
                {
                    MessageBox.Show("执行失败");
                }
            }
        }
        #endregion

        #region 是否自启动按钮点击事件 -void dgvlinkAutoStart_Click(object sender, DataGridViewCellEventArgs e)
        /// <summary>
        /// 是否自启动按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvlinkAutoStart_Click(object sender, DataGridViewCellEventArgs e)
        {

            bool retState = false;
            if (e.ColumnIndex >= 0 && dgvJobs.Columns[e.ColumnIndex] != null && dgvJobs.Columns[e.ColumnIndex].HeaderText == "是否自启动" && e.RowIndex >= 0)
            {
                bool isAuto = false;
                if (dgvJobs.Rows.Count > e.RowIndex && dgvJobs.Rows[e.RowIndex] != null)
                {
                    if (dgvJobs.Rows[e.RowIndex].Cells["IsAuto"] != null && bool.TryParse(dgvJobs.Rows[e.RowIndex].Cells["IsAuto"].Value.ToString(), out isAuto))
                    {
                        if (dgvJobs.Rows[e.RowIndex].Cells["Name"] != null)
                        {
                            string ids = dgvJobs.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                            string names = dgvJobs.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                            Guid Id = Guid.Empty;
                            if (Guid.TryParse(ids, out Id))
                            {
                                Model.Jobs jobEntity = BLL.Jobs.GetModelById(Id);
                                if (jobEntity != null && jobEntity.ID != Guid.Empty)
                                {
                                    MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                                    if (isAuto == true)
                                    {
                                        DialogResult dr = MessageBox.Show("确定要将" + names + "服务设置为自动启动吗?", "自动启动", messButton);
                                        if (dr == DialogResult.OK)//如果点击“确定”按钮
                                        {
                                            jobEntity.IsAuto = true;
                                            if (BLL.Jobs.Add(jobEntity))
                                            {
                                                BindList();
                                                retState = true;
                                            }
                                        }
                                    }
                                    else
                                        if (isAuto == false)
                                        {
                                            DialogResult dr = MessageBox.Show("确定要将" + names + "服务设置为不自动启动吗?", "自动启动", messButton);
                                            if (dr == DialogResult.OK)//如果点击“确定”按钮
                                            {
                                                jobEntity.IsAuto = false;
                                                if (BLL.Jobs.Add(jobEntity))
                                                {
                                                    BindList();
                                                    retState = true;
                                                }
                                            }
                                        }
                                }
                            }
                        }
                    }
                }

                if (retState)
                {
                    MessageBox.Show("执行成功");
                }
                else
                {
                    MessageBox.Show("执行失败");
                }
            }
        }
        #endregion

        #region 格式化 -void dgvJobs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJobs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.ColumnIndex == 6) //哪一列
            {
                DateTime times = DateTime.MinValue;
                if (DateTime.TryParse(e.Value.ToString(), out times))
                {
                    e.Value = times.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }
        #endregion

        #region 暂停按钮 -void btnStand_Click(object sender, EventArgs e)
        /// <summary>
        /// 暂停按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStand_Click(object sender, EventArgs e)
        {
            string pauses = btnStand.Text;
            if (pauses == "暂停Quartz")
            {
                Common.QuartzHelp.quartzHelp.PauseAll();
                btnStand.Text = "继续Quartz";
            }
            else
                if (pauses == "继续Quartz")
                {
                    Common.QuartzHelp.quartzHelp.ResumeAll();
                    btnStand.Text = "暂停Quartz";
                }
        }
        #endregion

        #region 开始按钮 -void btnStart_Click(object sender, EventArgs e)
        /// <summary>
        /// 开始按钮 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            string operations = btnStart.Text;
            if (operations == "开始Quartz")
            {
                Common.QuartzHelp.quartzHelp.Start();
                btnStart.Text = "暂停Quartz";
            }
            else if (operations == "暂停Quartz")
            {
                Common.QuartzHelp.quartzHelp.PauseAll();
                btnStart.Text = "继续Quartz";
            }
            else
                if (operations == "继续Quartz")
                {
                    Common.QuartzHelp.quartzHelp.ResumeAll();
                    btnStart.Text = "暂停Quartz";
                }
            BindList();
        }
        #endregion

        #region 全部重新添加按钮 -void btnAddAll_Click(object sender, EventArgs e)
        /// <summary>
        /// 全部重新添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAll_Click(object sender, EventArgs e)
        {
            int josnum = 0;
            int josnume = 0;
            List<Model.Jobs> models = BLL.Jobs.GetJobs();
            foreach (Model.Jobs model in models)
            {
                if (model != null)
                {
                    if (model.State == (int)TriggerState.Complete)
                    {
                        if (BLL.Jobs.ReAddJobs(model))
                        {
                            josnum++;
                        }
                        else
                        {
                            josnume++;
                        }
                    }
                }
            }
            BindList();
            MessageBox.Show("重新添加成功" + josnum + "条！" + "重新添加失败" + josnume + "条！");
        }
        #endregion

    }
}
