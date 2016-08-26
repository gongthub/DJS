using DJS.Common;
using Quartz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJS.WinApp
{
    public partial class AddJob : Form
    {
        /// <summary>
        /// 文件附件路径
        /// </summary>
        private static string JobFileSrcPath = ConfigHelp.JobFileSrcPath;

        private static Dictionary<string, string> FILENAMES = new Dictionary<string,string>();

        public AddJob()
        {
            InitializeComponent();
            FILENAMES = new Dictionary<string, string>();
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
            string times = dtpTime.Text.Trim();
            string crons = txtCron.Text.Trim();
            string jobNames = txtJobName.Text.Trim();
            string jobGroups = cbJobGroup.Text.Trim();
            string triggerNames = txtTriggerName.Text.Trim();
            string triggerGroups = cbTriggerGroup.Text.Trim();
            string name = cbNameSpace.Text.Trim();
            string classNames = cbClassName.Text.Trim();
            string configNames = txtConfigName.Text.Trim();

            string types = cbType.Text;
            string typesval = cbType.SelectedValue.ToString();


            Model.Jobs model = new Model.Jobs();
            model.ID = Guid.NewGuid();
            model.Name = jobNames;
            model.GroupName = jobGroups;
            model.TriggerName = triggerNames;
            model.TriggerGroup = triggerGroups;
            model.Crons = crons;
            model.ConfigName = configNames;
            DateTime time = DateTime.MinValue;
            if (DateTime.TryParse(times, out time))
            {
                model.Time = time;
            }
            int typet = 0;
            if (Int32.TryParse(typesval, out typet))
            {
                model.Type = typet;
            }


            string nameSpaces = "";
            Model.DllMgr ddlmgr = BLL.DllMgr.GetModels(m => m.Name == name).FirstOrDefault();
            if (ddlmgr != null)
            {
                nameSpaces = ddlmgr.NameSpace;
                model.DLLID = ddlmgr.ID;
                model.DLLName = ddlmgr.Name;
            }
            Type type = Common.AssemblyHelp.assembly.GetDllType(name, nameSpaces, classNames);
            model.AssType = type;
            model.State = (int)Model.Enums.TriggerState.Normal;
            if (CheckTxt(model))
            {
                if (!BLL.Jobs.IsExist(jobNames))
                {
                    if (BLL.Jobs.AddJobs(model))
                    { 
                        if (FILENAMES != null && FILENAMES.Count > 0)
                        {
                            BLL.JobFiles.DelByJobName(model.Name);
                            foreach (var file in FILENAMES)
                            {

                                Upload(file.Value, model.Name);
                                Model.JobFiles jobfile = new Model.JobFiles();
                                jobfile.ID = Guid.NewGuid();
                                jobfile.JobID = model.ID;
                                jobfile.JobName = model.Name;
                                jobfile.Name = file.Key; 
                                jobfile.Src = JobFileSrcPath + @"\" + model.Name + @"\" + file.Key;
                                BLL.JobFiles.Add(jobfile);
                            }
                        }

                        MessageBox.Show("添加成功！");
                    }
                    else
                    {
                        MessageBox.Show("添加失败！");
                    }
                }
                else
                {
                    MessageBox.Show("任务已经存在！");
                }
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
            //ArrayList arry = Common.FileHelp.GetDirectoryList(PATH);
            List<Model.DllMgr> models = BLL.DllMgr.GetModels();
            cbNameSpace.DataSource = models;
            cbNameSpace.DisplayMember = "Name";
        }
        #endregion

        #region 绑定操作类下拉框 -void BindClassName(string name)
        /// <summary>
        /// 绑定操作类下拉框
        /// </summary>
        private void BindClassName(string name)
        {
            Model.DllMgr model = BLL.DllMgr.GetModels(m => m.Name == name).FirstOrDefault();
            if (model != null)
            {

                ArrayList arry = Common.QuartzHelp.quartzHelp.GetIClassName(model.Name, model.NameSpace);
                cbClassName.DataSource = arry;
                cbClassName.DisplayMember = "Name";
            }
        }
        #endregion

        #region 绑定类型下拉框 -void BindType()
        /// <summary>
        /// 绑定类型下拉框
        /// </summary>
        private void BindType()
        {
            //ArrayList arry = Common.EnumHelp.enumHelp.ToArrayList(typeof(Model.Enums.TimeType));
            List<Model.SelectLists> list = Common.EnumHelp.enumHelp.ToSelectLists(typeof(Model.Enums.TimeType));
            cbType.DataSource = list;
            cbType.DisplayMember = "Name";
            cbType.ValueMember = "Value";
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

        #region 验证数据 -void CheckTxt(Model.Jobs model)
        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="model"></param>
        private bool CheckTxt(Model.Jobs model)
        {
            bool ret = true;
            if (model.Type == 0)
            {
                if (model.Crons == "")
                {
                    MessageBox.Show("策略不能为空！");
                    ret = false;
                }
            }

            if (model.Type == 1)
            {
                if (model.Time == DateTime.MinValue)
                {
                    MessageBox.Show("时间不能为空！");
                    ret = false;
                }
            }
            if (model.Name == "")
            {
                MessageBox.Show("job名称不能为空！");
                ret = false;
            }
            if (model.TriggerName == "")
            {
                MessageBox.Show("trigger名称不能为空！");
                ret = false;
            }
            if (model.ConfigName == "")
            {
                MessageBox.Show("配置名称不能为空！");
                ret = false;
            }
            if (model.GroupName == "")
            {
                MessageBox.Show("job组名称不能为空！");
                ret = false;
            }
            if (model.TriggerGroup == "")
            {
                MessageBox.Show("trigger组名称不能为空！");
                ret = false;
            }
            if (model.AssType == null)
            {
                MessageBox.Show("类名不能为空！");
                ret = false;
            }
            return ret;
        }
        #endregion

        #region 附件双击事件
        /// <summary>
        /// 附件双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFiles_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofdUpLoad = new OpenFileDialog();
            ofdUpLoad.Multiselect = true;
            try
            {
                if (ofdUpLoad.ShowDialog() == DialogResult.OK)
                {
                    string[] filenames = ofdUpLoad.FileNames;

                    string[] safeFileNames = ofdUpLoad.SafeFileNames;
                    for (int i = 0; i < safeFileNames.Length; i++)
                    {
                        FILENAMES.Add(safeFileNames[i], filenames[i]);
                        txtFiles.Text += safeFileNames[i] + "\r\n";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 上传文件 +void Upload(string filename, string name)
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="name"></param>
        public void Upload(string filename, string name)
        {
            FileInfo FILE = new FileInfo(filename);
            if (FILE.Exists)
            {
                string paths = JobFileSrcPath + @"\" + name + @"\";
                if (!DJS.Common.FileHelp.DirectoryIsExists(paths))
                {
                    DJS.Common.FileHelp.CreateDirectory(paths);
                }
                string fileNamePaths = JobFileSrcPath + @"\" + name + @"\" + FILE.Name;
                //文件存在时先删除
                if (FileHelp.FileExists(fileNamePaths))
                {
                    FileHelp.DeleteFiles(fileNamePaths);
                }
                FILE.CopyTo(JobFileSrcPath + @"\" + name + @"\" + FILE.Name);

            }
        }
        #endregion

        #region 取消按钮 - void btnNo_Click(object sender, EventArgs e)
        /// <summary>
        /// 取消按钮 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        } 
        #endregion

    }
}
