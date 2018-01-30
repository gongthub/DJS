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
        public delegate void SetdataHandler();
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
            dgvTriggerGroups.CellClick += new DataGridViewCellEventHandler(dgvlinkDel_Click);
            BindList();
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
            TriggerGroupAdd triggerGroupAdd = new TriggerGroupAdd(new SetdataHandler(BindList));
            triggerGroupAdd.ShowDialog();
        }
        #endregion

        #region 数据列表绑定完成事件 -void dgvTriggerGroups_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        /// <summary>
        /// 数据列表绑定完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTriggerGroups_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvTriggerGroups.Columns["ID"].Visible = false;//隐藏某列：
            dgvTriggerGroups.Columns["TriggerNum"].Visible = false;//隐藏某列：
            dgvTriggerGroups.Columns["No"].HeaderText = "序号";
            dgvTriggerGroups.Columns["Name"].HeaderText = "名称";
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
            BindList();
        }
        #endregion

        #region 绑定列表 +void BindList()
        /// <summary>
        /// 绑定列表
        /// </summary>
        public void BindList()
        {
            string name = txtTriggerName.Text;
            if (dgvTriggerGroups.Columns.Contains("dgvlinkDel"))
            {
                dgvTriggerGroups.Columns.Remove("dgvlinkDel");
            }
            List<Model.TriggerGroup> models = BLL.TriggerGroup.GetModels(m => m.Name.Contains(name));
            if (models != null && models.Count > 0)
            {
                models = models.OrderBy(m => m.SortCode).ToList();
            }
            dgvTriggerGroups.DataSource = models;

            DataGridViewLinkColumn dgvlinkDel = new DataGridViewLinkColumn();
            {
                dgvlinkDel.Name = "dgvlinkDel";
                dgvlinkDel.HeaderText = "操作";
                dgvlinkDel.Text = "删除";
                dgvlinkDel.LinkColor = Color.Red;
                dgvlinkDel.ActiveLinkColor = Color.Red;
                dgvlinkDel.UseColumnTextForLinkValue = true;
                dgvlinkDel.AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.AllCells;
                //dgvbtnDel.FlatStyle = FlatStyle.Standard;
                dgvlinkDel.CellTemplate.Style.BackColor = Color.Honeydew;
            }
            dgvTriggerGroups.Columns.Add(dgvlinkDel);
            dgvTriggerGroups.Refresh(); 
        }
        #endregion

        #region 删除按钮点击事件 -void dgvlinkDel_Click(object sender, DataGridViewCellEventArgs e)
        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvlinkDel_Click(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvTriggerGroups.Columns[e.ColumnIndex] != null && dgvTriggerGroups.Columns[e.ColumnIndex].HeaderText == "操作" && e.RowIndex >= 0)
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确定要删除吗?", "是否删除", messButton);
                if (dr == DialogResult.OK)//如果点击“确定”按钮
                {

                    if (dgvTriggerGroups.Rows.Count > e.RowIndex && dgvTriggerGroups.Rows[e.RowIndex] != null)
                    {
                        if (dgvTriggerGroups.Rows[e.RowIndex].Cells["ID"] != null)
                        {
                            string ids = dgvTriggerGroups.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                            Guid id = Guid.Empty;
                            if (Guid.TryParse(ids, out id))
                            {
                                if (BLL.TriggerGroup.DeleteForm(ids))
                                {
                                    BindList();
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
