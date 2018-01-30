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
    public partial class JobGroupMgr : Form
    {

        public delegate void SetdataHandler();
        public JobGroupMgr()
        {
            InitializeComponent();
        }


        #region 页面加载事件 -void JobGroupMgr_Load(object sender, EventArgs e)
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobGroupMgr_Load(object sender, EventArgs e)
        {
            ControlSetting.controlSetting.DataGridViewSet(dgvJobGroups);
            dgvJobGroups.CellClick += new DataGridViewCellEventHandler(dgvlinkDel_Click);
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
            JobGroupAdd jobgroupadd = new JobGroupAdd(new SetdataHandler(BindList));
            jobgroupadd.ShowDialog();
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

        #region 数据列表绑定完成事件 -void dgvJobGroups_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        /// <summary>
        /// 数据列表绑定完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJobGroups_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvJobGroups.Columns["ID"].Visible = false;//隐藏某列：
            dgvJobGroups.Columns["JobNum"].Visible = false;//隐藏某列：
            dgvJobGroups.Columns["No"].HeaderText = "序号";
            dgvJobGroups.Columns["Name"].HeaderText = "名称";
        }
        #endregion

        #region 绑定列表 +void BindList()
        /// <summary>
        /// 绑定列表
        /// </summary>
        public void BindList()
        {
            string name = txtJobName.Text;
            if (dgvJobGroups.Columns.Contains("dgvlinkDel"))
            {
                dgvJobGroups.Columns.Remove("dgvlinkDel");
            }
            List<Model.JobGroup> models = BLL.JobGroup.GetModels(m => m.Name.Contains(name));
            if (models != null && models.Count > 0)
            {
                models = models.OrderBy(m => m.SortCode).ToList();
            }
            dgvJobGroups.DataSource = models;

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
            dgvJobGroups.Columns.Add(dgvlinkDel);
            dgvJobGroups.Refresh();
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
            if (e.ColumnIndex >= 0 && dgvJobGroups.Columns[e.ColumnIndex] != null && dgvJobGroups.Columns[e.ColumnIndex].HeaderText == "操作" && e.RowIndex >= 0)
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确定要删除吗?", "是否删除", messButton);
                if (dr == DialogResult.OK)//如果点击“确定”按钮
                {

                    if (dgvJobGroups.Rows.Count > e.RowIndex && dgvJobGroups.Rows[e.RowIndex] != null)
                    {
                        if (dgvJobGroups.Rows[e.RowIndex].Cells["ID"] != null)
                        {
                            string ids = dgvJobGroups.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                            Guid id = Guid.Empty;
                            if (Guid.TryParse(ids, out id))
                            {
                                if (BLL.JobGroup.DelById(ids))
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
