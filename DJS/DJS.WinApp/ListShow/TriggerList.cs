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
    public partial class TriggerList : Form
    {
        public TriggerList()
        {
            InitializeComponent();
        }


        //注册Triggers数量改变事件
        private BLL.TriggersListen triggersListen = new BLL.TriggersListen();


        #region 窗体加载事件 -void TriggerList_Load(object sender, EventArgs e)
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TriggerList_Load(object sender, EventArgs e)
        { 
            ControlSetting.controlSetting.DataGridViewSet(dgvTriggers);
            BindCombox();
            BindList("", "");

        }
        #endregion

        #region Triggers数量发生改变事件 -void OnChange_Triggers(object sender, EventArgs e)
        /// <summary>
        /// Triggers数量发生改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChange_Triggers(object sender, EventArgs e)
        {

            Control.CheckForIllegalCrossThreadCalls = false;
            List<Model.Triggers> models = BLL.Triggers.GetTriggersForQuartz();
            dgvTriggers.DataSource = models;
            BLL.TriggersListen.triggersCounts = models.Count;
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
                triggersListen.AddTriggersChangeHandler(OnChange_Triggers);
                btnRefresh.Text = "停止刷新";
            }
            else
                if (strs == "停止刷新")
                {
                    triggersListen.DelTriggersChangeHandler(OnChange_Triggers);
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
            string name = txtTriggerName.Text;
            string group = cbTriggerGroup.Text;
            BindList(name, group);
        }
        #endregion

        #region 绑定列表方法
        /// <summary>
        /// 绑定列表方法
        /// </summary>
        private void BindList(string name, string group)
        {
            List<Model.Triggers> models = BLL.Triggers.GetTriggersForQuartz(m => m.Name.Contains(name) && m.GroupName.Contains(group));
            dgvTriggers.DataSource = models;
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
            List<Model.TriggerGroup> models = BLL.TriggerGroup.GetModels();
            cbTriggerGroup.DataSource = models;
            cbTriggerGroup.ValueMember = "No";
            cbTriggerGroup.DisplayMember = "Name";
        }
        #endregion
    }
}
