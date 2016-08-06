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
    public partial class TriggerGroupMgr : Form
    {
        public TriggerGroupMgr()
        {
            InitializeComponent();
        }
         
        #region 页面加载事件 -void TriggerGroupMgr_Load(object sender, EventArgs e)
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TriggerGroupMgr_Load(object sender, EventArgs e)
        {
            ControlSetting.controlSetting.DataGridViewSet(dgvTriggerGroups);
            BindList("");
        }
        #endregion

        #region 添加按钮点击事件 -void btnAdd_Click(object sender, EventArgs e)
        /// <summary>
        /// 添加按钮点击事件
        /// </summary>
        /// <param name="sender"></pa ram>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            TriggerGroupAdd triggerGroupAdd = new TriggerGroupAdd();
            triggerGroupAdd.ShowDialog();
        }
        #endregion

        #region 查询 -void btnQuery_Click(object sender, EventArgs e)
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            string names = txtTriggerName.Text;
            BindList(names);
        }
        #endregion

        #region 绑定列表 +void BindList(string name)
        /// <summary>
        /// 绑定列表
        /// </summary>
        public void BindList(string name)
        {
            List<Model.TriggerGroup> models = BLL.TriggerGroup.GetModels(m => m.Name.Contains(name));
            dgvTriggerGroups.DataSource = models;
        }
        #endregion

    }
}
